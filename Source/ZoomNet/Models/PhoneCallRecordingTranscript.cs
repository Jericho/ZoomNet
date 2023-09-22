using System;
using System.Text.Json.Serialization;

namespace ZoomNet.Models
{
	/// <summary>
	/// Phone call recording transcript.
	/// </summary>
	/// <remarks>Not documented by Zoom.</remarks>
	public class PhoneCallRecordingTranscript
	{
		/// <summary>Gets or sets the recording type.</summary>
		/// <value>The type of the phone call recording.</value>
		/// <remarks>
		/// Not documented by Zoom.<br/>
		/// Suspected to be an Enum, but available values unknown (apart from "zoom_transcript").
		/// </remarks>
		[JsonPropertyName("type")]
		public string Type { get; set; }

		/// <summary>Gets or sets the recording version.</summary>
		/// <value>The version of the phone call recording.</value>
		/// <remarks>Not documented by Zoom.</remarks>
		[JsonPropertyName("ver")]
		public int Version { get; set; }

		/// <summary>Gets or sets the recording ID.</summary>
		/// <value>The ID of the phone call recording.</value>
		/// <remarks>Not documented by Zoom.</remarks>
		[JsonPropertyName("recording_id")]
		public string RecordingId { get; set; }

		/// <summary>Gets or sets the meeting ID.</summary>
		/// <value>The ID of the meeting.</value>
		/// <remarks>Not documented by Zoom.</remarks>
		[JsonPropertyName("meeting_id")]
		public string MeetingId { get; set; }

		/// <summary>Gets or sets the account ID.</summary>
		/// <value>The account ID.</value>
		/// <remarks>Not documented by Zoom.</remarks>
		[JsonPropertyName("account_id")]
		public string AccountId { get; set; }

		/// <summary>Gets or sets the host ID.</summary>
		/// <value>The host ID.</value>
		/// <remarks>Not documented by Zoom.</remarks>
		[JsonPropertyName("host_id")]
		public string HostId { get; set; }

		/// <summary>Gets or sets the call recording start datetime.</summary>
		/// <value>The phone call recording start datetime.</value>
		/// <remarks>Not documented by Zoom.</remarks>
		[JsonPropertyName("recording_start")]
		public DateTime StartDateTime { get; set; }

		/// <summary>Gets or sets the call recording end datetime.</summary>
		/// <value>The phone call recording end datetime.</value>
		/// <remarks>Not documented by Zoom.</remarks>
		[JsonPropertyName("recording_end")]
		public DateTime EndDateTime { get; set; }

		/// <summary>Gets or sets the call recording timeline.</summary>
		/// <value>The phone call recording timeline.</value>
		/// <remarks>Not documented by Zoom.</remarks>
		[JsonPropertyName("timeline")]
		public PhoneCallRecordingTranscriptTimelineFraction[] TimelineFractions { get; set; }
	}
}
