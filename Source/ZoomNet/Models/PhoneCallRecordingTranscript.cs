using System;
using System.Text.Json.Serialization;

namespace ZoomNet.Models
{
	/// <summary>Phone call recording transcript.</summary>
	/// <remarks>Not documented by Zoom.</remarks>
	public class PhoneCallRecordingTranscript
	{
		/// <summary>Gets or sets the account ID.</summary>
		/// <remarks>Not documented by Zoom.</remarks>
		[JsonPropertyName("account_id")]
		public string AccountId { get; set; }

		/// <summary>Gets or sets the host ID.</summary>
		/// <remarks>Not documented by Zoom.</remarks>
		[JsonPropertyName("host_id")]
		public string HostId { get; set; }

		/// <summary>Gets or sets the meeting ID.</summary>
		/// <remarks>Not documented by Zoom.</remarks>
		[JsonPropertyName("meeting_id")]
		public string MeetingId { get; set; }

		/// <summary>Gets or sets the call recording end datetime.</summary>
		/// <remarks>Not documented by Zoom.</remarks>
		[JsonPropertyName("recording_end")]
		public DateTime EndDateTime { get; set; }

		/// <summary>Gets or sets the recording ID.</summary>
		/// <remarks>Not documented by Zoom.</remarks>
		[JsonPropertyName("recording_id")]
		public string RecordingId { get; set; }

		/// <summary>Gets or sets the call recording start datetime.</summary>
		/// <remarks>Not documented by Zoom.</remarks>
		[JsonPropertyName("recording_start")]
		public DateTime StartDateTime { get; set; }

		/// <summary>Gets or sets the call recording timeline.</summary>
		/// <remarks>Not documented by Zoom.</remarks>
		[JsonPropertyName("timeline")]
		public PhoneCallRecordingTranscriptTimelineFraction[] TimelineFractions { get; set; }

		/// <summary>Gets or sets the recording type.</summary>
		/// <remarks>Not documented by Zoom.<br/> Suspected to be an Enum, but available values unknown (apart from "zoom_transcript").</remarks>
		[JsonPropertyName("type")]
		public string Type { get; set; }

		/// <summary>Gets or sets the recording version.</summary>
		/// <remarks>Not documented by Zoom.</remarks>
		[JsonPropertyName("ver")]
		public int Version { get; set; }
	}
}
