using System.Text.Json.Serialization;

namespace ZoomNet.Models
{
	/// <summary>
	/// Represents phone voicemail information (as provided in webhook events).
	/// </summary>
	public class WebhookVoicemail : VoicemailBase
	{
		/// <summary>
		/// Gets or sets callee's id.
		/// </summary>
		/// <remarks>
		/// Depending on <see cref="CalleeExtensionType"/> this value can be one of the following:
		/// - <see cref="PhoneCallExtensionType.User"/>: callee user id (same value as <see cref="CalleeUserId"/>).
		/// - <see cref="PhoneCallExtensionType.CallQueue"/>: call queue group id.
		/// - <see cref="PhoneCallExtensionType.AutoReceptionist"/>: auto receptionist id.
		/// - <see cref="PhoneCallExtensionType.SharedLineGroup"/>: shared line group id.
		/// </remarks>
		[JsonPropertyName("callee_id")]
		public string CalleeId { get; set; }

		/// <summary>
		/// Gets or sets callee's extension type.
		/// </summary>
		[JsonPropertyName("callee_extension_type")]
		public PhoneCallExtensionType? CalleeExtensionType { get; set; }

		/// <summary>
		/// Gets or sets callee's DID (direct inward dial) number.
		/// </summary>
		[JsonPropertyName("callee_did_number")]
		public string CalleeDidNumber { get; set; }

		/// <summary>
		/// Gets or sets callee's user id.
		/// </summary>
		[JsonPropertyName("callee_user_id")]
		public string CalleeUserId { get; set; }

		/// <summary>
		/// Gets or sets caller's DID (direct inward dial) number.
		/// </summary>
		[JsonPropertyName("caller_did_number")]
		public string CallerDidNumber { get; set; }

		/// <summary>
		/// Gets or sets caller's user id.
		/// </summary>
		[JsonPropertyName("caller_user_id")]
		public string CallerUserId { get; set; }
	}
}
