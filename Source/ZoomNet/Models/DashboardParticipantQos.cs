using System;
using System.Text.Json.Serialization;
using ZoomNet.Models.QualityOfService;

namespace ZoomNet.Models
{
	/// <summary>
	/// Quality of service provided to a participant.
	/// </summary>
	public class DashboardParticipantQos
	{
		/// <summary>
		/// Gets or sets date-time of QOS.
		/// </summary>
		/// <value>
		/// The date-time of the quality of service data.
		/// </value>
		[JsonPropertyName("date_time")]
		public DateTime DateTime { get; set; }

		/// <summary>
		/// Gets or sets audio input data.
		/// </summary>
		/// <value>
		/// The quality of service for audio input.
		/// </value>
		[JsonPropertyName("audio_input")]
		public PacketQualityOfServiceMetrics AudioInput { get; set; }

		/// <summary>
		/// Gets or sets audio output data.
		/// </summary>
		/// <value>
		/// The quality of service for audio output.
		/// </value>
		[JsonPropertyName("audio_output")]
		public PacketQualityOfServiceMetrics AudioOutput { get; set; }

		/// <summary>
		/// Gets or sets video input data.
		/// </summary>
		/// <value>
		/// The quality of service for video input.
		/// </value>
		[JsonPropertyName("video_input")]
		public VideoQualityOfServiceMetrics VideoInput { get; set; }

		/// <summary>
		/// Gets or sets video output data.
		/// </summary>
		/// <value>
		/// The quality of service for video output.
		/// </value>
		[JsonPropertyName("video_output")]
		public VideoQualityOfServiceMetrics VideoOutput { get; set; }

		/// <summary>
		/// Gets or sets screen share input data.
		/// </summary>
		/// <value>
		/// The quality of service for screen share input.
		/// </value>
		[JsonPropertyName("as_input")]
		public VideoQualityOfServiceMetrics ScreenShareInput { get; set; }

		/// <summary>
		/// Gets or sets screen share output data.
		/// </summary>
		/// <value>
		/// The quality of service for screen share output.
		/// </value>
		[JsonPropertyName("as_output")]
		public VideoQualityOfServiceMetrics ScreenShareOutput { get; set; }

		/// <summary>
		/// Gets or sets the CPU usage data.
		/// </summary>
		/// <value>
		/// The CPU usage data.
		/// </value>
		[JsonPropertyName("cpu_usage")]
		public CpuUsage CpuUsage { get; set; }

		/// <summary>
		/// Gets or sets the value of metrics on audio being sent from a Cloud Room Connector used by the participant to join the meeting.
		/// </summary>
		/// <value>
		/// Metrics on audio being sent from a Cloud Room Connector used by the participant to join the meeting.
		/// </value>
		[JsonPropertyName("audio_device_from_crc")]
		public PacketQualityOfServiceMetrics AudioDeviceFromCrc { get; set; }

		/// <summary>
		/// Gets or sets the value of metrics on audio received by a participant who joined the meeting via a Cloud Room Connector.
		/// </summary>
		/// <value>
		/// Metrics on audio received by a participant who joined the meeting via a Cloud Room Connector.
		/// </value>
		[JsonPropertyName("audio_device_to_crc")]
		public PacketQualityOfServiceMetrics AudioDeviceToCrc { get; set; }

		/// <summary>
		/// Gets or sets the value of metrics on video being sent from a Cloud Room Connector used by the participant to join the meeting.
		/// </summary>
		/// <value>
		/// Metrics on video being sent from a Cloud Room Connector used by the participant to join the meeting.
		/// </value>
		[JsonPropertyName("video_device_from_crc")]
		public PacketQualityOfServiceMetrics VideoDeviceFromCrc { get; set; }

		/// <summary>
		/// Gets or sets the value of metrics on video received by a participant who joined the meeting via a Cloud Room Connector.
		/// </summary>
		/// <value>
		/// Metrics on video received by a participant who joined the meeting via a Cloud Room Connector.
		/// </value>
		[JsonPropertyName("video_device_to_crc")]
		public PacketQualityOfServiceMetrics VideoDeviceToCrc { get; set; }

		/// <summary>
		/// Gets or sets the value of metrics on screen share being sent from a Cloud Room Connector used by the participant to join the meeting.
		/// </summary>
		/// <value>
		/// Metrics on screen share being sent from a Cloud Room Connector used by the participant to join the meeting.
		/// </value>
		[JsonPropertyName("as_device_from_crc")]
		public PacketQualityOfServiceMetrics ScreenShareDeviceFromCrc { get; set; }

		/// <summary>
		/// Gets or sets the value of metrics on screen share received by a participant who joined the meeting via a Cloud Room Connector.
		/// </summary>
		/// <value>
		/// Metrics on screen share received by a participant who joined the meeting via a Cloud Room Connector.
		/// </value>
		[JsonPropertyName("as_device_to_crc")]
		public PacketQualityOfServiceMetrics ScreenShareDeviceToCrc { get; set; }
	}
}
