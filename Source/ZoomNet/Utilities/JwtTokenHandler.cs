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

		private readonly string _apiKey;
		private readonly string _apiSecret;
		private readonly TimeSpan _clockSkew = TimeSpan.FromMinutes(5);
		private readonly TimeSpan _tokenLifeSpan = TimeSpan.FromMinutes(30);
		private readonly DateTime _jwtTokenExpiration = DateTime.MinValue;

		private string _jwtToken;

		public JwtTokenHandler(string apiKey, string apiSecret, TimeSpan? tokenLifeSpan = null, TimeSpan? clockSkew = null)
		{
			_apiKey = apiKey;
			_apiSecret = apiSecret;
			_tokenLifeSpan = tokenLifeSpan.GetValueOrDefault(TimeSpan.FromMinutes(30));
			_clockSkew = clockSkew.GetValueOrDefault(TimeSpan.FromMinutes(5));
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
						var jwtPayload = new Dictionary<string, object>()
						{
							{ "iss", _apiKey },
							{ "exp", DateTime.UtcNow.Add(_tokenLifeSpan).ToUnixTime() }
						};
						_jwtToken = JWT.Encode(jwtPayload, Encoding.ASCII.GetBytes(_apiSecret), JwsAlgorithm.HS256);
					}
				}
			}
		}

		private bool TokenIsExpired()
		{
			return _jwtTokenExpiration <= DateTime.UtcNow.Add(_clockSkew);
		}
	}
}
