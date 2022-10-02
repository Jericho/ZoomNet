using System.Text.Json.Serialization;

namespace ZoomNet.Models
{
	/// <summary>
	/// Feature user settings.
	/// </summary>
	public class FeatureUserSettings
	{
		/// <summary>
		/// Gets or sets a value indicating whether meetings can be hosted in China.
		/// </summary>
		[JsonPropertyName("cn_meeting")]
		public bool MeetingsInChina { get; set; }

		/// <summary>
		/// Gets or sets the user's assigned concurrent meeting type.
		/// </summary>
		[JsonPropertyName("concurrent_meeting")]
		public ConcurrentMeetingType ConcurrentMeetingType { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether meetings can be hosted in India.
		/// </summary>
		[JsonPropertyName("in_meeting")]
		public bool MeetingsInIndia { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether the large meeting feature is available.
		/// </summary>
		[JsonPropertyName("large_meeting")]
		public bool LargeMeetings { get; set; }

		/// <summary>Gets or sets the user's large meeting capacity.</summary>
		/// <remarks>Can be 500 or 1000, depending on if the user has a large meeting capacity plan subscription or not.</remarks>
		[JsonPropertyName("large_meeting_capacity")]
		public int LargeMeetingCapacity { get; set; }

		/// <summary>
		/// Gets or sets the user's meeting capacity.
		/// </summary>
		[JsonPropertyName("meeting_capacity")]
		public int MeetingCapacity { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether the webinar feature is available.
		/// </summary>
		[JsonPropertyName("webinar")]
		public bool Webinar { get; set; }

		/// <summary>
		/// Gets or sets the user's webinar capacity.
		/// </summary>
		[JsonPropertyName("webinar_capacity")]
		public int WebinarCapacity { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether the Zoom events feature is available.
		/// </summary>
		[JsonPropertyName("zoom_events")]
		public bool ZoomEvents { get; set; }

		/// <summary>
		/// Gets or sets the user's Zoom Events capacity.
		/// </summary>
		[JsonPropertyName("zoom_events_capacity")]
		public int ZoomEventsCapacity { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether the Zoom phone feature is available.
		/// </summary>
		[JsonPropertyName("zoom_phone")]
		public bool ZoomPhone { get; set; }
	}
}
