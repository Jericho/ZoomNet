using System;
using Shouldly;
using Xunit;
using ZoomNet.Models;
using ZoomNet.Models.Webhooks;
using ZoomNet.UnitTests.Properties;

namespace ZoomNet.UnitTests
{
	/// <summary>
	/// Unit tests that verify phone webhook events parsing.
	/// </summary>
	public partial class WebhookParserTests
	{
		#region private fields

		private const string CallId = "6998252113337041462";
		private const string PhoneNumberA = "+12092592844";
		private const string PhoneNumberB = "+12058945456";
		private const string CallElementId = "20231008-48c1dfd4-91ce-4df5-8495-7c9e33d10869";
		private const string CallHistoryId = "20231008-1ac1df2a-912e-d125-8a15-1a1233d10f1a";
		private const string SiteId = "8f71O6rWT8KFUGQmJIFAdQ";
		private const string RecordingId = "c71b360f6e774e3aa101453117b7e1a7";
		private const string ChannelId = "5c01a45b-53a8-4773-b244-beb670c13357";
		private const string SipId = "VeSbXj3yttX-ghGskIP_ew..d90446d0@10002222.zoomdev.us";

		private static readonly DateTime answeredTimestamp = new DateTime(2021, 8, 19, 21, 12, 38, DateTimeKind.Utc);
		private static readonly DateTime ringingStartedTimestamp = new DateTime(2021, 8, 19, 21, 12, 28, DateTimeKind.Utc);
		private static readonly DateTime callEndedTimestamp = new DateTime(2021, 8, 19, 21, 12, 44, DateTimeKind.Utc);

		#endregion

		#region tests

		[Fact]
		public void PhoneCalleeAnswered()
		{
			var parsedEvent = ParseWebhookEvent<PhoneCalleeAnsweredEvent>(Resource.phone_callee_answered_webhook);

			VerifyPhoneCallFlowEvent(parsedEvent, ZoomNet.Models.Webhooks.EventType.PhoneCalleeAnswered);

			parsedEvent.AnsweredOn.ShouldBe(answeredTimestamp);
			parsedEvent.RingingStartedOn.ShouldBe(ringingStartedTimestamp);

			VerifyVoipParty(parsedEvent.CallInfo.Caller, extensionNumber: null, extensionType: null);
			VerifyZoomUserParty(parsedEvent.CallInfo.Callee, extensionId: null, extensionType: null);
			VerifyForwardedBy(parsedEvent.ForwardedBy);
			VerifyRedirectForwardedBy(parsedEvent.RedirectForwardedBy);
		}

		[Fact]
		public void PhoneCalleeEnded()
		{
			var parsedEvent = ParseWebhookEvent<PhoneCalleeEndedEvent>(Resource.phone_callee_ended_webhook);

			VerifyPhoneCallFlowEvent(parsedEvent, ZoomNet.Models.Webhooks.EventType.PhoneCalleeEnded);

			parsedEvent.AnsweredOn.ShouldBe(answeredTimestamp);
			parsedEvent.EndedOn.ShouldBe(callEndedTimestamp);
			parsedEvent.RingingStartedOn.ShouldBe(ringingStartedTimestamp);

			VerifyVoipParty(parsedEvent.CallInfo.Caller, extensionNumber: null, extensionType: null);
			VerifyZoomUserParty(parsedEvent.CallInfo.Callee);
			VerifyForwardedBy(parsedEvent.ForwardedBy);
			VerifyRedirectForwardedBy(parsedEvent.RedirectForwardedBy);
		}

		[Fact]
		public void PhoneCalleeHold()
		{
			var parsedEvent = ParseWebhookEvent<PhoneCalleeHoldEvent>(Resource.phone_callee_hold_webhook);

			VerifyPhoneCallFlowEvent(parsedEvent, ZoomNet.Models.Webhooks.EventType.PhoneCalleeHold);

			parsedEvent.HoldStartedOn.ShouldBe(timestamp);

			VerifyVoipParty(parsedEvent.CallInfo.Caller);
			VerifyZoomUserParty(parsedEvent.CallInfo.Callee, deviceType: null);
		}

		[Fact]
		public void PhoneCalleeMeetingInviting()
		{
			var parsedEvent = ParseWebhookEvent<PhoneCalleeMeetingInvitingEvent>(Resource.phone_callee_meeting_inviting_webhook);

			VerifyPhoneCallFlowEvent(parsedEvent, ZoomNet.Models.Webhooks.EventType.PhoneCalleeMeetingInviting);

			parsedEvent.EscalatedAt.ShouldBe(timestamp);
			parsedEvent.MeetingId.ShouldBe("987654321");

			VerifyVoipParty(parsedEvent.CallInfo.Caller);
			VerifyZoomUserParty(parsedEvent.CallInfo.Callee, deviceType: null);
		}

		[Fact]
		public void PhoneCalleeMissed()
		{
			var parsedEvent = ParseWebhookEvent<PhoneCalleeMissedEvent>(Resource.phone_callee_missed_webhook);

			VerifyPhoneCallFlowEvent(parsedEvent, ZoomNet.Models.Webhooks.EventType.PhoneCalleeMissed);

			parsedEvent.RingingStartedOn.ShouldBe(ringingStartedTimestamp);
			parsedEvent.EndedOn.ShouldBe(callEndedTimestamp);
			parsedEvent.HandupResult.ShouldBe("No Answer");

			VerifyVoipParty(parsedEvent.CallInfo.Caller, extensionNumber: null, extensionType: null);
			VerifyZoomUserParty(parsedEvent.CallInfo.Callee);
			VerifyForwardedBy(parsedEvent.ForwardedBy);
			VerifyRedirectForwardedBy(parsedEvent.RedirectForwardedBy);
		}

		[Fact]
		public void PhoneCalleeMute()
		{
			var parsedEvent = ParseWebhookEvent<PhoneCalleeMuteEvent>(Resource.phone_callee_mute_webhook);

			VerifyPhoneCallFlowEvent(parsedEvent, ZoomNet.Models.Webhooks.EventType.PhoneCalleeMute);

			parsedEvent.MutedOn.ShouldBe(timestamp);

			VerifyVoipParty(parsedEvent.CallInfo.Caller);
			VerifyZoomUserParty(parsedEvent.CallInfo.Callee, deviceType: null);
		}

		[Fact]
		public void PhoneCalleeParked()
		{
			var parsedEvent = ParseWebhookEvent<PhoneCalleeParkedEvent>(Resource.phone_callee_parked_webhook);

			VerifyPhoneCallFlowEvent(parsedEvent, ZoomNet.Models.Webhooks.EventType.PhoneCalleeParked);

			parsedEvent.ParkedOn.ShouldBe(timestamp);
			parsedEvent.ParkCode.ShouldBe("*869");
			parsedEvent.ParkFailureReason.ShouldBe("Park successfully.");

			VerifyVoipParty(parsedEvent.CallInfo.Caller);
			VerifyZoomUserParty(parsedEvent.CallInfo.Callee, deviceType: null);
		}

