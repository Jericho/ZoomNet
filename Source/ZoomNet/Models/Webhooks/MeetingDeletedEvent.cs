using Newtonsoft.Json;

namespace ZoomNet.Models.Webhooks
{
	/// <summary>
	/// This event is triggered when a meeting is deleted.
	/// </summary>
	public class MeetingDeletedEvent : MeetingEvent
	{
		/// <summary>
		/// Gets or sets the email address of the user who deleted the meeting.
		/// </summary>
		[JsonProperty(PropertyName = "operator")]
		public string Operator { get; set; }

		/// <summary>
		/// Gets or sets the user ID of the operator who deleted the meeting.
		/// </summary>
		[JsonProperty(PropertyName = "operator_id")]
		public string OperatorId { get; set; }
	}
}
