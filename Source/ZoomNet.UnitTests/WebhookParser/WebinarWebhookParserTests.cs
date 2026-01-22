using Shouldly;
using System;
using Xunit;
using ZoomNet.Models;
using ZoomNet.Models.Webhooks;
using ZoomNet.UnitTests.Properties;

namespace ZoomNet.UnitTests.WebhookParser
{
	/// <summary>
	/// Unit tests that verify webinar webhook events parsing.
	/// </summary>
	public partial class WebhookParserTests
	{
		#region tests

		[Fact]
		public void WebinarServiceIssue()
		{
			var parsedEvent = ParseWebhookEvent<WebinarServiceIssueEvent>(Resource.webinar_alert_webhook);

			parsedEvent.EventType.ShouldBe(ZoomNet.Models.Webhooks.EventType.WebinarServiceIssue);
			parsedEvent.Timestamp.ShouldBe(eventTimestamp);
			parsedEvent.AccountId.ShouldBe(AccountId);

			VerifyWebinarInfo(parsedEvent.Webinar);

			parsedEvent.Issues.ShouldNotBeNull();
			parsedEvent.Issues.ShouldBe(new[] { "Unstable audio quality" });
		}

		[Fact]
		public void WebinarChatMessageFileDownloaded()
		{
			var parsedEvent = ParseWebhookEvent<WebinarChatMessageFileDownloadedEvent>(Resource.webinar_chat_message_file_downloaded_webhook);

			parsedEvent.EventType.ShouldBe(ZoomNet.Models.Webhooks.EventType.WebinarChatMessageFileDownloaded);

			parsedEvent.Timestamp.ShouldBe(eventTimestamp);
			parsedEvent.AccountId.ShouldBe(AccountId);
			parsedEvent.Operator.ShouldBe("user@example.com");
			parsedEvent.OperatorId.ShouldBe(OperatorId);
			parsedEvent.WebinarId.ShouldBe(MeetingId);
			parsedEvent.WebinarUuid.ShouldBe(MeetingUuid);
			parsedEvent.HostAccountId.ShouldBe("lmnop12345abcdefghijk");

			VerifyChatMessageFile(parsedEvent.File, "rw3dIsBRTpOyMOJmeKgdaQ", null);
		}

		[Fact]
		public void WebinarChatMessageFileSent()
		{
			var parsedEvent = ParseWebhookEvent<WebinarChatMessageFileSentEvent>(Resource.webinar_chat_message_file_sent_webhook);

			parsedEvent.EventType.ShouldBe(ZoomNet.Models.Webhooks.EventType.WebinarChatMessageFileSent);

			parsedEvent.Timestamp.ShouldBe(eventTimestamp);
			parsedEvent.AccountId.ShouldBe(AccountId);
			parsedEvent.WebinarId.ShouldBe(MeetingId);
			parsedEvent.WebinarUuid.ShouldBe(MeetingUuid);

			VerifyChatMessageFile(parsedEvent.File, null, "https://file.zoomdev.us/file/hsdAXySKRe2KgS-0YNeSXg");
			VerifyWebhookChatMessage(parsedEvent.Message, null, null);
			VerifyChatMessageSender(parsedEvent.Sender);
			VerifyChatMessageRecipient(parsedEvent.Recipient);
		}

		[Fact]
		public void WebinarChatMessageSent()
		{
			var parsedEvent = ParseWebhookEvent<WebinarChatMessageSentEvent>(Resource.webinar_chat_message_sent_webhook);

			parsedEvent.EventType.ShouldBe(ZoomNet.Models.Webhooks.EventType.WebinarChatMessageSent);

			parsedEvent.Timestamp.ShouldBe(eventTimestamp);
			parsedEvent.AccountId.ShouldBe(AccountId);
			parsedEvent.WebinarId.ShouldBe(MeetingId);
			parsedEvent.WebinarUuid.ShouldBe(MeetingUuid);

			VerifyWebhookChatMessage(
				parsedEvent.Message,
				"This is a test message",
				new[]
				{
					"MS17RDk0QTY3QUQtQkFGQy04QTJFLTI2RUEtNkYxQjRBRTU1MTk5fQ==",
					"MS17NDQ0OEU5MjMtM0JFOS1CMDA1LTQ0NDAtQjdGOTU0Rjk5MTkyfQ=="
				});
			VerifyChatMessageSender(parsedEvent.Sender);
			VerifyChatMessageRecipient(parsedEvent.Recipient);
		}