		[Fact]
		public void PhoneCalleeRejected()
		{
			var parsedEvent = ParseWebhookEvent<PhoneCalleeRejectedEvent>(Resource.phone_callee_rejected_webhook);

			VerifyPhoneCallFlowEvent(parsedEvent, ZoomNet.Models.Webhooks.EventType.PhoneCalleeRejected);

			parsedEvent.RingingStartedOn.ShouldBe(ringingStartedTimestamp);
			parsedEvent.EndedOn.ShouldBe(callEndedTimestamp);
			parsedEvent.HandupResult.ShouldBeNull();

			VerifyVoipParty(parsedEvent.CallInfo.Caller, deviceType: "Windows_Client(5.7.5.939)");
			VerifyZoomUserParty(parsedEvent.CallInfo.Callee, deviceType: "Windows_Client(5.7.5.939)");
		}

		[Fact]
		public void PhoneCalleeRinging()
		{
			var parsedEvent = ParseWebhookEvent<PhoneCalleeRingingEvent>(Resource.phone_callee_ringing_webhook);

			VerifyPhoneCallFlowEvent(parsedEvent, ZoomNet.Models.Webhooks.EventType.PhoneCalleeRinging);

			parsedEvent.RingingStartedOn.ShouldBe(ringingStartedTimestamp);

			VerifyVoipParty(parsedEvent.CallInfo.Caller, extensionNumber: null, extensionType: null);
			VerifyZoomUserParty(parsedEvent.CallInfo.Callee, extensionId: null);
			VerifyForwardedBy(parsedEvent.ForwardedBy);
			VerifyRedirectForwardedBy(parsedEvent.RedirectForwardedBy);
		}

		[Fact]
		public void PhoneCalleeUnhold()
		{
			var parsedEvent = ParseWebhookEvent<PhoneCalleeUnholdEvent>(Resource.phone_callee_unhold_webhook);

			VerifyPhoneCallFlowEvent(parsedEvent, ZoomNet.Models.Webhooks.EventType.PhoneCalleeUnhold);

			parsedEvent.HoldEndedOn.ShouldBe(timestamp);

			VerifyVoipParty(parsedEvent.CallInfo.Caller);
			VerifyZoomUserParty(parsedEvent.CallInfo.Callee, deviceType: null);
		}

		[Fact]
		public void PhoneCalleeUnmute()
		{
			var parsedEvent = ParseWebhookEvent<PhoneCalleeUnmuteEvent>(Resource.phone_callee_unmute_webhook);

			VerifyPhoneCallFlowEvent(parsedEvent, ZoomNet.Models.Webhooks.EventType.PhoneCalleeUnmute);

			parsedEvent.UnmutedOn.ShouldBe(timestamp);

			VerifyVoipParty(parsedEvent.CallInfo.Caller);
			VerifyZoomUserParty(parsedEvent.CallInfo.Callee, deviceType: null);
		}

		[Fact]
		public void PhoneCallerConnected()
		{
			var parsedEvent = ParseWebhookEvent<PhoneCallerConnectedEvent>(Resource.phone_caller_connected_webhook);

			VerifyPhoneCallFlowEvent(parsedEvent, ZoomNet.Models.Webhooks.EventType.PhoneCallerConnected);

			parsedEvent.RingingStartedOn.ShouldBe(ringingStartedTimestamp);
			parsedEvent.ConnectedOn.ShouldBe(answeredTimestamp);

			VerifyVoipParty(parsedEvent.CallInfo.Callee, extensionType: null);
			VerifyZoomUserParty(parsedEvent.CallInfo.Caller);
		}

		[Fact]
		public void PhoneCallerEnded()
		{
			var parsedEvent = ParseWebhookEvent<PhoneCallerEndedEvent>(Resource.phone_caller_ended_webhook);

			VerifyPhoneCallFlowEvent(parsedEvent, ZoomNet.Models.Webhooks.EventType.PhoneCallerEnded);

			parsedEvent.RingingStartedOn.ShouldBe(ringingStartedTimestamp);
			parsedEvent.AnsweredOn.ShouldBe(answeredTimestamp);
			parsedEvent.EndedOn.ShouldBe(callEndedTimestamp);
			parsedEvent.HandupResult.ShouldBeNull();

			VerifyVoipParty(parsedEvent.CallInfo.Callee);
			VerifyZoomUserParty(parsedEvent.CallInfo.Caller);
		}

		[Fact]
		public void PhoneCallerHold()
		{
			var parsedEvent = ParseWebhookEvent<PhoneCallerHoldEvent>(Resource.phone_caller_hold_webhook);

			VerifyPhoneCallFlowEvent(parsedEvent, ZoomNet.Models.Webhooks.EventType.PhoneCallerHold);

			parsedEvent.HoldStartedOn.ShouldBe(timestamp);

			VerifyVoipParty(parsedEvent.CallInfo.Callee);
			VerifyZoomUserParty(parsedEvent.CallInfo.Caller, deviceType: null);
		}

		[Fact]
		public void PhoneCallerMeetingInviting()
		{
			var parsedEvent = ParseWebhookEvent<PhoneCallerMeetingInvitingEvent>(Resource.phone_caller_meeting_inviting_webhook);

			VerifyPhoneCallFlowEvent(parsedEvent, ZoomNet.Models.Webhooks.EventType.PhoneCallerMeetingInviting);

			parsedEvent.EscalatedAt.ShouldBe(timestamp);
			parsedEvent.MeetingId.ShouldBe("987654321");

			VerifyVoipParty(parsedEvent.CallInfo.Callee);
			VerifyZoomUserParty(parsedEvent.CallInfo.Caller, deviceType: null);
		}

		[Fact]
		public void PhoneCallerMute()
		{
			var parsedEvent = ParseWebhookEvent<PhoneCallerMuteEvent>(Resource.phone_caller_mute_webhook);

			VerifyPhoneCallFlowEvent(parsedEvent, ZoomNet.Models.Webhooks.EventType.PhoneCallerMute);

			parsedEvent.MutedOn.ShouldBe(timestamp);

			VerifyVoipParty(parsedEvent.CallInfo.Callee);
			VerifyZoomUserParty(parsedEvent.CallInfo.Caller, deviceType: null);
		}

		[Fact]
		public void PhoneCallerRinging()
		{
			var parsedEvent = ParseWebhookEvent<PhoneCallerRingingEvent>(Resource.phone_caller_ringing_webhook);

			VerifyPhoneCallFlowEvent(parsedEvent, ZoomNet.Models.Webhooks.EventType.PhoneCallerRinging);

			parsedEvent.RingingStartedOn.ShouldBe(ringingStartedTimestamp);

			VerifyVoipParty(parsedEvent.CallInfo.Callee);
			VerifyZoomUserParty(parsedEvent.CallInfo.Caller);
		}

