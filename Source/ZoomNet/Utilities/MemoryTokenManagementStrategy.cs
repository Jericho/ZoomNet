using System;
using System.Threading;
using System.Threading.Tasks;

namespace ZoomNet.Utilities
{
	/// <summary>
	/// Store token in memory.
	/// </summary>
	internal class MemoryTokenManagementStrategy : ITokenManagementStrategy
	{
		private static readonly SemaphoreSlim _lock = new(1, 1);

		private string _refreshToken;
		private string _accessToken;
		private DateTime _tokenExpiration;
		private int _tokenIndex;

		public MemoryTokenManagementStrategy(string refreshToken, string accessToken, DateTime tokenExpiration, int tokenIndex)
		{
			_refreshToken = refreshToken;
			_accessToken = accessToken;
			_tokenExpiration = tokenExpiration;
			_tokenIndex = tokenIndex;
		}

		/// <inheritdoc/>
		public Task<(string RefreshToken, string AccessToken, DateTime TokenExpiration, int TokenIndex)> GetTokenInfoAsync(CancellationToken cancellationToken)
		{
			return Task.FromResult((_refreshToken, _accessToken, _tokenExpiration, _tokenIndex));
		}

		/// <inheritdoc/>
		public async Task AcquireLeaseAsync(CancellationToken cancellationToken)
		{
			// 2.5 seconds should more than enough time for previous lock to be released
			var lockAcquired = await _lock.WaitAsync(2500).ConfigureAwait(false);
			if (!lockAcquired) throw new TimeoutException("Unable to acquire an exclusive lease for the purpose of updating token information");
		}

		/// <inheritdoc/>
		public Task SaveTokenInfoAsync(string refreshToken, string accessToken, DateTime tokenExpiration, int tokenIndex, CancellationToken cancellationToken = default)
		{
			_refreshToken = refreshToken;
			_accessToken = accessToken;
			_tokenExpiration = tokenExpiration;
			_tokenIndex = tokenIndex;

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