		[Fact]
		public void WebinarConvertedToMeeting()
		{
			var parsedEvent = ParseWebhookEvent<WebinarConvertedToMeetingEvent>(Resource.webinar_converted_to_meeting_webhook);

			parsedEvent.EventType.ShouldBe(ZoomNet.Models.Webhooks.EventType.WebinarConvertedToMeeting);

			parsedEvent.Timestamp.ShouldBe(eventTimestamp);
			parsedEvent.AccountId.ShouldBe(AccountId);
			parsedEvent.Operator.ShouldBe(OperatorEmail);
			parsedEvent.OperatorId.ShouldBe(OperatorId);

			VerifyMeetingInfo(parsedEvent.Meeting);
		}

		[Fact]
		public void WebinarCreated()
		{
			var parsedEvent = ParseWebhookEvent<WebinarCreatedEvent>(Resource.webinar_created_webhook);

			parsedEvent.EventType.ShouldBe(ZoomNet.Models.Webhooks.EventType.WebinarCreated);
			parsedEvent.Timestamp.ShouldBe(eventTimestamp);
			parsedEvent.AccountId.ShouldBe(AccountId);
			parsedEvent.Operator.ShouldBe(OperatorEmail);
			parsedEvent.OperatorId.ShouldBe(OperatorId);
			parsedEvent.Operation.ShouldBe("single");
			parsedEvent.CreationSource.ShouldBe(CreationSource.OpenApi);

			VerifyRecurringWebinar(parsedEvent.Webinar, joinUrl: "https://example.com", password: "XYz@BC!D1087", occurrenceDuration: 60, occurrenceStatus: "available");

			var recurringWebinar = (RecurringWebinar)parsedEvent.Webinar;

			recurringWebinar.RecurrenceInfo.ShouldNotBeNull();
			recurringWebinar.RecurrenceInfo.Type.ShouldBe(RecurrenceType.Daily);
			recurringWebinar.RecurrenceInfo.RepeatInterval.ShouldBe(2);
			recurringWebinar.RecurrenceInfo.EndTimes.ShouldBe(5);

			recurringWebinar.Settings.ShouldNotBeNull();
			recurringWebinar.Settings.UsePmi.ShouldNotBeNull();
			recurringWebinar.Settings.UsePmi.Value.ShouldBeTrue();
			recurringWebinar.Settings.AlternativeHosts.ShouldBe(UserEmail);
		}

		[Fact]
		public void WebinarDeleted()
		{
			var parsedEvent = ParseWebhookEvent<WebinarDeletedEvent>(Resource.webinar_deleted_webhook);

			parsedEvent.EventType.ShouldBe(ZoomNet.Models.Webhooks.EventType.WebinarDeleted);
			parsedEvent.Timestamp.ShouldBe(eventTimestamp);
			parsedEvent.AccountId.ShouldBe(AccountId);
			parsedEvent.Operator.ShouldBe(OperatorEmail);
			parsedEvent.OperatorId.ShouldBe(OperatorId);
			parsedEvent.Operation.ShouldBe("single");

			VerifyRecurringWebinar(parsedEvent.Webinar);
		}

		[Fact]
		public void WebinarEnded()
		{
			var parsedEvent = ParseWebhookEvent<WebinarEndedEvent>(Resource.webinar_ended_webhook);

			parsedEvent.EventType.ShouldBe(ZoomNet.Models.Webhooks.EventType.WebinarEnded);
			parsedEvent.Timestamp.ShouldBe(eventTimestamp);
			parsedEvent.AccountId.ShouldBe(AccountId);

			VerifyWebinarInfo(parsedEvent.Webinar);

			parsedEvent.EndTime.ShouldBe(new DateTime(2021, 7, 13, 23, 0, 51, DateTimeKind.Utc));
			parsedEvent.PracticeSession.ShouldBe(true);
		}

