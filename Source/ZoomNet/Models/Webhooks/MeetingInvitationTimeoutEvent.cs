using System.Text.Json.Serialization;

namespace ZoomNet.Models.Webhooks
{
	/// <summary>
	/// This event is triggered every time an invitation to join a meeting or video call to one or more recipients times out.
	/// </summary>
	/// <remarks>
	/// This event is only suitable for triggering with the Zoom client.
	/// </remarks>
	public class MeetingInvitationTimeoutEvent : MeetingInvitationEvent
	{
		/// <summary>
		/// Gets or sets the email address of the user who missed the meeting invitation.
		/// </summary>
		[JsonPropertyName("operator")]
		public string Operator { get; set; }

		/// <summary>
		/// Gets or sets id of the user who missed the meeting invitation.
		/// </summary>
		[JsonPropertyName("operator_id")]
		public string OperatorId { get; set; }
	}
}
