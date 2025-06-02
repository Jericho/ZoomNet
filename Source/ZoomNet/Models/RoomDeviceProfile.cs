using System.Text.Json.Serialization;

namespace ZoomNet.Models
{
	/// <summary>
	/// Room device profile.
	/// </summary>
	public class RoomDeviceProfile
	{
		/// <summary>
		/// Gets or sets a value indicating whether audio processing is enabled.
		/// </summary>
		[JsonPropertyName("audio_processing")]
		public bool AudioProcessingEnabled { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether microphone level auto adjust is enabled.
		/// </summary>
		[JsonPropertyName("auto_adjust_mic_level")]
		public bool MicrophoneAutoAdjustEnabled { get; set; }

		/// <summary>
		/// Gets or sets the camera's device ID.
		/// </summary>
		[JsonPropertyName("camera_id")]
		public string CameraId { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether echo cancellation is enabled.
		/// </summary>
		[JsonPropertyName("echo_cancellation")]
		public bool EchoCancellationEnabled { get; set; }

		/// <summary>
		/// Gets or sets the device profile id.
		/// </summary>
		[JsonPropertyName("id")]
		public string Id { get; set; }

		/// <summary>
		/// Gets or sets the name of the device profile.
		/// </summary>
		[JsonPropertyName("name")]
		public string Name { get; set; }

		/// <summary>
		/// Gets or sets the noise suppression setting.
		/// </summary>
		[JsonPropertyName("noise_suppression")]
		public NoiseSuppressionType NoiseSuppression { get; set; }

		/// <summary>
		/// Gets or sets the speaker's device ID.
		/// </summary>
		[JsonPropertyName("speaker_id")]
		public string SpeakerId { get; set; }
	}
}
