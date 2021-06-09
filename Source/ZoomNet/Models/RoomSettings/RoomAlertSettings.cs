using System;
using Newtonsoft.Json;
using ZoomNet.Utilities;

namespace ZoomNet.Models.RoomSettings
{
	/// <summary>
	/// Room alert settings.
	/// </summary>
	public class RoomAlertSettings : IRoomSettings
	{
		/// <summary>
		/// Client alert settings.
		/// </summary>
		public class ClientAlertSettings
		{
			/// <summary>
			/// Display an alert message when an issue is detected with microphone.
			/// </summary>
			[JsonProperty("detect_microphone_error_alert")]
			public bool DetectMicrophoneErrorAlert { get; set; }

			/// <summary>
			/// Display an alert message when an issue is detected with a bluetooth microphone.
			/// </summary>
			[JsonProperty("detect_bluetooth_microphone_error_alert")]
			public bool DetectBluetoothMicrophoneErrorAlert { get; set; }

			/// <summary>
			/// Display an alert message when an issue is detected with a speaker.
			/// </summary>
			[JsonProperty("detect_speaker_error_alert")]
			public bool DetectSpeakerErrorAlert { get; set; }

			/// <summary>
			/// Display an alert message when an issue is detected with a bluetooth speaker.
			/// </summary>
			[JsonProperty("detect_bluetooth_speaker_error_alert")]
			public bool DetectBluetoothSpeakerErrorAlert { get; set; }

			/// <summary>
			/// Display an alert message when an issue is detected with a camera.
			/// </summary>
			[JsonProperty("detect_camera_error_alert")]
			public bool DetectCameraErrorAlert { get; set; }
		}

		/// <summary>
		/// Notification settings.
		/// </summary>
		public class NotificationSettings
		{
			/// <summary>
			/// Send an alert when the audio echo test result does not meet usability threshold.
			/// </summary>
			[JsonProperty("audio_not_meet_usability_threshold")]
			public bool AudioDoesNotMeetUsabilityThreshold { get; set; }

			/// <summary>
			/// Send an alert when the audio echo test result meets usability threshold.
			/// </summary>
			[JsonProperty("audio_meet_usability_threshold")]
			public bool AudioMeetsUsabilityThreshold { get; set; }

			/// <summary>
			/// Send an alert when the battery of the controller or the scheduling display is low (at 20%) and is not being charged.
			/// </summary>
			[JsonProperty("battery_low_and_not_charging")]
			public bool BatteryLowAndNotCharging { get; set; }

			/// <summary>
			/// Send an alert when the battery starts charging.
			/// </summary>
			[JsonProperty("battery_is_charging")]
			public bool BatteryIsCharging { get; set; }

			/// <summary>
			/// Specify a percentage so that an alert is sent when the battery is less than the battery percentage that you specified.
			/// </summary>
			[JsonProperty("battery_percentage")]
			[JsonConverter(typeof(PercentConverter))]
			public float BatteryPercentage { get; set; }

			/// <summary>
			/// Send an alert when the connection to the Controller or Scheduling Display cannot be detected.
			/// </summary>
			[JsonProperty("controller_scheduling_disconnected")]
			public bool ControllerSchedulingDisconnected { get; set; }

			/// <summary>
			/// Send an alert when the Controller or Scheduling Display can be detected again.
			/// </summary>
			[JsonProperty("controller_scheduling_reconnected")]
			public bool ControllerSchedulingReconnected { get; set; }

			/// <summary>
			/// Send an alert when CPU usage is above 90%.
			/// </summary>
			[JsonProperty("cpu_usage_high_detected")]
			public bool CpuUsageHighDetected { get; set; }

			/// <summary>
			/// Send an alert when low bandwidth network is detected.
			/// </summary>
			[JsonProperty("network_unstable_detected")]
			public bool NetworkUnstableDetected { get; set; }

			/// <summary>
			/// Send an alert when the machine hosting the Zoom Room application has a network issue or cannot connect with the Controller.
			/// </summary>
			[JsonProperty("zoom_room_offline")]
			public bool ZoomRoomOffline { get; set; }

			/// <summary>
			/// Send an alert when the Zoom Room is online after previously being offline.
			/// </summary>
			[JsonProperty("zoom_room_come_back_online")]
			public bool ZoomRoomComeBackOnline { get; set; }

			/// <summary>
			/// Send an alert when SIP registration stops working.
			/// </summary>
			[JsonProperty("sip_registration_failed")]
			public bool SipRegistrationFailed { get; set; }

			/// <summary>
			/// Send an alert after the SIP registration is re-enabled.
			/// </summary>
			[JsonProperty("sip_registration_re_enabled")]
			public bool SipRegistrationReEnabled { get; set; }

			/// <summary>
			/// Send an alert when the mic, speaker or camera is disconnected in the Zoom Room.
			/// </summary>
			[JsonProperty("mic_speaker_camera_disconnected")]
			public bool MicSpeakerCameraDisconnected { get; set; }

			/// <summary>
			/// Send an alert when the mic, speaker or camera is reconnected.
			/// </summary>
			[JsonProperty("mic_speaker_camera_reconnected")]
			public bool MicSpeakerCameraReconnected { get; set; }

