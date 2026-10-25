namespace ZoomNet.Models
{
	/// <summary>Authentication settings.</summary>
	public record AuthenticationSettings
	{
		/// <summary>Gets or sets a value indicating whether users must be authenticated.</summary>
		public bool RequireAuthentication { get; init; }

		/// <summary>Gets or sets the authentication options.</summary>
		public AuthenticationOptions[] AuthenticationOptions { get; init; }
	}
}