		[Fact]
		public void PhoneCallerUnhold()
		{
			var parsedEvent = ParseWebhookEvent<PhoneCallerUnholdEvent>(Resource.phone_caller_unhold_webhook);

			VerifyPhoneCallFlowEvent(parsedEvent, ZoomNet.Models.Webhooks.EventType.PhoneCallerUnhold);

			parsedEvent.HoldEndedOn.ShouldBe(timestamp);

			VerifyVoipParty(parsedEvent.CallInfo.Callee);
			VerifyZoomUserParty(parsedEvent.CallInfo.Caller, deviceType: null);
		}

		[Fact]
		public void PhoneCallerUnmute()
		{
			var parsedEvent = ParseWebhookEvent<PhoneCallerUnmuteEvent>(Resource.phone_caller_unmute_webhook);

			VerifyPhoneCallFlowEvent(parsedEvent, ZoomNet.Models.Webhooks.EventType.PhoneCallerUnmute);

			parsedEvent.UnmutedOn.ShouldBe(timestamp);

			VerifyVoipParty(parsedEvent.CallInfo.Callee);
			VerifyZoomUserParty(parsedEvent.CallInfo.Caller, deviceType: null);
		}

		[Fact]
		public void PhoneCallElementDeleted()
		{
			var parsedEvent = ParseWebhookEvent<PhoneCallElementDeletedEvent>(Resource.phone_call_element_deleted_webhook);

			VerifyPhoneCallLogOperationEvent(parsedEvent, ZoomNet.Models.Webhooks.EventType.PhoneCallElementDeleted);

			parsedEvent.DeleteAll.ShouldBeFalse();
			parsedEvent.MoveToTrash.ShouldBeTrue();
			parsedEvent.ExecuteTime.ShouldBe(timestamp);

			parsedEvent.CallElementIds.ShouldNotBeNull();
			parsedEvent.CallElementIds.ShouldHaveSingleItem();
			parsedEvent.CallElementIds[0].ShouldBe("20240724-9a0887ca-da53-4c62-b32a-563789ef264e");
		}

		[Fact]
		public void PhoneCallHistoryDeleted()
		{
			var parsedEvent = ParseWebhookEvent<PhoneCallHistoryDeletedEvent>(Resource.phone_call_history_deleted_webhook);

			VerifyPhoneCallLogOperationEvent(parsedEvent, ZoomNet.Models.Webhooks.EventType.PhoneCallHistoryDeleted);

			parsedEvent.DeleteAll.ShouldBeFalse();
			parsedEvent.MoveToTrash.ShouldBeTrue();
			parsedEvent.ExecuteTime.ShouldBe(timestamp);

			parsedEvent.CallLogIds.ShouldNotBeNull();
			parsedEvent.CallLogIds.ShouldHaveSingleItem();
			parsedEvent.CallLogIds[0].ShouldBe("20240724-9a0887ca-da53-4c62-b32a-563789ef264e");
		}

		[Fact]
		public void PhoneCallLogDeleted()
		{
			var parsedEvent = ParseWebhookEvent<PhoneCallLogDeletedEvent>(Resource.phone_call_log_deleted_webhook);

			VerifyPhoneCallLogOperationEvent(parsedEvent, ZoomNet.Models.Webhooks.EventType.PhoneCallLogDeleted);

			parsedEvent.CallLogs.ShouldNotBeNull();
			parsedEvent.CallLogs.ShouldHaveSingleItem();
			parsedEvent.CallLogs[0].Id.ShouldBe("9a0887ca-da53-4c62-b32a-563789ef264e");
			parsedEvent.CallLogs[0].CallId.ShouldBe(CallId);
		}

		[Fact]
		public void PhoneCallLogPermanentlyDeletedEvent()
		{
			var parsedEvent = ParseWebhookEvent<PhoneCallLogPermanentlyDeletedEvent>(Resource.phone_call_log_permanently_deleted_webhook);

			VerifyPhoneCallLogOperationEvent(parsedEvent, ZoomNet.Models.Webhooks.EventType.PhoneCallLogPermanentlyDeleted);

			parsedEvent.CallLogs.ShouldNotBeNull();
			parsedEvent.CallLogs.ShouldHaveSingleItem();
			parsedEvent.CallLogs[0].Id.ShouldBe("9a0887ca-da53-4c62-b32a-563789ef264e");
			parsedEvent.CallLogs[0].CallId.ShouldBe(CallId);
		}

		[Fact]
		public void PhoneCalleeCallElementCompleted()
		{
			var parsedEvent = ParseWebhookEvent<PhoneCalleeCallElementCompletedEvent>(Resource.phone_callee_call_element_completed_webhook);

			VerifyPhoneCallLogOperationEvent(parsedEvent, ZoomNet.Models.Webhooks.EventType.PhoneCalleeCallElementCompleted);

			parsedEvent.CallElements.ShouldNotBeNull();
			parsedEvent.CallElements.ShouldHaveSingleItem();

			VerifyCallElement(parsedEvent.CallElements[0], id: null, callPathId: null);
		}

		[Fact]
		public void PhoneCallerCallElementCompleted()
		{
			var parsedEvent = ParseWebhookEvent<PhoneCallerCallElementCompletedEvent>(Resource.phone_caller_call_element_completed_webhook);

			VerifyPhoneCallLogOperationEvent(parsedEvent, ZoomNet.Models.Webhooks.EventType.PhoneCallerCallElementCompleted);

			parsedEvent.CallElements.ShouldNotBeNull();
			parsedEvent.CallElements.ShouldHaveSingleItem();

			VerifyCallElement(parsedEvent.CallElements[0], id: null, callPathId: null);
		}

		[Fact]
		public void PhoneCalleeCallHistoryCompleted()
		{
			var parsedEvent = ParseWebhookEvent<PhoneCalleeCallHistoryCompletedEvent>(Resource.phone_callee_call_history_completed_webhook);

			VerifyPhoneCallLogOperationEvent(parsedEvent, ZoomNet.Models.Webhooks.EventType.PhoneCalleeCallHistoryCompleted);

			parsedEvent.CallLogs.ShouldNotBeNull();
			parsedEvent.CallLogs.ShouldHaveSingleItem();

			VerifyCallElement(parsedEvent.CallLogs[0], callElementId: null, callHistoryUuid: null);
		}

