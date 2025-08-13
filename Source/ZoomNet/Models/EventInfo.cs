using System;
using System.Text.Json.Serialization;

namespace ZoomNet.Models
{
	/// <summary>
	/// Info about an event.
	/// </summary>
	public class EventInfo
	{
		/// <summary>Gets or sets the event id.</summary>
		[JsonPropertyName("event_id")]
		public string Id { get; set; }

		/// <summary>Gets or sets the name of the event.</summary>
		[JsonPropertyName("name")]
		public string Name { get; set; }

		/// <summary>Gets or sets the event description.</summary>
		[JsonPropertyName("description")]
		public string Description { get; set; }

		/// <summary>Gets or sets the timezone.</summary>
		[JsonPropertyName("timezone")]
		public TimeZones Timezone { get; set; }

		/// <summary>Gets or sets the event type.</summary>
		[JsonPropertyName("event_type")]
		public EventType Type { get; set; }

		/// <summary>Gets or sets the access level.</summary>
		[JsonPropertyName("access_level")]
		public string AccessLevel { get; set; }

		/// <summary>Gets or sets the categories.</summary>
		[JsonPropertyName("categories")]
		public string[] Categories { get; set; }

		/// <summary>Gets or sets the tags.</summary>
		[JsonPropertyName("tags")]
		public string[] Tags { get; set; }

		/// <summary>Gets or sets the unique identifier of the hub.</summary>
		[JsonPropertyName("hub_id")]
		public string HubId { get; set; }

		/// <summary>Gets or sets the contact person's name for the event.</summary>
		[JsonPropertyName("contact_name")]
		public string ContactName { get; set; }

		/// <summary>Gets or sets the date and time when the lobby will open.</summary>
		[JsonPropertyName("lobby_start_time")]
		public DateTime LobbyStartTime { get; set; }

		/// <summary>Gets or sets the date and time when the lobby will close.</summary>
		[JsonPropertyName("lobby_end_time")]
		public DateTime LobbyEndTime { get; set; }

		/// <summary>Gets or sets the blocked countries.</summary>
		[JsonPropertyName("blocked_countries")]
		public string[] BlockedCountries { get; set; }

		/// <summary>Gets or sets the attendance type.</summary>
		[JsonPropertyName("attendance_type")]
		public EventAttendanceType AttendanceType { get; set; }

		/// <summary>Gets or sets the tag line displayed under the event detail page image.</summary>
		[JsonPropertyName("tagline")]
		public string TagLine { get; set; }
	}
}
