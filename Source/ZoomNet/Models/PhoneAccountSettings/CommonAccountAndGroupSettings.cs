using System.Text.Json.Serialization;

namespace ZoomNet.Models.PhoneAccountSettings
{
	/// <summary>
	/// Settings that are common for the account and the group.
	/// </summary>
	public abstract class CommonAccountAndGroupSettings
	{
		/// <summary>
		/// Gets or sets settings that control ad-hoc call recording.
		/// </summary>
		[JsonPropertyName("ad_hoc_call_recording")]
		public AdHocCallRecordingSettings AdHocCallRecording { get; set; }

		/// <summary>
		/// Gets or sets settings related to advanced encryption.
		/// </summary>
		[JsonPropertyName("advanced_encryption")]
		public AdvancedEncryptionSettings AdvancedEncryption { get; set; }

		/// <summary>
		/// Gets or sets settings that allow extension or user to make and accept calls and send SMS.
		/// </summary>
		[JsonPropertyName("allowed_call_locations")]
		public AllowedCallLocationsSettings AllowedCallLocations { get; set; }

		/// <summary>
		/// Gets or sets settings that allow hands-free peer-to-peer conversations.
		/// </summary>
		[JsonPropertyName("audio_intercom")]
		public AudioIntercomSettings AudioIntercom { get; set; }

		/// <summary>
		/// Gets or sets settings that control automatic calls recording.
		/// </summary>
		[JsonPropertyName("auto_call_recording")]
		public AutoCallRecordingSettings AutoCallRecording { get; set; }

		/// <summary>
		/// Gets or sets settings that allow to block calls without caller id.
		/// </summary>
		[JsonPropertyName("block_calls_without_caller_id")]
		public BlockCallsWithoutCallerIdSettings BlockCallsWithoutCallerId { get; set; }

		/// <summary>
		/// Gets or sets settings that allow to set rules for blocking external calls during business, closed, and holiday hours.
		/// </summary>
		[JsonPropertyName("block_external_calls")]
		public BlockExternalCallsSettings BlockExternalCalls { get; set; }

		/// <summary>
		/// Gets or sets settings that allow users and administrators to block inbound calls and SMS/MMS from phone numbers or prefixes.
		/// </summary>
		[JsonPropertyName("block_list_for_inbound_calls_and_messaging")]
		public BlockInboundCallsAndMessagingSettings BlockInboundCallsAndMessaging { get; set; }

		/// <summary>
		/// Gets or sets settings that allow users to forward their calls to other numbers.
		/// </summary>
		[JsonPropertyName("call_handling_forwarding_to_other_users")]
		public CallForwardingSettings CallHandlingForwardingToOtherUsers { get; set; }

		/// <summary>
		/// Gets or sets settings that allow users to turn on live transcriptions for a call.
		/// </summary>
		[JsonPropertyName("call_live_transcription")]
		public CallLiveTranscriptionSettings CallLiveTranscription { get; set; }

		/// <summary>
		/// Gets or sets settings that allow users to forward their calls to other numbers when a call is not answered.
		/// </summary>
		[JsonPropertyName("call_overflow")]
		public CallOverflowSettings CallOverflow { get; set; }

		/// <summary>
		/// Gets or sets settings that allow calls placed on hold to be resumed from another location using a retrieval code.
		/// </summary>
		[JsonPropertyName("call_park")]
		public CallParkSettings CallPark { get; set; }

		/// <summary>
		/// Gets or sets settings that allow users to warm or blind transfer their calls.
		/// </summary>
		[JsonPropertyName("call_transferring")]
		public CallTransferringSettings CallTransferring { get; set; }

		/// <summary>
		/// Gets or sets settings that allow extension owners or members of a shared line group to check voicemails for extension numbers over the phone using PIN code.
		/// </summary>
		[JsonPropertyName("check_voicemails_over_phone")]
		public CheckVoicemailsOverPhoneSettings CheckVoicemailsOverPhone { get; set; }

		/// <summary>
		/// Gets or sets settings that allow users to use call delegation.
		/// </summary>
		[JsonPropertyName("delegation")]
		public DelegationSettings Delegation { get; set; }

		/// <summary>
		/// Gets or sets settings that allow to display call feedback at the end of the calls.
		/// </summary>
		[JsonPropertyName("display_call_feedback_survey")]
		public DisplayCallFeedbackSurveySettings DisplayCallFeedbackSurvey { get; set; }

		/// <summary>
		/// Gets or sets settings that allow users to switch their calls to End-to-End Encryption.
		/// </summary>
		[JsonPropertyName("e2e_encryption")]
		public EndToEndEncryptionSettings EndToEndEncryption { get; set; }

		/// <summary>
		/// Gets or sets settings that allow users to elevate their phone calls to a meeting.
		/// </summary>
		[JsonPropertyName("elevate_to_meeting")]
		public ElevateToMeetingSettings ElevateToMeeting { get; set; }

