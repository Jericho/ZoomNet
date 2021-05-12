using Newtonsoft.Json;
using System;

namespace ZoomNet.Models.Webhooks
{
	/// <summary>
	/// This event is triggered when a meeting live stream stopped.
	/// </summary>
	public class MeetingLiveStreamStoppedEvent : MeetingEvent
	{
		/// <summary>
		/// Gets or sets the email address of the user who stopped the live stream.
		/// </summary>
		[JsonProperty(PropertyName = "operator")]
		public string Operator { get; set; }

		/// <summary>
		/// Gets or sets the user ID of the user who stopped the live stream.
		/// </summary>
		[JsonProperty(PropertyName = "operator_id")]
		public string OperatorId { get; set; }

		/// <summary>
		/// Gets or sets the date and time at which the live stream stopped.
		/// </summary>
		[JsonProperty(PropertyName = "date_time")]
		public DateTime StoppedOn { get; set; }

		/// <summary>
		/// Gets or sets the information about live stream.
		/// </summary>
		[JsonProperty(PropertyName = "live_streaming")]
		public LiveStreamingInfo StreamingInfo { get; set; }
	}
}
