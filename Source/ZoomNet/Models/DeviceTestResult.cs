using System.Text.Json.Serialization;

namespace ZoomNet.Models
{
	/// <summary>
	/// Represents information about meeting's devices test result.
	/// </summary>
	public class DeviceTestResult
	{
		/// <summary>
		/// Gets or sets user id.
		/// </summary>
		[JsonPropertyName("user_id")]
		public string UserId { get; set; }

		/// <summary>
		/// Gets or sets user name.
		/// </summary>
		[JsonPropertyName("user_name")]
		public string UserName { get; set; }

		/// <summary>
		/// Gets or sets the camera test status.
		/// </summary>
		[JsonPropertyName("camera_status")]
		public DeviceTestStatus CameraStatus { get; set; }

		/// <summary>
		/// Gets or sets the speaker test status.
		/// </summary>
		[JsonPropertyName("speaker_status")]
		public DeviceTestStatus SpeakerStatus { get; set; }

		/// <summary>
		/// Gets or sets the microphone test status.
		/// </summary>
		[JsonPropertyName("microphone_status")]
		public DeviceTestStatus MicrophoneStatus { get; set; }

		/// <summary>
		/// Gets or sets user's operating system name.
		/// </summary>
		[JsonPropertyName("os")]
		public string OperatingSystem { get; set; }
	}
}
