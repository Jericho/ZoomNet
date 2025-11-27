using System.Text.Json.Serialization;

namespace ZoomNet.Models
{
	/// <summary>
	/// SMS history participant owner information as provided in webhook events.
	/// </summary>
	public class WebhookSmsOwner : SmsOwnerBase
	{
		/// <summary>
		/// Gets or sets the owner's id in a case when <see cref="SmsOwnerBase.Type"/> is
		/// <see cref="SmsParticipantOwnerType.CallQueue"/> or <see cref="SmsParticipantOwnerType.AutoReceptionist"/>.
		/// </summary>
		[JsonPropertyName("team_id")]
		public string TeamId { get; set; }

		/// <summary>
		/// Gets or sets the user id of the call queue or auto receptionist member who sent the SMS
		/// in a case when <see cref="SmsOwnerBase.Type"/> is <see cref="SmsParticipantOwnerType.CallQueue"/> or
		/// <see cref="SmsParticipantOwnerType.AutoReceptionist"/>.
		/// </summary>
		[JsonPropertyName("sms_sender_user_id")]
		public string SenderUserId { get; set; }
	}
}