		[Fact]
		public void PhoneCallerCallHistoryCompleted()
		{
			var parsedEvent = ParseWebhookEvent<PhoneCallerCallHistoryCompletedEvent>(Resource.phone_caller_call_history_completed_webhook);

			VerifyPhoneCallLogOperationEvent(parsedEvent, ZoomNet.Models.Webhooks.EventType.PhoneCallerCallHistoryCompleted);

			parsedEvent.CallLogs.ShouldNotBeNull();
			parsedEvent.CallLogs.ShouldHaveSingleItem();

			VerifyCallElement(parsedEvent.CallLogs[0], callElementId: null, callHistoryUuid: null);
		}

		[Fact]
		public void PhoneCalleeCallLogCompleted()
		{
			var parsedEvent = ParseWebhookEvent<PhoneCalleeCallLogCompletedEvent>(Resource.phone_callee_call_log_completed_webhook);

			VerifyPhoneCallLogOperationEvent(parsedEvent, ZoomNet.Models.Webhooks.EventType.PhoneCalleeCallLogCompleted);

			parsedEvent.CallLogs.ShouldNotBeNull();
			parsedEvent.CallLogs.ShouldHaveSingleItem();

			VerifyUserCallLog(parsedEvent.CallLogs[0]);
		}

		[Fact]
		public void PhoneCallerCallLogCompletd()
		{
			var parsedEvent = ParseWebhookEvent<PhoneCallerCallLogCompletedEvent>(Resource.phone_caller_call_log_completed_webhook);

			VerifyPhoneCallLogOperationEvent(parsedEvent, ZoomNet.Models.Webhooks.EventType.PhoneCallerCallLogCompleted);

			parsedEvent.CallLogs.ShouldNotBeNull();
			parsedEvent.CallLogs.ShouldHaveSingleItem();

			VerifyUserCallLog(parsedEvent.CallLogs[0]);
		}

		[Fact]
		public void PhoneRecordingCompleted()
		{
			var parsedEvent = ParseWebhookEvent<PhoneRecordingCompletedEvent>(Resource.phone_recording_completed_webhook);

			VerifyRecordingEvent(parsedEvent, ZoomNet.Models.Webhooks.EventType.PhoneRecordingCompleted);

			parsedEvent.Recordings.ShouldNotBeNull();
			parsedEvent.Recordings.ShouldHaveSingleItem();

			VerifyPhoneCallRecording(parsedEvent.Recordings[0]);
		}

		[Fact]
		public void PhoneRecordingCompletedForAccessMember()
		{
			var parsedEvent = ParseWebhookEvent<PhoneRecordingCompletedForAccessMemberEvent>(Resource.phone_recording_completed_for_access_member_webhook);

			VerifyRecordingEvent(parsedEvent, ZoomNet.Models.Webhooks.EventType.PhoneRecordingCompletedForAccessMember);

			parsedEvent.Recordings.ShouldNotBeNull();
			parsedEvent.Recordings.ShouldHaveSingleItem();

			VerifyPhoneCallRecording(parsedEvent.Recordings[0]);
		}

		[Fact]
		public void PhoneRecordingDeleted()
		{
			var parsedEvent = ParseWebhookEvent<PhoneRecordingDeletedEvent>(Resource.phone_recording_deleted_webhook);

			VerifyRecordingEvent(parsedEvent, ZoomNet.Models.Webhooks.EventType.PhoneRecordingDeleted);

			parsedEvent.Recordings.ShouldNotBeNull();
			parsedEvent.Recordings.ShouldHaveSingleItem();

			parsedEvent.Recordings[0].Id.ShouldBe(RecordingId);
			parsedEvent.Recordings[0].CallId.ShouldBe(CallId);
		}

		[Fact]
		public void PhoneRecordingFailed()
		{
			var parsedEvent = ParseWebhookEvent<PhoneRecordingFailedEvent>(Resource.phone_recording_failed_webhook);

			VerifyRecordingEvent(parsedEvent, ZoomNet.Models.Webhooks.EventType.PhoneRecordingFailed);

			VerifyPhoneCallRecordingSimple(parsedEvent.Recording);

			parsedEvent.Recording.ChannelId.ShouldBe(ChannelId);
			parsedEvent.Recording.SipId.ShouldBe(SipId);

			VerifyPhoneCallRecordingOwner(parsedEvent.Recording.Owner);
		}

		[Fact]
		public void PhoneRecordingPaused()
		{
			var parsedEvent = ParseWebhookEvent<PhoneRecordingPausedEvent>(Resource.phone_recording_paused_webhook);

			VerifyRecordingEvent(parsedEvent, ZoomNet.Models.Webhooks.EventType.PhoneRecordingPaused);

			VerifyPhoneCallRecordingSimple(parsedEvent.Recording);

			VerifyPhoneCallRecordingOwner(parsedEvent.Recording.Owner);
		}

		[Fact]
		public void PhoneRecordingPermanentlyDeleted()
		{
			var parsedEvent = ParseWebhookEvent<PhoneRecordingPermanentlyDeletedEvent>(Resource.phone_recording_permanently_deleted_webhook);

			VerifyRecordingEvent(parsedEvent, ZoomNet.Models.Webhooks.EventType.PhoneRecordingPermanentlyDeleted);

			parsedEvent.Recordings.ShouldNotBeNull();
			parsedEvent.Recordings.ShouldHaveSingleItem();

			parsedEvent.Recordings[0].Id.ShouldBe(RecordingId);
			parsedEvent.Recordings[0].CallId.ShouldBe(CallId);
		}

		[Fact]
		public void PhoneRecordingResumed()
		{
			var parsedEvent = ParseWebhookEvent<PhoneRecordingResumedEvent>(Resource.phone_recording_resumed_webhook);

			VerifyRecordingEvent(parsedEvent, ZoomNet.Models.Webhooks.EventType.PhoneRecordingResumed);

			VerifyPhoneCallRecordingSimple(parsedEvent.Recording);

			VerifyPhoneCallRecordingOwner(parsedEvent.Recording.Owner);
		}

		[Fact]
		public void PhoneRecordingStarted()
		{
			var parsedEvent = ParseWebhookEvent<PhoneRecordingStartedEvent>(Resource.phone_recording_started_webhook);

			VerifyRecordingEvent(parsedEvent, ZoomNet.Models.Webhooks.EventType.PhoneRecordingStarted);

			VerifyPhoneCallRecordingSimple(parsedEvent.Recording);

			parsedEvent.Recording.ChannelId.ShouldBe(ChannelId);
			parsedEvent.Recording.SipId.ShouldBe(SipId);

			VerifyPhoneCallRecordingOwner(parsedEvent.Recording.Owner);
		}

