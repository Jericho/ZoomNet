using System.Text.Json.Serialization;

namespace ZoomNet.Models.Webhooks
{
	/// <summary>This event is triggered when a callee misses a call and receives a voicemail from the caller.</summary>
	public record PhoneVoicemailReceivedForAccessMemberEvent : Event
	{
		/// <summary>Gets or sets account id of the access member.</summary>
		[JsonPropertyName("account_id")]
		public string AccountId { get; init; }

		/// <summary>Gets or sets information about received voicemail.</summary>
		[JsonPropertyName("object")]
		public WebhookVoicemail Voicemail { get; init; }

		/// <summary>Gets or sets access member's extension type.</summary>
		public PhoneCallExtensionType AccessMemberExtensionType { get; init; }

		/// <summary>Gets or sets access member's id.</summary>
		/// <remarks>
		/// Depending on <see cref="AccessMemberExtensionType"/> this value can be one of the following:
		/// - <see cref="PhoneCallExtensionType.User"/>: callee user id.
		/// - <see cref="PhoneCallExtensionType.CommonAreaPhone"/>: common area id.
		/// </remarks>
		public string AccessMemberId { get; init; }
	}
}
