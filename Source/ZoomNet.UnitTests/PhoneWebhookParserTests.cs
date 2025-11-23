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
			string callElementId = "20231008-48c1dfd4-91ce-4df5-8495-7c9e33d10869",
			string callHistoryUuid = "20231008-1ac1df2a-912e-d125-8a15-1a1233d10f1a",
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
			callElement.CalleeSiteId.ShouldBe("8f71O6rWT8KFUGQmJIFAdQ");
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

			callElement.RecordingId.ShouldBe("c71b360f6e774e3aa101453117b7e1a7");
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
			callLog.RecordingId.ShouldBe("c71b360f6e774e3aa101453117b7e1a7");
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
			callLog.Site.Id.ShouldBe("8f71O6rWT8KFUGQmJIFAdQ");
		}

		#endregion
	}
}
