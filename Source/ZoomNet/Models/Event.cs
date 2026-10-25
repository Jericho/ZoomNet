using System;
using System.Text.Json.Serialization;

namespace ZoomNet.Models
{
	/// <summary>An event.</summary>
	public abstract record Event
	{
		/// <summary>Gets or sets the access level.</summary>
		[JsonPropertyName("access_level")]
		public string AccessLevel { get; init; }

		/// <summary>Gets or sets the categories.</summary>
		[JsonPropertyName("categories")]
		public string[] Categories { get; init; }

		/// <summary>Gets or sets the event description.</summary>
		[JsonPropertyName("description")]
		public string Description { get; init; }

		/// <summary>Gets or sets the event id.</summary>
		[JsonPropertyName("event_id")]
		public string Id { get; init; }

		/// <summary>Gets or sets the event type.</summary>
		[JsonPropertyName("event_type")]
		public EventType Type { get; init; }

		/// <summary>Gets or sets the name of the event.</summary>
		[JsonPropertyName("name")]
		public string Name { get; init; }

		/// <summary>Gets or sets the tags.</summary>
		[JsonPropertyName("tags")]
		public string[] Tags { get; init; }

		/// <summary>Gets or sets the timezone.</summary>
		[JsonPropertyName("timezone")]
		public TimeZones Timezone { get; init; }

		/// <summary>Gets or sets the status.</summary>
		public EventStatus Status { get; init; }

		/// <summary>Gets or sets the attendance type.</summary>
		[JsonPropertyName("attendance_type")]
		public EventAttendanceType AttendanceType { get; init; }

		/// <summary>Gets or sets the blocked countries.</summary>
		[JsonPropertyName("blocked_countries")]
		public string[] BlockedCountries { get; init; }

		/// <summary>Gets or sets the contact person's name for the event.</summary>
		[JsonPropertyName("contact_name")]
		public string ContactName { get; init; }

		/// <summary>Gets or sets the unique identifier of the hub.</summary>
		[JsonPropertyName("hub_id")]
		public string HubId { get; init; }

		/// <summary>Gets or sets the date and time when the lobby will close.</summary>
		[JsonPropertyName("lobby_end_time")]
		public DateTime LobbyEndTime { get; init; }

		/// <summary>Gets or sets the date and time when the lobby will open.</summary>
		[JsonPropertyName("lobby_start_time")]
		public DateTime LobbyStartTime { get; init; }

		/// <summary>Gets or sets the tag line displayed under the event detail page image.</summary>
		[JsonPropertyName("tagline")]
		public string TagLine { get; init; }
	}
}
