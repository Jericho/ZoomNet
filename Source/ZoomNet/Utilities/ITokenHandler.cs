namespace ZoomNet.Utilities
{
	/// <summary>
	/// Interface for token handlers.
	/// </summary>
	internal interface ITokenHandler
	{
		/// <summary>
		/// Refresh the access token if the previous one has expired.
		/// </summary>
		/// <param name="forceRefresh">Indicates if the token should be refreshes even if it's not expired.</param>
		/// <returns>The refreshed access token or the current token if we determined that it didn't need to be refreshed.</returns>
		string RefreshTokenIfNecessary(bool forceRefresh);
	}
}
