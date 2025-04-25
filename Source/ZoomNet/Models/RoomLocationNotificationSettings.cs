using System.Text.Json.Serialization;

namespace ZoomNet.Models
{
	/// <summary>
	/// The model of room location notification settings.
	/// </summary>
	public class RoomLocationNotificationSettings
	{
		/// <summary>
		/// Gets or sets a value indicating whether to send an alert when the audio echo test result meets usability threshold.
		/// </summary>
		[JsonPropertyName("audio_meet_usability_threshold")]
		public bool? AudioMeetUsabilityThreshold { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether to send an alert when the audio echo test result does not meet usability threshold.
		/// </summary>
		[JsonPropertyName("audio_not_meet_usability_threshold")]
		public bool? AudioNotMeetUsabilityThreshold { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether to send an alert when the battery of the controller or the scheduling display is low (at 20%) and is not being charged.
		/// </summary>
		[JsonPropertyName("battery_is_charging")]
		public bool? Batteryharging { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether to send an alert when the battery of the controller or the scheduling display is low (at 20%) and is not being charged.
		/// </summary>
		[JsonPropertyName("battery_low_and_not_charging")]
		public bool? BatteryLowNotCharging { get; set; }

		/// <summary>
		/// Gets or sets a percentage so that an alert is sent when the battery is less than the specified value.
		/// </summary>
		[JsonPropertyName("battery_percentage")]
		public int? BatteryPercentage { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether to send an alert when the connection to the Controller or Scheduling Display cannot be detected.
		/// </summary>
		[JsonPropertyName("controller_scheduling_disconnected")]
		public bool? ControllerOrDisplayDisconnected { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether to send an alert when the Controller or Scheduling Display can be detected again.
		/// </summary>
		[JsonPropertyName("controller_scheduling_reconnected")]
		public bool? ControllerOrDisplayReconnected { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether to send an alert when CPU usage is above 90%.
		/// </summary>
		[JsonPropertyName("cpu_usage_high_detected")]
		public bool? HighCpuUsageDetected { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether to send an alert when the mic, speaker or camera is disconnected in the Zoom Room..
		/// </summary>
		[JsonPropertyName("mic_speaker_camera_disconnected")]
		public bool? MicrophoneSpeakerCameraDisconnected { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether to send an alert when the mic, speaker or camera is reconnected.
		/// </summary>
		[JsonPropertyName("mic_speaker_camera_reconnected")]
		public bool? MicrophoneSpeakerCameraReconnected { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether to send an alert when low bandwidth network is detected.
		/// </summary>
		[JsonPropertyName("network_unstable_detected")]
		public bool? UnstableNetworkDetected { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether to send an alert when SIP registration stops working.
		/// </summary>
		[JsonPropertyName("sip_registration_failed")]
		public bool? SipRegistrationFailed { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether to send an alert after the SIP registration is re-enabled.
		/// </summary>
		[JsonPropertyName("sip_registration_re_enabled")]
		public bool? SipRegistrationReenabled { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether to send an alert when the Zoom Room is online after previously being offline.
		/// </summary>
		[JsonPropertyName("zoom_room_come_back_online")]
		public bool? ZoomRoomBackOnline { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether to send an alert when the TV display is disconnected.
		/// </summary>
		[JsonPropertyName("zoom_room_display_disconnected")]
		public bool? ZoomRoomDisplayDisconnected { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether to send an alert when the machine hosting the Zoom Room application has a network issue or cannot connect with the Controller.
		/// </summary>
		[JsonPropertyName("zoom_room_offline")]
		public bool? ZoomRoomOffline { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether to send an alert when the Zoom room calendar resource is inaccessible.
		/// </summary>
		/// <remarks>Not documented in Zoom API documentation.</remarks>
		[JsonPropertyName("zoom_room_calendar_resource_is_inaccessible")]
		public bool? ZoomRoomCalendarResourceInaccessible { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether to send an alert when the Zoom room digital signage cannot access a whiteboard in its contnt list.
		/// </summary>
		/// <remarks>Not documented in Zoom API documentation.</remarks>
		[JsonPropertyName("zoom_room_digital_signage_cannot_access_a_whiteboard_in_its_content_list")]
		public bool? ZoomRoomDigitalSignageCannotAccessWhiteboard { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether to send an alert when the Zoom room digital signage is enabled with empty content list.
		/// </summary>
		/// <remarks>Not documented in Zoom API documentation.</remarks>
		[JsonPropertyName("zoom_room_digital_signage_is_enabled_with_an_empty_digital_signage_content_list")]
		public bool? ZoomRoomDigitalSignageEnabledWithEmptyDigitalSignageContentList { get; set; }
	}
}
