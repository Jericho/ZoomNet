using System.Text.Json.Serialization;

namespace ZoomNet.Models.Webhooks;

/// <summary>
/// This event is triggered when all recordings for an event are completed.
/// </summary>
public class RecordingCompletedEvent : RecordingEvent
{
	/// <summary>
	/// Gets or sets authentication token to use to retrieve the recording.
	/// </summary>
	/// <remarks>
	/// Use the generated token value with the download_url value to download
	/// the cloud recording via an OAuth app.
	/// The download token only lasts for 24 hours after its creation,
	/// and you can only download the file within 24 hours of receiving the
	/// event notification. Pass download_token as a bearer token in
	/// the authorization header of your HTTP request.
	/// </remarks>
	[JsonPropertyName("download_token")]
	public string DownloadToken { get; set; }

	/// <summary>
	/// Gets or sets information about the meeting or webinar recording.
	/// </summary>
	[JsonPropertyName("object")]
	public Recording Recording { get; set; }
}
