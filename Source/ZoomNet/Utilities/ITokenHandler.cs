using System;
using System.Threading;
using System.Threading.Tasks;

namespace ZoomNet.Utilities
{
	/// <summary>
	/// Interface for token handlers.
	/// </summary>
	internal interface ITokenHandler
	{
		/// <summary>
		/// Gets the connection information.
		/// </summary>
		IConnectionInfo ConnectionInfo { get; }

		/// <summary>
		/// Get the token token information.
		/// This method is responsible for refreshing the access token if the previous one has expired.
		/// </summary>
		/// <param name="forceRefresh">Indicates if the token should be refreshes even if it's not expired.</param>
		/// <param name="previousTokenInfo">The token information that was used in the previous API call.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>The refreshed token information or the current token information if we determined that it didn't need to be refreshed.</returns>
		Task<(string RefreshToken, string AccessToken, DateTime TokenExpiration, int TokenIndex)> GetTokenInfoAsync(bool forceRefresh, (string RefreshToken, string AccessToken, DateTime TokenExpiration, int TokenIndex) previousTokenInfo, CancellationToken cancellationToken = default);
	}
}
