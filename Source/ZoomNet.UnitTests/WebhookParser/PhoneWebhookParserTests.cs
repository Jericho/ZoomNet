using Shouldly;
using System;
using Xunit;
using ZoomNet.Models;
using ZoomNet.Models.PhoneAccountSettings;
using ZoomNet.Models.Webhooks;
using ZoomNet.UnitTests.Properties;

namespace ZoomNet.UnitTests.WebhookParser
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
			var parsedEvent = ParseWebhookEvent<PhoneCalleeAnsweredEvent>(WebhooksDataResource.phone_callee_answered);

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
			var parsedEvent = ParseWebhookEvent<PhoneCalleeEndedEvent>(WebhooksDataResource.phone_callee_ended);

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
			var parsedEvent = ParseWebhookEvent<PhoneCalleeHoldEvent>(WebhooksDataResource.phone_callee_hold);

			VerifyPhoneCallFlowEvent(parsedEvent, ZoomNet.Models.Webhooks.EventType.PhoneCalleeHold);

			parsedEvent.HoldStartedOn.ShouldBe(timestamp);

			VerifyVoipParty(parsedEvent.CallInfo.Caller);
			VerifyZoomUserParty(parsedEvent.CallInfo.Callee, deviceType: null);
		}

		[Fact]
		public void PhoneCalleeMeetingInviting()
		{
			var parsedEvent = ParseWebhookEvent<PhoneCalleeMeetingInvitingEvent>(WebhooksDataResource.phone_callee_meeting_inviting);

			VerifyPhoneCallFlowEvent(parsedEvent, ZoomNet.Models.Webhooks.EventType.PhoneCalleeMeetingInviting);

			parsedEvent.EscalatedAt.ShouldBe(timestamp);
			parsedEvent.MeetingId.ShouldBe("987654321");

			VerifyVoipParty(parsedEvent.CallInfo.Caller);
			VerifyZoomUserParty(parsedEvent.CallInfo.Callee, deviceType: null);
		}

		[Fact]
		public void PhoneCalleeMissed()
		{
			var parsedEvent = ParseWebhookEvent<PhoneCalleeMissedEvent>(WebhooksDataResource.phone_callee_missed);

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
			var parsedEvent = ParseWebhookEvent<PhoneCalleeMuteEvent>(WebhooksDataResource.phone_callee_mute);

			VerifyPhoneCallFlowEvent(parsedEvent, ZoomNet.Models.Webhooks.EventType.PhoneCalleeMute);

			parsedEvent.MutedOn.ShouldBe(timestamp);

			VerifyVoipParty(parsedEvent.CallInfo.Caller);
			VerifyZoomUserParty(parsedEvent.CallInfo.Callee, deviceType: null);
		}

		[Fact]
		public void PhoneCalleeParked()
		{
			var parsedEvent = ParseWebhookEvent<PhoneCalleeParkedEvent>(WebhooksDataResource.phone_callee_parked);

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
			var parsedEvent = ParseWebhookEvent<PhoneCalleeRejectedEvent>(WebhooksDataResource.phone_callee_rejected);

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
			var parsedEvent = ParseWebhookEvent<PhoneCalleeRingingEvent>(WebhooksDataResource.phone_callee_ringing);

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
			var parsedEvent = ParseWebhookEvent<PhoneCalleeUnholdEvent>(WebhooksDataResource.phone_callee_unhold);

			VerifyPhoneCallFlowEvent(parsedEvent, ZoomNet.Models.Webhooks.EventType.PhoneCalleeUnhold);

			parsedEvent.HoldEndedOn.ShouldBe(timestamp);

			VerifyVoipParty(parsedEvent.CallInfo.Caller);
			VerifyZoomUserParty(parsedEvent.CallInfo.Callee, deviceType: null);
		}

		[Fact]
		public void PhoneCalleeUnmute()
		{
			var parsedEvent = ParseWebhookEvent<PhoneCalleeUnmuteEvent>(WebhooksDataResource.phone_callee_unmute);

			VerifyPhoneCallFlowEvent(parsedEvent, ZoomNet.Models.Webhooks.EventType.PhoneCalleeUnmute);

			parsedEvent.UnmutedOn.ShouldBe(timestamp);

			VerifyVoipParty(parsedEvent.CallInfo.Caller);
			VerifyZoomUserParty(parsedEvent.CallInfo.Callee, deviceType: null);
		}

		[Fact]
		public void PhoneCallerConnected()
		{
			var parsedEvent = ParseWebhookEvent<PhoneCallerConnectedEvent>(WebhooksDataResource.phone_caller_connected);

			VerifyPhoneCallFlowEvent(parsedEvent, ZoomNet.Models.Webhooks.EventType.PhoneCallerConnected);

			parsedEvent.RingingStartedOn.ShouldBe(ringingStartedTimestamp);
			parsedEvent.ConnectedOn.ShouldBe(answeredTimestamp);

			VerifyVoipParty(parsedEvent.CallInfo.Callee, extensionType: null);
			VerifyZoomUserParty(parsedEvent.CallInfo.Caller);
		}

		[Fact]
		public void PhoneCallerEnded()
		{
			var parsedEvent = ParseWebhookEvent<PhoneCallerEndedEvent>(WebhooksDataResource.phone_caller_ended);

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
			var parsedEvent = ParseWebhookEvent<PhoneCallerHoldEvent>(WebhooksDataResource.phone_caller_hold);

			VerifyPhoneCallFlowEvent(parsedEvent, ZoomNet.Models.Webhooks.EventType.PhoneCallerHold);

			parsedEvent.HoldStartedOn.ShouldBe(timestamp);

			VerifyVoipParty(parsedEvent.CallInfo.Callee);
			VerifyZoomUserParty(parsedEvent.CallInfo.Caller, deviceType: null);
		}

		[Fact]
		public void PhoneCallerMeetingInviting()
		{
			var parsedEvent = ParseWebhookEvent<PhoneCallerMeetingInvitingEvent>(WebhooksDataResource.phone_caller_meeting_inviting);

			VerifyPhoneCallFlowEvent(parsedEvent, ZoomNet.Models.Webhooks.EventType.PhoneCallerMeetingInviting);

			parsedEvent.EscalatedAt.ShouldBe(timestamp);
			parsedEvent.MeetingId.ShouldBe("987654321");

			VerifyVoipParty(parsedEvent.CallInfo.Callee);
			VerifyZoomUserParty(parsedEvent.CallInfo.Caller, deviceType: null);
		}

		[Fact]
		public void PhoneCallerMute()
		{
			var parsedEvent = ParseWebhookEvent<PhoneCallerMuteEvent>(WebhooksDataResource.phone_caller_mute);

			VerifyPhoneCallFlowEvent(parsedEvent, ZoomNet.Models.Webhooks.EventType.PhoneCallerMute);

			parsedEvent.MutedOn.ShouldBe(timestamp);

			VerifyVoipParty(parsedEvent.CallInfo.Callee);
			VerifyZoomUserParty(parsedEvent.CallInfo.Caller, deviceType: null);
		}

		[Fact]
		public void PhoneCallerRinging()
		{
			var parsedEvent = ParseWebhookEvent<PhoneCallerRingingEvent>(WebhooksDataResource.phone_caller_ringing);

			VerifyPhoneCallFlowEvent(parsedEvent, ZoomNet.Models.Webhooks.EventType.PhoneCallerRinging);

			parsedEvent.RingingStartedOn.ShouldBe(ringingStartedTimestamp);

			VerifyVoipParty(parsedEvent.CallInfo.Callee);
			VerifyZoomUserParty(parsedEvent.CallInfo.Caller);
		}

		[Fact]
		public void PhoneCallerUnhold()
		{
			var parsedEvent = ParseWebhookEvent<PhoneCallerUnholdEvent>(WebhooksDataResource.phone_caller_unhold);

			VerifyPhoneCallFlowEvent(parsedEvent, ZoomNet.Models.Webhooks.EventType.PhoneCallerUnhold);

			parsedEvent.HoldEndedOn.ShouldBe(timestamp);

			VerifyVoipParty(parsedEvent.CallInfo.Callee);
			VerifyZoomUserParty(parsedEvent.CallInfo.Caller, deviceType: null);
		}

		[Fact]
		public void PhoneCallerUnmute()
		{
			var parsedEvent = ParseWebhookEvent<PhoneCallerUnmuteEvent>(WebhooksDataResource.phone_caller_unmute);

			VerifyPhoneCallFlowEvent(parsedEvent, ZoomNet.Models.Webhooks.EventType.PhoneCallerUnmute);

			parsedEvent.UnmutedOn.ShouldBe(timestamp);

			VerifyVoipParty(parsedEvent.CallInfo.Callee);
			VerifyZoomUserParty(parsedEvent.CallInfo.Caller, deviceType: null);
		}

		[Fact]
		public void PhoneCallElementDeleted()
		{
			var parsedEvent = ParseWebhookEvent<PhoneCallElementDeletedEvent>(WebhooksDataResource.phone_call_element_deleted);

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
			var parsedEvent = ParseWebhookEvent<PhoneCallHistoryDeletedEvent>(WebhooksDataResource.phone_call_history_deleted);

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
			var parsedEvent = ParseWebhookEvent<PhoneCallLogDeletedEvent>(WebhooksDataResource.phone_call_log_deleted);

			VerifyPhoneCallLogOperationEvent(parsedEvent, ZoomNet.Models.Webhooks.EventType.PhoneCallLogDeleted);

			parsedEvent.CallLogs.ShouldNotBeNull();
			parsedEvent.CallLogs.ShouldHaveSingleItem();
			parsedEvent.CallLogs[0].Id.ShouldBe("9a0887ca-da53-4c62-b32a-563789ef264e");
			parsedEvent.CallLogs[0].CallId.ShouldBe(CallId);
		}

		[Fact]
		public void PhoneCallLogPermanentlyDeletedEvent()
		{
			var parsedEvent = ParseWebhookEvent<PhoneCallLogPermanentlyDeletedEvent>(WebhooksDataResource.phone_call_log_permanently_deleted);

			VerifyPhoneCallLogOperationEvent(parsedEvent, ZoomNet.Models.Webhooks.EventType.PhoneCallLogPermanentlyDeleted);

			parsedEvent.CallLogs.ShouldNotBeNull();
			parsedEvent.CallLogs.ShouldHaveSingleItem();
			parsedEvent.CallLogs[0].Id.ShouldBe("9a0887ca-da53-4c62-b32a-563789ef264e");
			parsedEvent.CallLogs[0].CallId.ShouldBe(CallId);
		}

		[Fact]
		public void PhoneCalleeCallElementCompleted()
		{
			var parsedEvent = ParseWebhookEvent<PhoneCalleeCallElementCompletedEvent>(WebhooksDataResource.phone_callee_call_element_completed);

			VerifyPhoneCallLogOperationEvent(parsedEvent, ZoomNet.Models.Webhooks.EventType.PhoneCalleeCallElementCompleted);

			parsedEvent.CallElements.ShouldNotBeNull();
			parsedEvent.CallElements.ShouldHaveSingleItem();

			VerifyCallElement(parsedEvent.CallElements[0], id: null, callPathId: null);
		}

		[Fact]
		public void PhoneCallerCallElementCompleted()
		{
			var parsedEvent = ParseWebhookEvent<PhoneCallerCallElementCompletedEvent>(WebhooksDataResource.phone_caller_call_element_completed);

			VerifyPhoneCallLogOperationEvent(parsedEvent, ZoomNet.Models.Webhooks.EventType.PhoneCallerCallElementCompleted);

			parsedEvent.CallElements.ShouldNotBeNull();
			parsedEvent.CallElements.ShouldHaveSingleItem();

			VerifyCallElement(parsedEvent.CallElements[0], id: null, callPathId: null);
		}

		[Fact]
		public void PhoneCalleeCallHistoryCompleted()
		{
			var parsedEvent = ParseWebhookEvent<PhoneCalleeCallHistoryCompletedEvent>(WebhooksDataResource.phone_callee_call_history_completed);

			VerifyPhoneCallLogOperationEvent(parsedEvent, ZoomNet.Models.Webhooks.EventType.PhoneCalleeCallHistoryCompleted);

			parsedEvent.CallLogs.ShouldNotBeNull();
			parsedEvent.CallLogs.ShouldHaveSingleItem();

			VerifyCallElement(parsedEvent.CallLogs[0], callElementId: null, callHistoryUuid: null);
		}

		[Fact]
		public void PhoneCallerCallHistoryCompleted()
		{
			var parsedEvent = ParseWebhookEvent<PhoneCallerCallHistoryCompletedEvent>(WebhooksDataResource.phone_caller_call_history_completed);

			VerifyPhoneCallLogOperationEvent(parsedEvent, ZoomNet.Models.Webhooks.EventType.PhoneCallerCallHistoryCompleted);

			parsedEvent.CallLogs.ShouldNotBeNull();
			parsedEvent.CallLogs.ShouldHaveSingleItem();

			VerifyCallElement(parsedEvent.CallLogs[0], callElementId: null, callHistoryUuid: null);
		}

		[Fact]
		public void PhoneCalleeCallLogCompleted()
		{
			var parsedEvent = ParseWebhookEvent<PhoneCalleeCallLogCompletedEvent>(WebhooksDataResource.phone_callee_call_log_completed);

			VerifyPhoneCallLogOperationEvent(parsedEvent, ZoomNet.Models.Webhooks.EventType.PhoneCalleeCallLogCompleted);

			parsedEvent.CallLogs.ShouldNotBeNull();
			parsedEvent.CallLogs.ShouldHaveSingleItem();

			VerifyUserCallLog(parsedEvent.CallLogs[0]);
		}

		[Fact]
		public void PhoneCallerCallLogCompletd()
		{
			var parsedEvent = ParseWebhookEvent<PhoneCallerCallLogCompletedEvent>(WebhooksDataResource.phone_caller_call_log_completed);

			VerifyPhoneCallLogOperationEvent(parsedEvent, ZoomNet.Models.Webhooks.EventType.PhoneCallerCallLogCompleted);

			parsedEvent.CallLogs.ShouldNotBeNull();
			parsedEvent.CallLogs.ShouldHaveSingleItem();

			VerifyUserCallLog(parsedEvent.CallLogs[0]);
		}

		[Fact]
		public void PhoneRecordingCompleted()
		{
			var parsedEvent = ParseWebhookEvent<PhoneRecordingCompletedEvent>(WebhooksDataResource.phone_recording_completed);

			VerifyRecordingEvent(parsedEvent, ZoomNet.Models.Webhooks.EventType.PhoneRecordingCompleted);

			parsedEvent.Recordings.ShouldNotBeNull();
			parsedEvent.Recordings.ShouldHaveSingleItem();

			VerifyPhoneCallRecording(parsedEvent.Recordings[0]);
		}

		[Fact]
		public void PhoneRecordingCompletedForAccessMember()
		{
			var parsedEvent = ParseWebhookEvent<PhoneRecordingCompletedForAccessMemberEvent>(WebhooksDataResource.phone_recording_completed_for_access_member);

			VerifyRecordingEvent(parsedEvent, ZoomNet.Models.Webhooks.EventType.PhoneRecordingCompletedForAccessMember);

			parsedEvent.Recordings.ShouldNotBeNull();
			parsedEvent.Recordings.ShouldHaveSingleItem();

			VerifyPhoneCallRecording(parsedEvent.Recordings[0]);
		}

		[Fact]
		public void PhoneRecordingDeleted()
		{
			var parsedEvent = ParseWebhookEvent<PhoneRecordingDeletedEvent>(WebhooksDataResource.phone_recording_deleted);

			VerifyRecordingEvent(parsedEvent, ZoomNet.Models.Webhooks.EventType.PhoneRecordingDeleted);

			parsedEvent.Recordings.ShouldNotBeNull();
			parsedEvent.Recordings.ShouldHaveSingleItem();

			parsedEvent.Recordings[0].Id.ShouldBe(RecordingId);
			parsedEvent.Recordings[0].CallId.ShouldBe(CallId);
		}

		[Fact]
		public void PhoneRecordingFailed()
		{
			var parsedEvent = ParseWebhookEvent<PhoneRecordingFailedEvent>(WebhooksDataResource.phone_recording_failed);

			VerifyRecordingEvent(parsedEvent, ZoomNet.Models.Webhooks.EventType.PhoneRecordingFailed);

			VerifyPhoneCallRecordingSimple(parsedEvent.Recording);

			parsedEvent.Recording.ChannelId.ShouldBe(ChannelId);
			parsedEvent.Recording.SipId.ShouldBe(SipId);

			VerifyPhoneCallRecordingOwner(parsedEvent.Recording.Owner);
		}

		[Fact]
		public void PhoneRecordingPaused()
		{
			var parsedEvent = ParseWebhookEvent<PhoneRecordingPausedEvent>(WebhooksDataResource.phone_recording_paused);

			VerifyRecordingEvent(parsedEvent, ZoomNet.Models.Webhooks.EventType.PhoneRecordingPaused);

			VerifyPhoneCallRecordingSimple(parsedEvent.Recording);

			VerifyPhoneCallRecordingOwner(parsedEvent.Recording.Owner);
		}

		[Fact]
		public void PhoneRecordingPermanentlyDeleted()
		{
			var parsedEvent = ParseWebhookEvent<PhoneRecordingPermanentlyDeletedEvent>(WebhooksDataResource.phone_recording_permanently_deleted);

			VerifyRecordingEvent(parsedEvent, ZoomNet.Models.Webhooks.EventType.PhoneRecordingPermanentlyDeleted);

			parsedEvent.Recordings.ShouldNotBeNull();
			parsedEvent.Recordings.ShouldHaveSingleItem();

			parsedEvent.Recordings[0].Id.ShouldBe(RecordingId);
			parsedEvent.Recordings[0].CallId.ShouldBe(CallId);
		}

		[Fact]
		public void PhoneRecordingResumed()
		{
			var parsedEvent = ParseWebhookEvent<PhoneRecordingResumedEvent>(WebhooksDataResource.phone_recording_resumed);

			VerifyRecordingEvent(parsedEvent, ZoomNet.Models.Webhooks.EventType.PhoneRecordingResumed);

			VerifyPhoneCallRecordingSimple(parsedEvent.Recording);

			VerifyPhoneCallRecordingOwner(parsedEvent.Recording.Owner);
		}

		[Fact]
		public void PhoneRecordingStarted()
		{
			var parsedEvent = ParseWebhookEvent<PhoneRecordingStartedEvent>(WebhooksDataResource.phone_recording_started);

			VerifyRecordingEvent(parsedEvent, ZoomNet.Models.Webhooks.EventType.PhoneRecordingStarted);

			VerifyPhoneCallRecordingSimple(parsedEvent.Recording);

			parsedEvent.Recording.ChannelId.ShouldBe(ChannelId);
			parsedEvent.Recording.SipId.ShouldBe(SipId);

			VerifyPhoneCallRecordingOwner(parsedEvent.Recording.Owner);
		}

		[Fact]
		public void PhoneRecordingStopped()
		{
			var parsedEvent = ParseWebhookEvent<PhoneRecordingStoppedEvent>(WebhooksDataResource.phone_recording_stopped);

			VerifyRecordingEvent(parsedEvent, ZoomNet.Models.Webhooks.EventType.PhoneRecordingStopped);

			VerifyPhoneCallRecordingSimple(parsedEvent.Recording);

			parsedEvent.Recording.ChannelId.ShouldBe(ChannelId);
			parsedEvent.Recording.SipId.ShouldBe(SipId);

			VerifyPhoneCallRecordingOwner(parsedEvent.Recording.Owner);
		}

		[Fact]
		public void PhoneRecordingTranscriptCompleted()
		{
			var parsedEvent = ParseWebhookEvent<PhoneRecordingTranscriptCompletedEvent>(WebhooksDataResource.phone_recording_transcript_completed);

			VerifyRecordingEvent(parsedEvent, ZoomNet.Models.Webhooks.EventType.PhoneRecordingTranscriptCompleted);

			parsedEvent.Recordings.ShouldNotBeNull();
			parsedEvent.Recordings.ShouldHaveSingleItem();

			VerifyPhoneCallRecording(parsedEvent.Recordings[0], transcriptRecording: true);
		}

		[Fact]
		public void PhoneSmsCampaignNumberOptIn()
		{
			var parsedEvent = ParseWebhookEvent<PhoneSmsCampaignNumberOptInEvent>(WebhooksDataResource.phone_sms_campaign_number_opt_in);

			VerifyPhoneSmsEvent(parsedEvent, ZoomNet.Models.Webhooks.EventType.PhoneSmsCampaignNumberOptIn);

			VerifySmsCampaignOptStatuses(parsedEvent.SmsCampaignNumbers);
		}

		[Fact]
		public void PhoneSmsCampaignNumberOptOut()
		{
			var parsedEvent = ParseWebhookEvent<PhoneSmsCampaignNumberOptOutEvent>(WebhooksDataResource.phone_sms_campaign_number_opt_out);

			VerifyPhoneSmsEvent(parsedEvent, ZoomNet.Models.Webhooks.EventType.PhoneSmsCampaignNumberOptOut);

			VerifySmsCampaignOptStatuses(parsedEvent.SmsCampaignNumbers);
		}

		[Fact]
		public void PhoneSmsEtiquetteBlock()
		{
			var parsedEvent = ParseWebhookEvent<PhoneSmsEtiquetteBlockEvent>(WebhooksDataResource.phone_sms_etiquette_block);

			VerifyPhoneSmsEvent(parsedEvent, ZoomNet.Models.Webhooks.EventType.PhoneSmsEtiquetteBlock);

			VerifySmsEtiquettePolicy(parsedEvent.Policy, "Company's SSN blocking policy");
		}

		[Fact]
		public void PhoneSmsEtiquetteWarn()
		{
			var parsedEvent = ParseWebhookEvent<PhoneSmsEtiquetteWarnEvent>(WebhooksDataResource.phone_sms_etiquette_warn);

			VerifyPhoneSmsEvent(parsedEvent, ZoomNet.Models.Webhooks.EventType.PhoneSmsEtiquetteWarn);

			VerifySmsEtiquettePolicy(parsedEvent.Policy, "Company's SSN warning policy");
		}

		[Fact]
		public void PhoneSmsReceived()
		{
			var parsedEvent = ParseWebhookEvent<PhoneSmsReceivedEvent>(WebhooksDataResource.phone_sms_received);

			VerifyPhoneSmsEvent(parsedEvent, ZoomNet.Models.Webhooks.EventType.PhoneSmsReceived);

			VerifySmsMessage(parsedEvent.Message, ownerTeamId: "milmMfm3SYCwkraYIriNiQ", ownerSenderUserId: null, failureReason: null);
		}

		[Fact]
		public void PhoneSmsSent()
		{
			var parsedEvent = ParseWebhookEvent<PhoneSmsSentEvent>(WebhooksDataResource.phone_sms_sent);

			VerifyPhoneSmsEvent(parsedEvent, ZoomNet.Models.Webhooks.EventType.PhoneSmsSent);

			VerifySmsMessage(parsedEvent.Message, isMessageOwner: null);
		}

		[Fact]
		public void PhoneSmsSentFailed()
		{
			var parsedEvent = ParseWebhookEvent<PhoneSmsSentFailedEvent>(WebhooksDataResource.phone_sms_sent_failed);

			VerifyPhoneSmsEvent(parsedEvent, ZoomNet.Models.Webhooks.EventType.PhoneSmsSentFailed);

			VerifySmsMessage(parsedEvent.Message);
		}

		[Fact]
		public void PhoneVoicemailDeleted()
		{
			var parsedEvent = ParseWebhookEvent<PhoneVoicemailDeletedEvent>(WebhooksDataResource.phone_voicemail_deleted);

			VerifyPhoneVoicemailEvent(parsedEvent, ZoomNet.Models.Webhooks.EventType.PhoneVoicemailDeleted);

			parsedEvent.Voicemails.ShouldNotBeNull();
			parsedEvent.Voicemails.ShouldHaveSingleItem();
			parsedEvent.Voicemails[0].Id.ShouldBe("0388975092074598b47330a6a87e9a7b");
		}

		[Fact]
		public void PhoneVoicemailPermanentlyDeleted()
		{
			var parsedEvent = ParseWebhookEvent<PhoneVoicemailPermanentlyDeletedEvent>(WebhooksDataResource.phone_voicemail_permanently_deleted);

			VerifyPhoneVoicemailEvent(parsedEvent, ZoomNet.Models.Webhooks.EventType.PhoneVoicemailPermanentlyDeleted);

			parsedEvent.Voicemails.ShouldNotBeNull();
			parsedEvent.Voicemails.ShouldHaveSingleItem();
			parsedEvent.Voicemails[0].Id.ShouldBe("0388975092074598b47330a6a87e9a7b");
		}

		[Fact]
		public void PhoneVoicemailReceived()
		{
			var parsedEvent = ParseWebhookEvent<PhoneVoicemailReceivedEvent>(WebhooksDataResource.phone_voicemail_received);

			VerifyPhoneVoicemailEvent(parsedEvent, ZoomNet.Models.Webhooks.EventType.PhoneVoicemailReceived);

			VerifyVoicemail(parsedEvent.Voicemail);
		}

		[Fact]
		public void PhoneVoicemailReceivedForAccessMember()
		{
			var parsedEvent = ParseWebhookEvent<PhoneVoicemailReceivedForAccessMemberEvent>(WebhooksDataResource.phone_voicemail_received_for_access_member);

			parsedEvent.EventType.ShouldBe(ZoomNet.Models.Webhooks.EventType.PhoneVoicemailReceivedForAccessMember);
			parsedEvent.Timestamp.ShouldBe(eventTimestamp);
			parsedEvent.AccountId.ShouldBe(AccountId);

			VerifyVoicemail(parsedEvent.Voicemail, callerUserId: null);

			parsedEvent.AccessMemberExtensionType.ShouldBe(PhoneCallExtensionType.User);
			parsedEvent.AccessMemberId.ShouldBe("z8yCxjabcdEFGHfp8uQ");
		}

		[Fact]
		public void PhoneVoicemailTranscriptCompleted()
		{
			var parsedEvent = ParseWebhookEvent<PhoneVoicemailTranscriptCompletedEvent>(WebhooksDataResource.phone_voicemail_transcript_completed);

			VerifyPhoneVoicemailEvent(parsedEvent, ZoomNet.Models.Webhooks.EventType.PhoneVoicemailTranscriptCompleted);

			VerifyVoicemail(parsedEvent.Voicemail, voicemailTranscript: true, callerUserId: null);
		}

		[Fact]
		public void PhoneBlindTransferInitiated()
		{
			var parsedEvent = ParseWebhookEvent<PhoneBlindTransferInitiatedEvent>(WebhooksDataResource.phone_blind_transfer_initiated);

			VerifyPhoneCallTransferEvent(parsedEvent, ZoomNet.Models.Webhooks.EventType.PhoneBlindTransferInitiated);

			VerifyCallTransferInfo(parsedEvent.CallTransfer);
		}

		[Fact]
		public void PhoneTransferCallToVoicemailInitiated()
		{
			var parsedEvent = ParseWebhookEvent<PhoneTransferCallToVoicemailInitiatedEvent>(WebhooksDataResource.phone_transfer_call_to_voicemail_initiated);

			VerifyPhoneCallTransferEvent(parsedEvent, ZoomNet.Models.Webhooks.EventType.PhoneTransferCallToVoicemailInitiated);

			VerifyCallTransferInfo(parsedEvent.CallTransfer);
		}

		[Fact]
		public void PhoneTransferRecipientUpdated()
		{
			var parsedEvent = ParseWebhookEvent<PhoneTransferRecipientUpdatedEvent>(WebhooksDataResource.phone_transfer_recipient_updated);

			VerifyPhoneCallTransferEvent(parsedEvent, ZoomNet.Models.Webhooks.EventType.PhoneTransferRecipientUpdated);

			VerifyCallTransferInfo(parsedEvent.CallTransfer, transferPhoneNumber: null, transferAccountCode: null);

			VerifyCallTransferRecipient(parsedEvent.Recipient);
		}

		[Fact]
		public void PhoneWarmTransferCancelled()
		{
			var parsedEvent = ParseWebhookEvent<PhoneWarmTransferCancelledEvent>(WebhooksDataResource.phone_warm_transfer_cancelled);

			VerifyPhoneCallTransferEvent(parsedEvent, ZoomNet.Models.Webhooks.EventType.PhoneWarmTransferCancelled);

			VerifyCallTransferInfo(parsedEvent.CallTransfer);
		}

		[Fact]
		public void PhoneWarmTransferCompleted()
		{
			var parsedEvent = ParseWebhookEvent<PhoneWarmTransferCompletedEvent>(WebhooksDataResource.phone_warm_transfer_completed);

			VerifyPhoneCallTransferEvent(parsedEvent, ZoomNet.Models.Webhooks.EventType.PhoneWarmTransferCompleted);

			VerifyCallTransferInfo(parsedEvent.CallTransfer);
		}

		[Fact]
		public void PhoneWarmTransferInitiated()
		{
			var parsedEvent = ParseWebhookEvent<PhoneWarmTransferInitiatedEvent>(WebhooksDataResource.phone_warm_transfer_initiated);

			VerifyPhoneCallTransferEvent(parsedEvent, ZoomNet.Models.Webhooks.EventType.PhoneWarmTransferInitiated);

			VerifyCallTransferInfo(parsedEvent.CallTransfer);

			parsedEvent.TransferCallId.ShouldBe("6986878782238080584");
		}

		[Fact]
		public void PhoneAiCallSummaryChanged()
		{
			var parsedEvent = ParseWebhookEvent<PhoneAiCallSummaryChangedEvent>(WebhooksDataResource.phone_ai_call_summary_changed);

			parsedEvent.EventType.ShouldBe(ZoomNet.Models.Webhooks.EventType.PhoneAiCallSummaryChanged);
			parsedEvent.Timestamp.ShouldBe(eventTimestamp);
			parsedEvent.AccountId.ShouldBe(AccountId);

			VerifyAiCallSummary(parsedEvent.CallSummary);
		}

		[Fact]
		public void PhoneConferenceStarted()
		{
			var parsedEvent = ParseWebhookEvent<PhoneConferenceStartedEvent>(WebhooksDataResource.phone_conference_started);

			parsedEvent.EventType.ShouldBe(ZoomNet.Models.Webhooks.EventType.PhoneConferenceStarted);
			parsedEvent.Timestamp.ShouldBe(eventTimestamp);
			parsedEvent.AccountId.ShouldBe(AccountId);

			parsedEvent.ConferenceCall.ShouldNotBeNull();
			parsedEvent.ConferenceCall.Id.ShouldBe("2074969d-621f-41d3-890c-8a44a03fa3e0");
			parsedEvent.ConferenceCall.CallId.ShouldBe(CallId);
			parsedEvent.ConferenceCall.StartedOn.ShouldBe(timestamp);
			parsedEvent.ConferenceCall.EnableMultiplePartyConference.ShouldBeTrue();
			parsedEvent.ConferenceCall.FailureReason.ShouldBeEmpty();

			VerifyCallOwnerInfo(parsedEvent.ConferenceCall.Owner);
		}

		[Fact]
		public void PhoneDeviceRegistration()
		{
			var parsedEvent = ParseWebhookEvent<PhoneDeviceRegistrationEvent>(WebhooksDataResource.phone_device_registration);

			parsedEvent.EventType.ShouldBe(ZoomNet.Models.Webhooks.EventType.PhoneDeviceRegistration);
			parsedEvent.Timestamp.ShouldBe(eventTimestamp);
			parsedEvent.AccountId.ShouldBe(AccountId);

			parsedEvent.Device.ShouldNotBeNull();
			parsedEvent.Device.Id.ShouldBe("JOZmuJ30Spyrw-v9vUzIrA");
			parsedEvent.Device.Name.ShouldBe("New_DeskPhone");
			parsedEvent.Device.MacAddress.ShouldBe("012345678912");
		}

		[Fact]
		public void PhoneEmergencyAlert()
		{
			var parsedEvent = ParseWebhookEvent<PhoneEmergencyAlertEvent>(WebhooksDataResource.phone_emergency_alert);

			parsedEvent.EventType.ShouldBe(ZoomNet.Models.Webhooks.EventType.PhoneEmergencyAlert);
			parsedEvent.Timestamp.ShouldBe(eventTimestamp);
			parsedEvent.AccountId.ShouldBe(AccountId);

			VerifyEmergencyCallAlert(parsedEvent.Alert);
		}

		[Fact]
		public void PhoneGenericDeviceProvision()
		{
			var parsedEvent = ParseWebhookEvent<PhoneGenericDeviceProvisionEvent>(WebhooksDataResource.phone_generic_device_provision);

			parsedEvent.EventType.ShouldBe(ZoomNet.Models.Webhooks.EventType.PhoneGenericDeviceProvision);
			parsedEvent.Timestamp.ShouldBe(eventTimestamp);
			parsedEvent.AccountId.ShouldBe(AccountId);

			VerifyPhoneDevice(parsedEvent.Device);
		}

		[Fact]
		public void PhonePeeringNumberCallerIdNameUpdated()
		{
			var parsedEvent = ParseWebhookEvent<PhonePeeringNumberCallerIdNameUpdatedEvent>(WebhooksDataResource.phone_peering_number_cnam_updated);

			VerifyPeeringNumberUpdatedEvent(parsedEvent, ZoomNet.Models.Webhooks.EventType.PhonePeeringNumberCallerIdNameUpdated);

			VerifyWebhookPeeringNumberUpdate(parsedEvent.PeeringNumber);
		}

		[Fact]
		public void PhonePeeringNumberEmergencyAddressUpdated()
		{
			var parsedEvent = ParseWebhookEvent<PhonePeeringNumberEmergencyAddressUpdatedEvent>(WebhooksDataResource.phone_peering_number_emergency_address_updated);

			VerifyPeeringNumberUpdatedEvent(parsedEvent, ZoomNet.Models.Webhooks.EventType.PhonePeeringNumberEmergencyAddressUpdated);

			VerifyWebhookPeeringNumberUpdate(parsedEvent.PeeringNumber, callerIdName: null);
		}

		[Fact]
		public void PhoneAccountSettingsUpdated()
		{
			var parsedEvent = ParseWebhookEvent<PhoneAccountSettingsUpdatedEvent>(WebhooksDataResource.phone_account_settings_updated);

			parsedEvent.EventType.ShouldBe(ZoomNet.Models.Webhooks.EventType.PhoneAccountSettingsUpdated);
			parsedEvent.Timestamp.ShouldBe(eventTimestamp);
			parsedEvent.AccountId.ShouldBe(AccountId);

			parsedEvent.OldSettings.ShouldNotBeNull();
			parsedEvent.OldSettings.Id.ShouldBe("eUblMKLnTMKX0pVvNAlA0Q");
			parsedEvent.OldSettings.Settings.ShouldNotBeNull();

			parsedEvent.NewSettings.ShouldNotBeNull();
			parsedEvent.NewSettings.Id.ShouldBe("eUblMKLnTMKX0pVvNAlA0Q");

			var accountSettings = parsedEvent.NewSettings.Settings;
			accountSettings.ShouldNotBeNull();

			VerifyAdHocCallRecordingSettings(accountSettings.AdHocCallRecording);
			VerifyAdvancedEncryptionSettings(accountSettings.AdvancedEncryption);
			VerifyAllowedCallLocationsSettings(accountSettings.AllowedCallLocations);
			VerifySettingGroupBase(accountSettings.AudioIntercom);
			VerifySettingGroupBase(accountSettings.AutoCallFromThirdPartyApps);
			VerifyAutoCallRecordingSettings(accountSettings.AutoCallRecording);
			VerifyAutoDeleteDataSetings(accountSettings.AutoDeleteDataAfterRetentionDuration);
			VerifySettingGroupBase(accountSettings.BlockCallsAsThreat);
			VerifySettingGroupBase(accountSettings.BlockCallsWithoutCallerId);
			VerifyBlockExternalCallsSettings(accountSettings.BlockExternalCalls);
			VerifySettingGroupBase(accountSettings.BlockInboundCallsAndMessaging);
			VerifyCallForwardingSettings(accountSettings.CallHandlingForwardingToOtherUsers);
			VerifyCallLiveTranscriptionSettings(accountSettings.CallLiveTranscription);
			VerifyCallOverflowSettings(accountSettings.CallOverflow);
			VerifyCallParkSettings(accountSettings.CallPark);
			VerifyCallQueueOptOutReasonSettings(accountSettings.CallQueueOptOutReason);
			VerifyCallTransferringSettings(accountSettings.CallTransferring);
			VerifySettingGroupBase(accountSettings.CheckVoicemailsOverPhone);
			VerifySettingGroupBase(accountSettings.Delegation);
			VerifyDisplayCallFeedbackSurveySettings(accountSettings.DisplayCallFeedbackSurvey);
			VerifySettingGroupBase(accountSettings.EndToEndEncryption);
			VerifySettingGroupBase(accountSettings.ElevateToMeeting);
			VerifySettingGroupBase(accountSettings.ExternalCallingOnZoomRoomCommonArea);
			VerifySettingGroupBase(accountSettings.HandOffToRoom);
			VerifySettingGroupBase(accountSettings.InternationalCalling);
			VerifySettingGroupBase(accountSettings.LocalSurvivabilityMode);
			VerifySettingGroupBase(accountSettings.MobileSwitchToCarrier);
			VerifySettingGroupBase(accountSettings.OutboundCalling);
			VerifyOutboundSmsSettings(accountSettings.OutboundSms);
			VerifyOverrideDefaultPortSettings(accountSettings.OverrideDefaultPort);
			VerifySettingGroupBase(accountSettings.PeerToPeerMedia);
			VerifyPersonalAudioLibrarySettings(accountSettings.PersonalAudioLibrary);
			VerifyRestrictedCallHoursSettings(accountSettings.RestrictedCallHours);
			VerifySelectOutboundCallerIdSettings(accountSettings.SelectOutboundCallerId);
			VerifySettingGroupBase(accountSettings.SharedVoicemailNotificationByEmail);
			VerifySmsSettings(accountSettings.Sms);
			VerifySmsEtiquetteToolSettings(accountSettings.SmsEtiquetteTool);
			VerifyVoicemailSettings(accountSettings.Voicemail);
			VerifyVoicemailNotificationByEmailSettings(accountSettings.VoicemailNotificationByEmail);
			VerifySettingGroupBase(accountSettings.VoicemailTranscription);
			VerifyZoomPhoneOnDesktopSettings(accountSettings.ZoomPhoneOnDesktop);
			VerifyZoomPhoneOnMobileSettings(accountSettings.ZoomPhoneOnMobile);
			VerifyZoomPhoneOnPwaSettings(accountSettings.ZoomPhoneOnPwa);
		}

		[Fact]
		public void PhoneGroupSettingsUpdated()
		{
			var parsedEvent = ParseWebhookEvent<PhoneGroupSettingsUpdatedEvent>(WebhooksDataResource.phone_group_settings_updated);

			parsedEvent.EventType.ShouldBe(ZoomNet.Models.Webhooks.EventType.PhoneGroupSettingsUpdated);
			parsedEvent.Timestamp.ShouldBe(eventTimestamp);
			parsedEvent.AccountId.ShouldBe(AccountId);

			parsedEvent.OldSettings.ShouldNotBeNull();
			parsedEvent.OldSettings.GroupId.ShouldBe("IfRqsMZSSzGBKm2AFJ7r3Q");
			parsedEvent.OldSettings.Settings.ShouldNotBeNull();

			parsedEvent.NewSettings.ShouldNotBeNull();
			parsedEvent.NewSettings.GroupId.ShouldBe("IfRqsMZSSzGBKm2AFJ7r3Q");

			var groupSettings = parsedEvent.NewSettings.Settings;
			groupSettings.ShouldNotBeNull();

			VerifyAdHocCallRecordingSettings(groupSettings.AdHocCallRecording, AdministratorLevel.Site);
			VerifyAdvancedEncryptionSettings(groupSettings.AdvancedEncryption, AdministratorLevel.UserGroup);
			VerifyAllowEmergencyCallsSettings(groupSettings.AllowEmergencyCalls);
			VerifyAllowedCallLocationsSettings(groupSettings.AllowedCallLocations, AdministratorLevel.UserGroup);
			VerifySettingGroupBase(groupSettings.AudioIntercom, AdministratorLevel.UserGroup);
			VerifyAutoCallRecordingSettings(groupSettings.AutoCallRecording, AdministratorLevel.UserGroup);
			VerifySettingGroupBase(groupSettings.BlockCallsWithoutCallerId, AdministratorLevel.UserGroup);
			VerifyBlockExternalCallsSettings(groupSettings.BlockExternalCalls, AdministratorLevel.UserGroup);
			VerifySettingGroupBase(groupSettings.BlockInboundCallsAndMessaging, AdministratorLevel.UserGroup);
			VerifyCallForwardingSettings(groupSettings.CallHandlingForwardingToOtherUsers, AdministratorLevel.UserGroup);
			VerifyCallLiveTranscriptionSettings(groupSettings.CallLiveTranscription, AdministratorLevel.UserGroup);
			VerifyCallOverflowSettings(groupSettings.CallOverflow, AdministratorLevel.UserGroup);
			VerifyCallParkSettings(groupSettings.CallPark, AdministratorLevel.UserGroup);
			VerifyCallTransferringSettings(groupSettings.CallTransferring, AdministratorLevel.UserGroup);
			VerifySettingGroupBase(groupSettings.CheckVoicemailsOverPhone, AdministratorLevel.UserGroup);
			VerifySettingGroupBase(groupSettings.Delegation, AdministratorLevel.UserGroup);
			VerifyDisplayCallFeedbackSurveySettings(groupSettings.DisplayCallFeedbackSurvey, AdministratorLevel.UserGroup);
			VerifySettingGroupBase(groupSettings.EndToEndEncryption, AdministratorLevel.UserGroup);
			VerifySettingGroupBase(groupSettings.ElevateToMeeting, AdministratorLevel.UserGroup);
			VerifySettingGroupBase(groupSettings.HandOffToRoom, AdministratorLevel.UserGroup);
			VerifySettingGroupBase(groupSettings.InternationalCalling, AdministratorLevel.UserGroup);
			VerifySettingGroupBase(groupSettings.LocalSurvivabilityMode, AdministratorLevel.UserGroup);
			VerifySettingGroupBase(groupSettings.MobileSwitchToCarrier, AdministratorLevel.UserGroup);
			VerifySettingGroupBase(groupSettings.OutboundCalling, AdministratorLevel.UserGroup);
			VerifyOutboundSmsSettings(groupSettings.OutboundSms, AdministratorLevel.UserGroup);
			VerifySettingGroupBase(groupSettings.PeerToPeerMedia, AdministratorLevel.UserGroup);
			VerifyPersonalAudioLibrarySettings(groupSettings.PersonalAudioLibrary, AdministratorLevel.UserGroup);
			VerifyRestrictedCallHoursSettings(groupSettings.RestrictedCallHours, AdministratorLevel.UserGroup);
			VerifySelectOutboundCallerIdSettings(groupSettings.SelectOutboundCallerId, AdministratorLevel.UserGroup);
			VerifySettingGroupBase(groupSettings.SharedVoicemailNotificationByEmail, AdministratorLevel.UserGroup);
			VerifySmsSettings(groupSettings.Sms, AdministratorLevel.UserGroup);
			VerifySmsEtiquetteToolSettings(groupSettings.SmsEtiquetteTool, AdministratorLevel.UserGroup);
			VerifyVoicemailSettings(groupSettings.Voicemail, AdministratorLevel.UserGroup);
			VerifyVoicemailNotificationByEmailSettings(groupSettings.VoicemailNotificationByEmail, AdministratorLevel.UserGroup);
			VerifySettingGroupBase(groupSettings.VoicemailTranscription, AdministratorLevel.UserGroup);
			VerifyZoomPhoneOnDesktopSettings(groupSettings.ZoomPhoneOnDesktop, AdministratorLevel.Site);
			VerifyZoomPhoneOnMobileSettings(groupSettings.ZoomPhoneOnMobile, AdministratorLevel.UserGroup);
			VerifyZoomPhoneOnPwaSettings(groupSettings.ZoomPhoneOnPwa, AdministratorLevel.UserGroup);
		}

		[Fact]
		public void NumberManagementPeeringNumberCallerIdNameUpdated()
		{
			var parsedEvent = ParseWebhookEvent<NumberManagementPeeringNumberCallerIdNameUpdatedEvent>(WebhooksDataResource.number_management_peering_number_cnam_updated);

			VerifyPeeringNumberUpdatedEvent(parsedEvent, ZoomNet.Models.Webhooks.EventType.NumberManagementPeeringNumberCallerIdNameUpdated);

			VerifyWebhookPeeringNumberUpdate(parsedEvent.PeeringNumber);
		}

		[Fact]
		public void NumberManagementPeeringNumberEmergencyAddressUpdated()
		{
			var parsedEvent = ParseWebhookEvent<NumberManagementPeeringNumberEmergencyAddressUpdatedEvent>(WebhooksDataResource.number_management_peering_number_emergency_address_updated);

			VerifyPeeringNumberUpdatedEvent(parsedEvent, ZoomNet.Models.Webhooks.EventType.NumberManagementPeeringNumberEmergencyAddressUpdated);

			VerifyWebhookPeeringNumberUpdate(parsedEvent.PeeringNumber, callerIdName: null);
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

			recording.CallerNumberType.ShouldBe(PhoneCallNumberType.Internal);
			recording.CallerName.ShouldBe("Caller Name");

			recording.CalleeNumberType.ShouldBe(PhoneCallNumberType.External);
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

		/// <summary>
		/// Verify <see cref="PhoneVoicemailEvent"/> properties.
		/// </summary>
		private static void VerifyPhoneVoicemailEvent(PhoneVoicemailEvent parsedEvent, ZoomNet.Models.Webhooks.EventType eventType)
		{
			parsedEvent.EventType.ShouldBe(eventType);
			parsedEvent.Timestamp.ShouldBe(eventTimestamp);
			parsedEvent.AccountId.ShouldBe(AccountId);
		}

		/// <summary>
		/// Verify <see cref="WebhookVoicemail"/> properties.
		/// </summary>
		private static void VerifyVoicemail(WebhookVoicemail voicemail, bool voicemailTranscript = false, string callerUserId = "9Y0sGVcgRrW_9eV6kwLf7w")
		{
			voicemail.ShouldNotBeNull();
			voicemail.Id.ShouldBe("70ea156dc8414742839569c1556c3314");
			voicemail.CallId.ShouldBe(CallId);
			voicemail.CallLogId.ShouldBe("55fd0af0-beb0-433a-be97-388de5e99ab4");
			voicemail.CallElementId.ShouldBe(CallElementId);
			voicemail.CallHistoryId.ShouldBe(CallHistoryId);
			voicemail.CreatedOn.ShouldBe(timestamp);

			// There are some differences in available properties depending on voicemail or its transcript.
			if (!voicemailTranscript)
			{
				voicemail.DownloadUrl.ShouldBe("https://example.com");
				voicemail.Transcription.ShouldBeNull();

				voicemail.Duration.ShouldBe(3);

				voicemail.CalleeDidNumber.ShouldBe(PhoneNumberA);
				voicemail.CallerDidNumber.ShouldBe(PhoneNumberB);
			}
			else
			{
				voicemail.DownloadUrl.ShouldBeNull();
				voicemail.Transcription.ShouldNotBeNull();
				voicemail.Transcription.Status.ShouldBe(VoicemailTranscriptStatus.Completed);
				voicemail.Transcription.Content.ShouldBe("Some Transcript content");

				voicemail.Duration.ShouldBeNull();

				voicemail.CalleeDidNumber.ShouldBeNull();
				voicemail.CallerDidNumber.ShouldBeNull();
			}

			voicemail.CalleeId.ShouldBe("s6njoqZLT6aPvUQ0JyydeQ");
			voicemail.CalleeExtensionType.ShouldBe(PhoneCallExtensionType.User);
			voicemail.CalleeName.ShouldBe("Jill Chill");
			voicemail.CalleeNumber.ShouldBe("58011");
			voicemail.CalleeNumberType.ShouldBe(PhoneCallNumberType.Internal);
			voicemail.CalleeAccountCode.ShouldBe("123");
			voicemail.CalleeUserId.ShouldBe("s6njoqZLT6aPvUQ0JyydeQ");

			voicemail.CallerName.ShouldBe("Rascoe Thomas");
			voicemail.CallerNumber.ShouldBe("12058945456");
			voicemail.CallerNumberType.ShouldBe(PhoneCallNumberType.External);
			voicemail.CallerAccountCode.ShouldBe("456");
			voicemail.CallerUserId.ShouldBe(callerUserId);
		}

		/// <summary>
		/// Verify <see cref="PhoneCallTransferEvent"/> properties.
		/// </summary>
		private static void VerifyPhoneCallTransferEvent(PhoneCallTransferEvent parsedEvent, ZoomNet.Models.Webhooks.EventType eventType)
		{
			parsedEvent.EventType.ShouldBe(eventType);
			parsedEvent.Timestamp.ShouldBe(eventTimestamp);
			parsedEvent.AccountId.ShouldBe(AccountId);
		}

		/// <summary>
		/// Verify <see cref="WebhookCallTransferInfo"/> properties.
		/// </summary>
		private static void VerifyCallTransferInfo(
			WebhookCallTransferInfo info,
			string transferPhoneNumber = PhoneNumberA,
			string transferAccountCode = "123")
		{
			info.ShouldNotBeNull();
			info.CallId.ShouldBe(CallId);
			info.Timestamp.ShouldBe(timestamp);
			info.TransferPhoneNumber.ShouldBe(transferPhoneNumber);
			info.TransferAccountCode.ShouldBe(transferAccountCode);
			info.FailureReason.ShouldBe(string.Empty);

			VerifyCallOwnerInfo(info.Owner);
		}

		/// <summary>
		/// Verify <see cref="CallLogOwnerInfo"/> properties.
		/// </summary>
		private static void VerifyCallOwnerInfo(CallLogOwnerInfo info)
		{
			info.ShouldNotBeNull();
			info.Type.ShouldBe(CallLogOwnerType.User);
			info.Id.ShouldBe("z8yCxjabcdEFGHfp8uQ");
			info.Name.ShouldBe("Jill Chill");
			info.ExtensionNumber.ShouldBe(6666);
		}

		/// <summary>
		/// Verify <see cref="WebhookCallTransferRecipient"/> properties.
		/// </summary>
		private static void VerifyCallTransferRecipient(WebhookCallTransferRecipient info)
		{
			info.ShouldNotBeNull();
			info.ExtensionNumber.ShouldBe(1001020);
			info.PhoneNumber.ShouldBe(PhoneNumberB);
			info.Name.ShouldBe("Jane Smith");
		}

		/// <summary>
		/// Verify <see cref="AiCallSummary"/> properties.
		/// </summary>
		private static void VerifyAiCallSummary(AiCallSummary summary)
		{
			summary.ShouldNotBeNull();
			summary.Id.ShouldBe("iNsfqK6gQOCiILiKhrlLqQ");
			summary.CallId.ShouldBe(CallId);
			summary.UserId.ShouldBe("FvB3CRfOQUuhF1IOB176Tg");
			summary.CreatedOn.ShouldBe(new DateTime(2023, 10, 8, 16, 12, 4));
			summary.ModifiedOn.ShouldBe(new DateTime(2023, 10, 8, 16, 13, 5));
			summary.IsModified.ShouldBeTrue();
			summary.IsDeleted.ShouldBeFalse();

			summary.CallLogIds.ShouldNotBeNull();
			summary.CallLogIds.Length.ShouldBe(2);
			summary.CallLogIds.ShouldBeSubsetOf(new[] { "6afdf3e3-87e3-47d0-834c-6ee3598e3b96", "6afdf3e3-87e3-47d0-834c-6ee3598e3b00" });
		}

		/// <summary>
		/// Verify <see cref="EmergencyCallAlert"/> properties.
		/// </summary>
		private static void VerifyEmergencyCallAlert(EmergencyCallAlert alert)
		{
			alert.ShouldNotBeNull();
			alert.CallId.ShouldBe(CallId);
			alert.Router.ShouldBe(EmergencyCallSource.Zoom);
			alert.DeliverTo.ShouldBe(EmergencyCallDestination.SafetyTeam);
			alert.RingingStartedOn.ShouldBe(ringingStartedTimestamp);

			alert.Callee.ShouldNotBeNull();
			alert.Callee.PhoneNumber.ShouldBe("933");

			alert.Caller.ShouldNotBeNull();
			alert.Caller.UserId.ShouldBe("DnEopNmXQEGU2uvvzjgojw");
			alert.Caller.ExtensionNumber.ShouldBe("1002");
			alert.Caller.ExtensionType.ShouldBe(EmergencyCallExtensionType.User);
			alert.Caller.DisplayName.ShouldBe("pbxta api");
			alert.Caller.SiteId.ShouldBe(SiteId);
			alert.Caller.SiteName.ShouldBe("Main Site");
			alert.Caller.PhoneNumber.ShouldBe("+12192818492");
			alert.Caller.Timezone.ShouldBe(TimeZones.America_Los_Angeles);

			alert.Location.ShouldNotBeNull();
			alert.Location.BssId.ShouldNotBeNull();
			alert.Location.BssId.ShouldHaveSingleItem();
			alert.Location.BssId[0].ShouldBe("fc:7f:49:12:45:01");
			alert.Location.Gps.ShouldNotBeNull();
			alert.Location.Gps.ShouldHaveSingleItem();
			alert.Location.Gps[0].ShouldBe("31.29846,120.6645");
			alert.Location.IpAddress.ShouldNotBeNull();
			alert.Location.IpAddress.ShouldHaveSingleItem();
			alert.Location.IpAddress[0].ShouldBe("192.0.2.1,192.0.2.2");

			VerifyEmergencyAddress(alert.EmergencyAddress);
		}

		/// <summary>
		/// Verify <see cref="EmergencyAddress"/> properties.
		/// </summary>
		private static void VerifyEmergencyAddress(EmergencyAddress address)
		{
			address.ShouldNotBeNull();
			address.AddressLine1.ShouldBe("55 ALMADEN BLVD");
			address.AddressLine2.ShouldBe("8 Floor");
			address.City.ShouldBe("San Jose");
			address.Country.ShouldBe("US");
			address.StateCode.ShouldBe("CA");
			address.Zip.ShouldBe("95113");
		}

		/// <summary>
		/// Verify <see cref="PhoneDevice"/> properties.
		/// </summary>
		private static void VerifyPhoneDevice(PhoneDevice device)
		{
			device.ShouldNotBeNull();
			device.Id.ShouldBe("JOZmuJ30Spyrw-v9vUzIrA");
			device.DisplayName.ShouldBe("test_cap");
			device.Type.ShouldBe("Other");
			device.MacAddress.ShouldBe("012345678912");

			device.Site.ShouldNotBeNull();
			device.Site.Id.ShouldBe(SiteId);
			device.Site.Name.ShouldBe("Main Site");

			device.Provision.ShouldNotBeNull();
			device.Provision.Type.ShouldBe(DeviceProvisioningType.Manual);
			device.Provision.SipAccounts.ShouldNotBeNull();
			device.Provision.SipAccounts.ShouldHaveSingleItem();

			SipAccount sipAccount = device.Provision.SipAccounts[0];

			sipAccount.AuthorizationId.ShouldBe("875586205903");
			sipAccount.OutboundProxy.ShouldBe("example.com");
			sipAccount.Password.ShouldBe("4dL09r0H");
			sipAccount.SecondaryOutboundProxy.ShouldBe("example.com");
			sipAccount.SipDomain.ShouldBe("example.com");
			sipAccount.UserName.ShouldBe("83600015247557791369");

			SharedLine sharedLine = sipAccount.SharedLine;

			sharedLine.ShouldNotBeNull();
			sharedLine.Alias.ShouldBe("Example line");
			sharedLine.OutboundCallerId.ShouldBe(PhoneNumberB);
			sharedLine.LineSubscription.ShouldNotBeNull();
			sharedLine.LineSubscription.PhoneNumber.ShouldBe(PhoneNumberA);
			sharedLine.LineSubscription.ExtensionNumber.ShouldBe(1040);
			sharedLine.LineSubscription.DisplayName.ShouldBe("test_cap");
		}

		/// <summary>
		/// Verify <see cref="PeeringNumberUpdatedEvent"/> properties.
		/// </summary>
		private static void VerifyPeeringNumberUpdatedEvent(PeeringNumberUpdatedEvent parsedEvent, ZoomNet.Models.Webhooks.EventType eventType)
		{
			parsedEvent.EventType.ShouldBe(eventType);
			parsedEvent.Timestamp.ShouldBe(eventTimestamp);
			parsedEvent.AccountId.ShouldBe(AccountId);
		}

		/// <summary>
		/// Verify <see cref="WebhookPeeringNumberUpdate"/> properties.
		/// </summary>
		private static void VerifyWebhookPeeringNumberUpdate(WebhookPeeringNumberUpdate info, string callerIdName = "name")
		{
			info.ShouldNotBeNull();
			info.CarrierCode.ShouldBe(3457);
			info.PhoneNumbers.ShouldNotBeNull();
			info.PhoneNumbers.ShouldHaveSingleItem();
			info.PhoneNumbers[0].ShouldBe("+18008001000");

			if (!string.IsNullOrEmpty(callerIdName))
			{
				info.CallerIdName.ShouldBe(callerIdName);
				info.EmergencyAddress.ShouldBeNull();
			}
			else
			{
				VerifyEmergencyAddress(info.EmergencyAddress);

				info.CallerIdName.ShouldBeNull();
			}
		}

		/// <summary>
		/// Verify <see cref="SettingsGroupBase"/> properties.
		/// </summary>
		private static void VerifySettingGroupBase(SettingsGroupBase settings, AdministratorLevel level = AdministratorLevel.Account)
		{
			settings.ShouldNotBeNull();
			settings.Enabled.ShouldBeTrue();
			settings.LockedBy.ShouldBe(level);

			if (level == AdministratorLevel.Account)
			{
				settings.Locked.ShouldBe(false);
				settings.Modified.ShouldBeNull();
			}
			else
			{
				settings.Locked.ShouldBe(true);
				settings.Modified.ShouldBe(true);
			}
		}

		/// <summary>
		/// Verify <see cref="AdHocCallRecordingSettings"/> properties.
		/// </summary>
		private static void VerifyAdHocCallRecordingSettings(AdHocCallRecordingSettings settings, AdministratorLevel level = AdministratorLevel.Account)
		{
			VerifySettingGroupBase(settings, level);

			settings.AllowDelete.ShouldBe(true);
			settings.AllowDownload.ShouldBe(true);
			settings.RecordingExplicitConsent.ShouldBe(true);
			settings.RecordingStartPrompt.ShouldBe(true);
			settings.RecordingTranscription.ShouldBe(true);

			VerifyPlayRecordingBeepTone(settings.PlayRecordingBeepTone);
		}

		/// <summary>
		/// Verify <see cref="PlayRecordingBeepTone"/> properties.
		/// </summary>
		private static void VerifyPlayRecordingBeepTone(PlayRecordingBeepTone tone)
		{
			tone.ShouldNotBeNull();
			tone.Enabled.ShouldBeTrue();
			tone.PlayBeepMember.ShouldBe(PlayBeepMember.AllMembers);
			tone.PlayBeepVolume.ShouldBe(60);
			tone.PlayBeepTimeInterval.ShouldBe(15);
		}

		/// <summary>
		/// Verify <see cref="AdvancedEncryptionSettings"/> properties.
		/// </summary>
		private static void VerifyAdvancedEncryptionSettings(AdvancedEncryptionSettings settings, AdministratorLevel level = AdministratorLevel.Account)
		{
			VerifySettingGroupBase(settings, level);

			settings.DisableIncomingUnencryptedVoicemail.ShouldBe(true);
		}

		/// <summary>
		/// Verify <see cref="AllowedCallLocationsSettings"/> properties.
		/// </summary>
		private static void VerifyAllowedCallLocationsSettings(AllowedCallLocationsSettings settings, AdministratorLevel level = AdministratorLevel.Account)
		{
			VerifySettingGroupBase(settings, level);

			settings.AllowInternalCalls.ShouldBe(true);
			settings.LocationsApplied.ShouldBe(true);
		}

		/// <summary>
		/// Verify <see cref="AutoCallRecordingSettings"/> properties.
		/// </summary>
		private static void VerifyAutoCallRecordingSettings(AutoCallRecordingSettings settings, AdministratorLevel level = AdministratorLevel.Account)
		{
			VerifySettingGroupBase(settings, level);

			VerifyCallRecordingSettings(settings);

			settings.AllowStopResumeRecording.ShouldBe(true);
			settings.DisconnectOnRecordingFailure.ShouldBe(true);
			settings.RecordingCalls.ShouldBe(AutoRecordedCallType.Both);
			settings.RecordingStartPromptAudioId.ShouldBe("yCT14TwySDGVUypVlKNEyA");
		}

		/// <summary>
		/// Verify <see cref="CallRecordingSettings"/> properties.
		/// </summary>
		private static void VerifyCallRecordingSettings(CallRecordingSettings settings)
		{
			settings.RecordingExplicitConsent.ShouldBe(true);
			settings.RecordingStartPrompt.ShouldBe(true);
			settings.RecordingTranscription.ShouldBe(true);

			VerifyPlayRecordingBeepTone(settings.PlayRecordingBeepTone);
			VerifyRecordingAudioNotification(settings.InboundAudioNotification);
			VerifyRecordingAudioNotification(settings.OutboundAudioNotification);
		}

		/// <summary>
		/// Verify <see cref="RecordingAudioNotification"/> properties.
		/// </summary>
		private static void VerifyRecordingAudioNotification(RecordingAudioNotification notification)
		{
			notification.ShouldNotBeNull();
			notification.RecordingExplicitConsent.ShouldBeTrue();
			notification.RecordingStartPrompt.ShouldBeTrue();
			notification.RecordingStartPromptAudioId.ShouldBe("ySMexBgBQsioV8KKCUybTA");
		}

		/// <summary>
		/// Verify <see cref="AutoDeleteDataSettings"/> properties.
		/// </summary>
		private static void VerifyAutoDeleteDataSetings(AutoDeleteDataSettings settings)
		{
			VerifySettingGroupBase(settings);

			settings.DeleteType.ShouldBe(DeleteDataPolicy.Soft);
			settings.Items.ShouldNotBeNull();
			settings.Items.ShouldHaveSingleItem();
			settings.Items[0].Type.ShouldBe(DeleteDataType.CallLog);
			settings.Items[0].RetentionDuration.ShouldBe(-1);
			settings.Items[0].DurationUnit.ShouldBe(RetentionDurationUnit.Year);
		}

		/// <summary>
		/// Verify <see cref="BlockExternalCallsSettings"/> properties.
		/// </summary>
		private static void VerifyBlockExternalCallsSettings(BlockExternalCallsSettings settings, AdministratorLevel level = AdministratorLevel.Account)
		{
			VerifySettingGroupBase(settings, level);

			settings.BlockBusinessHours.ShouldBe(true);
			settings.BlockClosedHours.ShouldBe(true);
			settings.BlockHolidayHours.ShouldBe(true);
			settings.BlockCallAction.ShouldBe(BlockCallAction.ForwardToVoicemail);
		}

		/// <summary>
		/// Verify <see cref="CallForwardingSettings"/> properties.
		/// </summary>
		private static void VerifyCallForwardingSettings(CallForwardingSettings settings, AdministratorLevel level = AdministratorLevel.Account)
		{
			VerifySettingGroupBase(settings, level);

			settings.CallForwardingType.ShouldBe(CallRestrictionType.Low);
		}

		/// <summary>
		/// Verify <see cref="CallLiveTranscriptionSettings"/> properties.
		/// </summary>
		private static void VerifyCallLiveTranscriptionSettings(CallLiveTranscriptionSettings settings, AdministratorLevel level = AdministratorLevel.Account)
		{
			VerifySettingGroupBase(settings, level);

			VerifyTranscriptionStartPrompt(settings.TranscriptionStartPrompt);
		}

		/// <summary>
		/// Verify <see cref="TranscriptionStartPrompt"/> properties.
		/// </summary>
		private static void VerifyTranscriptionStartPrompt(TranscriptionStartPrompt prompt)
		{
			prompt.ShouldNotBeNull();
			prompt.Enabled.ShouldBeTrue();
			prompt.AudioId.ShouldBe("yCT14TwySDGVUypVlKNEyA");
			prompt.AudioName.ShouldBe("example.mp3");
		}

		/// <summary>
		/// Verify <see cref="CallOverflowSettings"/> properties.
		/// </summary>
		private static void VerifyCallOverflowSettings(CallOverflowSettings settings, AdministratorLevel level = AdministratorLevel.Account)
		{
			VerifySettingGroupBase(settings, level);

			settings.CallOverflowType.ShouldBe(CallRestrictionType.Low);
		}

		/// <summary>
		/// Verify <see cref="CallParkSettings"/> properties.
		/// </summary>
		private static void VerifyCallParkSettings(CallParkSettings settings, AdministratorLevel level = AdministratorLevel.Account)
		{
			VerifySettingGroupBase(settings, level);

			settings.ExpirationPeriod.ShouldBe(3);
			settings.CallNotPickedUpAction.ShouldBe(CallNotPickedUpAction.ForwardToAnotherExtension);
			settings.Sequence.ShouldBe(ParkedCallsAssignmentSequence.Random);

			VerifyForwardingExtension(settings.ForwardTo);
		}

		/// <summary>
		/// Verify <see cref="ForwardingExtension"/> properties.
		/// </summary>
		private static void VerifyForwardingExtension(ForwardingExtension extension)
		{
			extension.ShouldNotBeNull();
			extension.DisplayName.ShouldBe("ZOOM_API Test");
			extension.ExtensionId.ShouldBe("TO586CYlQFC_WCUvPRXytA");
			extension.ExtensionNumber.ShouldBe(100014);
			extension.ExtensionType.ShouldBe(ForwardingExtensionType.User);
			extension.Id.ShouldBe("oG_nYRFuTJiY1tu0Fur_4Q");
		}

		/// <summary>
		/// Verify <see cref="CallQueueOptOutReasonSettings"/> properties.
		/// </summary>
		private static void VerifyCallQueueOptOutReasonSettings(CallQueueOptOutReasonSettings settings)
		{
			VerifySettingGroupBase(settings);

			settings.OptOutReasons.ShouldNotBeNull();
			settings.OptOutReasons.ShouldHaveSingleItem();
			settings.OptOutReasons[0].ShouldNotBeNull();
			settings.OptOutReasons[0].Enabled.ShouldBeTrue();
			settings.OptOutReasons[0].Code.ShouldBe("Break");
			settings.OptOutReasons[0].IsSystem.ShouldBeTrue();
		}

		/// <summary>
		/// Verify <see cref="CallTransferringSettings"/> properties.
		/// </summary>
		private static void VerifyCallTransferringSettings(CallTransferringSettings settings, AdministratorLevel level = AdministratorLevel.Account)
		{
			VerifySettingGroupBase(settings, level);

			settings.CallTransferringType.ShouldBe(CallRestrictionType.Low);
		}

		/// <summary>
		/// Verify <see cref="DisplayCallFeedbackSurveySettings"/> properties.
		/// </summary>
		private static void VerifyDisplayCallFeedbackSurveySettings(DisplayCallFeedbackSurveySettings settings, AdministratorLevel level = AdministratorLevel.Account)
		{
			VerifySettingGroupBase(settings, level);

			settings.FeedbackType.ShouldBe(CallFeedbackType.EveryCall);

			settings.FeedbackDuration.ShouldNotBeNull();
			settings.FeedbackDuration.Enabled.ShouldBeTrue();
			settings.FeedbackDuration.Min.ShouldBe(0);
			settings.FeedbackDuration.Max.ShouldBe(60);

			settings.FeedbackMeanOpinionScore.ShouldNotBeNull();
			settings.FeedbackMeanOpinionScore.Enabled.ShouldBeTrue();
			settings.FeedbackMeanOpinionScore.Min.ShouldBe(1.1);
			settings.FeedbackMeanOpinionScore.Max.ShouldBe(3.0);
		}

		/// <summary>
		/// Verify <see cref="OutboundSmsSettings"/> properties.
		/// </summary>
		private static void VerifyOutboundSmsSettings(OutboundSmsSettings settings, AdministratorLevel level = AdministratorLevel.Account)
		{
			VerifySettingGroupBase(settings, level);

			settings.AllowCopy.ShouldBe(true);
			settings.AllowPaste.ShouldBe(true);
		}

		/// <summary>
		/// Verify <see cref="OverrideDefaultPortSettings"/> properties.
		/// </summary>
		private static void VerifyOverrideDefaultPortSettings(OverrideDefaultPortSettings settings)
		{
			VerifySettingGroupBase(settings);

			settings.MinPort.ShouldBe(9000);
			settings.MaxPort.ShouldBe(9998);
		}

		/// <summary>
		/// Verify <see cref="PersonalAudioLibrarySettings"/> properties.
		/// </summary>
		private static void VerifyPersonalAudioLibrarySettings(PersonalAudioLibrarySettings settings, AdministratorLevel level = AdministratorLevel.Account)
		{
			VerifySettingGroupBase(settings, level);

			settings.AllowMusicOnHoldCustomization.ShouldBe(true);
			settings.AllowVoicemailAndMessageGreetingCustomization.ShouldBe(true);
		}

		/// <summary>
		/// Verify <see cref="RestrictedCallHoursSettings"/> properties.
		/// </summary>
		private static void VerifyRestrictedCallHoursSettings(RestrictedCallHoursSettings settings, AdministratorLevel level = AdministratorLevel.Account)
		{
			VerifySettingGroupBase(settings, level);

			settings.RestrictedHolidayHoursApplied.ShouldBe(true);
			settings.RestrictedHolidayHoursApplied.ShouldBe(true);
			settings.AllowInternalCalls.ShouldBe(true);

			settings.Timezone.ShouldNotBeNull();
			settings.Timezone.Id.ShouldBe(TimeZones.America_Los_Angeles);
			settings.Timezone.Name.ShouldBe("(GMT-8:00) Los Angeles");
		}

		/// <summary>
		/// Verify <see cref="SelectOutboundCallerIdSettings"/> properties.
		/// </summary>
		private static void VerifySelectOutboundCallerIdSettings(SelectOutboundCallerIdSettings settings, AdministratorLevel level = AdministratorLevel.Account)
		{
			VerifySettingGroupBase(settings, level);

			settings.AllowHideOutboundCallerId.ShouldBe(true);
		}

		/// <summary>
		/// Verify <see cref="SmsSettings"/> properties.
		/// </summary>
		private static void VerifySmsSettings(SmsSettings settings, AdministratorLevel level = AdministratorLevel.Account)
		{
			VerifySettingGroupBase(settings, level);

			settings.AllowCopy.ShouldBe(true);
			settings.AllowPaste.ShouldBe(true);
			settings.InternationalSms.ShouldBe(true);
		}

		/// <summary>
		/// Verify <see cref="SmsEtiquetteToolSettings"/> properties.
		/// </summary>
		private static void VerifySmsEtiquetteToolSettings(SmsEtiquetteToolSettings settings, AdministratorLevel level = AdministratorLevel.Account)
		{
			VerifySettingGroupBase(settings, level);

			settings.SmsEtiquettePolicy.ShouldNotBeNull();
			settings.SmsEtiquettePolicy.ShouldHaveSingleItem();

			VerifySmsEtiquettePolicy(settings.SmsEtiquettePolicy[0]);
		}

		/// <summary>
		/// Verify <see cref="SmsEtiquettePolicy"/> properties.
		/// </summary>
		private static void VerifySmsEtiquettePolicy(SmsEtiquettePolicy policy)
		{
			policy.ShouldNotBeNull();
			policy.Id.ShouldBe("PdPtFFDbQhKr05WepCHhWQ");
			policy.Name.ShouldBe("invalid symbol");
			policy.Description.ShouldBe("invalid symbol description");
			policy.Rule.ShouldBe(SmsEtiquettePolicyRuleKind.Keywords);
			policy.Content.ShouldBe("test");
			policy.Action.ShouldBe(SmsEtiquettePolicyAction.BlockMessage);
			policy.Active.ShouldBeTrue();
		}

		/// <summary>
		/// Verify <see cref="VoicemailSettings"/> properties.
		/// </summary>
		private static void VerifyVoicemailSettings(VoicemailSettings settings, AdministratorLevel level = AdministratorLevel.Account)
		{
			VerifySettingGroupBase(settings, level);

			settings.AllowDelete.ShouldBe(true);
			settings.AllowDownload.ShouldBe(true);
			settings.AllowShare.ShouldBe(true);
			settings.AllowVideomail.ShouldBe(true);
			settings.AllowVirtualBackground.ShouldBe(true);
		}

		/// <summary>
		/// Verify <see cref="VoicemailNotificationByEmailSettings"/> properties.
		/// </summary>
		private static void VerifyVoicemailNotificationByEmailSettings(VoicemailNotificationByEmailSettings settings, AdministratorLevel level = AdministratorLevel.Account)
		{
			VerifySettingGroupBase(settings, level);

			settings.IncludeVoicemailFile.ShouldBe(true);
			settings.IncludeVoicemailTranscription.ShouldBe(true);
			settings.ForwardVoicemailToEmail.ShouldBe(true);
		}

		/// <summary>
		/// Verify <see cref="ZoomPhoneOnDesktopSettings"/> properties.
		/// </summary>
		private static void VerifyZoomPhoneOnDesktopSettings(ZoomPhoneOnDesktopSettings settings, AdministratorLevel level = AdministratorLevel.Account)
		{
			VerifySettingGroupBase(settings, level);

			settings.AllowCallingClients.ShouldNotBeNull();
			settings.AllowCallingClients.Length.ShouldBe(2);
			settings.AllowCallingClients[0].ShouldBe(DesktopClientType.MacOs);
			settings.AllowCallingClients[1].ShouldBe(DesktopClientType.Windows);

			settings.AllowSmsMmsClients.ShouldNotBeNull();
			settings.AllowSmsMmsClients.ShouldHaveSingleItem();
			settings.AllowSmsMmsClients[0].ShouldBe(DesktopClientType.VirtualDesktop);
		}

		/// <summary>
		/// Verify <see cref="ZoomPhoneOnMobileSettings"/> properties.
		/// </summary>
		private static void VerifyZoomPhoneOnMobileSettings(ZoomPhoneOnMobileSettings settings, AdministratorLevel level = AdministratorLevel.Account)
		{
			VerifySettingGroupBase(settings, level);

			settings.AllowCallingSmsMms.ShouldBe(true);

			settings.AllowCallingClients.ShouldNotBeNull();
			settings.AllowCallingClients.Length.ShouldBe(2);
			settings.AllowCallingClients[0].ShouldBe(MobileClientType.IOs);
			settings.AllowCallingClients[1].ShouldBe(MobileClientType.Android);

			settings.AllowSmsMmsClients.ShouldNotBeNull();
			settings.AllowSmsMmsClients.ShouldHaveSingleItem();
			settings.AllowSmsMmsClients[0].ShouldBe(MobileClientType.BlackBerry);
		}

		/// <summary>
		/// Verify <see cref="ZoomPhoneOnPwaSettings"/> properties.
		/// </summary>
		private static void VerifyZoomPhoneOnPwaSettings(ZoomPhoneOnPwaSettings settings, AdministratorLevel level = AdministratorLevel.Account)
		{
			VerifySettingGroupBase(settings, level);

			settings.AllowCalling.ShouldBe(true);
			settings.AllowSmsMms.ShouldBe(true);
		}

		/// <summary>
		/// Verify <see cref="AllowEmergencyCallsSettings"/> properties.
		/// </summary>
		private static void VerifyAllowEmergencyCallsSettings(AllowEmergencyCallsSettings settings)
		{
			VerifySettingGroupBase(settings, AdministratorLevel.UserGroup);

			settings.AllowEmergencyCallsFromClients.ShouldBe(true);
			settings.AllowEmergencyCallsFromDeskphones.ShouldBe(true);
		}

		#endregion
	}
}
