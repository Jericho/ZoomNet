using System;
using System.Text.Json.Serialization;

namespace ZoomNet.Models
{
	/// <summary>
	/// Summary information about a webinar.
	/// </summary>
	public class WebinarSummary : WebinarInfo
	{
		/// <summary>
		/// Gets or sets the webinar agenda.
		/// </summary>
		[JsonPropertyName("agenda")]
		public string Agenda { get; set; }

		/// <summary>
		/// Gets or sets the date and time when the meeting was created.
		/// </summary>
		[JsonPropertyName("created_at")]
		public DateTime CreatedOn { get; set; }

		/// <summary>
		/// Gets or sets the URL to join the webinar.
		/// </summary>
		[JsonPropertyName("join_url")]
		public string JoinUrl { get; set; }
	}
}
