using System;
using System.Diagnostics;
using System.Text.Json.Serialization;

namespace ZoomNet.Models
{
	/// <summary>Represents extended information about the meeting.</summary>
	public record MeetingInfo : MeetingBasicInfo
	{
		/// <summary>Gets or sets the meeting's duration in minutes.</summary>
		[JsonPropertyName("duration")]
		public int Duration { get; set; }

		/// <summary>Gets or sets the meeting's start time.</summary>
		[JsonPropertyName("start_time")]
		public DateTime StartTime { get; set; }

		/// <summary>Gets or sets the meeting's timezone.</summary>
		[JsonPropertyName("timezone")]
		public TimeZones Timezone { get; set; }

		/// <summary>Gets or sets the meeting's type.</summary>
		[JsonPropertyName("type")]
		public MeetingType Type { get; set; }

		/// <summary>Gets or sets the meeting's join time (which, I believe, is the same as StartTime).</summary>
		/// <remarks>
		/// The reason why I added this property on this model is to be able to handle scenarios where the
		/// JSON from Zoom API contains a node called 'join_time' rather than the expected start_time.
		/// One such example is the "meeting.alert" webhook.
		/// </remarks>
		[JsonPropertyName("join_time")]
#if NET5_0_OR_GREATER
		[JsonIgnore(Condition = JsonIgnoreCondition.WhenWriting)]
#endif
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public DateTime? JoinTime
		{
			get { return StartTime; }
			set { if (value.HasValue) StartTime = value.Value; }
		}
	}
}