		/// <summary>
		/// Gets or sets settings that allow users to send a call to a Zoom Room.
		/// </summary>
		[JsonPropertyName("hand_off_to_room")]
		public HandOffToRoomSettings HandOffToRoom { get; set; }

		/// <summary>
		/// Gets or sets settings that allow extensions to place international calls outside of the calling plan.
		/// </summary>
		[JsonPropertyName("international_calling")]
		public InternationalCallingSettings InternationalCalling { get; set; }

		/// <summary>
		/// Gets or sets settings that allow user or extension to have core phone services in the event of an outage.
		/// </summary>
		[JsonPropertyName("local_survivability_mode")]
		public LocalSurvivabilityModeSettings LocalSurvivabilityMode { get; set; }

		/// <summary>
		/// Gets or sets settings that allow users to switch from a Zoom Phone to their native carrier.
		/// </summary>
		[JsonPropertyName("mobile_switch_to_carrier")]
		public MobileSwitchToCarrierSettings MobileSwitchToCarrier { get; set; }

		/// <summary>
		/// Gets or sets settings that allow to define calling rules to restrict user or extension from calling specific countries, cities, or numbers.
		/// </summary>
		[JsonPropertyName("outbound_calling")]
		public OutboundCallingSettings OutboundCalling { get; set; }

		/// <summary>
		/// Gets or sets settings that allow users to send and receive messages.
		/// </summary>
		[JsonPropertyName("outbound_sms")]
		public OutboundSmsSettings OutboundSms { get; set; }

		/// <summary>
		/// Gets or sets settings that allow Zoom clients to send media directly to each other.
		/// </summary>
		[JsonPropertyName("peer_to_peer_media")]
		public PeerToPeerMediaSettings PeerToPeerMedia { get; set; }

		/// <summary>
		/// Gets or sets settings that control user's personal audio library.
		/// </summary>
		[JsonPropertyName("personal_audio_library")]
		public PersonalAudioLibrarySettings PersonalAudioLibrary { get; set; }

		/// <summary>
		/// Gets or sets settings that allow to define when the extension or user cannot make or accept calls and send SMS.
		/// </summary>
		[JsonPropertyName("restricted_call_hours")]
		public RestrictedCallHoursSettings RestrictedCallHours { get; set; }

		/// <summary>
		/// Gets or sets settings that allow extensions to change outbound caller ID when placing calls.
		/// </summary>
		[JsonPropertyName("select_outbound_caller_id")]
		public SelectOutboundCallerIdSettings SelectOutboundCallerId { get; set; }

		/// <summary>
		/// Gets or sets settings that allow users to receive email notification when there is a new shared voicemail or videomail.
		/// </summary>
		[JsonPropertyName("shared_voicemail_notification_by_email")]
		public SharedVoicemailNotificationByEmailSettings SharedVoicemailNotificationByEmail { get; set; }

		/// <summary>
		/// Gets or sets settings that allow users to send and receive messages.
		/// </summary>
		[JsonPropertyName("sms")]
		public SmsSettings Sms { get; set; }

		/// <summary>
		/// Gets or sets settings that allow to define keywords and text patterns over SMS and prevents users from sharing unwanted messages.
		/// </summary>
		[JsonPropertyName("sms_etiquette_tool")]
		public SmsEtiquetteToolSettings SmsEtiquetteTool { get; set; }

		/// <summary>
		/// Gets or sets settings that control voicemail and videomail access and behavior.
		/// </summary>
		[JsonPropertyName("voicemail")]
		public VoicemailSettings Voicemail { get; set; }

		/// <summary>
		/// Gets or sets settings that allow to control voicemail and videomail email notifications.
		/// </summary>
		[JsonPropertyName("voicemail_notification_by_email")]
		public VoicemailNotificationByEmailSettings VoicemailNotificationByEmail { get; set; }

		/// <summary>
		/// Gets or sets settings that allow voicemail transcription.
		/// </summary>
		[JsonPropertyName("voicemail_transcription")]
		public VoicemailTranscriptionSettings VoicemailTranscription { get; set; }

		/// <summary>
		/// Gets or sets settings that allow to use Zoom Phone on desktop clients.
		/// </summary>
		[JsonPropertyName("zoom_phone_on_desktop")]
		public ZoomPhoneOnDesktopSettings ZoomPhoneOnDesktop { get; set; }

		/// <summary>
		/// Gets or sets settings that allow to use Zoom Phone on mobile clients.
		/// </summary>
		[JsonPropertyName("zoom_phone_on_mobile")]
		public ZoomPhoneOnMobileSettings ZoomPhoneOnMobile { get; set; }

		/// <summary>
		/// Gets or sets settings that allow to use Zoom Phone on Zoom Progressive Web App.
		/// </summary>
		[JsonPropertyName("zoom_phone_on_pwa")]
		public ZoomPhoneOnPwaSettings ZoomPhoneOnPwa { get; set; }
	}
}