		[Fact]
		public void WebinarParticipantBind()
		{
			var parsedEvent = ParseWebhookEvent<WebinarParticipantBindEvent>(Resource.webinar_participant_bind_webhook);

			parsedEvent.EventType.ShouldBe(ZoomNet.Models.Webhooks.EventType.WebinarParticipantBind);

			VerifyWebinarParticipantEvent(parsedEvent);

			VerifyWebhookParticipant(parsedEvent.Participant, customerKey: null, email: null, participantUserId: null);

			parsedEvent.BindUserId.ShouldBe("16778240");
			parsedEvent.BindParticipantUuid.ShouldBe("B8E2844F-F162-4808-A997-F267BCE8B0BB");
			parsedEvent.JoinTime.ShouldBe(timestamp);
		}

		[Fact]
		public void WebinarParticipantFeedback()
		{
			var parsedEvent = ParseWebhookEvent<WebinarParticipantFeedbackEvent>(Resource.webinar_participant_feedback_webhook);

			parsedEvent.EventType.ShouldBe(ZoomNet.Models.Webhooks.EventType.WebinarParticipantFeedback);
			parsedEvent.Timestamp.ShouldBe(eventTimestamp);
			parsedEvent.AccountId.ShouldBe("q6gBJVO5TzexKYTb_I2rpg");

			parsedEvent.Webinar.ShouldNotBeNull();
			parsedEvent.Webinar.Id.ShouldBe(7357282288);
			parsedEvent.Webinar.Uuid.ShouldBe("0AErJTQBTI2bSXKyAxIq5A==");

			parsedEvent.Participant.ShouldNotBeNull();
			parsedEvent.Participant.ParticipantUserId.ShouldBe("zJKyaiAyTNC-MWjiWC18KQ");
			parsedEvent.Participant.ParticipantUuid.ShouldBe("I2rpg_q6gBJVO5TzexKYTb");
			parsedEvent.Participant.DisplayName.ShouldBe("jchill");

			parsedEvent.Feedback.ShouldNotBeNull();
			parsedEvent.Feedback.Satisfied.ShouldBeFalse();
			parsedEvent.Feedback.Comments.ShouldBe("good enough");
			parsedEvent.Feedback.Details.ShouldNotBeNull();
			parsedEvent.Feedback.Details.Length.ShouldBe(1);
			parsedEvent.Feedback.Details[0].Key.ShouldBe("2");
			parsedEvent.Feedback.Details[0].Value.ShouldBe("Poor audio quality");
		}

		[Fact]
		public void WebinarParticipantJoined()
		{
			var parsedEvent = ParseWebhookEvent<WebinarParticipantJoinedEvent>(Resource.webinar_participant_joined_webhook);

			parsedEvent.EventType.ShouldBe(ZoomNet.Models.Webhooks.EventType.WebinarParticipantJoined);

			VerifyWebinarParticipantEvent(parsedEvent);

			VerifyWebhookParticipant(parsedEvent.Participant);

			parsedEvent.JoinedOn.ShouldBe(timestamp);
		}

		[Fact]
		public void WebinarParticipantLeft()
		{
			var parsedEvent = ParseWebhookEvent<WebinarParticipantLeftEvent>(Resource.webinar_participant_left_webhook);

			parsedEvent.EventType.ShouldBe(ZoomNet.Models.Webhooks.EventType.WebinarParticipantLeft);

			VerifyWebinarParticipantEvent(parsedEvent);

			VerifyWebhookParticipant(parsedEvent.Participant);

			parsedEvent.LeftOn.ShouldBe(new DateTime(2021, 7, 13, 22, 50, 51, DateTimeKind.Utc));
			parsedEvent.LeaveReason.ShouldBe("Jill Chill left the webinar.<br>Reason: Host ended the webinar.");
		}

