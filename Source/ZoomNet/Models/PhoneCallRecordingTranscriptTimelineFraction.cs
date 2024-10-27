using System;
using System.Text.Json.Serialization;

namespace ZoomNet.Models
{
	/// <summary>
	/// Phone call recording transcript timeline fraction.
	/// </summary>
	/// <remarks>Not documented by Zoom.</remarks>
	public class PhoneCallRecordingTranscriptTimelineFraction
	{
		/// <summary>Gets or sets the transcribed text.</summary>
		/// <value>The phone call recording transcript fraction text.</value>
		/// <remarks>Not documented by Zoom.</remarks>
		[JsonPropertyName("text")]
		public string Text { get; set; }

		/// <summary>Gets or sets the transcribed text start timespan.</summary>
		/// <value>The phone call recording transcript fraction start timespan.</value>
		/// <remarks>Not documented by Zoom.</remarks>
		[JsonPropertyName("ts")]
		public TimeSpan StartTimeSpan { get; set; }

		/// <summary>Gets or sets the transcribed text end timespan.</summary>
		/// <value>The phone call recording transcript fraction end timespan.</value>
		/// <remarks>Not documented by Zoom.</remarks>
		[JsonPropertyName("end_ts")]
		public TimeSpan EndTimeSpan { get; set; }

		/// <summary>Gets or sets the transcribed text users.</summary>
		/// <value>The phone call recording transcript fraction users.</value>
		/// <remarks>
		/// Not documented by Zoom.<br/>
		/// Might be empty.
		/// </remarks>
		[JsonPropertyName("users")]
		public PhoneCallRecordingTranscriptTimelineUser[] Users { get; set; }
	}
}
