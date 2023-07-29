using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using ZoomNet.Models;

namespace ZoomNet.Utilities
{
	/// <summary>
	/// Handler to ensure requests to the Zoom API include a valid OAuth token.
	/// </summary>
	/// <seealso cref="Pathoschild.Http.Client.Extensibility.IHttpFilter" />
	internal class OAuthTokenHandler : ITokenHandler
	{
		public IConnectionInfo ConnectionInfo
		{
			get => _connectionInfo;
		}

		private static readonly TimeSpan DEFAULT_CLOCKSKEW = TimeSpan.FromMinutes(5);

		private readonly OAuthConnectionInfo _connectionInfo;
		private readonly HttpClient _httpClient;
		private readonly TimeSpan _clockSkew;

		public OAuthTokenHandler(OAuthConnectionInfo connectionInfo, HttpClient httpClient, TimeSpan? clockSkew = null)
		{
			if (connectionInfo == null) throw new ArgumentNullException(nameof(connectionInfo));
			if (string.IsNullOrEmpty(connectionInfo.ClientId)) throw new ArgumentNullException("connectionInfo.ClientId");
			if (string.IsNullOrEmpty(connectionInfo.ClientSecret)) throw new ArgumentNullException("connectionInfo.ClientSecret");
			if (httpClient == null) throw new ArgumentNullException(nameof(httpClient));

			_connectionInfo = connectionInfo;
			_httpClient = httpClient;
			_clockSkew = clockSkew.GetValueOrDefault(DEFAULT_CLOCKSKEW);
		}

		/// <inheritdoc/>
		public async Task<(string RefreshToken, string AccessToken, DateTime TokenExpiration)> GetTokenInfoAsync(bool forceRefresh, (string RefreshToken, string AccessToken, DateTime TokenExpiration) previousTokenInfo, CancellationToken cancellationToken = default)
		{
			// Check if the previous token appears to be valid for a reasonable period of time (unless forceRefresh is true).
			// The clock scew represents the "reasonable period of time" and is 5 minutes by default.
			if (!forceRefresh && previousTokenInfo.TokenExpiration > DateTime.UtcNow.Add(_clockSkew)) return previousTokenInfo;

			// Retrieve the token info from storage.
			// By default token info is stored in memory but developers can provide their own "token repository" and store token in a different location such as Azure, AWS, Redis, SQL Server, etc.
			var currentTokenInfo = await _connectionInfo.TokenRepository.GetTokenInfoAsync(cancellationToken).ConfigureAwait(false);

			// Check if the token appears to be valid for a reasonable period of time (unless forceRefresh is true).
			if (!forceRefresh && currentTokenInfo.TokenExpiration > DateTime.UtcNow.Add(_clockSkew)) return currentTokenInfo;

			try
			{
				// Acquire a lock to prevent other processes from updating the token at the same time
				await _connectionInfo.TokenRepository.AcquireLeaseAsync(cancellationToken).ConfigureAwait(false);

				// Retrieve the token info a second time and check if it has been updated while we were waiting for the lock
				currentTokenInfo = _connectionInfo.TokenRepository.GetTokenInfoAsync(cancellationToken).GetAwaiter().GetResult();

				// Again, check if the token needs to be refreshed using the same rules
				if (previousTokenInfo != currentTokenInfo) return currentTokenInfo;
				if (!forceRefresh && currentTokenInfo.TokenExpiration > DateTime.UtcNow.Add(_clockSkew)) return currentTokenInfo;

				// We have determined that the token needs to be refreshed
				var contentValues = new Dictionary<string, string>()
				{
					{ "grant_type", _connectionInfo.GrantType.ToEnumString() },
				};

				var requestTime = DateTime.UtcNow;
				var request = new HttpRequestMessage(HttpMethod.Post, "https://api.zoom.us/oauth/token");
				request.Headers.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(Encoding.ASCII.GetBytes($"{_connectionInfo.ClientId}:{_connectionInfo.ClientSecret}")));
				request.Content = new FormUrlEncodedContent(contentValues);
				var response = await _httpClient.SendAsync(request, cancellationToken).ConfigureAwait(false);
				var responseContent = await response.Content.ReadAsStringAsync(null, cancellationToken).ConfigureAwait(false);

				if (string.IsNullOrEmpty(responseContent)) throw new Exception(response.ReasonPhrase);

				var jsonResponse = JsonDocument.Parse(responseContent).RootElement;

				if (!response.IsSuccessStatusCode)
				{
					var reason = jsonResponse.GetPropertyValue("reason", "The Zoom API did not provide a reason");
					throw new ZoomException(reason, response, "No diagnostic available", null);
				}

				var newRefreshToken = jsonResponse.GetPropertyValue("refresh_token", string.Empty);
				var newAccessToken = jsonResponse.GetPropertyValue("access_token", string.Empty);
				var newTokenExpiration = requestTime.AddSeconds(jsonResponse.GetPropertyValue("expires_in", 60 * 60));
				currentTokenInfo = (newRefreshToken, newAccessToken, newTokenExpiration);

				_connectionInfo.TokenScope = new ReadOnlyDictionary<string, string[]>(
					jsonResponse.GetPropertyValue("scope", string.Empty)
						.Split(' ')
						.Select(x => x.Split(new[] { ':' }, 2))
						.Select(x => new KeyValuePair<string, string[]>(x[0], x.Skip(1).ToArray()))
						.GroupBy(x => x.Key)
						.ToDictionary(
							x => x.Key,
							x => x.SelectMany(c => c.Value).ToArray()));

				// Please note that Server-to-Server OAuth does not use the refresh token.
				// Therefore change the grant type to 'RefreshToken' only when the response includes a refresh token.
				if (!string.IsNullOrEmpty(newRefreshToken)) _connectionInfo.GrantType = OAuthGrantType.RefreshToken;

				await _connectionInfo.TokenRepository.SaveTokenInfoAsync(newRefreshToken, newAccessToken, newTokenExpiration, cancellationToken).ConfigureAwait(false);
			}
			catch (Exception e)
			{
				Debug.WriteLine(e.Message);
			}
			finally
			{
				await _connectionInfo.TokenRepository.ReleaseLeaseAsync(cancellationToken).ConfigureAwait(false);
			}

			_connectionInfo.OnTokenRefreshed?.Invoke(currentTokenInfo.RefreshToken, currentTokenInfo.AccessToken);

			return currentTokenInfo;
		}
	}
}
