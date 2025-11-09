using System.Collections.Generic;
using System.Text.Json.Serialization;
using ZoomNet.Json;

namespace ZoomNet.Models
{
	/// <summary>
	/// Meeting participant feedback provided at the end of the meeting.
	/// </summary>
	public class MeetingParticipantFeedback
	{
		/// <summary>
		/// Gets or sets a value indicating whether the participant is satisfied.
		/// </summary>
		[JsonPropertyName("satisfied")]
		public bool Satisfied { get; set; }

		/// <summary>
		/// Gets or sets the participant's comments.
		/// </summary>
		/// <remarks>
		/// Should be null if participant is satisfied.
		/// </remarks>
		[JsonPropertyName("comments")]
		public string Comments { get; set; }

		/// <summary>
		/// Gets or sets the feedback details.
		/// </summary>
		/// <remarks>
		/// Should be null if participant is satisfied.
		/// </remarks>
		[JsonPropertyName("feedback_details")]
		[JsonConverter(typeof(MeetingParticipantFeedbackDetailsConverter))]
		public KeyValuePair<string, string>[] Details { get; set; }
	}
}