		[Fact]
		public void PhoneRecordingStopped()
		{
			var parsedEvent = ParseWebhookEvent<PhoneRecordingStoppedEvent>(Resource.phone_recording_stopped_webhook);

			VerifyRecordingEvent(parsedEvent, ZoomNet.Models.Webhooks.EventType.PhoneRecordingStopped);

			VerifyPhoneCallRecordingSimple(parsedEvent.Recording);

			parsedEvent.Recording.ChannelId.ShouldBe(ChannelId);
			parsedEvent.Recording.SipId.ShouldBe(SipId);

			VerifyPhoneCallRecordingOwner(parsedEvent.Recording.Owner);
		}

		[Fact]
		public void PhoneRecordingTranscriptCompleted()
		{
			var parsedEvent = ParseWebhookEvent<PhoneRecordingTranscriptCompletedEvent>(Resource.phone_recording_transcript_completed_webhook);

			VerifyRecordingEvent(parsedEvent, ZoomNet.Models.Webhooks.EventType.PhoneRecordingTranscriptCompleted);

			parsedEvent.Recordings.ShouldNotBeNull();
			parsedEvent.Recordings.ShouldHaveSingleItem();

			VerifyPhoneCallRecording(parsedEvent.Recordings[0], transcriptRecording: true);
		}

		[Fact]
		public void PhoneSmsCampaignNumberOptIn()
		{
			var parsedEvent = ParseWebhookEvent<PhoneSmsCampaignNumberOptInEvent>(Resource.phone_sms_campaign_number_opt_in_webhook);

			VerifyPhoneSmsEvent(parsedEvent, ZoomNet.Models.Webhooks.EventType.PhoneSmsCampaignNumberOptIn);

			VerifySmsCampaignOptStatuses(parsedEvent.SmsCampaignNumbers);
		}

		[Fact]
		public void PhoneSmsCampaignNumberOptOut()
		{
			var parsedEvent = ParseWebhookEvent<PhoneSmsCampaignNumberOptOutEvent>(Resource.phone_sms_campaign_number_opt_out_webhook);

			VerifyPhoneSmsEvent(parsedEvent, ZoomNet.Models.Webhooks.EventType.PhoneSmsCampaignNumberOptOut);

			VerifySmsCampaignOptStatuses(parsedEvent.SmsCampaignNumbers);
		}

		[Fact]
		public void PhoneSmsEtiquetteBlock()
		{
			var parsedEvent = ParseWebhookEvent<PhoneSmsEtiquetteBlockEvent>(Resource.phone_sms_etiquette_block_webhook);

			VerifyPhoneSmsEvent(parsedEvent, ZoomNet.Models.Webhooks.EventType.PhoneSmsEtiquetteBlock);

			VerifySmsEtiquettePolicy(parsedEvent.Policy, "Company's SSN blocking policy");
		}

		[Fact]
		public void PhoneSmsEtiquetteWarn()
		{
			var parsedEvent = ParseWebhookEvent<PhoneSmsEtiquetteWarnEvent>(Resource.phone_sms_etiquette_warn_webhook);

			VerifyPhoneSmsEvent(parsedEvent, ZoomNet.Models.Webhooks.EventType.PhoneSmsEtiquetteWarn);

			VerifySmsEtiquettePolicy(parsedEvent.Policy, "Company's SSN warning policy");
		}

		[Fact]
		public void PhoneSmsReceived()
		{
			var parsedEvent = ParseWebhookEvent<PhoneSmsReceivedEvent>(Resource.phone_sms_received_webhook);

			VerifyPhoneSmsEvent(parsedEvent, ZoomNet.Models.Webhooks.EventType.PhoneSmsReceived);

			VerifySmsMessage(parsedEvent.Message, ownerTeamId: "milmMfm3SYCwkraYIriNiQ", ownerSenderUserId: null, failureReason: null);
		}

		[Fact]
		public void PhoneSmsSent()
		{
			var parsedEvent = ParseWebhookEvent<PhoneSmsSentEvent>(Resource.phone_sms_sent_webhook);

			VerifyPhoneSmsEvent(parsedEvent, ZoomNet.Models.Webhooks.EventType.PhoneSmsSent);

			VerifySmsMessage(parsedEvent.Message, isMessageOwner: null);
		}

		[Fact]
		public void PhoneSmsSentFailed()
		{
			var parsedEvent = ParseWebhookEvent<PhoneSmsSentFailedEvent>(Resource.phone_sms_sent_failed_webhook);

			VerifyPhoneSmsEvent(parsedEvent, ZoomNet.Models.Webhooks.EventType.PhoneSmsSentFailed);

			VerifySmsMessage(parsedEvent.Message);
		}

		#endregion

		#region private methods

		/// <summary>
		/// Verify <see cref="PhoneCallFlowEvent"/> properties.
		/// </summary>
		private static void VerifyPhoneCallFlowEvent(PhoneCallFlowEvent parsedEvent, ZoomNet.Models.Webhooks.EventType eventType)
		{
			parsedEvent.EventType.ShouldBe(eventType);
			parsedEvent.Timestamp.ShouldBe(eventTimestamp);
			parsedEvent.AccountId.ShouldBe(AccountId);

			parsedEvent.CallInfo.ShouldNotBeNull();
			parsedEvent.CallInfo.CallId.ShouldBe(CallId);
		}

		/// <summary>
		/// Verify <see cref="PhoneCallParty"/> properties.
		/// </summary>
		private static void VerifyVoipParty(
			PhoneCallParty partyInfo,
			long? extensionNumber = 810003,
			PhoneCallExtensionType? extensionType = PhoneCallExtensionType.User,
			string deviceType = null)
		{
			partyInfo.ShouldNotBeNull();
			partyInfo.PhoneNumber.ShouldBe(PhoneNumberA);
			partyInfo.ConnectionType.ShouldBe(PhoneCallConnectionType.Voip);
			partyInfo.ExtensionNumber.ShouldBe(extensionNumber);
			partyInfo.ExtensionType.ShouldBe(extensionType);
			partyInfo.DeviceType.ShouldBe(deviceType);
		}

		/// <summary>
		/// Verify <see cref="PhoneCallParty"/> properties.
		/// </summary>
		private static void VerifyZoomUserParty(
			PhoneCallParty partyInfo,
			string extensionId = "52OdSKGSSS-EOyJwQncFvA",
			PhoneCallExtensionType? extensionType = PhoneCallExtensionType.User,
			string deviceType = "MAC_Client(5.7.5.1123)")
		{
			partyInfo.ShouldNotBeNull();
			partyInfo.PhoneNumber.ShouldBe(PhoneNumberB);
			partyInfo.ExtensionId.ShouldBe(extensionId);
			partyInfo.ExtensionType.ShouldBe(extensionType);
			partyInfo.ExtensionNumber.ShouldBe(1002);
			partyInfo.UserId.ShouldBe("DnEopNmXQEGU2uvvzjgojw");
			partyInfo.Timezone.ShouldBe(TimeZones.America_Los_Angeles);
			partyInfo.DeviceType.ShouldBe(deviceType);
			partyInfo.DeviceId.ShouldBe("f7aLLSmqRpiWP0U3U6CaNA");
			partyInfo.ConnectionType.ShouldBe(PhoneCallConnectionType.PstnOffNet);
		}

