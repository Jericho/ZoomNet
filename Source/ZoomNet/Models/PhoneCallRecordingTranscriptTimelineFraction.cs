using System;
using System.Text.Json.Serialization;

namespace ZoomNet.Models
{
	/// <summary>Phone call recording transcript timeline fraction.</summary>
	/// <remarks>Not documented by Zoom.</remarks>
	public class PhoneCallRecordingTranscriptTimelineFraction
	{
		/// <summary>Gets or sets the transcribed text end timespan.</summary>
		/// <remarks>Not documented by Zoom.</remarks>
		[JsonPropertyName("end_ts")]
		public TimeSpan EndTimeSpan { get; set; }

		/// <summary>Gets or sets the transcribed text.</summary>
		/// <remarks>Not documented by Zoom.</remarks>
		[JsonPropertyName("text")]
		public string Text { get; set; }

		/// <summary>Gets or sets the transcribed text start timespan.</summary>
		/// <remarks>Not documented by Zoom.</remarks>
		[JsonPropertyName("ts")]
		public TimeSpan StartTimeSpan { get; set; }

		/// <summary>Gets or sets the transcribed text users.</summary>
		/// <remarks>Not documented by Zoom.<br/> Might be empty.</remarks>
		[JsonPropertyName("users")]
		public PhoneCallRecordingTranscriptTimelineUser[] Users { get; set; }
	}
}
