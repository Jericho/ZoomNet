using System.Text.Json.Serialization;

namespace ZoomNet.Models
{
	/// <summary>
	/// The user who sent/received the SMS (as provided in webhook events).
	/// </summary>
	public class WebhookSmsParticipant : SmsParticipantBase
	{
		/// <summary>
		/// Gets or sets sender's/receiver's id.
		/// </summary>
		[JsonPropertyName("id")]
		public string Id { get; set; }

		/// <summary>
		/// Gets or sets sender's/receiver's type.
		/// </summary>
		[JsonPropertyName("type")]
		public SmsParticipantOwnerType? Type { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether this is the owner of the message.
		/// </summary>
		[JsonPropertyName("is_message_owner")]
		public bool? IsMessageOwner { get; set; }
	}
}