		/// <summary>
		/// Verify <see cref="PhoneCallParty"/> as forwarded by properties.
		/// </summary>
		private static void VerifyForwardedBy(PhoneCallParty forwardedBy)
		{
			forwardedBy.ShouldNotBeNull();
			forwardedBy.Name.ShouldBe("TestCAP01");
			forwardedBy.ExtensionType.ShouldBe(PhoneCallExtensionType.CallQueue);
			forwardedBy.ExtensionNumber.ShouldBe(1022);
		}

		/// <summary>
		/// Verify <see cref="PhoneCallParty"/> as redirect forwarded by properties.
		/// </summary>
		private static void VerifyRedirectForwardedBy(PhoneCallParty redirectForwardedBy)
		{
			redirectForwardedBy.ShouldNotBeNull();
			redirectForwardedBy.Name.ShouldBe("TestAR01");
			redirectForwardedBy.ExtensionType.ShouldBe(PhoneCallExtensionType.AutoReceptionist);
			redirectForwardedBy.ExtensionNumber.ShouldBe(100154);
			redirectForwardedBy.PhoneNumber.ShouldBe("+12053953655");
		}

		/// <summary>
		/// Verify <see cref="PhoneCallLogOperationEvent"/> properties.
		/// </summary>
		private static void VerifyPhoneCallLogOperationEvent(PhoneCallLogOperationEvent parsedEvent, ZoomNet.Models.Webhooks.EventType eventType)
		{
			parsedEvent.EventType.ShouldBe(eventType);
			parsedEvent.Timestamp.ShouldBe(eventTimestamp);
			parsedEvent.AccountId.ShouldBe(AccountId);
			parsedEvent.UserId.ShouldBe("DYHrdpjrS3uaOf7dPkkg8w");
		}

		/// <summary>
		/// Verify <see cref="CallElement"/> properties.
		/// </summary>
		private static void VerifyCallElement(
			CallElement callElement,
			string callElementId = CallElementId,
			string callHistoryUuid = CallHistoryId,
			string id = "20231008-0f5c57cd-49a7-467d-9570-a012316edbb7",
			string callPathId = "20231008-e13e793c-a5fb-4e8b-884b-b0c1e867a9c6")
		{
			callElement.ShouldNotBeNull();
			callElement.CallElementId.ShouldBe(callElementId);
			callElement.CallHistoryUuid.ShouldBe(callHistoryUuid);
			callElement.Id.ShouldBe(id);
			callElement.CallPathId.ShouldBe(callPathId);
			callElement.CallId.ShouldBe(CallId);
			callElement.GroupId.ShouldBe("_sj190JDFasa19321_adA7");
			callElement.ConnectType.ShouldBe(CallElementConnectType.Internal);
			callElement.CallType.ShouldBe(CallElementCallType.General);
			callElement.Direction.ShouldBe(CallLogDirection.Inbound);
			callElement.HideCallerId.ShouldBe(true);
			callElement.EndToEnd.ShouldBe(false);

			callElement.CallerExtensionId.ShouldBe("ATu63--9TjudZetpf4UuQg");
			callElement.CallerName.ShouldBe("Caller name");
			callElement.CallerEmail.ShouldBe("caller@abc.com");
			callElement.CallerEmployeeId.ShouldBe("102911");
			callElement.CallerDidNumber.ShouldBe(PhoneNumberA);
			callElement.CallerExtensionNumber.ShouldBe("101229");
			callElement.CallerExtensionType.ShouldBe(CallElementExtensionType.User);
			callElement.CallerNumberType.ShouldBe(CallElementNumberType.ExternalPstn);
			callElement.CallerDevicePrivateIp.ShouldBe("10.0.0.1");
			callElement.CallerDevicePublicIp.ShouldBe("135.0.0.1");
			callElement.CallerDeviceType.ShouldBe("MAC_Client(6.0.2.33403)");
			callElement.CallerCountryCode.ShouldBe("1");
			callElement.CallerCountryIsoCode.ShouldBe(Country.United_States_of_America);
			callElement.CallerSiteId.ShouldBe("BpCTBMRARBefUrprildVqw");
			callElement.CallerDepartment.ShouldBe("web-api1");
			callElement.CallerCostCenter.ShouldBe("cost-center1");

			callElement.CalleeExtensionId.ShouldBe("52OdSKGSSS-EOyJwQncFvA");
			callElement.CalleeName.ShouldBe("Callee name");
			callElement.CalleeEmail.ShouldBe("callee@abc.com");
			callElement.CalleeEmployeeId.ShouldBe("102912");
			callElement.CalleeDidNumber.ShouldBe(PhoneNumberB);
			callElement.CalleeExtensionNumber.ShouldBe("2345");
			callElement.CalleeExtensionType.ShouldBe(CallElementExtensionType.ExternalContact);
			callElement.CalleeNumberType.ShouldBe(CallElementNumberType.ZoomPstn);
			callElement.CalleeDevicePrivateIp.ShouldBe("10.0.0.2");
			callElement.CalleeDevicePublicIp.ShouldBe("135.0.0.2");
			callElement.CalleeDeviceType.ShouldBe("MAC_Client(6.0.2.33404)");
			callElement.CalleeCountryCode.ShouldBe("1");
			callElement.CalleeCountryIsoCode.ShouldBe(Country.United_States_of_America);
			callElement.CalleeSiteId.ShouldBe(SiteId);
			callElement.CalleeDepartment.ShouldBe("web-api2");
			callElement.CalleeCostCenter.ShouldBe("cost-center2");

			callElement.StartTime.ShouldBe(ringingStartedTimestamp);
			callElement.AnswerTime.ShouldBe(answeredTimestamp);
			callElement.EndTime.ShouldBe(callEndedTimestamp);

			callElement.EventType.ShouldBe(CallElementEventType.Outgoing);
			callElement.Result.ShouldBe(CallElementResult.Answered);
			callElement.ResultReason.ShouldBe(CallElementResultReason.AnsweredByOther);

			callElement.OperatorExtensionNumber.ShouldBe("3456");
			callElement.OperatorExtensionId.ShouldBe("NN9rA4fZSsScB2YiCqw7Ig");
			callElement.OperatorExtensionType.ShouldBe(CallElementExtensionType.AutoReceptionist);
			callElement.OperatorName.ShouldBe("operator name");

			callElement.RecordingId.ShouldBe(RecordingId);
			callElement.RecordingType.ShouldBe(CallElementRecordingType.Automatic);

			callElement.VoicemailId.ShouldBe("6cd2da01bcaa47f58e3250a575c5f2bf");

			callElement.TalkTime.ShouldBe(31);
			callElement.HoldTime.ShouldBe(20);
			callElement.WaitTime.ShouldBe(10);
		}

