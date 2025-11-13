using System.Text.Json.Serialization;

namespace ZoomNet.Models.Webhooks
{
	/// <summary>
	/// Represents an event related to recordings with files.
	/// </summary>
	public abstract class RecordingFilesEvent : RecordingEvent
	{
		/// <summary>
		/// Gets or sets information about the meeting or webinar recording.
		/// </summary>
		[JsonPropertyName("object")]
		public Recording Recording { get; set; }
	}
}
