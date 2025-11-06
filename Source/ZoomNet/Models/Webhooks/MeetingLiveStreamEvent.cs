using System.Text.Json.Serialization;

namespace ZoomNet.Models.Webhooks
{
	/// <summary>
	/// Represents an event related to a meeting live streaming.
	/// </summary>
	public abstract class MeetingLiveStreamEvent : MeetingInfoEvent
	{
		/// <summary>
		/// Gets or sets the email address of the user who started the live stream.
		/// </summary>
		[JsonPropertyName("operator")]
		public string Operator { get; set; }

		/// <summary>
		/// Gets or sets the user ID of the user who started the live stream.
		/// </summary>
		[JsonPropertyName("operator_id")]
		public string OperatorId { get; set; }

		/// <summary>
		/// Gets or sets the information about live stream.
		/// </summary>
		public LiveStreamingInfo StreamingInfo { get; set; }
	}
}
