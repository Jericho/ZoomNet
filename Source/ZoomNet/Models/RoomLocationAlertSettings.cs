using System.Text.Json.Serialization;

namespace ZoomNet.Models
{
	/// <summary>
	/// The model of room location alert settings.
	/// </summary>
	public class RoomLocationAlertSettings
	{
		/// <summary>
		/// Gets or sets a value indicating whether to display a notification message on the Zoom Room display when CPU usage is above 90%.
		/// </summary>
		[JsonPropertyName("cpu_usage_high_detected_notification_on_zr_display")]
		public bool? HighCpuUsageAlert { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether to display an alert message when an issue is detected with a bluetooth microphone.
		/// </summary>
		[JsonPropertyName("detect_bluetooth_microphone_error_alert")]
		public bool? BluetoothMicrophoneErrorAlert { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether to display an alert message when an issue is detected with a bluetooth speaker.
		/// </summary>
		[JsonPropertyName("detect_bluetooth_speaker_error_alert")]
		public bool? BluetoothSpeakerErrorAlert { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether to display an alert message when an issue is detected with a camera.
		/// </summary>
		[JsonPropertyName("detect_camera_error_aler")]
		public bool? CameraErrorAlert { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether to display an alert message when an issue is detected with a microphone.
		/// </summary>
		[JsonPropertyName("detect_microphone_error_alert")]
		public bool? MicrophoneErrorAlert { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether to display an alert message when an issue is detected with a speaker.
		/// </summary>
		[JsonPropertyName("detect_speaker_error_alert")]
		public bool? SpeakerErrorAlert { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether to display a notification message on the Zoom Room display when low bandwidth network is detected.
		/// </summary>
		[JsonPropertyName("network_unstable_detected_notification_on_zr_display")]
		public bool? NetworkUnstableNotification { get; set; }
	}
}
