namespace ZoomNet
{
	/// <summary>
	/// Connect using JWT.
	/// </summary>
	public class JwtConnectionInfo : IConnectionInfo
	{
		/// <summary>
		/// Gets the API Key.
		/// </summary>
		public string ApiKey { get; }

		/// <summary>
		/// Gets the API Secret.
		/// </summary>
		public string ApiSecret { get; }

		/// <summary>
		/// Initializes a new instance of the <see cref="JwtConnectionInfo"/> class.
		/// </summary>
		/// <param name="apiKey">Your JWT app API Key.</param>
		/// <param name="apiSecret">Your JWT app API Secret.</param>
		public JwtConnectionInfo(string apiKey, string apiSecret)
		{
			ApiKey = apiKey;
			ApiSecret = apiSecret;
		}
	}
}
