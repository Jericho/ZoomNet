using Newtonsoft.Json;

namespace ZoomNet.Models
{
	/// <summary>
	/// Instant Meeting user settings.
	/// </summary>
	public class InstantMeetingUserSettings
	{
		/// <summary>
		/// Gets or sets a value indicating whether end-to-end encryption is required.
		/// </summary>
		[JsonProperty(PropertyName = "e2e_encryption")]
		public bool RequireEndToEndEncryption { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether chat is enabled during meeting for all participants.
		/// </summary>
		[JsonProperty(PropertyName = "chat")]
		public bool EnableChat { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether one-on-one private chat between participants is enabled.
		/// </summary>
		[JsonProperty(PropertyName = "private_chat")]
		public bool EnablePrivateChat { get; set; }

		/// <summary>
		/// Gets or sets the chime type.
		/// </summary>
		[JsonProperty(PropertyName = "entry_exit_chime")]
		public ChimeType Chime { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether record and play their own voice.
		/// </summary>
		[JsonProperty(PropertyName = "record_play_voice")]
		public bool EnableRecordAndPlayOwnVoice { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether to enable option to send feedback to Zoom at the end of the meeting.
		/// </summary>
		[JsonProperty(PropertyName = "feedback")]
		public bool EnableFeedback { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether to allow the host to add co-hosts.
		/// </summary>
		[JsonProperty(PropertyName = "co_host")]
		public bool AllowCoHosts { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether to add polls to the meeting controls.
		/// </summary>
		[JsonProperty(PropertyName = "polling")]
		public bool EnablePolling { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether to allow host to put attendees on hold.
		/// </summary>
		[JsonProperty(PropertyName = "attendees_on_hold")]
		public bool AllowAttendeesOnHold { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether to allow participants to use annotation tools.
		/// </summary>
		[JsonProperty(PropertyName = "annotation")]
		public bool AllowAnnotation { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether to enable remote control during screen share.
		/// </summary>
		[JsonProperty(PropertyName = "remote_control")]
		public bool EnableRemoteControl { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether to enable non-verbal feedback through screens.
		/// </summary>
		[JsonProperty(PropertyName = "non_verbal_control")]
		public bool EnableNonVerbalFeedback { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether to allow host to split meeting participants into separate breakout rooms.
		/// </summary>
		[JsonProperty(PropertyName = "breakout_room")]
		public bool AllowBreakoutRoom { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether to allow host to provide one-on-one support to participants.
		/// </summary>
		[JsonProperty(PropertyName = "remote_support")]
		public bool AllowRemoteSupport { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether to enable close captions.
		/// </summary>
		[JsonProperty(PropertyName = "close_caption")]
		public bool EnableCloseCaptions { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether to enable group HD video.
		/// </summary>
		[JsonProperty(PropertyName = "group_hd")]
		public bool EnableGroupHdVideo { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether to enable virtual background.
		/// </summary>
		[JsonProperty(PropertyName = "virtual_background")]
		public bool EnableVirtualBackground { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether to allow another user to take control of the camera.
		/// </summary>
		[JsonProperty(PropertyName = "far_end_camera_control")]
		public bool AllowTakeCameraControl { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether to enable waiting room.
		/// </summary>
		[JsonProperty(PropertyName = "waiting_room")]
		public bool EnableWaitingRoom { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether to allow live streaming.
		/// </summary>
		[JsonProperty(PropertyName = "allow_live_streaming")]
		public bool AllowLiveStreaming { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether to allow live streaming by host through Workplace by Facebook.
		/// </summary>
		[JsonProperty(PropertyName = "workplace_by_facebook")]
		public bool AllowWorkplaceByFacebook { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether to allow custom live streaming.
		/// </summary>
		[JsonProperty(PropertyName = "custom_live_streaming_service")]
		public bool AllowCustomLiveStreaming { get; set; }

		/// <summary>
		/// Gets or sets the PMI password.
		/// </summary>
		[JsonProperty(PropertyName = "custom_service_instructions")]
		public string CustomLiveStreamingInstructions { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether to always show meeting controls during a meeting.
		/// </summary>
		[JsonProperty(PropertyName = "show_meeting_control_toolbar")]
		public bool ShowMeetingControlToolbar { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether to allow setting the data center regions to use for hosting your real-time meeting and webinar traffic.
		/// </summary>
		[JsonProperty(PropertyName = "custom_data_center_regions")]
		public bool AllowCustomDataCenterRegions { get; set; }

		/// <summary>
		/// Gets or sets the data center regions to use for hosting your real-time meeting and webinar traffic.
		/// </summary>
		/// <remarks>
		/// The available regions are: EU, HK, AU, IN, TY, CN, US and CA.
		/// </remarks>
		[JsonProperty(PropertyName = "data_center_regions")]
		public string[] CustomDataCenterRegions { get; set; }
	}
}
