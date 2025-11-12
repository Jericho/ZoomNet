using System.Text.Json.Serialization;

namespace ZoomNet.Models.Webhooks
{
	/// <summary>
	/// This event is triggered every time storage usage changes.
	/// </summary>
	public class RecordingCloudStorageUsageUpdatedEvent : RecordingEvent
	{
		/// <summary>
		/// Gets or sets the information about recording storage usage.
		/// </summary>
		[JsonPropertyName("object")]
		public RecordingStorageUsage StorageUsage { get; set; }
	}
}