		[Fact]
		public void WebinarParticipantRoleChanged()
		{
			var parsedEvent = ParseWebhookEvent<WebinarParticipantRoleChangedEvent>(Resource.webinar_participant_role_changed_webhook);

			parsedEvent.EventType.ShouldBe(ZoomNet.Models.Webhooks.EventType.WebinarParticipantRoleChanged);

			VerifyWebinarParticipantEvent(parsedEvent);

			VerifyWebhookParticipant(parsedEvent.Participant, customerKey: null, phoneNumber: null);

			parsedEvent.ChangedOn.ShouldBe(timestamp);
			parsedEvent.NewRole.ShouldBe(ParticipantRole.CoHost);
			parsedEvent.OldRole.ShouldBe(ParticipantRole.Host);
		}

		[Fact]
		public void WebinarPermanentlyDeleted()
		{
			var parsedEvent = ParseWebhookEvent<WebinarPermanentlyDeletedEvent>(Resource.webinar_permanently_deleted_webhook);

			parsedEvent.EventType.ShouldBe(ZoomNet.Models.Webhooks.EventType.WebinarPermanentlyDeleted);
			parsedEvent.Timestamp.ShouldBe(eventTimestamp);
			parsedEvent.AccountId.ShouldBe(AccountId);
			parsedEvent.Operator.ShouldBe(OperatorEmail);
			parsedEvent.OperatorId.ShouldBe(OperatorId);
			parsedEvent.Operation.ShouldBe("single");

			VerifyRecurringWebinar(parsedEvent.Webinar);
		}

		[Fact]
		public void WebinarRecovered()
		{
			var parsedEvent = ParseWebhookEvent<WebinarRecoveredEvent>(Resource.webinar_recovered_webhook);

			parsedEvent.EventType.ShouldBe(ZoomNet.Models.Webhooks.EventType.WebinarRecovered);
			parsedEvent.Timestamp.ShouldBe(eventTimestamp);
			parsedEvent.AccountId.ShouldBe(AccountId);
			parsedEvent.Operator.ShouldBe(OperatorEmail);
			parsedEvent.OperatorId.ShouldBe(OperatorId);
			parsedEvent.Operation.ShouldBe("single");

			VerifyRecurringWebinar(parsedEvent.Webinar);
		}

		[Fact]
		public void WebinarRegistrationApproved()
		{
			var parsedEvent = ParseWebhookEvent<WebinarRegistrationApprovedEvent>(Resource.webinar_registration_approved_webhook);

			parsedEvent.EventType.ShouldBe(ZoomNet.Models.Webhooks.EventType.WebinarRegistrationApproved);

			VerifyWebinarRegistrationEvent(parsedEvent);

			parsedEvent.Operator.ShouldBe(OperatorEmail);
			parsedEvent.OperatorId.ShouldBe(OperatorId);

			VerifyRegistrant(parsedEvent.Registrant);

			parsedEvent.Registrant.JoinUrl.ShouldBe("https://example.com");
		}

		[Fact]
		public void WebinarRegistrationCancelled()
		{
			var parsedEvent = ParseWebhookEvent<WebinarRegistrationCancelledEvent>(Resource.webinar_registration_cancelled_webhook);

			parsedEvent.EventType.ShouldBe(ZoomNet.Models.Webhooks.EventType.WebinarRegistrationCancelled);

			VerifyWebinarRegistrationEvent(parsedEvent);

			parsedEvent.Operator.ShouldBe(OperatorEmail);
			parsedEvent.OperatorId.ShouldBe(OperatorId);

			VerifyRegistrant(parsedEvent.Registrant);
		}

		[Fact]
		public void WebinarRegistrationCreated()
		{
			var parsedEvent = ParseWebhookEvent<WebinarRegistrationCreatedEvent>(Resource.webinar_registration_created_webhook);

			parsedEvent.EventType.ShouldBe(ZoomNet.Models.Webhooks.EventType.WebinarRegistrationCreated);

			VerifyWebinarRegistrationEvent(parsedEvent);

			VerifyRegistrantExtended(parsedEvent.Registrant);

			parsedEvent.Registrant.Comments.ShouldBe("Looking forward to the webinar");
			parsedEvent.Registrant.CustomQuestions.ShouldNotBeNull();
			parsedEvent.Registrant.CustomQuestions.Length.ShouldBe(1);
			parsedEvent.Registrant.CustomQuestions[0].Key.ShouldBe("What do you hope to learn from this webinar?");
			parsedEvent.Registrant.CustomQuestions[0].Value.ShouldBe("Look forward to learning how you come up with new recipes and what other services you offer.");
		}

