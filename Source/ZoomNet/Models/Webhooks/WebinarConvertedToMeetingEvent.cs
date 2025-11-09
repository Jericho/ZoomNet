using System.Text.Json.Serialization;

namespace ZoomNet.Models.Webhooks
{
	/// <summary>
	/// This event is triggered every time one of your app users or account users converts a webinar into a meeting.
	/// </summary>
	public class WebinarConvertedToMeetingEvent : Event
	{
		/// <summary>
		/// Gets or sets the account id of the user who converted the webinar into a meeting.
		/// </summary>
		[JsonPropertyName("account_id")]
		public string AccountId { get; set; }

		/// <summary>
		/// Gets or sets the email address of the user who converted the webinar into a meeting.
		/// </summary>
		[JsonPropertyName("operator")]
		public string Operator { get; set; }

		/// <summary>
		/// Gets or sets id of the user who converted the webinar into a meeting.
		/// </summary>
		[JsonPropertyName("operator_id")]
		public string OperatorId { get; set; }

		/// <summary>
		/// Gets or sets information about the meeting converted from a webinar.
		/// </summary>
		[JsonPropertyName("object")]
		public MeetingInfo Meeting { get; set; }
	}
}
