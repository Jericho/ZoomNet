using Pathoschild.Http.Client;
using Pathoschild.Http.Client.Extensibility;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.Serialization;
using System.Text;
using System.Text.Json;
using System.Threading;
using ZoomNet.Models;

namespace ZoomNet.Utilities
{
	/// <summary>
	/// Handler to ensure requests to the Zoom API include a valid JWT token.
	/// </summary>
	/// <seealso cref="Pathoschild.Http.Client.Extensibility.IHttpFilter" />
	internal class OAuthTokenHandler : IHttpFilter, ITokenHandler
	{
		public string Token
		{
			get
			{
				RefreshTokenIfNecessary(false);
				return _connectionInfo.AccessToken;
			}
		}

		public IConnectionInfo ConnectionInfo
		{
			get => _connectionInfo;
		}

		private static readonly ReaderWriterLockSlim _lock = new ReaderWriterLockSlim();

		private readonly OAuthConnectionInfo _connectionInfo;
		private readonly HttpClient _httpClient;
		private readonly TimeSpan _clockSkew;

		public OAuthTokenHandler(OAuthConnectionInfo connectionInfo, HttpClient httpClient, TimeSpan? clockSkew = null)
		{
			if (connectionInfo == null) throw new ArgumentNullException(nameof(connectionInfo));
			if (string.IsNullOrEmpty(connectionInfo.ClientId)) throw new ArgumentNullException(nameof(connectionInfo.ClientId));
			if (string.IsNullOrEmpty(connectionInfo.ClientSecret)) throw new ArgumentNullException(nameof(connectionInfo.ClientSecret));

			_connectionInfo = connectionInfo;
			_httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
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

		public string RefreshTokenIfNecessary(bool forceRefresh)
		{
			try
			{
				_lock.EnterUpgradeableReadLock();

				if (forceRefresh || TokenIsExpired())
				{
					try
					{
						_lock.EnterWriteLock();

						var grantType = _connectionInfo.GrantType.GetAttributeOfType<EnumMemberAttribute>().Value;
						var requestUrl = $"https://api.zoom.us/oauth/token?grant_type={grantType}";
						switch (_connectionInfo.GrantType)
						{
							case OAuthGrantType.AuthorizationCode:
								requestUrl += $"&code={_connectionInfo.AuthorizationCode}";
								if (!string.IsNullOrEmpty(_connectionInfo.RedirectUri)) requestUrl += $"&redirect_uri={_connectionInfo.RedirectUri}";
								break;
							case OAuthGrantType.RefreshToken:
								requestUrl += $"&refresh_token={_connectionInfo.RefreshToken}";
								break;
						}

						var requestTime = DateTime.UtcNow;
						var request = new HttpRequestMessage(HttpMethod.Post, requestUrl);
						request.Headers.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(Encoding.ASCII.GetBytes($"{_connectionInfo.ClientId}:{_connectionInfo.ClientSecret}")));
						var response = _httpClient.SendAsync(request).GetAwaiter().GetResult();
						var responseContent = response.Content.ReadAsStringAsync(null).ConfigureAwait(false).GetAwaiter().GetResult();

						if (string.IsNullOrEmpty(responseContent)) throw new Exception(response.ReasonPhrase);

						var jsonResponse = JsonDocument.Parse(responseContent).RootElement;

						if (!response.IsSuccessStatusCode)
						{
							var reason = jsonResponse.GetPropertyValue("reason", "The Zoom API did not provide a reason");
							throw new ZoomException(reason, response, "No diagnostic available", null);
						}

						_connectionInfo.RefreshToken = jsonResponse.GetPropertyValue("refresh_token", string.Empty);
						_connectionInfo.AccessToken = jsonResponse.GetPropertyValue("access_token", string.Empty);
						_connectionInfo.GrantType = OAuthGrantType.RefreshToken;
						_connectionInfo.TokenExpiration = requestTime.AddSeconds(jsonResponse.GetPropertyValue("expires_in", 60 * 60));
						_connectionInfo.TokenScope = new ReadOnlyDictionary<string, string[]>(
							jsonResponse.GetPropertyValue("scope", string.Empty)
								.Split(' ')
								.Select(x => x.Split(new[] { ':' }, 2))
								.Select(x => new KeyValuePair<string, string[]>(x[0], x.Skip(1).ToArray()))
								.GroupBy(x => x.Key)
								.ToDictionary(
									x => x.Key,
									x => x.SelectMany(c => c.Value).ToArray()));
						_connectionInfo.OnTokenRefreshed(_connectionInfo.RefreshToken, _connectionInfo.AccessToken);
					}
					finally
					{
						if (_lock.IsWriteLockHeld) _lock.ExitWriteLock();
					}
				}
			}
			finally
			{
				if (_lock.IsUpgradeableReadLockHeld) _lock.ExitUpgradeableReadLock();
			}

			return _connectionInfo.AccessToken;
		}

		private bool TokenIsExpired()
		{
			return _connectionInfo.TokenExpiration <= DateTime.UtcNow.Add(_clockSkew);
		}
	}
}
