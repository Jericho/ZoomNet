using System.Text.Json.Serialization;

namespace ZoomNet.Models
{
	/// <summary>
	/// SIP account information.
	/// </summary>
	public class SipAccount
	{
		/// <summary>
		/// Gets or sets the authorization id of the SIP account.
		/// </summary>
		[JsonPropertyName("authorization_id")]
		public string AuthorizationId { get; set; }

		/// <summary>
		/// Gets or sets the outbound proxy.
		/// </summary>
		[JsonPropertyName("outbound_proxy")]
		public string OutboundProxy { get; set; }

		/// <summary>
		/// Gets or sets the password.
		/// </summary>
		[JsonPropertyName("password")]
		public string Password { get; set; }

		/// <summary>
		/// Gets or sets the secondary outbound proxy.
		/// </summary>
		[JsonPropertyName("secondary_outbound_proxy")]
		public string SecondaryOutboundProxy { get; set; }

		/// <summary>
		/// Gets or sets the SIP domain.
		/// </summary>
		[JsonPropertyName("sip_domain")]
		public string SipDomain { get; set; }

		/// <summary>
		/// Gets or sets the user name of the SIP account.
		/// </summary>
		[JsonPropertyName("user_name")]
		public string UserName { get; set; }

		/// <summary>
		/// Gets or sets shared line information.
		/// </summary>
		[JsonPropertyName("shared_line")]
		public SharedLine SharedLine { get; set; }
	}
}