		/// <summary>
		/// Verify <see cref="UserCallLog"/> properties.
		/// </summary>
		private static void VerifyUserCallLog(UserCallLog callLog)
		{
			callLog.ShouldNotBeNull();
			callLog.Id.ShouldBe("4a4ed8ec-6be4-4e42-96e6-352a4396204d");
			callLog.CallId.ShouldBe(CallId);
			callLog.CallType.ShouldBe(CallLogCallType.Voip);

			callLog.CallerNumber.ShouldBe("1045");
			callLog.CallerNumberType.ShouldBe(CallLogCallerNumberType.Extension);
			callLog.CallerName.ShouldBe("usersubsetting0001@testapi.com");
			callLog.CallerCountryCode.ShouldBe("1");
			callLog.CallerCountryIsoCode.ShouldBe(Country.United_States_of_America);
			callLog.CallerDidNumber.ShouldBe(PhoneNumberA);
			callLog.CallerNumberSource.ShouldBe(CallLogNumberSource.External);
			callLog.CallerLocation.ShouldBe("Washington");
			callLog.CallerUserId.ShouldBe("NN9rA4fZSsScB2YiCqw7Ig");

			callLog.CalleeNumber.ShouldBe("1026");
			callLog.CalleeNumberType.ShouldBe(CallLogCalleeNumberType.Phone);
			callLog.CalleeName.ShouldBe("ZOOM_API Test");
			callLog.CalleeCountryCode.ShouldBe("1");
			callLog.CalleeCountryIsoCode.ShouldBe(Country.United_States_of_America);
			callLog.CalleeDidNumber.ShouldBe(PhoneNumberB);
			callLog.CalleeNumberSource.ShouldBe(CallLogNumberSource.Internal);
			callLog.CalleeLocation.ShouldBe("New York");
			callLog.CalleeUserId.ShouldBe("52OdSKGSSS-EOyJwQncFvA");

			callLog.Direction.ShouldBe(CallLogDirection.Inbound);
			callLog.Duration.ShouldBe(30);
			callLog.Result.ShouldBe(CallLogResult.Rejected);
			callLog.Path.ShouldBe("extension");

			callLog.StartedTime.ShouldBe(ringingStartedTimestamp);
			callLog.AnswerStartTime.ShouldBe(answeredTimestamp);
			callLog.CallEndTime.ShouldBe(callEndedTimestamp);

			callLog.HasRecording.ShouldBe(true);
			callLog.RecordingId.ShouldBe(RecordingId);
			callLog.RecordingType.ShouldBe(PhoneCallRecordingType.Automatic);

			callLog.ClientCode.ShouldBe("741");
			callLog.UserCostCenter.ShouldBe("cost_center");
			callLog.UserDepartment.ShouldBe("My department");
			callLog.HoldTime.ShouldBe(20);
			callLog.WaitingTime.ShouldBe(10);

			callLog.ForwardedBy.ShouldNotBeNull();
			callLog.ForwardedBy.ExtensionNumber.ShouldBe("1009");
			callLog.ForwardedBy.ExtensionType.ShouldBe(CallLogTransferInfoExtensionType.CallQueue);
			callLog.ForwardedBy.Location.ShouldBe("Glendale CA");
			callLog.ForwardedBy.Name.ShouldBe("Display name by");
			callLog.ForwardedBy.NumberType.ShouldBe(CallLogTransferInfoNumberType.Extension);
			callLog.ForwardedBy.PhoneNumber.ShouldBe("+12055432724");

			callLog.ForwardedTo.ShouldNotBeNull();
			callLog.ForwardedTo.ExtensionNumber.ShouldBe("1019");
			callLog.ForwardedTo.ExtensionType.ShouldBe(CallLogTransferInfoExtensionType.User);
			callLog.ForwardedTo.Location.ShouldBe("Virginia");
			callLog.ForwardedTo.Name.ShouldBe("Display name to");
			callLog.ForwardedTo.NumberType.ShouldBe(CallLogTransferInfoNumberType.E164);
			callLog.ForwardedTo.PhoneNumber.ShouldBe("+12053954669");

			callLog.Site.ShouldNotBeNull();
			callLog.Site.Id.ShouldBe(SiteId);
		}

		/// <summary>
		/// Verify some of the <see cref="PhoneCallRecording"/> properties.
		/// </summary>
		private static void VerifyPhoneCallRecordingSimple(PhoneCallRecording recording)
		{
			recording.ShouldNotBeNull();
			recording.Id.ShouldBe(RecordingId);
			recording.UserId.ShouldBe("z8yCxjabcdEFGHfp8uQ");
			recording.CallerNumber.ShouldBe("1026");
			recording.CallerAccountCode.ShouldBe("123");
			recording.CalleeNumber.ShouldBe("1045");
			recording.CalleeAccountCode.ShouldBe("456");
			recording.Direction.ShouldBe(CallLogDirection.Outbound);
			recording.StartDateTime.ShouldBe(answeredTimestamp);
			recording.Type.ShouldBe(PhoneCallRecordingType.OnDemand);
			recording.CallId.ShouldBe(CallId);
		}

		/// <summary>
		/// Verify <see cref="PhoneCallRecording"/> properties.
		/// </summary>
		private static void VerifyPhoneCallRecording(PhoneCallRecording recording, bool transcriptRecording = false)
		{
			VerifyPhoneCallRecordingSimple(recording);

			recording.CallLogId.ShouldBe("a297ae04-a875-4cfd-85ab-4adcead91edb");

			recording.CallerNumberType.ShouldBe(PhoneCallRecordingCallerNumberType.Internal);
			recording.CallerName.ShouldBe("Caller Name");

			recording.CalleeNumberType.ShouldBe(PhoneCallRecordingCalleeNumberType.External);
			recording.CalleeName.ShouldBe("Callee Name");

			recording.Duration.ShouldBe(43);
			recording.EndDateTime.ShouldBe(callEndedTimestamp);

			// There are some differences in available properties depending on audio recording or transcript.
			if (!transcriptRecording)
			{
				recording.CallElementId.ShouldBe(CallElementId);
				recording.CallHistoryId.ShouldBe(CallHistoryId);

				recording.CallerDidNumber.ShouldBe(PhoneNumberA);
				recording.CalleeDidNumber.ShouldBe(PhoneNumberB);

				recording.DownloadUrl.ShouldBe("https://some.url.com");
				recording.TranscriptDownloadUrl.ShouldBeNull();
			}
			else
			{
				recording.CallElementId.ShouldBeNull();
				recording.CallHistoryId.ShouldBeNull();

				recording.CallerDidNumber.ShouldBeNull();
				recording.CalleeDidNumber.ShouldBeNull();

				recording.TranscriptDownloadUrl.ShouldBe("https://some.url.com");
				recording.DownloadUrl.ShouldBeNull();
			}

			recording.CallReceiver.ShouldNotBeNull();
			recording.CallReceiver.ExtensionNumber.ShouldBe("900");
			recording.CallReceiver.Name.ShouldBe("Accepted by");

			recording.CallInitiator.ShouldNotBeNull();
			recording.CallInitiator.ExtensionNumber.ShouldBe("800");
			recording.CallInitiator.Name.ShouldBe("Outgoing by");

			VerifyPhoneCallRecordingOwner(recording.Owner, hasAccessPermission: !transcriptRecording ? true : (bool?)null);

			recording.Site.ShouldNotBeNull();
			recording.Site.Id.ShouldBe(SiteId);
		}

