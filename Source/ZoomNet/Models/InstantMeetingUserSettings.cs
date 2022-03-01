using System.Text.Json.Serialization;

namespace ZoomNet.Models
{
	/// <summary>
	/// Instant Meeting user settings.
	/// </summary>
	public class InstantMeetingUserSettings
	{
		/// <summary>Gets or sets a value indicating whether the host can enable Focus Mode when scheduling a meeting.</summary>
		/// <remarks>This value defaults to null.</remarks>
		[JsonPropertyName("allow_host_to_enable_focus_mode")]
		public bool? AllowFocusMode { get; set; }

		/// <summary>Gets or sets a value indicating whether to allow live streaming.</summary>
		[JsonPropertyName("allow_live_streaming")]
		public bool AllowLiveStreaming { get; set; }

		/// <summary>Gets or sets the value indicating what groups a given participant can chat with.</summary>
		[JsonPropertyName("allow_participants_chat_with")]
		public ParticipantChatType ParticipantChatType { get; set; }

		/// <summary>Gets or sets the value indicating whether to allow participants to save meeting chats.</summary>
		[JsonPropertyName("allow_users_save_chats")]
		public ParticipantChatSaveType ParticipantChatSaveType { get; set; }

		/// <summary>Gets or sets a value indicating whether to allow participants to use annotation tools.</summary>
		[JsonPropertyName("annotation")]
		public bool AllowAnnotation { get; set; }

		/// <summary>Gets or sets a value indicating whether the Focus Mode feature is enabled.</summary>
		/// <remarks>When enabled, this feature only displays the host and co-hosts' video and profile pictures during a meeting.</remarks>
		[JsonPropertyName("attention_mode_focus_mode")]
		public bool FocusModeEnabled { get; set; }

		/// <summary>Gets or sets a value indicating whether the all in-meeting chats are automatically saved.</summary>
		[JsonPropertyName("auto_saving_chat")]
		public bool AutomaticallySaveChat { get; set; }

		/// <summary>Gets or sets a value indicating whether to allow host to split meeting participants into separate breakout rooms.</summary>
		[JsonPropertyName("breakout_room")]
		public bool AllowBreakoutRoom { get; set; }

		/// <summary>Gets or sets a value indicating whether the host can assign participants to breakout rooms when scheduling.</summary>
		/// <remarks>This feature is only available in version 4.5.0 or higher.</remarks>
		[JsonPropertyName("breakout_room_schedule")]
		public bool AllowBreakoutRoomWhenScheduling { get; set; }

		/// <summary>Gets or sets a value indicating whether chat is enabled during meeting for all participants.</summary>
		[JsonPropertyName("chat")]
		public bool EnableChat { get; set; }

		/// <summary>Gets or sets a value indicating whether to enable closed captions.</summary>
		[JsonPropertyName("closed_caption")]
		public bool EnableClosedCaptions { get; set; }

		/// <summary>Gets or sets the closed captioning settings.</summary>
		[JsonPropertyName("closed_captioning")]
		public ClosedCaptioningSettings CloseCaptioningSettings { get; set; }

		/// <summary>Gets or sets a value indicating whether to allow the host to add co-hosts.</summary>
		[JsonPropertyName("co_host")]
		public bool AllowCoHosts { get; set; }

		/// <summary>Gets or sets a value indicating whether to allow setting the data center regions to use for hosting your real-time meeting and webinar traffic.</summary>
		[JsonPropertyName("custom_data_center_regions")]
		public bool AllowCustomDataCenterRegions { get; set; }

		/// <summary>Gets or sets a value indicating whether to allow custom live streaming.</summary>
		[JsonPropertyName("custom_live_streaming_service")]
		public bool AllowCustomLiveStreaming { get; set; }

		/// <summary>Gets or sets the custom live streaming service instructions.</summary>
		[JsonPropertyName("custom_service_instructions")]
		public string CustomLiveStreamingInstructions { get; set; }

		/// <summary>Gets or sets the data center regions to use for hosting your real-time meeting and webinar traffic.</summary>
		[JsonPropertyName("data_center_regions")]
		public DataCenterRegion[] CustomDataCenterRegions { get; set; }