			/// <summary>
			/// Send an alert when the TV display is disconnected.
			/// </summary>
			[JsonProperty("zoom_room_display_disconnected")]
			public bool ZoomRoomDisplayDisconnected { get; set; }
		}

		/// <summary>
		/// Digital signage settings.
		/// </summary>
		public class DigitalSignageSettings
		{
			public class BannerSettings
			{
				/// <summary>
				/// Display or hide banner room name.
				/// </summary>
				[JsonProperty("banner_room_name")]
				public bool RoomName { get; set; }

				/// <summary>
				/// Display or hide banner sharing key.
				/// </summary>
				[JsonProperty("banner_sharing_key")]
				public bool SharingKey { get; set; }

				/// <summary>
				/// Display or hide time in the banner.
				/// </summary>
				[JsonProperty("banner_time")]
				public bool Time { get; set; }
			}

			public class DisplayPeriodSettings
			{
				/// <summary>
				/// Start displaying digital signage content after certain duration after the meeting ends.
				/// The value of this field indicates the duration in minutes.
				/// </summary>
				[JsonProperty("start_displaying_content")]
				public int StartDisplayingContent { get; set; }

				/// <summary>
				/// Stop displaying content certain duration before a meeting is scheduled to begin.
				/// The value of this field indicates the duration in minutes.
				/// </summary>
				[JsonProperty("stop_displaying_content")]
				public int StopDisplayingContent { get; set; }
			}

			public class ContentListSettings
			{
				public class ContentSettings
				{
					/// <summary>
					/// Content Id.
					/// </summary>
					[JsonProperty("id")]
					public string ContentId { get; set; }

					/// <summary>
					/// Name of the content.
					/// </summary>
					[JsonProperty("name")]
					public string Name { get; set; }

					/// <summary>
					/// Duration for how long the content will be displayed.
					/// </summary>
					[JsonProperty("duration")]
					public int Duration { get; set; }

					/// <summary>
					/// Order of the content in the display.
					/// </summary>
					[JsonProperty("order")]
					public int Order { get; set; }

					/// <summary>
					/// Unique identifier.
					/// </summary>
					[JsonProperty("id")]
					public string Id { get; set; }
				}

				/// <summary>
				/// Specify an action for the content list.
				/// </summary>
				[JsonProperty("action")]
				public ActionType Action { get; set; }

				/// <summary>
				/// Unique identifier of the content list. This field is only required if you would like to remove or update the content list.
				/// </summary>
				[JsonProperty("id")]
				public string Id { get; set; }

				/// <summary>
				/// Name of the content list.
				/// </summary>
				[JsonProperty("name")]
				public string Name { get; set; }

				/// <summary>
				/// Specify the display start time for the content list in GMT.
				/// </summary>
				[JsonProperty("start_time")]
				public DateTime StartTime { get; set; }

				/// <summary>
				/// Specify the display end time for the content list in GMT.
				/// </summary>
				[JsonProperty("end_time")]
				public DateTime EndTime { get; set; }

				/// <summary>
				/// Content list.
				/// </summary>
				[JsonProperty("contents")]
				public ContentSettings[] Contents { get; set; }
			}

			/// <summary>
			/// Gets or sets the layout.
			/// </summary>
			[JsonProperty("layout")]
			public Layout Layout { get; set; }

			/// <summary>
			/// Gets or sets the banner settings.
			/// </summary>
			[JsonProperty("banner")]
			public BannerSettings Banner { get; set; }

			/// <summary>
			/// Gets or sets the display period settings.
			/// </summary>
			[JsonProperty("display_period")]
			public DisplayPeriodSettings DisplayPeriod { get; set; }

			/// <summary>
			/// Sound of all contents will be muted if the value of this field is set to true.
			/// </summary>
			[JsonProperty("mute")]
			public bool Mute { get; set; }

			/// <summary>
			/// Indicates whether digital signage is on or off.
			/// </summary>
			[JsonProperty("enable_digital_signage")]
			public bool EnableDigitalSignage { get; set; }

			/// <summary>
			/// Gets or sets the content lists.
			/// </summary>
			[JsonProperty("play_list")]
			public ContentListSettings[] PlayList { get; set; }
		}

		/// <summary>
		/// The Client Alert Settings section includes alerts that display on the TV screen of the Zoom Room. Disable these settings
		/// if you have deliberately disconnected one or more peripheral devices or have never enabled them.
		/// </summary>
		[JsonProperty("client_alert")]
		public ClientAlertSettings ClientAlert { get; set; }

		/// <summary>
		/// Notifications Settings includes the circumstances in which the room sends an email alert to the support team to
		/// notify them of a potentially urgent issue. These issues can affect the operation of the room, but do not display
		/// on the TV screen. The email alert is sent to the email address specified in the Notification Email Recipients section. 
		/// </summary>
		[JsonProperty("notification")]
		public NotificationSettings Notification { get; set; }

		/// <summary>
		/// Digital signage.
		/// </summary>
		[JsonProperty("digital_signage")]
		public DigitalSignageSettings DigitalSignage { get; set; }
	}
}