		/// <summary>
		/// Verify <see cref="PhoneCallRecordingOwner"/> properties.
		/// </summary>
		private static void VerifyPhoneCallRecordingOwner(PhoneCallRecordingOwner owner, bool? hasAccessPermission = null)
		{
			owner.ShouldNotBeNull();
			owner.Id.ShouldBe(OwnerId);
			owner.Type.ShouldBe(PhoneCallRecordingOwnerType.User);
			owner.Name.ShouldBe("Owner name");
			owner.ExtensionNumber.ShouldBe(6666);
			owner.HasAccessPermission.ShouldBe(hasAccessPermission);
		}

		/// <summary>
		/// Verify <see cref="PhoneSmsEvent"/> properties.
		/// </summary>
		private static void VerifyPhoneSmsEvent(PhoneSmsEvent parsedEvent, ZoomNet.Models.Webhooks.EventType eventType)
		{
			parsedEvent.EventType.ShouldBe(eventType);
			parsedEvent.Timestamp.ShouldBe(eventTimestamp);
			parsedEvent.AccountId.ShouldBe(AccountId);
		}

		/// <summary>
		/// Verify <see cref="WebhookSmsCampaignOptStatuses"/> properties.
		/// </summary>
		private static void VerifySmsCampaignOptStatuses(WebhookSmsCampaignOptStatuses optStatuses)
		{
			optStatuses.ShouldNotBeNull();
			optStatuses.Timestamp.ShouldBe(timestamp);
			optStatuses.PhoneNumbers.ShouldNotBeNull();
			optStatuses.PhoneNumbers.ShouldHaveSingleItem();
			optStatuses.PhoneNumbers[0].ConsumerPhoneNumber.ShouldBe(PhoneNumberA);
			optStatuses.PhoneNumbers[0].ZoomPhoneUserNumber.ShouldBe(PhoneNumberB);
		}

		/// <summary>
		/// Verify <see cref="WebhookSmsEtiquettePolicy"/> properties.
		/// </summary>
		private static void VerifySmsEtiquettePolicy(WebhookSmsEtiquettePolicy policy, string policyName)
		{
			policy.ShouldNotBeNull();
			policy.Timestamp.ShouldBe(timestamp);
			policy.Email.ShouldBe(UserEmail);
			policy.Message.ShouldBe("this is my SSN 123-45-6789");
			policy.PolicyName.ShouldBe(policyName);
		}

		/// <summary>
		/// Verify <see cref="WebhookSmsMessage"/> properties.
		/// </summary>
		private static void VerifySmsMessage(
			WebhookSmsMessage message,
			string ownerTeamId = null,
			string ownerSenderUserId = "JduDVJJXTS6VGXiJiro7ZQ",
			bool? isMessageOwner = true,
			string failureReason = "1101-phone message error send")
		{
			message.ShouldNotBeNull();
			message.MessageId.ShouldBe("72abd192-97ee-4ddb-b498-85048f4c34d7");
			message.Type.ShouldBe(SmsMessageType.Sms);
			message.CreatedOn.ShouldBe(timestamp);
			message.Message.ShouldBe("ttt");
			message.SessionId.ShouldBe("e5e9b8417c1f2da2cd3006f3d9d4641a");
			message.FailureReason.ShouldBe(failureReason);

			message.Attachments.ShouldNotBeNull();
			message.Attachments.ShouldHaveSingleItem();
			message.Attachments[0].Id.ShouldBe("ux7LtGDPRmqbo0nLQ2v9jQ");
			message.Attachments[0].Size.ShouldBe(10692);
			message.Attachments[0].Name.ShouldBe("Screenshot2021_05_17_183842.jpg");
			message.Attachments[0].Type.ShouldBe(SmsAttachmentType.Jpg);
			message.Attachments[0].DownloadUrl.ShouldBe("https://downloadUrl.zoom.us");

			message.Owner.ShouldNotBeNull();
			message.Owner.Id.ShouldBe("29QVgYBGRmOM5VlC0DmLgg");
			message.Owner.Type.ShouldBe(SmsParticipantOwnerType.CallQueue);
			message.Owner.TeamId.ShouldBe(ownerTeamId);
			message.Owner.SenderUserId.ShouldBe(ownerSenderUserId);

			message.Sender.ShouldNotBeNull();
			message.Sender.Id.ShouldBe("RMIGplfpSLauTMDMTi3Kfg");
			message.Sender.PhoneNumber.ShouldBe("12132822256");
			message.Sender.Type.ShouldBe(SmsParticipantOwnerType.User);
			message.Sender.DisplayName.ShouldBe("Jackson");

			message.Recipients.ShouldNotBeNull();
			message.Recipients.ShouldHaveSingleItem();
			message.Recipients[0].Id.ShouldBe("RMITfuhuuxyzyuyyi3Kfg");
			message.Recipients[0].PhoneNumber.ShouldBe("12132792348");
			message.Recipients[0].Type.ShouldBe(SmsParticipantOwnerType.User);
			message.Recipients[0].DisplayName.ShouldBe("Tom");
			message.Recipients[0].IsMessageOwner.ShouldBe(isMessageOwner);

			message.PhoneNumbersOptStatuses.ShouldNotBeNull();
			message.PhoneNumbersOptStatuses.ShouldHaveSingleItem();
			message.PhoneNumbersOptStatuses[0].ConsumerPhoneNumber.ShouldBe("+120970XXXXX");
			message.PhoneNumbersOptStatuses[0].ZoomPhoneUserNumber.ShouldBe("+120971XXXXX");
			message.PhoneNumbersOptStatuses[0].OptStatus.ShouldBe(SmsOptStatus.Pending);
			message.PhoneNumbersOptStatuses[0].OptInStatus.ShouldBe(SmsOptInStatus.NewSession);
			message.PhoneNumbersOptStatuses[0].OptInMessage.ShouldBe("Text START to receive text messages from ZOOM.");
		}

		#endregion
	}
}
