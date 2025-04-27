using System.Text.Json.Serialization;

namespace ZoomNet.Models
{
	/// <summary>
	/// The model of room location meeting settings.
	/// </summary>
	public class RoomLocationSettings
	{
		/// <summary>
		/// Gets or sets a value indicating whether to allow multiple content sharing.
		/// </summary>
		[JsonPropertyName("allow_multiple_content_sharing")]
		public bool? AllowMultipleContentSharing { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether to enable automated audio test to ensure high quality audio.
		/// </summary>
		[JsonPropertyName("audio_device_daily_auto_test")]
		public bool? EnableDailyAudioAutoTest { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether to automatically accept incoming calls made from other Zoom Rooms or contacts in your account.
		/// Enabling this setting will also automatically allow far-end camera control.
		/// This setting is returned only for location type - "country".
		/// </summary>
		[JsonPropertyName("auto_accept_incoming_call_and_fecc")]
		public bool? AutomaticallyAcceptIncomingCallsFromOtherRooms { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether to enable participants in a Zoom Room to share their laptop screen on the Zoom Room TV without entering a meeting ID or sharing code.
		/// </summary>
		[JsonPropertyName("auto_direct_sharing")]
		public bool? EnableDirectSharing { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether to automatically start scheduled meetings according to the start time listed on the calendar associated with the room.
		/// A meeting alert will appear 10 minutes prior to the scheduled time on the TV.
		/// </summary>
		[JsonPropertyName("auto_start_scheduled_meeting")]
		public bool? AutomaticallyStartScheduledMeetings { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether to automatically stop the meeting at the end time as scheduled and listed in the calendar associated with the room.
		/// </summary>
		[JsonPropertyName("auto_stop_scheduled_meeting")]
		public bool? AutomaticallyStopScheduledMeeting { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether to use facial detection technology to determine and display the attendees count after meetings on Dashboard.
		/// </summary>
		[JsonPropertyName("count_attendees_number_in_room")]
		public bool? CountAttendees { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether to display a survey at the end of each meeting regarding the audio and video quality on the Zoom Rooms Controller.
		/// </summary>
		[JsonPropertyName("display_feedback_survey")]
		public bool? DisplayFeedbackSurvey { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether to show the same information on the TV that is shown on the controller.
		/// </summary>
		[JsonPropertyName("display_meeting_list")]
		public bool? DisplayMeetingList { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether to display room name, time and sharing key on the top portion of TV.
		/// </summary>
		[JsonPropertyName("display_top_banner")]
		public bool? DisplayTopBanner { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether to encrypt screen and content shared in meetings.
		/// </summary>
		[JsonPropertyName("encrypt_shared_screen_content")]
		public bool? EncryptSharedScreenAndContent { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether to hide the meeting host and meeting ID (in addition to the meeting topic) from the Zoom Rooms display for private meetings.
		/// This affects meetings that were originally scheduled as private, as well as public meetings that were transformed to private.
		/// </summary>
		[JsonPropertyName("hide_id_for_private_meeting")]
		public bool? HideIdForPrivateMeetings { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether to hide your own video from yourself but other people in the meeting can still see your video.
		/// This setting is returned only for location type - "country".
		/// </summary>
		[JsonPropertyName("hide_self_view")]
		public bool? HideSelfView { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether to lock the speaker volume control on controller.
		/// This setting is returned only for location type - "country".
		/// </summary>
		[JsonPropertyName("lock_speaker_volume_control")]
		public bool? LockSpeakerVolumeControl { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether to r sending Whiteboard sessions to contacts or internal users only.
		/// </summary>
		[JsonPropertyName("send_whiteboard_to_internal_contact_only")]
		public bool? SendWhiteboardToInternalContactOnly { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether to show the upcoming meeting alert message on the TV display.
		/// <see cref="UpcomingMeetingAlert"/> should be set to true to use this field.
		/// </summary>
		[JsonPropertyName("show_alert_before_meeting")]
		public bool? ShowAlertBeforeMeeting { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether to allow users to see call history of joined meetings and phone calls from the Zoom Rooms controller.
		/// </summary>
		[JsonPropertyName("show_call_history_in_room")]
		public bool? ShowCallHistoryInRoom { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether to allow inviting participants from the contact list during a meeting or when starting a meeting.
		/// </summary>
		[JsonPropertyName("show_contact_list_on_controller")]
		public bool? ShowContactListOnController { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether to show meeting participants that are audio only or have their video turned off on the Zoom Rooms display by default.
		/// </summary>
		[JsonPropertyName("show_non_video_participants")]
		public bool? ShowNonVideoParticipants { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether to require the AirPlay service to be started by an administrator rather than always being available.
		/// </summary>
		[JsonPropertyName("start_airplay_manually")]
		public bool? StartAirplayManually { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether to allow users to share content via Apple Screen Mirroring (called Airplay on iOS 11 or earlier) in Zoom Rooms.
		/// </summary>
		[JsonPropertyName("start_airplay_mirroring")]
		public bool? StartAirplayMirroring { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether to disable automatic detection and enable manual content sharing from a device to a Zoom Room.
		/// Learn more <a href="https://support.zoom.com/hc/en/article?id=zm_kb&amp;sysparm_article=KB0068437">here</a>.
		/// This setting is returned only for location type - "country".
		/// </summary>
		[JsonPropertyName("start_hdmi_content_share_manualy")]
		public bool? StartHdmiContentShareManualy { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether to integrate with Skype for Business, GoToMeeting, or WebEx and show the meeting dial-in button on the meeting list tab for Zoom Rooms Controllers.
		/// </summary>
		[JsonPropertyName("support_join_3rd_party_meeting")]
		public bool? SupportJoinThirdPartyMeeting { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether to treat all meetings in this room as private meetings, and the Zoom Room will display "Your Name's Meeting" instead of the real meeting topic.
		/// </summary>
		[JsonPropertyName("transform_meeting_to_private")]
		public bool? TransformMeetingsToPrivate { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether to display a reminder 10 minutes prior to the next scheduled meeting on the controller.
		/// </summary>
		[JsonPropertyName("upcoming_meeting_alert")]
		public bool? UpcomingMeetingAlert { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether to enable voice commands with Zoom Rooms.
		/// </summary>
		[JsonPropertyName("voice_commands")]
		public bool? EnableVoiceCommands { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether to restart the Zoom Rooms computer and controller once a week.
		/// </summary>
		[JsonPropertyName("weekly_system_restart")]
		public bool? WeeklySystemRestart { get; set; }

		/// <summary>
		/// Gets or sets settings related to how incoming meeting requests are handled.
		/// </summary>
		[JsonPropertyName("incoming_meeting_request")]
		public RoomLocationIncomingMeetingRequestSettings IncomingMeetingRequestHandling { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether to automatically accept incoming far end camera control (FECC) requests from other Zoom clients received during a Zoom meeting.
		/// </summary>
		[JsonPropertyName("automatically_accept_far_end_camera_control_request")]
		public bool? AutomaticallyAcceptFarEndCameraControlRequest { get; set; }
	}
}