		[Fact]
		public void WebinarRegistrationDenied()
		{
			var parsedEvent = ParseWebhookEvent<WebinarRegistrationDeniedEvent>(Resource.webinar_registration_denied_webhook);

			parsedEvent.EventType.ShouldBe(ZoomNet.Models.Webhooks.EventType.WebinarRegistrationDenied);

			VerifyWebinarRegistrationEvent(parsedEvent);

			parsedEvent.Operator.ShouldBe(OperatorEmail);
			parsedEvent.OperatorId.ShouldBe(OperatorId);

			VerifyRegistrant(parsedEvent.Registrant);
		}

		[Fact]
		public void WebinarSharingStarted()
		{
			var parsedEvent = ParseWebhookEvent<WebinarSharingStartedEvent>(Resource.webinar_sharing_started_webhook);

			parsedEvent.EventType.ShouldBe(ZoomNet.Models.Webhooks.EventType.WebinarSharingStarted);

			VerifyWebinarSharingEvent(parsedEvent);
		}

		[Fact]
		public void WebinarSharingEnded()
		{
			var parsedEvent = ParseWebhookEvent<WebinarSharingEndedEvent>(Resource.webinar_sharing_ended_webhook);

			parsedEvent.EventType.ShouldBe(ZoomNet.Models.Webhooks.EventType.WebinarSharingEnded);

			VerifyWebinarSharingEvent(parsedEvent);
		}

		[Fact]
		public void WebinarStarted()
		{
			var parsedEvent = ParseWebhookEvent<WebinarStartedEvent>(Resource.webinar_started_webhook);

			parsedEvent.EventType.ShouldBe(ZoomNet.Models.Webhooks.EventType.WebinarStarted);
			parsedEvent.Timestamp.ShouldBe(eventTimestamp);
			parsedEvent.AccountId.ShouldBe(AccountId);

			VerifyWebinarInfo(parsedEvent.Webinar);
		}

		[Fact]
		public void WebinarUpdated()
		{
			var parsedEvent = ParseWebhookEvent<WebinarUpdatedEvent>(Resource.webinar_updated_webhook);

			parsedEvent.EventType.ShouldBe(ZoomNet.Models.Webhooks.EventType.WebinarUpdated);
			parsedEvent.Timestamp.ShouldBe(eventTimestamp);
			parsedEvent.Operator.ShouldBe(OperatorEmail);
			parsedEvent.OperatorId.ShouldBe(OperatorId);
			parsedEvent.Operation.ShouldBe("single");
			parsedEvent.UpdatedOn.ShouldBe(1617628463664.FromUnixTime(Internal.UnixTimePrecision.Milliseconds));

			parsedEvent.ModifiedFields.ShouldNotBeNull();
			parsedEvent.ModifiedFields.Length.ShouldBe(2);
			parsedEvent.ModifiedFields[0].FieldName.ShouldBe("topic");
			parsedEvent.ModifiedFields[0].OldValue.ShouldBe("ZoomNet Unit Testing: scheduled webinar");
			parsedEvent.ModifiedFields[0].NewValue.ShouldBe("ZoomNet Unit Testing: UPDATED scheduled webinar");
			parsedEvent.ModifiedFields[1].FieldName.ShouldBe("settings");

			parsedEvent.WebinarFields.ShouldNotBeNull();
			parsedEvent.WebinarFields.Length.ShouldBe(1);
			parsedEvent.WebinarFields[0].FieldName.ShouldBe("id");
			parsedEvent.WebinarFields[0].Value.ShouldBe(MeetingId);
		}

		#endregion

		#region private methods

