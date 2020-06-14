namespace ZoomNet.Models
{
	/// <summary>
	/// Authentication settings.
	/// </summary>
	public class AuthenticationSettings
	{
		/// <summary>
		/// Gets or sets a value indicating whether users must be authenticated.
		/// </summary>
		public bool RequireAuthentication { get; set; }

		/// <summary>
		/// Gets or sets the authentication options.
		/// </summary>
		public AuthenticationOptions[] AuthenticationOptions { get; set; }
	}
}
