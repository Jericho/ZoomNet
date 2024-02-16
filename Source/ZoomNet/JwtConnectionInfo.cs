using System;

namespace ZoomNet
{
	/// <summary>
	/// Connect using JWT.
	/// </summary>
	[Obsolete("As of September 8, 2023, the JWT app type has been deprecated. Use Server-to-Server OAuth or OAuth apps to replace the functionality of all JWT apps in your account. See the JWT deprecation FAQ and migration guide for details.")]
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