		/// <summary>Gets or sets a value indicating whether end-to-end encryption is required.</summary>
		[JsonPropertyName("e2e_encryption")]
		public bool RequireEndToEndEncryption { get; set; }

		/// <summary>Gets or sets the chime type.</summary>
		[JsonPropertyName("entry_exit_chime")]
		public ChimeType Chime { get; set; }

		/// <summary>Gets or sets a value indicating whether to allow another user to take control of the camera.</summary>
		[JsonPropertyName("far_end_camera_control")]
		public bool AllowTakeCameraControl { get; set; }

		/// <summary>Gets or sets a value indicating whether to enable option to send feedback to Zoom at the end of the meeting.</summary>
		[JsonPropertyName("feedback")]
		public bool EnableFeedback { get; set; }

		/// <summary>Gets or sets a value indicating whether in-meeting file transfer setting has been enabled for the user or not.</summary>
		[JsonPropertyName("file_transfer")]
		public bool AllowFileTransfer { get; set; }

		/// <summary>Gets or sets a value indicating whether to enable group HD video.</summary>
		[JsonPropertyName("group_hd")]
		public bool EnableGroupHdVideo { get; set; }

		/// <summary>Gets or sets a value indicating whether to allow participants to join a meeting directly from their desktop browser.</summary>
		/// <remarks>Note that the meeting experience from the desktop browser is limited.</remarks>
		[JsonPropertyName("join_from_desktop")]
		public bool AllowJoinFromDesktopBrowser { get; set; }

		/// <summary>Gets or sets a value indicating whether to allow participants to join a meeting directly from their mobile browser.</summary>
		/// <remarks>Note that the meeting experience from the mobile browser is limited.</remarks>
		[JsonPropertyName("join_from_mobile")]
		public bool AllowJoinFromMobileBrowser { get; set; }

		/// <summary>Gets or sets the language interpretation settings.</summary>
		[JsonPropertyName("language_interpretation")]
		public LanguageInterpretationSettings LanguageInterpretationSettings { get; set; }

		/// <summary>Gets or sets a value indicating whether to allow Facebok live streaming.</summary>
		[JsonPropertyName("live_streaming_facebook")]
		public bool AllowFacebookLivestreaming { get; set; }

		/// <summary>Gets or sets a value indicating whether to allow YouTube live streaming.</summary>
		[JsonPropertyName("live_streaming_youtube")]
		public bool AllowYouTubeLivestreaming { get; set; }

		/// <summary>Gets or sets the manual captioning settings.</summary>
		[JsonPropertyName("manual_captioning")]
		public ManualCaptioningSettings ManualCaptioningSettings { get; set; }

		/// <summary>Gets or sets a value indicating whether meeting participants can use the emoji reactions.</summary>
		[JsonPropertyName("meeting_reactions")]
		public bool AllowEmojiReactions { get; set; }

		/// <summary>Gets or sets a value indicating whether to allow the host to present a survey to participants once a meeting has ended.</summary>
		/// <remarks>This feature is only available in version 5.7.3 or higher.</remarks>
		[JsonPropertyName("meeting_survey")]
		public bool AllowMeetingSurvey { get; set; }

		/// <summary>Gets or sets a value indicating whether to enable non-verbal feedback through screens.</summary>
		[JsonPropertyName("non_verbal_control")]
		public bool EnableNonVerbalFeedback { get; set; }

		/// <summary>Gets or sets a value indicating whether to add polls to the meeting controls.</summary>
		[JsonPropertyName("polling")]
		public bool EnablePolling { get; set; }

		/// <summary>Gets or sets a value indicating whether one-on-one private chat between participants is enabled.</summary>
		[JsonPropertyName("private_chat")]
		public bool EnablePrivateChat { get; set; }

		/// <summary>Gets or sets a value indicating whether record and play their own voice.</summary>
		[JsonPropertyName("record_play_voice")]
		public bool EnableRecordAndPlayOwnVoice { get; set; }

