using Newtonsoft.Json;
using System;

namespace ZoomNet.Models
{
	/// <summary>
	/// Details of participant feedback on Zoom meetings client.
	/// </summary>
	public class ClientFeedbackDetail
	{
		/// <summary>
		/// Gets or sets the participant's name.
		/// </summary>
		/// <value>The participant's name.</value>
		[JsonProperty(PropertyName = "participant_name")]
		public string Name { get; set; }

		/// <summary>
		/// Gets or sets the meeting id.
		/// </summary>
		/// <value>The meeting id.</value>
		[JsonProperty(PropertyName = "meeting_id")]
		public string MeetingId { get; set; }

		/// <summary>
		/// Gets or sets the time the feedback was submitted by the participant.
		/// </summary>
		/// <value>The time the feedback was submitted by the participant.</value>
		[JsonProperty(PropertyName = "time")]
		public DateTime Time { get; set; }

		/// <summary>
		/// Gets or sets the participant's email address.
		/// </summary>
		/// <value>The participants email.</value>
		[JsonProperty(PropertyName = "email")]
		public string Email { get; set; }
	}
}
