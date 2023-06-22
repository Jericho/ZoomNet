using System.Text.Json.Serialization;

namespace ZoomNet.Models.Webhooks;

/// <summary>
/// Represents an event related to a recording.
/// </summary>
public class RecordingEvent : Event
{
	/// <summary>
	/// Gets or sets the unique identifier of the user who completed the meeting or webinar recording.
	/// </summary>
	[JsonPropertyName("account_id")]
	public string AccountId { get; set; }
}
