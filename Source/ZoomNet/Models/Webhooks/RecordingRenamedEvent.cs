using System;
using System.Text.Json.Serialization;

namespace ZoomNet.Models.Webhooks
{
	/// <summary>
	/// This event is triggered every time the title of a cloud recording is changed.
	/// </summary>
	public class RecordingRenamedEvent : RecordingEvent
	{
		/// <summary>
		/// Gets or sets the email address of the user who renamed the recording.
		/// </summary>
		[JsonPropertyName("operator")]
		public string Operator { get; set; }

		/// <summary>
		/// Gets or sets the id of the user who renamed the recording.
		/// </summary>
		[JsonPropertyName("operator_id")]
		public string OperatorId { get; set; }

		/// <summary>
		/// Gets or sets the timestamp when user renamed the meeting or webinar recording.
		/// </summary>
		public DateTime UpdatedOn { get; set; }

		/// <summary>
		/// Gets or sets id of the recorded meeting or webinar.
		/// </summary>
		public long Id { get; set; }

		/// <summary>
		/// Gets or sets the uuid of the recorded meeting or webinar.
		/// </summary>
		public string Uuid { get; set; }

		/// <summary>
		/// Gets or sets id of the user set as the host of the meeting or webinar.
		/// </summary>
		public string HostId { get; set; }

		/// <summary>
		/// Gets or sets the type of the recorded meeting or webinar.
		/// </summary>
		public RecordingType Type { get; set; }

		/// <summary>
		/// Gets or sets the previous recording title.
		/// </summary>
		public string OldTitle { get; set; }

		/// <summary>
		/// Gets or sets the updated recodring title.
		/// </summary>
		public string NewTitle { get; set; }
	}
}
