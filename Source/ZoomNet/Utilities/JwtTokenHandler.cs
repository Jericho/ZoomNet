using Jose;
using Pathoschild.Http.Client;
using Pathoschild.Http.Client.Extensibility;
using System;
using System.Collections.Generic;
using System.Text;

namespace ZoomNet.Utilities
{
	/// <summary>
	/// Handler to ensure requests to the Zoom API include a valid JWT token.
	/// </summary>
	/// <seealso cref="Pathoschild.Http.Client.Extensibility.IHttpFilter" />
	internal class JwtTokenHandler : IHttpFilter
	{
		private static readonly object _lock = new object();

		private readonly JwtConnectionInfo _connectionInfo;
		private readonly TimeSpan _clockSkew;
		private readonly TimeSpan _tokenLifeSpan;

		private string _jwtToken;
		private DateTime _tokenExpiration;

		public JwtTokenHandler(JwtConnectionInfo connectionInfo, TimeSpan? tokenLifeSpan = null, TimeSpan? clockSkew = null)
		{
			if (string.IsNullOrEmpty(connectionInfo.ApiKey)) throw new ArgumentNullException(nameof(connectionInfo.ApiKey));
			if (string.IsNullOrEmpty(connectionInfo.ApiSecret)) throw new ArgumentNullException(nameof(connectionInfo.ApiSecret));

			_connectionInfo = connectionInfo;
			_tokenLifeSpan = tokenLifeSpan.GetValueOrDefault(TimeSpan.FromMinutes(30));
			_clockSkew = clockSkew.GetValueOrDefault(TimeSpan.FromMinutes(5));
			_tokenExpiration = DateTime.MinValue;
		}

		/// <summary>Method invoked just before the HTTP request is submitted. This method can modify the outgoing HTTP request.</summary>
		/// <param name="request">The HTTP request.</param>
		public void OnRequest(IRequest request)
		{
			RefreshTokenIfNecessary();
			request.WithBearerAuthentication(_jwtToken);
		}

		/// <summary>Method invoked just after the HTTP response is received. This method can modify the incoming HTTP response.</summary>
		/// <param name="response">The HTTP response.</param>
		/// <param name="httpErrorAsException">Whether HTTP error responses should be raised as exceptions.</param>
		public void OnResponse(IResponse response, bool httpErrorAsException) { }

		private void RefreshTokenIfNecessary()
		{
			if (TokenIsExpired())
			{
				lock (_lock)
				{
					if (TokenIsExpired())
					{
						_tokenExpiration = DateTime.UtcNow.Add(_tokenLifeSpan);
						var jwtPayload = new Dictionary<string, object>()
						{
							{ "iss", _connectionInfo.ApiKey },
							{ "exp", _tokenExpiration.ToUnixTime() }
						};
						_jwtToken = JWT.Encode(jwtPayload, Encoding.ASCII.GetBytes(_connectionInfo.ApiSecret), JwsAlgorithm.HS256);
					}
				}
			}
		}

		private bool TokenIsExpired()
		{
			return _tokenExpiration <= DateTime.UtcNow.Add(_clockSkew);
		}
	}
}
