using Pathoschild.Http.Client;
using Pathoschild.Http.Client.Extensibility;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
	internal class OAuthTokenHandler : IHttpFilter, ITokenHandler
	{
		public string Token
		{
			get
			{
				var tokenInfo = RefreshTokenIfNecessaryAsync(false, CancellationToken.None).ConfigureAwait(false).GetAwaiter().GetResult();
				return tokenInfo.AccessToken;
			}
		}

		public IConnectionInfo ConnectionInfo
		{
			get => _connectionInfo;
		}

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
			_clockSkew = clockSkew.GetValueOrDefault(TimeSpan.FromMinutes(5));
		}

		/// <summary>Method invoked just before the HTTP request is submitted. This method can modify the outgoing HTTP request.</summary>
		/// <param name="request">The HTTP request.</param>
		public void OnRequest(IRequest request)
		{
			request.WithBearerAuthentication(Token);
		}

		/// <summary>Method invoked just after the HTTP response is received. This method can modify the incoming HTTP response.</summary>
		/// <param name="response">The HTTP response.</param>
		/// <param name="httpErrorAsException">Whether HTTP error responses should be raised as exceptions.</param>
		public void OnResponse(IResponse response, bool httpErrorAsException) { }

		public async Task<(string RefreshToken, string AccessToken, DateTime TokenExpiration, int TokenIndex)> RefreshTokenIfNecessaryAsync(bool forceRefresh, CancellationToken cancellationToken = default)
		{
			// Retrieve the token info and check if it needs to be refreshed
			var tokenInfo = _connectionInfo.TokenManagementStrategy.GetTokenInfoAsync(cancellationToken).GetAwaiter().GetResult();
			if (!forceRefresh && tokenInfo.TokenExpiration > DateTime.UtcNow.Add(_clockSkew)) return tokenInfo;

			try
			{
				// Acquire a lock to prevent other processes from updating the token at the same time
				await _connectionInfo.TokenManagementStrategy.AcquireLeaseAsync(cancellationToken).ConfigureAwait(false);

				// Retrieve the token info a second time and check if it has been updated while we were waiting for the lock
				tokenInfo = _connectionInfo.TokenManagementStrategy.GetTokenInfoAsync(cancellationToken).GetAwaiter().GetResult();
				if (!forceRefresh && tokenInfo.TokenExpiration > DateTime.UtcNow.Add(_clockSkew)) return tokenInfo;

				// We have determined that the token needs to be refreshed
				var contentValues = new Dictionary<string, string>()
				{
					{ "grant_type", _connectionInfo.GrantType.ToEnumString() },
				};

				if (tokenInfo.TokenIndex != 0) contentValues.Add("token_index", tokenInfo.TokenIndex.ToString());

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
				tokenInfo = (newRefreshToken, newAccessToken, newTokenExpiration, tokenInfo.TokenIndex);

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

				await _connectionInfo.TokenManagementStrategy.SaveTokenInfoAsync(newRefreshToken, newAccessToken, newTokenExpiration, tokenInfo.TokenIndex, cancellationToken).ConfigureAwait(false);
			}
			finally
			{
				await _connectionInfo.TokenManagementStrategy.ReleaseLeaseAsync(cancellationToken).ConfigureAwait(false);
			}

			_connectionInfo.OnTokenRefreshed?.Invoke(tokenInfo.RefreshToken, tokenInfo.AccessToken);

			return tokenInfo;
		}
	}
}
