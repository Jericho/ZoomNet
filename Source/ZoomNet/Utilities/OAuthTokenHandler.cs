using Newtonsoft.Json.Linq;
using Pathoschild.Http.Client;
using Pathoschild.Http.Client.Extensibility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.Serialization;
using System.Text;
using System.Threading;
using ZoomNet.Models;

namespace ZoomNet.Utilities
{
	/// <summary>
	/// Handler to ensure requests to the Zoom API include a valid JWT token.
	/// </summary>
	/// <seealso cref="Pathoschild.Http.Client.Extensibility.IHttpFilter" />
	internal class OAuthTokenHandler : IHttpFilter
	{
		private static readonly ReaderWriterLockSlim _lock = new ReaderWriterLockSlim();

		private readonly OAuthConnectionInfo _connectionInfo;
		private readonly HttpClient _httpClient;
		private readonly TimeSpan _clockSkew;

		private string _accessToken;
		private IDictionary<string, string[]> _tokenScope;
		private DateTime _tokenExpiration;

		public OAuthTokenHandler(OAuthConnectionInfo connectionInfo, HttpClient httpClient, TimeSpan? clockSkew = null)
		{
			if (string.IsNullOrEmpty(connectionInfo.ClientId)) throw new ArgumentNullException(nameof(connectionInfo.ClientId));
			if (string.IsNullOrEmpty(connectionInfo.ClientSecret)) throw new ArgumentNullException(nameof(connectionInfo.ClientSecret));

			_connectionInfo = connectionInfo;
			_httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
			_clockSkew = clockSkew.GetValueOrDefault(TimeSpan.FromMinutes(5));
			_tokenExpiration = DateTime.MinValue;
		}

		/// <summary>Method invoked just before the HTTP request is submitted. This method can modify the outgoing HTTP request.</summary>
		/// <param name="request">The HTTP request.</param>
		public void OnRequest(IRequest request)
		{
			RefreshTokenIfNecessary();
			request.WithBearerAuthentication(_accessToken);
		}

		/// <summary>Method invoked just after the HTTP response is received. This method can modify the incoming HTTP response.</summary>
		/// <param name="response">The HTTP response.</param>
		/// <param name="httpErrorAsException">Whether HTTP error responses should be raised as exceptions.</param>
		public void OnResponse(IResponse response, bool httpErrorAsException) { }

		private void RefreshTokenIfNecessary()
		{
			_lock.EnterUpgradeableReadLock();

			if (TokenIsExpired())
			{
				try
				{
					_lock.EnterWriteLock();

					var grantType = _connectionInfo.GrantType.GetAttributeOfType<EnumMemberAttribute>().Value;
					var requestUrl = $"https://api.zoom.us/oauth/token?grant_type={grantType}";
					if (_connectionInfo.GrantType == OAuthGrantType.AuthorizationCode) requestUrl += $"&code={_connectionInfo.AuthorizationCode}";

					var request = new HttpRequestMessage(HttpMethod.Post, requestUrl);
					request.Headers.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(Encoding.ASCII.GetBytes($"{_connectionInfo.ClientId}:{_connectionInfo.ClientSecret}")));
					var response = _httpClient.SendAsync(request).GetAwaiter().GetResult();
					var responseContent = response.Content.ReadAsStringAsync(null).ConfigureAwait(false).GetAwaiter().GetResult();
					var jObject = JObject.Parse(responseContent);

					if (!response.IsSuccessStatusCode)
					{
						throw new ZoomException(jObject.GetPropertyValue<string>("reason"), response, "No diagnostic available");
					}

					_accessToken = jObject.GetPropertyValue<string>("access_token");
					_tokenScope = jObject.GetPropertyValue<string>("scope")
						.Split(' ')
						.Select(x => x.Split(':'))
						.ToDictionary(x => x[0], x => x.Skip(1).ToArray());
					_tokenExpiration = DateTime.UtcNow.AddSeconds(jObject.GetPropertyValue<int>("expires_in", 60 * 60));
				}
				finally
				{
					_lock.ExitWriteLock();
				}
			}

			_lock.ExitUpgradeableReadLock();
		}

		private bool TokenIsExpired()
		{
			return _tokenExpiration <= DateTime.UtcNow.Add(_clockSkew);
		}
	}
}
