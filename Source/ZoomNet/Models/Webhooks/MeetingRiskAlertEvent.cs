using System.Text.Json.Serialization;

namespace ZoomNet.Models.Webhooks
{
	/// <summary>
	/// This event is triggered every time someone posts a Zoom meeting link to a social media account.
	/// </summary>
	public class MeetingRiskAlertEvent : Event
	{
		/// <summary>
		/// Gets or sets the account id of the user who created the meeting.
		/// </summary>
		[JsonPropertyName("account_id")]
		public string AccountId { get; set; }

		/// <summary>
		/// Gets or sets the meeting information.
		/// </summary>
		[JsonPropertyName("object")]
		public MeetingInfo Meeting { get; set; }

		/// <summary>
		/// Gets or sets the information about at-risk meeting notifier.
		/// </summary>
		public MeetingAtRiskDetails ArmnDetails { get; set; }
	}
}
