using Newtonsoft.Json;

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
		[JsonProperty(PropertyName = "recording_storage_location")]
		public UserRecordingStorageSettings RecordingStorageLocation { get; set; }
	}
}