		/// <summary>Gets or sets a value indicating whether to enable remote control during screen share.</summary>
		[JsonPropertyName("remote_control")]
		public bool EnableRemoteControl { get; set; }

		/// <summary>Gets or sets a value indicating whether to allow host to provide one-on-one support to participants.</summary>
		[JsonPropertyName("remote_support")]
		public bool AllowRemoteSupport { get; set; }

		/// <summary>Gets or sets a value indicating whether the Request permission to unmute participants option has been enabled for the user or not.</summary>
		[JsonPropertyName("request_permission_to_unmute")]
		public bool AllowRequestPermissionToUnmute { get; set; }

		/// <summary>Gets or sets a value indicating whether the host and participants can share their screen or content.</summary>
		[JsonPropertyName("screen_sharing")]
		public bool AllowScreenSharing { get; set; }

		/// <summary>Gets or sets a value indicating whether to allow participants to join a meeting directly from their browser and bypass the Zoom application download process.</summary>
		/// <remarks>This is useful for participants who cannot download, install, or run applications. Note that the meeting experience from the browser is limited.</remarks>
		[JsonPropertyName("show_a_join_from_your_browser_link")]
		public bool AllowJoinFromBrowser { get; set; }

		/// <summary>Gets or sets a value indicating whether to always show meeting controls during a meeting.</summary>
		[JsonPropertyName("show_meeting_control_toolbar")]
		public bool ShowMeetingControlToolbar { get; set; }

		/// <summary>Gets or sets a value indicating whether the person sharing during a presentation can allow others to control the slide presentation.</summary>
		/// <remarks>This feature is only available in version 5.8.3 or higher.</remarks>
		[JsonPropertyName("slide_control")]
		public bool AllowShareSlidePresentationControl { get; set; }

		/// <summary>Gets or sets the data center regions to NOT opt in to.</summary>
		[JsonPropertyName("unchecked_data_center_regions")]
		public DataCenterRegion[] UncheckedDataCenterRegions { get; set; }

		/// <summary>Gets or sets a value indicating whether to enable virtual background.</summary>
		[JsonPropertyName("virtual_background")]
		public bool EnableVirtualBackground { get; set; }

		/// <summary>Gets or sets the virtual background settings.</summary>
		[JsonPropertyName("virtual_background_settings")]
		public VirtualBackgroundSettings VirtualBackgroundSettings { get; set; }

		/// <summary>Gets or sets a value indicating whether to enable waiting room.</summary>
		[JsonPropertyName("waiting_room")]
		public bool EnableWaitingRoom { get; set; }

		/// <summary>Gets or sets the settings for webinar chat.</summary>
		[JsonPropertyName("webinar_chat")]
		public WebinarChatSettings WebinarChatSettings { get; set; }

		/// <summary>Gets or sets the settings for webinar live streaming.</summary>
		[JsonPropertyName("webinar_live_streaming")]
		public WebinarLiveStreamingSettings WebinarLiveStreamingSettings { get; set; }

		/// <summary>Gets or sets the settings for webinar polling.</summary>
		[JsonPropertyName("webinar_polling")]
		public WebinarPollingSettings WebinarPollingSettings { get; set; }

		/// <summary>Gets or sets a value indicating whether to allow the host to present surveys to attendees once a webinar has ended.</summary>
		[JsonPropertyName("webinar_survey")]
		public bool AllowWebinarSurvey { get; set; }

		/// <summary>Gets or sets who can share their screen or content during meetings.</summary>
		[JsonPropertyName("who_can_share_screen")]
		public WhoCanShare WhoCanShareScreen { get; set; }

		/// <summary>Gets or sets who is allowed to start sharing screen when someone else in the meeting is sharing their screen.</summary>
		[JsonPropertyName("who_can_share_screen_when_someone_is_sharing")]
		public WhoCanShare WhoCanShareScreenWhenSomeoneIsSharing { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether to allow live streaming by host through Workplace by Facebook.
		/// </summary>
		[JsonPropertyName("workplace_by_facebook")]
		public bool AllowWorkplaceByFacebook { get; set; }
	}
}
