using System;
using System.Threading;
using System.Threading.Tasks;

namespace ZoomNet.Utilities
{
	/// <summary>
	/// Store token in memory.
	/// </summary>
	internal class MemoryTokenRepository : ITokenRepository
	{
		private static readonly SemaphoreSlim _lock = new(1, 1);

		private string _refreshToken;
		private string _accessToken;
		private DateTime _tokenExpiration;

		public MemoryTokenRepository(string refreshToken, string accessToken, DateTime tokenExpiration)
		{
			_refreshToken = refreshToken;
			_accessToken = accessToken;
			_tokenExpiration = tokenExpiration;
		}

		/// <inheritdoc/>
		public Task<(string RefreshToken, string AccessToken, DateTime TokenExpiration)> GetTokenInfoAsync(CancellationToken cancellationToken)
		{
			return Task.FromResult((_refreshToken, _accessToken, _tokenExpiration));
		}

		/// <inheritdoc/>
		public async Task AcquireLeaseAsync(CancellationToken cancellationToken)
		{
			// 2.5 seconds should more than enough time for previous lock to be released
			var lockAcquired = await _lock.WaitAsync(2500, cancellationToken).ConfigureAwait(false);
			if (!lockAcquired) throw new TimeoutException("Unable to acquire an exclusive lease for the purpose of updating token information");
		}

		/// <inheritdoc/>
		public Task SaveTokenInfoAsync(string refreshToken, string accessToken, DateTime tokenExpiration, CancellationToken cancellationToken = default)
		{
			_refreshToken = refreshToken;
			_accessToken = accessToken;
			_tokenExpiration = tokenExpiration;

			return Task.CompletedTask;
		}

		/// <inheritdoc/>
		public Task ReleaseLeaseAsync(CancellationToken cancellationToken)
		{
			_lock.Release();
			return Task.CompletedTask;
		}
	}
}
