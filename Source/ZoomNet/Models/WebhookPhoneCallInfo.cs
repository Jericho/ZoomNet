using System.Text.Json.Serialization;

namespace ZoomNet.Models
{
	/// <summary>
	/// Information about phone call that includes call id and two parties - caller and callee.
	/// </summary>
	public class WebhookPhoneCallInfo
	{
		/// <summary>
		/// Gets or sets the phone call unique id.
		/// </summary>
		[JsonPropertyName("call_id")]
		public string CallId { get; set; }

		/// <summary>
		/// Gets or sets the caller of the phone call (i.e. party who initiated the call).
		/// </summary>
		[JsonPropertyName("caller")]
		public PhoneCallParty Caller { get; set; }

		/// <summary>
		/// Gets or sets the callee of the phone call (i.e. party who received the call).
		/// </summary>
		[JsonPropertyName("callee")]
		public PhoneCallParty Callee { get; set; }
	}
}
