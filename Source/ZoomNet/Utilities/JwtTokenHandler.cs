using Jose;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ZoomNet.Utilities
{
	/// <summary>
	/// Handler to ensure requests to the Zoom API include a valid JWT token.
	/// </summary>
	/// <seealso cref="Pathoschild.Http.Client.Extensibility.IHttpFilter" />
	/// <seealso cref="ITokenHandler" />
	internal class JwtTokenHandler : ITokenHandler
	{
		public IConnectionInfo ConnectionInfo
		{
			get => _connectionInfo;
		}

		private static readonly ReaderWriterLockSlim _lock = new();

		private readonly JwtConnectionInfo _connectionInfo;
		private readonly TimeSpan _clockSkew;
		private readonly TimeSpan _tokenLifeSpan;

		private string _jwtToken;
		private DateTime _tokenExpiration;

		public JwtTokenHandler(JwtConnectionInfo connectionInfo, TimeSpan? tokenLifeSpan = null, TimeSpan? clockSkew = null)
		{
			if (connectionInfo == null) throw new ArgumentNullException(nameof(connectionInfo));
			if (string.IsNullOrEmpty(connectionInfo.ApiKey)) throw new ArgumentNullException("connectionInfo.ApiKey");
			if (string.IsNullOrEmpty(connectionInfo.ApiSecret)) throw new ArgumentNullException("connectionInfo.ApiSecret");

			_connectionInfo = connectionInfo;
			_tokenLifeSpan = tokenLifeSpan.GetValueOrDefault(TimeSpan.FromMinutes(30));
			_clockSkew = clockSkew.GetValueOrDefault(TimeSpan.FromMinutes(5));
			_tokenExpiration = DateTime.MinValue;
		}

		public Task<(string RefreshToken, string AccessToken, DateTime TokenExpiration, int TokenIndex)> GetTokenInfoAsync(bool forceRefresh, (string RefreshToken, string AccessToken, DateTime TokenExpiration, int TokenIndex) currentTokenInfo, CancellationToken cancellationToken = default)
		{
			_lock.EnterUpgradeableReadLock();

			if (forceRefresh || TokenIsExpired())
			{
				try
				{
					_lock.EnterWriteLock();

					if (forceRefresh || TokenIsExpired())
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
				finally
				{
					_lock.ExitWriteLock();
				}
			}

			_lock.ExitUpgradeableReadLock();

			return Task.FromResult<(string, string, DateTime, int)>((null, _jwtToken, _tokenExpiration, 0));
		}

		private bool TokenIsExpired()
		{
			return _tokenExpiration <= DateTime.UtcNow.Add(_clockSkew);
		}
	}
}