		/// <summary>
		/// Verify <see cref="WebinarParticipantEvent"/> properties.
		/// </summary>
		private static void VerifyWebinarParticipantEvent(WebinarParticipantEvent parsedEvent)
		{
			parsedEvent.Timestamp.ShouldBe(eventTimestamp);
			parsedEvent.AccountId.ShouldBe(AccountId);

			VerifyWebinarInfo(parsedEvent.Webinar);
		}

		/// <summary>
		/// Verify <see cref="WebinarRegistrationEvent"/> properties.
		/// </summary>
		private static void VerifyWebinarRegistrationEvent(WebinarRegistrationEvent parsedEvent)
		{
			parsedEvent.Timestamp.ShouldBe(eventTimestamp);
			parsedEvent.AccountId.ShouldBe(AccountId);

			VerifyRecurringWebinar(parsedEvent.Webinar);

			VerifyTrackingSource(parsedEvent.TrackingSource);
		}

		/// <summary>
		/// Verify <see cref="WebinarInfo"/> properties.
		/// </summary>
		private static void VerifyWebinarInfo(WebinarInfo info)
		{
			info.ShouldNotBeNull();
			info.Id.ShouldBe(MeetingId);
			info.Uuid.ShouldBe(MeetingUuid);
			info.HostId.ShouldBe(HostId);
			info.Topic.ShouldBe("My Webinar");
			info.Type.ShouldBe(WebinarType.Regular);
			info.StartTime.ShouldBe(timestamp);
			info.Timezone.ShouldBe(TimeZones.America_Los_Angeles);
			info.Duration.ShouldBe(60);
		}

		/// <summary>
		/// Verify that webinar is a recurring webinar.
		/// </summary>
		private static void VerifyRecurringWebinar(
			Webinar info,
			string joinUrl = null,
			string password = null,
			int? occurrenceDuration = null,
			string occurrenceStatus = null)
		{
			info.ShouldNotBeNull();
			info.ShouldBeOfType<RecurringWebinar>();

			var parsedWebinar = (RecurringWebinar)info;

			parsedWebinar.Id.ShouldBe(MeetingId);
			parsedWebinar.Uuid.ShouldBe(MeetingUuid);
			parsedWebinar.HostId.ShouldBe(HostId);
			parsedWebinar.Topic.ShouldBe("My Webinar");
			parsedWebinar.Type.ShouldBe(WebinarType.RecurringFixedTime);
			parsedWebinar.Duration.ShouldBe(60);
			parsedWebinar.JoinUrl.ShouldBe(joinUrl);
			parsedWebinar.Password.ShouldBe(password);

			VerifyMeetingOccurrences(parsedWebinar.Occurrences, occurrenceDuration, occurrenceStatus);
		}

		/// <summary>
		/// Verify <see cref="TrackingSource"/> properties.
		/// </summary>
		private static void VerifyTrackingSource(TrackingSource trackingSource)
		{
			trackingSource.ShouldNotBeNull();
			trackingSource.Id.ShouldBe("5516482804110");
			trackingSource.Name.ShouldBe("general");
			trackingSource.TrackingUrl.ShouldBe("https://example.com/webinar/register/5516482804110/WN_juM2BGyLQMyQ_ZrqiGRhLg");
		}

		/// <summary>
		/// Verify <see cref="WebinarSharingEvent"/> properties.
		/// </summary>
		private static void VerifyWebinarSharingEvent(WebinarSharingEvent parsedEvent)
		{
			parsedEvent.Timestamp.ShouldBe(eventTimestamp);
			parsedEvent.AccountId.ShouldBe(AccountId);

			parsedEvent.Participant.ShouldNotBeNull();
			parsedEvent.Participant.DisplayName.ShouldBe("Jill Chill");
			parsedEvent.Participant.Email.ShouldBeNullOrEmpty();
			parsedEvent.Participant.ParticipantId.ShouldBe("iFxeBPYun6SAiWUzBcEkX");
			parsedEvent.Participant.UserId.ShouldBe("ABCDE12345");

			VerifyWebinarInfo(parsedEvent.Webinar);

			VerifyScreenshareDetails(parsedEvent.ScreenshareDetails);
		}

		#endregion
	}
}
