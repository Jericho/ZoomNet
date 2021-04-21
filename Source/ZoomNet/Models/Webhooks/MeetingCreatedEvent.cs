using Newtonsoft.Json;

namespace ZoomNet.Models.Webhooks
{
	/// <summary>
	/// This event is triggered when a meeting is created.
	/// </summary>
	public class MeetingCreatedEvent : MeetingEvent
	{
		/// <summary>
		/// Gets or sets the email address of the user who created the meeting.
		/// </summary>
		[JsonProperty(PropertyName = "operator")]
		public string Operator { get; set; }

		/// <summary>
		/// Gets or sets the user ID of the operator who created the meeting.
		/// </summary>
		[JsonProperty(PropertyName = "operator_id")]
		public string OperatorId { get; set; }

		/// <summary>
		/// Gets or sets the operation (allowed values: all, single).
		/// </summary>
		[JsonProperty(PropertyName = "operation")]
		public string Operation { get; set; }
	}
}
