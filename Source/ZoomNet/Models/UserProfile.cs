using System.Text.Json.Serialization;

namespace ZoomNet.Models
{
	/// <summary>
	/// User profile.
	/// </summary>
	public class UserProfile
	{
		/// <summary>
		/// Gets or sets the user's recording storage settings.
		/// </summary>
		[JsonPropertyName("recording_storage_location")]
		public UserRecordingStorageSettings RecordingStorageLocation { get; set; }
	}
}
