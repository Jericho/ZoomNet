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
		/// <param name="forwardedBy"></param>
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

		#endregion
	}
}
