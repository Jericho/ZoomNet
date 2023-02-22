using System;
using System.Threading;
using System.Threading.Tasks;

namespace ZoomNet
{
	/// <summary>
	/// Interface for token repository.
	/// </summary>
	public interface ITokenRepository
	{
		/// <summary>
		/// Retrieve token information from your central repository.
		/// </summary>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>The information about the previously persisted OAuth token.</returns>
		Task<(string RefreshToken, string AccessToken, DateTime TokenExpiration, int TokenIndex)> GetTokenInfoAsync(CancellationToken cancellationToken);

		/// <summary>
		/// Acquire a lease prior to renewing a token to prevent other instances from renewing the token at the same time.
		/// </summary>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>The async task.</returns>
		Task AcquireLeaseAsync(CancellationToken cancellationToken);

		/// <summary>
		/// Persist token information to your central repository.
		/// </summary>
		/// <param name="refreshToken">The refresh token.</param>
		/// <param name="accessToken">The access token.</param>
		/// <param name="tokenExpiration">The expiration date of the token.</param>
		/// <param name="tokenIndex">The token index.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>The async task.</returns>
		Task SaveTokenInfoAsync(string refreshToken, string accessToken, DateTime tokenExpiration, int tokenIndex, CancellationToken cancellationToken = default);

		/// <summary>
		/// Release the lock that was previously acquired.
		/// </summary>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>The async task.</returns>
		Task ReleaseLeaseAsync(CancellationToken cancellationToken);
	}
}
