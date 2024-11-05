// Ignore Spelling: Voicemail

using System.Text.Json.Serialization;

namespace ZoomNet.Models.CallHandlingSettings
{
	/// <summary>
	/// The model of call handling subsettings.
	/// </summary>
	public class CallHandlingSubsettings : CallHandlingSubsettingsBase
	{
		/// <summary>
		/// Gets or sets a value indicating whether to allow the callers to check voicemails.
		/// This field is only available in the following scenarios:
		/// - The <see cref="CallNotAnswerAction"/> is set to 1 (Forward to a voicemail).
		/// - The <see cref="BusyOnAnotherCallAction"/> is set to 1 (Forward to a voicemail).(Only applicable to the User)
		/// </summary>
		[JsonPropertyName("allow_callers_check_voicemail")]
		public bool? AllowCallersCheckVoicemail { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether the queue members to set their own business hours.
		/// This field allows queue members' business Hours to override the default hours of the call queue.
		/// </summary>
		[JsonPropertyName("allow_members_to_reset")]
		public bool? AllowMembersToReset { get; set; }

		/// <summary>
		/// Gets or sets the audio while connecting the prompt ID.
		/// This option can select the audio played for the inbound callers when they are waiting to be routed to the next available call queue member.
		/// Options:
		/// - empty char - default;
		/// - 0 - disable.
		/// </summary>
		[JsonPropertyName("audio_while_connecting_id")]
		public string AudioWhileConnectingId { get; set; }

		/// <summary>
		/// Gets or sets the action to take when a call is not answered.
		/// </summary>
		[JsonPropertyName("call_not_answer_action")]
		public CallNotAnswerActionType? CallNotAnswerAction { get; set; }

		/// <summary>
		/// Gets or sets the action to take when the user is busy on another call.
		/// </summary>
		[JsonPropertyName("busy_on_another_call_action")]
		public BusyOnAnotherCallActionType? BusyOnAnotherCallAction { get; set; }

		/// <summary>
		/// Gets or sets this option to distribute incoming calls.
		/// If Sequential or Rotating ring mode is selected <see cref="CallDistributionSettings.RingMode"/>,
		/// calls will ring for a specific time before trying the next available queue member.
		/// </summary>
		[JsonPropertyName("busy_on_another_call_action")]
		public CallDistributionSettings CallDistribution { get; set; }

		/// <summary>
		/// Gets or sets the value indicating whether the receiver needs to press 1 before connecting the call for it to be forwarded to an external contact or a number,
		/// when one is busy on another call.
		/// This option ensures that forwarded calls won't reach the voicemail box for the external contact or a number.
		/// </summary>
		[JsonPropertyName("busy_require_press_1_before_connecting")]
		public bool? BusyRequirePress1BeforeConnecting { get; set; }

		/// <summary>
		/// Gets or sets the value indicating whether press 1 before connecting the call to forward to an external contact or a number,
		/// when a call is unanswered.
		/// This option ensures that forwarded calls won't reach the voicemail box for the external contact or a number.
		/// </summary>
		/// <remarks>This field is only available if the <see cref="CallNotAnswerAction"/> is set to:
		/// - 9(Forward to an External Contact);
		/// - 10(Forward to a Phone Number).
		/// </remarks>
		[JsonPropertyName("un_answered_require_press_1_before_connecting")]
		public bool? UnAnsweredRequirePress1BeforeConnecting { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether to play callee's voicemail greeting when the caller reaches the end of the forwarding sequence.
		/// This field is only available if the <see cref="CallNotAnswerAction"/> setting is set to:
		/// - <see cref="CallNotAnswerActionType.ForwardToUser"/>,
		/// - <see cref="CallNotAnswerActionType.ForwardToZoomRoom"/>,
		/// - <see cref="CallNotAnswerActionType.ForwardToCommonArea"/>,
		/// - <see cref="CallNotAnswerActionType.ForwardToCiscoOrPolycomRoom"/>,
		/// - <see cref="CallNotAnswerActionType.ForwardToAutoReceptionist"/>,
		/// - <see cref="CallNotAnswerActionType.ForwardToCallQueue"/>,
		/// - <see cref="CallNotAnswerActionType.ForwardToSharedLineGroup"/>.
		/// </summary>
		[JsonPropertyName("overflow_play_callee_voicemail_greeting")]
		public bool? OverflowPlayCalleeVoicemailGreeting { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether to play callee's voicemail greeting when the caller reaches the end of forwarding sequence.
		/// This field is only available in the following scenarios:
		/// <list type="bullet">
		/// <item>The <see cref="CallNotAnswerAction"/> is set to <see cref="CallNotAnswerActionType.ForwardToVoicemail"/> and the <see cref="ConnectToOperator"/> is true.</item>
		/// <item>The <see cref="BusyOnAnotherCallAction"/> is set to <see cref="BusyOnAnotherCallActionType.ForwardToVoicemail"/> and the <see cref="ConnectToOperator"/> is true.</item>
		/// </list>
		/// </summary>
		[JsonPropertyName("play_callee_voicemail_greeting")]
		public bool? PlayCalleeVoicemailGreeting { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether to allow callers to reach an operator.
		/// This field is only available in the following scenarios:
		/// <list type="bullet">
		/// <item>The <see cref="CallNotAnswerAction"/> is set to <see cref="CallNotAnswerActionType.ForwardToVoicemail"/>.</item>
		/// <item>The <see cref="BusyOnAnotherCallAction"/> is set to <see cref="BusyOnAnotherCallActionType.ForwardToVoicemail"/>.</item>
		/// </list>
		/// </summary>
		[JsonPropertyName("connect_to_operator ")]
		public bool? ConnectToOperator { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether to play callee's voicemail greeting when the caller reaches the end of the forwarding sequence.
		/// It displays when <see cref="BusyOnAnotherCallAction"/> is set to one of the following values:
		/// <list type="bullet">
		/// <item><see cref="BusyOnAnotherCallActionType.ForwardToUser"/></item>
		/// <item><see cref="BusyOnAnotherCallActionType.ForwardToCommonArea"/></item>
		/// <item><see cref="BusyOnAnotherCallActionType.ForwardToAutoReceptionist"/></item>
		/// <item><see cref="BusyOnAnotherCallActionType.ForwardToCallQueue"/></item>
		/// <item><see cref="BusyOnAnotherCallActionType.ForwardToSharedLineGroup"/></item>
		/// <item><see cref="BusyOnAnotherCallActionType.ForwardToExternalContact"/></item>
		/// <item><see cref="BusyOnAnotherCallActionType.ForwardToPhoneNumber"/></item>
		/// </list>
		/// </summary>
		[JsonPropertyName("busy_play_callee_voicemail_greeting")]
		public bool? BusyPlayCalleeVoicemailGreeting { get; set; }

		/// <summary>
		/// Gets or sets the extension's phone number or forward to an external number in E.164 format.
		/// This field is only available if <see cref="CallNotAnswerAction"/> is set to <see cref="CallNotAnswerActionType.ForwardToPhoneNumber"/>.
		/// </summary>
		[JsonPropertyName("phone_number")]
		public string PhoneNumber { get; set; }

		/// <summary>
		/// Gets or sets description of the phone number to which the call is forwarded.
		/// This field is only available if <see cref="CallNotAnswerAction"/> is set to <see cref="CallNotAnswerActionType.ForwardToPhoneNumber"/>.
		/// </summary>
		[JsonPropertyName("description")]
		public string Description { get; set; }

		/// <summary>
		/// Gets or sets the extension's phone number or forward to an external number in E.164 format.
		/// It sets when <see cref="BusyOnAnotherCallAction"/> action is set to <see cref="BusyOnAnotherCallActionType.ForwardToPhoneNumber"/>.
		/// </summary>
		[JsonPropertyName("busy_phone_number")]
		public string BusyPhoneNumber { get; set; }

		/// <summary>
		/// Gets or sets description of the phone number to which the call is forwarded.
		/// It sets when <see cref="BusyOnAnotherCallAction"/> action is set to <see cref="BusyOnAnotherCallActionType.ForwardToPhoneNumber"/>.
		/// </summary>
		[JsonPropertyName("busy_description")]
		public string BusyDescription { get; set; }

		/// <summary>
		/// Gets or sets the forwarding extension ID.
		/// This field is only available in the following scenarios:
		/// <list type="number">
		///     <item>
		///         When <see cref="CallNotAnswerAction"/> is set to <see cref="CallNotAnswerActionType.ForwardToVoicemail"/> for unanswered calls,
		///         this field is used to set the specific extension to which voicemails are forwarded. This scenario applies to Auto Receptionist and Call Queue.
		///     </item>
		///     <item>
		///         When <see cref="CallNotAnswerAction"/> is set to one of the following values:
		///         <list type="bullet">
		///             <item><see cref="CallNotAnswerActionType.ForwardToUser"/></item>
		///             <item><see cref="CallNotAnswerActionType.ForwardToZoomRoom"/></item>
		///             <item><see cref="CallNotAnswerActionType.ForwardToCommonArea"/></item>
		///             <item><see cref="CallNotAnswerActionType.ForwardToCiscoOrPolycomRoom"/></item>
		///             <item><see cref="CallNotAnswerActionType.ForwardToAutoReceptionist"/></item>
		///             <item><see cref="CallNotAnswerActionType.ForwardToCallQueue"/></item>
		///             <item><see cref="CallNotAnswerActionType.ForwardToSharedLineGroup"/></item>
		///             <item><see cref="CallNotAnswerActionType.ForwardToExternalContact"/></item>
		///         </list>
		///     </item>
		/// </list>
		/// </summary>
		[JsonPropertyName("forward_to_extension_id")]
		public string ForwardToExtensionId { get; set; }

		/// <summary>
		/// Gets or sets the forwarding extension ID that's required only when <see cref="BusyOnAnotherCallAction"/> setting is set to:
		/// <list type="bullet">
		/// <item><see cref="BusyOnAnotherCallActionType.ForwardToUser"/></item>
		/// <item><see cref="BusyOnAnotherCallActionType.ForwardToCommonArea"/></item>
		/// <item><see cref="BusyOnAnotherCallActionType.ForwardToAutoReceptionist"/></item>
		/// <item><see cref="BusyOnAnotherCallActionType.ForwardToCallQueue"/></item>
		/// <item><see cref="BusyOnAnotherCallActionType.ForwardToSharedLineGroup"/></item>
		/// <item><see cref="BusyOnAnotherCallActionType.ForwardToExternalContact"/></item>
		/// </list>
		/// </summary>
		[JsonPropertyName("busy_forward_to_extension_id")]
		public string BusyForwardToExtensionId { get; set; }

		/// <summary>
		/// Gets or sets the greeting audio prompt ID.
		/// Options: empty char - default and 0 - disable
		/// This is only required for the Call Queue or Auto Receptionist call_handling sub-setting.
		/// </summary>
		[JsonPropertyName("greeting_prompt_id")]
		public string GreetingPromptId { get; set; }

		/// <summary>
		/// Gets or sets the maximum number of calls in queue. Specify the maximum number of callers to place in the queue.
		/// When this number is exceeded, callers will be routed based on the overflow option. Up to 60.
		/// It's required for the Call Queue call_handling sub-setting.
		/// </summary>
		[JsonPropertyName("max_call_in_queue")]
		public int? MaxCallInQueue { get; set; }

		/// <summary>
		/// Gets or sets the maximum wait time, in seconds, for <see cref="RingModeType.Simultaneous"/> ring mode
		/// or the ring duration for each device for <see cref="RingModeType.Sequential"/> ring mode.
		/// <see href="https://developers.zoom.us/docs/api/phone/#tag/call-handling/PATCH/phone/extension/{extensionId}/call_handling/settings/{settingType}"/> to check allowed values.
		/// </summary>
		[JsonPropertyName("max_wait_time")]
		public int? MaxWaitTime { get; set; }

		/// <summary>
		/// Gets or sets the music on hold prompt ID. This field is an option to choose music for inbound callers when they're placed on hold by a call queue member.
		/// Options: empty char - default and 0 - disable.
		/// Only required for the Call Queue call_handling sub-setting.
		/// </summary>
		[JsonPropertyName("music_on_hold_id")]
		public string MusicOnHoldId { get; set; }

		/// <summary>
		/// Gets or sets the extension ID of the operator to whom the call is being forwarded.
		/// This field is only available in the following scenarios:
		/// <list type="number">
		/// <item><see cref="CallNotAnswerAction"/> is set to <see cref="CallNotAnswerActionType.ForwardToVoicemail"/> and <see cref="ConnectToOperator"/> is true.</item>
		/// <item><see cref="BusyOnAnotherCallAction"/> is set to <see cref="BusyOnAnotherCallActionType.ForwardToVoicemail"/> and <see cref="ConnectToOperator"/> is true. (Only applicable to the User)</item>
		/// </list>
		/// </summary>
		[JsonPropertyName("operator_extension_id")]
		public string OperatorExtensionId { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether call queue members can receive new incoming calls notification even on the call.
		/// This field receives calls while on a call.
		/// It's required for the Call Queue call handling sub-setting.
		/// </summary>
		[JsonPropertyName("receive_call")]
		public bool? ReceiveCall { get; set; }

		/// <summary>
		/// Gets or sets the call handling ring mode, only allowed values:
		/// <list type="bullet">
		/// <item><see cref="RingModeType.Simultaneous"/></item>
		/// <item><see cref="RingModeType.Sequential"/></item>
		/// </list>
		/// </summary>
		[JsonPropertyName("ring_mode")]
		public RingModeType? RingMode { get; set; }

		/// <summary>
		/// Gets or sets the voicemail greeting prompt ID.
		/// This field is only available in the following scenarios:
		/// <list type="bullet">
		/// <item><see cref="CallNotAnswerAction"/> is set to <see cref="CallNotAnswerActionType.ForwardToVoicemail"/></item>
		/// <item><see cref="BusyOnAnotherCallAction"/> is set to <see cref="BusyOnAnotherCallActionType.ForwardToVoicemail"/> (Only applicable to the User)</item>
		/// </list>
		/// </summary>
		[JsonPropertyName("voicemail_greeting_id")]
		public string VoicemailGreetingId { get; set; }

		/// <summary>
		/// Gets or sets the voicemail leaving instruction prompt ID.
		/// This field is only available in the following scenarios:
		/// <list type="bullet">
		/// <item><see cref="CallNotAnswerAction"/> is set to <see cref="CallNotAnswerActionType.ForwardToVoicemail"/> and either <see cref="ConnectToOperator"/> or <see cref="AllowCallersCheckVoicemail"/> is set to true</item>
		/// <item><see cref="BusyOnAnotherCallAction"/> is set to <see cref="BusyOnAnotherCallActionType.ForwardToVoicemail"/> (Only applicable to the User),  and either <see cref="ConnectToOperator"/> or <see cref="AllowCallersCheckVoicemail"/> is set to true</item>
		/// </list>
		/// </summary>
		[JsonPropertyName("voicemail_leaving_instruction_id")]
		public string VoicemailLeavingInstructionId { get; set; }

		/// <summary>
		/// Gets or sets the message greeting prompt ID.
		/// This field is only available if <see cref="CallNotAnswerAction"/> is set to <see cref="CallNotAnswerActionType.PlayMessageThenDisconnect"/>.
		/// </summary>
		[JsonPropertyName("message_greeting_id")]
		public string MessageGreetingId { get; set; }

		/// <summary>
		/// Gets or sets the Zoom Contact Center phone number, in E.164 format, to which the call is forwarded.
		/// This field is only available if <see cref="CallNotAnswerAction"/> is set to <see cref="CallNotAnswerActionType.ForwardToZoomContactCenter"/>.
		/// </summary>
		[JsonPropertyName("forward_to_zcc_phone_number")]
		public string ForwardToZccPhoneNumber { get; set; }

		/// <summary>
		/// Gets or sets the Partner Contact Center Setting ID to which the call is forwarded.
		/// This field is only available if <see cref="CallNotAnswerAction"/> is set to <see cref="CallNotAnswerActionType.ForwardToPartnerContactCenter"/>.
		/// </summary>
		[JsonPropertyName("forward_to_partner_contact_center_id")]
		public string ForwardToPartnerContactCenterId { get; set; }

		/// <summary>
		/// Gets or sets the Microsoft Teams Voice App ID to which the call is forwarded.
		/// This field is only available if <see cref="CallNotAnswerAction"/> is set to <see cref="CallNotAnswerActionType.ForwardToMicrosoftTeamsResourceAccount"/>.
		/// </summary>
		[JsonPropertyName("forward_to_teams_id")]
		public string ForwardToTeamsId { get; set; }

		/// <summary>
		/// Gets or sets the wrap up time in seconds. Specify the duration before the next queue call is routed to a member in call queue.
		/// See the <see href="https://developers.zoom.us/docs/api/phone/#tag/call-handling/PATCH/phone/extension/{extensionId}/call_handling/settings/{settingType}"/>  to check allowed values.
		/// </summary>
		[JsonPropertyName("wrap_up_time")]
		public int? WrapUpTime { get; set; }

		/// <summary>
		/// Gets the type of sub-setting.
		/// </summary>
		[JsonIgnore]
		public override CallHandlingSubsettingsType SubsettingType => CallHandlingSubsettingsType.CallHandling;
	}
}
