using Newtonsoft.Json;

namespace ZoomNet.Models.RoomSettings
{
	/// <summary>
	/// Room meeting settings.
	/// </summary>
	public class RoomMeetingSettings : IRoomSettings
	{
		public class ZoomRoomsSettings
		{
			/// <summary>
			/// If enabled, a reminder will display 10 minutes prior to the next scheduled meeting on the controller.
			/// </summary>
			[JsonProperty("upcoming_meeting_alert")]
			public bool UpcomingMeetingAlert { get; set; }

			/// <summary>
			/// If enabled, the upcoming meeting alert message will be shown on the TV display.
			/// The value of UpcomingMeetingAlert should be set to true to use this field.
			/// </summary>
			[JsonProperty("show_alert_before_meeting")]
			public bool ShowAlertBeforeMeeting { get; set; }

			/// <summary>
			/// Allow users to share content via Apple Screen Mirroring (called Airplay on iOS 11 or earlier) in Zoom Rooms.
			/// </summary>
			[JsonProperty("start_airplay_mirroring")]
			public bool StartAirplayMirroring { get; set; }

			/// <summary>
			/// Require the AirPlay service to be started by an administrator rather than always being available.
			/// </summary>
			[JsonProperty("start_airplay_manually")]
			public bool StartAirplayManually { get; set; }

			/// <summary>
			/// Restart the Zoom Rooms computer and controller once a week.
			/// </summary>
			[JsonProperty("weekly_system_restart")]
			public bool WeeklySystemRestart { get; set; }

			/// <summary>
			/// Show the same information on the TV that is shown on the controller.
			/// </summary>
			[JsonProperty("display_meeting_list")]
			public bool DisplayMeetingList { get; set; }

			/// <summary>
			/// Allow to display room name, time and sharing key on the top portion of TV.
			/// </summary>
			[JsonProperty("display_top_banner")]
			public bool DisplayTopBanner { get; set; }

			/// <summary>
			/// Display a survey at the end of each meeting regarding the audio and video quality on the Zoom Rooms Controller.
			/// </summary>
			[JsonProperty("display_feedback_survey")]
			public bool DisplayFeedbackSurvey { get; set; }

			/// <summary>
			/// Enable participants in a Zoom Room to share their laptop screen on the Zoom Room TV without entering a meeting ID or sharing code.
			/// </summary>
			[JsonProperty("auto_direct_sharing")]
			public bool AutoDirectSharing { get; set; }

			/// <summary>
			/// If enabled, all meetings in this room will be treated as private meetings, and the Zoom Room will
			/// display "Your Name's Meeting" instead of the real meeting topic.
			/// </summary>
			[JsonProperty("transform_meeting_to_private")]
			public bool TransformMeetingToPrivate { get; set; }

			/// <summary>
			/// If enabled, the meeting host and meeting ID (in addition to the meeting topic) are hidden from the Zoom Rooms display for private meetings.
			/// This affects meetings that were originally scheduled as private, as well as public meetings that were transformed to private.
			/// </summary>
			[JsonProperty("hide_id_for_private_meeting")]
			public bool HideIdForPrivateMeeting { get; set; }

			/// <summary>
			/// Automatically start scheduled meetings according to the start time listed on the calendar associated with the room.
			/// A meeting alert will appear 10 minutes prior to the scheduled time on the TV.
			/// </summary>
			[JsonProperty("auto_start_scheduled_meeting")]
			public bool AutoStartScheduledMeeting { get; set; }

			/// <summary>
			/// Automatically stop the meeting at the end time as scheduled and listed in the calendar associated with the room.
			/// </summary>
			[JsonProperty("auto_stop_scheduled_meeting")]
			public bool AutoStopScheduledMeeting { get; set; }

			/// <summary>
			/// Hide share instructions from TV.
			/// </summary>
			[JsonProperty("hide_share_instruction")]
			public bool HideShareInstruction { get; set; }

			/// <summary>
			/// Enable automated audio test to ensure high quality audio.
			/// </summary>
			[JsonProperty("audio_device_daily_auto_test")]
			public bool AudioDeviceDailyAutoTest { get; set; }

			/// <summary>
			/// Integrate with Skype for Business, GoToMeeting, or WebEx and show the meeting dial-in button on the meeting list tab for Zoom Rooms Controllers.
			/// </summary>
			[JsonProperty("support_join_3rd_party_meeting")]
			public bool SupportJoinThirdPartyMeeting { get; set; }

			/// <summary>
			/// Encrypt screen and content shared in meetings.
			/// </summary>
			[JsonProperty("encrypt_shared_screen_content")]
			public bool EncryptSharedScreenContent { get; set; }

			/// <summary>
			/// Enable multiple participants to share content simultaneously by default.
			/// </summary>
			[JsonProperty("allow_multiple_content_sharing")]
			public bool AllowMultipleContentSharing { get; set; }

			/// <summary>
			/// When enabled, meeting participants that are audio only or have their video turned off will also be shown on the Zoom Rooms display by default.
			/// </summary>
			[JsonProperty("show_non_video_participants")]
			public bool ShowNonVideoParticipants { get; set; }

			/// <summary>
			/// Allow users to see call history of joined meetings and phone calls from the Zoom Rooms controller.
			/// </summary>
			[JsonProperty("show_call_history_in_room")]
			public bool ShowCallHistoryInRoom { get; set; }

			/// <summary>
			/// If enabled, you can invite participants from the contact list during a meeting or when starting a meeting.
			/// </summary>
			[JsonProperty("show_contact_list_on_controller")]
			public bool ShowContactListOnController { get; set; }

			/// <summary>
			/// Use facial detection technology to determine and display the attendees count after meetings on Dashboard.
			/// </summary>
			[JsonProperty("count_attendees_number_in_room")]
			public bool CountAttendeesNumberInRoom { get; set; }

			/// <summary>
			/// Restrict sending Whiteboard sessions to contacts or internal users only.
			/// </summary>
			[JsonProperty("send_whiteboard_to_internal_contact_only")]
			public bool SendWhiteboardToInternalContactOnly { get; set; }
		}

		public class MeetingSecuritySettings
		{
			/// <summary>
			/// Gets or sets a value determining the use of end-to-end encryption for meetings.
			/// </summary>
			[JsonProperty("end_to_end_encrypted_meetings")]
			public bool EndToEndEncryptedMeetings { get; set; }

			/// <summary>
			/// Gets or sets the meeting encryption type.
			/// </summary>
			[JsonProperty("encryption_type")]
			public EncryptionType EncryptionType { get; set; }
		}

		/// <summary>
		/// Gets or sets the zoom rooms settings.
		/// </summary>
		[JsonProperty("zoom_rooms")]
		public ZoomRoomsSettings ZoomRooms { get; set; }

		/// <summary>
		/// Gets or sets the meeting security settings.
		/// </summary>
		[JsonProperty("meeting_security")]
		public MeetingSecuritySettings MeetingSecurity { get; set; }
	}
}
