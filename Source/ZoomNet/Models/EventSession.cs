using System;
using System.Text.Json.Serialization;

namespace ZoomNet.Models
{
	/// <summary>
	/// An event session.
	/// </summary>
	public class EventSession
	{
		/// <summary>Gets or sets the session id.</summary>
		[JsonPropertyName("session_id")]
		public string Id { get; set; }

		/// <summary>Gets or sets the name of the session.</summary>
		[JsonPropertyName("name")]
		public string Name { get; set; }

		/// <summary>Gets or sets the event description.</summary>
		[JsonPropertyName("description")]
		public string Description { get; set; }

		/// <summary>Gets or sets the timezone.</summary>
		[JsonPropertyName("timezone")]
		public TimeZones Timezone { get; set; }

		/// <summary>Gets or sets the event type.</summary>
		[JsonPropertyName("type")]
		public EventSessionType Type { get; set; }

		/// <summary>Gets or sets the meeting number.</summary>
		/// <remarks>Applicable if the session type is of meeting ( i.e type = 0). This ID is null for an unpublished event.</remarks>
		public long? MeetingId { get; set; }

		/// <summary>Gets or sets the webinar number.</summary>
		/// <remarks>Applicable if the session type is of webinar ( i.e type = 2). This ID is null for an unpublished event.</remarks>
		public long? WebinarId { get; set; }

		/// <summary>Gets or sets the start date and time.</summary>
		[JsonPropertyName("start_time")]
		public DateTime StartTime { get; set; }

		/// <summary>Gets or sets the end date and time.</summary>
		[JsonPropertyName("end_time")]
		public DateTime EndTime { get; set; }

		/// <summary>Gets or sets the speakers.</summary>
		/// <remarks>
		/// Speakers are the people who will be presenting during the session.
		/// The speakers join as attendees in a meeting session and as panelists in a webinar session.
		/// </remarks>
		[JsonPropertyName("session_speakers")]
		public EventSessionSpeaker[] Speakers { get; set; }

		/// <summary>Gets or sets a value indicating whether the session is featured.</summary>
		[JsonPropertyName("featured")]
		public bool IsFeatured { get; set; }

		/// <summary>Gets or sets a value indicating whether the session is visible in the landing page.</summary>
		[JsonPropertyName("visible_in_landing_page")]
		public bool IsVisibleInLandingPage { get; set; }

		/// <summary>Gets or sets a value indicating whether the session is featured in the event lobby.</summary>
		[JsonPropertyName("featured_in_lobby")]
		public bool IsFeaturedInLobby { get; set; }

		/// <summary>Gets or sets a value indicating whether the session is visible in the event lobby.</summary>
		[JsonPropertyName("visible_in_lobby")]
		public bool IsVisibleInLobby { get; set; }

		/// <summary>Gets or sets a value indicating whether the webinar is simulive.</summary>
		[JsonPropertyName("is_simulive")]
		public bool IsSimulive { get; set; }

		/// <summary>Gets or sets the file ID of the previously recorded simulive.</summary>
		[JsonPropertyName("record_file_id")]
		public string RecordingFileId { get; set; }

		/// <summary>Gets or sets a value indicating whether session chat in lobby is enabled.</summary>
		/// <remarks>Attendees need to have Zoom Chat enabled by their Account Admin to view and participate in the conference chat.</remarks>
		[JsonPropertyName("chat_channel")]
		public bool IsChatInLobbyEnabled { get; set; }

		/// <summary>Gets or sets a value indicating whether the session is led by a sponsor.</summary>
		[JsonPropertyName("led_by_sponsor")]
		public bool IsLedBySponsor { get; set; }

		/// <summary>Gets or sets the track or classification that seperates events into different categories.</summary>
		/// <remarks>Tickets for these tracks link to specific sessions. Only ticket holders can join these sessions.</remarks>
		[JsonPropertyName("track_labels")]
		public string[] TrackLabels { get; set; }

		/// <summary>Gets or sets the list of audience type tags for the session.</summary>
		[JsonPropertyName("audience_labels")]
		public string[] AudienceLabels { get; set; }

		/// <summary>Gets or sets the list of product type tags for the session.</summary>
		[JsonPropertyName("product_labels")]
		public string[] ProductLabels { get; set; }

		/// <summary>Gets or sets the list of level type tags for the session.</summary>
		[JsonPropertyName("level")]
		public string[] Levels { get; set; }

		/// <summary>Gets or sets the list of alternative hosts.</summary>
		/// <remarks>Alternative hosts can start the session on the host’s behalf. The "Alternative Host" ticket is auto-assigned to alternative hosts.</remarks>
		[JsonPropertyName("alternative_host")]
		public string[] AlternativeHosts { get; set; }

		/// <summary>Gets or sets the list of panelists.</summary>
		/// <remarks>Panelist can mute or unmute themselves, start or stop their own video, view and respond to all questions and answers.</remarks>
		[JsonPropertyName("panelist")]
		public string[] Panelists { get; set; }

		/// <summary>Gets or sets the attendance type.</summary>
		[JsonPropertyName("attendance_type")]
		public EventAttendanceType AttendanceType { get; set; }

		/// <summary>Gets or sets the The physical location of the event.</summary>
		/// <remarks>This is only applicable for in-person or hybrid events.</remarks>
		[JsonPropertyName("physical_location")]
		public string PhysicalLocation { get; set; }

		/// <summary>Gets or sets the information about the session reservation option.</summary>
		/// <remarks>This option is supported for multi-session type events only.</remarks>
		[JsonPropertyName("session_reservation")]
		public EventSessionReservationInfo ReservationInfo { get; set; }
	}
}
