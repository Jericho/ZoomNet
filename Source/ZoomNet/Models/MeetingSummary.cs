using System;
using System.Text.Json.Serialization;

namespace ZoomNet.Models
{
	/// <summary>
	/// Summary information about a meeting.
	/// </summary>
	public class MeetingSummary : MeetingInfo
	{
		/// <summary>Gets or sets the meeting description.</summary>
		/// <remarks>
		/// The length of agenda gets truncated to 250 characters
		/// when you list all meetings for a user. To view the complete
		/// agenda of a meeting, retrieve details for a single meeting,
		/// use the Get a meeting API.
		/// </remarks>
		[JsonPropertyName("agenda")]
		public string Agenda { get; set; }

		/// <summary>
		/// Gets or sets the date and time when the meeting was created.
		/// </summary>
		[JsonPropertyName("created_at")]
		public DateTime CreatedOn { get; set; }

		/// <summary>
		/// Gets or sets the URL for the host to start the meeting.
		/// </summary>
		[JsonPropertyName("start_url")]
		public string StartUrl { get; set; }

		/// <summary>
		/// Gets or sets the personal meeting id.
		/// </summary>
		[JsonPropertyName("pmi")]
		public string PersonalMeetingId { get; set; }
	}
}
