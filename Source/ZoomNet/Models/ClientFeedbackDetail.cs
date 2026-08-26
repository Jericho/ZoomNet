using System;
using System.Text.Json.Serialization;

namespace ZoomNet.Models
{
	/// <summary>Details of participant feedback on Zoom meetings client.</summary>
	public class ClientFeedbackDetail
	{
		/// <summary>Gets or sets the participant's email address.</summary>
		[JsonPropertyName("email")]
		public string Email { get; set; }

		/// <summary>Gets or sets the meeting id.</summary>
		[JsonPropertyName("meeting_id")]
		public string MeetingId { get; set; }

		/// <summary>Gets or sets the participant's name.</summary>
		[JsonPropertyName("participant_name")]
		public string Name { get; set; }

		/// <summary>Gets or sets the time the feedback was submitted by the participant.</summary>
		[JsonPropertyName("time")]
		public DateTime Time { get; set; }
	}
}
