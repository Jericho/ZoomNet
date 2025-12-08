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
using ZoomNet.Models;

namespace ZoomNet.Utilities
{
	/// <summary>
	/// Handler to ensure requests to the Zoom API include a valid OAuth token.
	/// </summary>
	/// <seealso cref="IHttpFilter" />
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

		private static readonly ReaderWriterLockSlim _lock = new();

		private readonly OAuthConnectionInfo _connectionInfo;
		private readonly HttpClient _httpClient;
		private readonly TimeSpan _clockSkew;

		public OAuthTokenHandler(OAuthConnectionInfo connectionInfo, HttpClient httpClient, TimeSpan? clockSkew = null)
		{
			ArgumentNullException.ThrowIfNull(connectionInfo);
			ArgumentNullException.ThrowIfNullOrEmpty(connectionInfo.ClientId, $"{nameof(connectionInfo)}.{connectionInfo.ClientId}");
			ArgumentNullException.ThrowIfNullOrEmpty(connectionInfo.ClientSecret, $"{nameof(connectionInfo)}.{connectionInfo.ClientSecret}");
			ArgumentNullException.ThrowIfNull(httpClient);

			_connectionInfo = connectionInfo;
			_httpClient = httpClient;
			_clockSkew = clockSkew.GetValueOrDefault(TimeSpan.FromMinutes(5));
		}

		/// <summary>Method invoked just before the HTTP request is submitted. This method can modify the outgoing HTTP request.</summary>
		/// <param name="request">The HTTP request.</param>
		public void OnRequest(IRequest request)
		{
			// Do not overwrite the Authorization header if it is already set.
			// One example where it's important to preserve the Authorization
			// header is CloudRecordings.DownloadFileAsync where developers can
			// specify a custom bearer token which must take precedence.
			if (request.Message.Headers.Authorization == null) request.WithBearerAuthentication(Token);
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

						if (forceRefresh || TokenIsExpired())
						{
							var contentValues = new Dictionary<string, string>()
							{
								{ "grant_type", _connectionInfo.GrantType.ToEnumString() },
							};

							switch (_connectionInfo.GrantType)
							{
								case OAuthGrantType.AccountCredentials:
									contentValues.Add("account_id", _connectionInfo.AccountId);
									break;
								case OAuthGrantType.AuthorizationCode:
									contentValues.Add("code", _connectionInfo.AuthorizationCode);
									if (!string.IsNullOrEmpty(_connectionInfo.RedirectUri)) contentValues.Add("redirect_uri", _connectionInfo.RedirectUri);
									if (!string.IsNullOrEmpty(_connectionInfo.CodeVerifier)) contentValues.Add("code_verifier", _connectionInfo.CodeVerifier);
									break;
								case OAuthGrantType.RefreshToken:
									contentValues.Add("refresh_token", _connectionInfo.RefreshToken);
									break;
							}

							var requestTime = DateTime.UtcNow;
							var request = new HttpRequestMessage(HttpMethod.Post, "https://api.zoom.us/oauth/token");
							request.Headers.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(Encoding.ASCII.GetBytes($"{_connectionInfo.ClientId}:{_connectionInfo.ClientSecret}")));
							request.Content = new FormUrlEncodedContent(contentValues);
							var response = _httpClient.SendAsync(request).ConfigureAwait(false).GetAwaiter().GetResult();
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
							_connectionInfo.TokenExpiration = requestTime.AddSeconds(jsonResponse.GetPropertyValue("expires_in", 60 * 60));
							_connectionInfo.Scopes = new ReadOnlyCollection<string>(jsonResponse.GetPropertyValue("scope", string.Empty).Split(' ').OrderBy(x => x).ToList());

							// Please note that Server-to-Server OAuth does not use the refresh token.
							// Therefore change the grant type to 'RefreshToken' only when the response includes a refresh token.
							if (!string.IsNullOrEmpty(_connectionInfo.RefreshToken)) _connectionInfo.GrantType = OAuthGrantType.RefreshToken;

							_connectionInfo.OnTokenRefreshed?.Invoke(_connectionInfo.RefreshToken, _connectionInfo.AccessToken);
						}
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
