using System.Text.Json.Serialization;

namespace ZoomNet.Models
{
	/// <summary>
	/// A session speaker.
	/// </summary>
	public class EventSessionSpeaker
	{
		/// <summary>Gets or sets the ID of the session speaker.</summary>
		[JsonPropertyName("speaker_id")]
		public string Id { get; set; }

		/// <summary>Gets or sets a value indicating whether the speaker has access to edit the session information.</summary>
		[JsonPropertyName("access_to_edit_session")]
		public bool CanEditSession { get; set; }

		/// <summary>Gets or sets a value indicating whether to show the speaker information under session details.</summary>
		[JsonPropertyName("show_in_session_detail")]
		public bool IsDisplayedInSessionDetails { get; set; }

		/// <summary>Gets or sets a value indicating whether the speaker can act as an alternative host for the session.</summary>
		[JsonPropertyName("has_alternative_host_permission")]
		public bool CanActAsAlternativeHost { get; set; }

		/// <summary>Gets or sets the role of the speaker.</summary>
		[JsonPropertyName("meeting_role")]
		public EventSpeakerRole Role { get; set; }

		/// <summary>Gets or sets the name of the speaker.</summary>
		[JsonPropertyName("name")]
		public string Name { get; set; }

		/// <summary>Gets or sets the company name of the speaker.</summary>
		[JsonPropertyName("company")]
		public string Company { get; set; }

		/// <summary>Gets or sets the job title of the speaker.</summary>
		[JsonPropertyName("title")]
		public string Title { get; set; }
	}
}
