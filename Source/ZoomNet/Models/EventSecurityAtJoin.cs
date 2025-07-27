using System.Text.Json.Serialization;

namespace ZoomNet.Models
{
	/// <summary>
	/// Security options at the time of joining an event.
	/// </summary>
	public class EventSecurityAtJoin
	{
		/// <summary>Gets or sets a value indicating whether attendees will be required to authenticate with the email that was used at registration when joining.</summary>
		[JsonPropertyName("email_authentication")]
		public bool RequireEmailAuthentication { get; set; }

		/// <summary>Gets or sets a value indicating whether attendees will be required to verify a security code.</summary>
		[JsonPropertyName("security_code_verification")]
		public bool RequireSecurityCodeVerification { get; set; }
	}
}
