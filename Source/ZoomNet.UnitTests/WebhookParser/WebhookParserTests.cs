using Shouldly;
using System;
using Xunit;
using ZoomNet.Models;
using ZoomNet.Models.Webhooks;
using ZoomNet.UnitTests.Properties;

namespace ZoomNet.UnitTests.WebhookParser
{
	/// <summary>
	/// Unit tests that verify webhook events parsing.
	/// </summary>
	public partial class WebhookParserTests
	{
		#region private fields

		/// <summary>
		/// Timestamp widely used in test data.
		/// </summary>
		private static readonly DateTime timestamp = new DateTime(2021, 7, 13, 21, 44, 51, DateTimeKind.Utc);

		/// <summary>
		/// Timestamp widely used as event timestamp.
		/// </summary>
		private static readonly DateTime eventTimestamp = 1626230691572.FromUnixTime(Internal.UnixTimePrecision.Milliseconds);

		private const string AccountId = "AAAAAABBBB";
		private const string OperatorEmail = "admin@example.com";
		private const string OperatorId = "z8yCxjabcdEFGHfp8uQ";
		private const long MeetingId = 1234567890;
		private const string MeetingUuid = "4444AAAiAAAAAiAiAiiAii==";
		private const string HostId = "x1yCzABCDEfg23HiJKl4mN";
		private const string UserEmail = "jchill@example.com";
		private const string OwnerId = "rw3dIsBRTpOyMOJmeKgdaQ";

		#endregion

		#region tests

		[Fact]
		public void MeetingCreated()
		{
			var parsedEvent = ParseWebhookEvent<MeetingCreatedEvent>(Resource.meeting_created_webhook);

			parsedEvent.EventType.ShouldBe(ZoomNet.Models.Webhooks.EventType.MeetingCreated);
			parsedEvent.Timestamp.ShouldBe(1617628462392.FromUnixTime(Internal.UnixTimePrecision.Milliseconds));
			parsedEvent.AccountId.ShouldBe("VjZoEArIT5y-HlWxkV-tVA");
			parsedEvent.Operator.ShouldBe("someone@example.com");
			parsedEvent.OperatorId.ShouldBe("8lzIwvZTSOqjndWPbPqzuA");
			parsedEvent.Operation.ShouldBe("single");

			VerifyRecurringMeeting(
				parsedEvent.Meeting,
				joinUrl: "https://zoom.us/j/98884753832?pwd=c21EQzg0SXY2dlNTOFF2TENpSm1aQT09",
				password: "PaSsWoRd",
				pmi: "ABc$DE@F234",
				occurrenceDuration: 60,
				occurrenceStatus: "available");

			var recurringMeeting = (RecurringMeeting)parsedEvent.Meeting;

			recurringMeeting.RecurrenceInfo.ShouldNotBeNull();
			recurringMeeting.RecurrenceInfo.Type.ShouldBe(RecurrenceType.Daily);
			recurringMeeting.RecurrenceInfo.RepeatInterval.ShouldBe(2);
			recurringMeeting.RecurrenceInfo.EndTimes.ShouldBe(5);

			recurringMeeting.Settings.ShouldNotBeNull();
			recurringMeeting.Settings.UsePmi.ShouldNotBeNull();
			recurringMeeting.Settings.UsePmi.Value.ShouldBeFalse();
			recurringMeeting.Settings.AlternativeHosts.ShouldBe("althost1@example.com;althost2@example.com");
			recurringMeeting.Settings.JoinBeforeHost.ShouldNotBeNull();
			recurringMeeting.Settings.JoinBeforeHost.Value.ShouldBeTrue();
			recurringMeeting.Settings.JoinBeforeHostTime.ShouldNotBeNull();
			recurringMeeting.Settings.JoinBeforeHostTime.Value.ShouldBe(JoinBeforeHostTime.TenMinutes);
			recurringMeeting.Settings.Invitees.ShouldNotBeNull();
			recurringMeeting.Settings.Invitees.Length.ShouldBe(1);
			recurringMeeting.Settings.Invitees[0].Email.ShouldBe(UserEmail);
		}

		[Fact]
		public void MeetingDeleted()
		{
			var parsedEvent = ParseWebhookEvent<MeetingDeletedEvent>(Resource.meeting_deleted_webhook);

			parsedEvent.EventType.ShouldBe(ZoomNet.Models.Webhooks.EventType.MeetingDeleted);
			parsedEvent.Timestamp.ShouldBe(1617628462764.FromUnixTime(Internal.UnixTimePrecision.Milliseconds));
			parsedEvent.AccountId.ShouldBe("VjZoEArIT5y-HlWxkV-tVA");
			parsedEvent.Operator.ShouldBe("someone@example.com");
			parsedEvent.OperatorId.ShouldBe("8lzIwvZTSOqjndWPbPqzuA");
			parsedEvent.Operation.ShouldBe("single");

			parsedEvent.Meeting.ShouldNotBeNull();
			parsedEvent.Meeting.Uuid.ShouldBe("5yDZGNlQSV6qOjg4NxajHQ==");
			parsedEvent.Meeting.Id.ShouldBe(98884753832);
			parsedEvent.Meeting.HostId.ShouldBe("8lzIwvZTSOqjndWPbPqzuA");
			parsedEvent.Meeting.Topic.ShouldBe("ZoomNet Unit Testing: instant meeting");
			parsedEvent.Meeting.Type.ShouldBe(MeetingType.Instant);
			parsedEvent.Meeting.Timezone.ShouldBe(TimeZones.America_New_York);
			parsedEvent.Meeting.JoinUrl.ShouldBeNull();
			parsedEvent.Meeting.Password.ShouldBeNull();
			parsedEvent.Meeting.Settings.ShouldBeNull();
		}

		[Fact]
		public void MeetingPermanentlyDeleted()
		{
			var parsedEvent = ParseWebhookEvent<MeetingPermanentlyDeletedEvent>(Resource.meeting_permanently_deleted_webhook);

			parsedEvent.EventType.ShouldBe(ZoomNet.Models.Webhooks.EventType.MeetingPermanentlyDeleted);
			parsedEvent.Timestamp.ShouldBe(eventTimestamp);
			parsedEvent.AccountId.ShouldBe(AccountId);
			parsedEvent.Operator.ShouldBe(OperatorEmail);
			parsedEvent.OperatorId.ShouldBe(OperatorId);
			parsedEvent.Operation.ShouldBe("single");

			VerifyRecurringMeeting(parsedEvent.Meeting);
		}

		[Fact]
		public void MeetingRecovered()
		{
			var parsedEvent = ParseWebhookEvent<MeetingRecoveredEvent>(Resource.meeting_recovered_webhook);

			parsedEvent.EventType.ShouldBe(ZoomNet.Models.Webhooks.EventType.MeetingRecovered);
			parsedEvent.Timestamp.ShouldBe(eventTimestamp);
			parsedEvent.AccountId.ShouldBe(AccountId);
			parsedEvent.Operator.ShouldBe(OperatorEmail);
			parsedEvent.OperatorId.ShouldBe(OperatorId);
			parsedEvent.Operation.ShouldBe("single");

			VerifyRecurringMeeting(parsedEvent.Meeting);
		}

		[Fact]
		public void MeetingEnded()
		{
			var parsedEvent = ParseWebhookEvent<MeetingEndedEvent>(Resource.meeting_ended_webhook);

			parsedEvent.EventType.ShouldBe(ZoomNet.Models.Webhooks.EventType.MeetingEnded);
			parsedEvent.Timestamp.ShouldBe(eventTimestamp);
			parsedEvent.AccountId.ShouldBe(AccountId);

			VerifyMeetingInfo(parsedEvent.Meeting);

			parsedEvent.EndTime.ShouldBe(new DateTime(2021, 7, 13, 23, 0, 51, DateTimeKind.Utc));
		}

		[Fact]
		public void MeetingUpdated()
		{
			var parsedEvent = ParseWebhookEvent<MeetingUpdatedEvent>(Resource.meeting_updated_webhook);

			parsedEvent.EventType.ShouldBe(ZoomNet.Models.Webhooks.EventType.MeetingUpdated);
			parsedEvent.Timestamp.ShouldBe(1617628464664.FromUnixTime(Internal.UnixTimePrecision.Milliseconds));
			parsedEvent.Operator.ShouldBe("someone@example.com");
			parsedEvent.OperatorId.ShouldBe("8lzIwvZTSOqjndWPbPqzuA");
			parsedEvent.Operation.ShouldBe("single");
			parsedEvent.UpdatedOn.ShouldBe(1617628463664.FromUnixTime(Internal.UnixTimePrecision.Milliseconds));

			parsedEvent.ModifiedFields.ShouldNotBeNull();
			parsedEvent.ModifiedFields.Length.ShouldBe(2);
			parsedEvent.ModifiedFields[0].FieldName.ShouldBe("topic");
			parsedEvent.ModifiedFields[0].OldValue.ShouldBe("ZoomNet Unit Testing: scheduled meeting");
			parsedEvent.ModifiedFields[0].NewValue.ShouldBe("ZoomNet Unit Testing: UPDATED scheduled meeting");
			parsedEvent.ModifiedFields[1].FieldName.ShouldBe("settings");

			parsedEvent.MeetingFields.ShouldNotBeNull();
			parsedEvent.MeetingFields.Length.ShouldBe(1);
			parsedEvent.MeetingFields[0].FieldName.ShouldBe("id");
			parsedEvent.MeetingFields[0].Value.ShouldBe(94890226305);
		}

		[Fact]
		public void MeetingStarted()
		{
			var parsedEvent = ParseWebhookEvent<MeetingStartedEvent>(Resource.meeting_started_webhook);

			parsedEvent.EventType.ShouldBe(ZoomNet.Models.Webhooks.EventType.MeetingStarted);
			parsedEvent.Timestamp.ShouldBe(1619016544371.FromUnixTime(Internal.UnixTimePrecision.Milliseconds));
			parsedEvent.AccountId.ShouldBe("VjZoEArIT5y-HlWxkV-tVA");

			VerifyMeetingInfo(parsedEvent.Meeting);
		}

		[Fact]
		public void MeetingSharingStarted()
		{
			var parsedEvent = ParseWebhookEvent<MeetingSharingStartedEvent>(Resource.meeting_sharing_started_webhook);

			parsedEvent.EventType.ShouldBe(ZoomNet.Models.Webhooks.EventType.MeetingSharingStarted);

			VerifyMeetingSharingEvent(parsedEvent);
		}

		[Fact]
		public void MeetingSharingEnded()
		{
			var parsedEvent = ParseWebhookEvent<MeetingSharingEndedEvent>(Resource.meeting_sharing_ended_webhook);

			parsedEvent.EventType.ShouldBe(ZoomNet.Models.Webhooks.EventType.MeetingSharingEnded);

			VerifyMeetingSharingEvent(parsedEvent);
		}

		[Fact]
		public void MeetingServiceIssue()
		{
			var parsedEvent = ParseWebhookEvent<MeetingServiceIssueEvent>(Resource.meeting_alert_webhook);

			parsedEvent.EventType.ShouldBe(ZoomNet.Models.Webhooks.EventType.MeetingServiceIssue);
			parsedEvent.Timestamp.ShouldBe(eventTimestamp);
			parsedEvent.AccountId.ShouldBe(AccountId);

			parsedEvent.Issues.ShouldNotBeNull();
			parsedEvent.Issues.ShouldBe(["Unstable audio quality"]);

			VerifyMeetingInfo(parsedEvent.Meeting);
		}

		[Fact]
		public void MeetingParticipantAdmitted()
		{
			var parsedEvent = ParseWebhookEvent<MeetingParticipantAdmittedEvent>(Resource.meeting_participant_admitted_webhook);

			parsedEvent.EventType.ShouldBe(ZoomNet.Models.Webhooks.EventType.MeetingParticipantAdmitted);

			VerifyMeetingParticipantEvent(parsedEvent);

			VerifyWebhookParticipant(parsedEvent.Participant);

			parsedEvent.AdmittedOn.ShouldBe(timestamp);
		}

		[Fact]
		public void MeetingParticipantBind()
		{
			var parsedEvent = ParseWebhookEvent<MeetingParticipantBindEvent>(Resource.meeting_participant_bind_webhook);

			parsedEvent.EventType.ShouldBe(ZoomNet.Models.Webhooks.EventType.MeetingParticipantBind);

			VerifyMeetingParticipantEvent(parsedEvent);

			VerifyWebhookParticipant(parsedEvent.Participant, customerKey: null);

			parsedEvent.BindUserId.ShouldBe("16778240");
			parsedEvent.BindParticipantUuid.ShouldBe("B8E2844F-F162-4808-A997-F267BCE8B0BB");
			parsedEvent.BoundOn.ShouldBe(timestamp);
		}

		[Fact]
		public void MeetingParticipantFeedback()
		{
			var parsedEvent = ParseWebhookEvent<MeetingParticipantFeedbackEvent>(Resource.meeting_participant_feedback_webhook);

			parsedEvent.EventType.ShouldBe(ZoomNet.Models.Webhooks.EventType.MeetingParticipantFeedback);
			parsedEvent.Timestamp.ShouldBe(eventTimestamp);
			parsedEvent.AccountId.ShouldBe("q6gBJVO5TzexKYTb_I2rpg");

			parsedEvent.Meeting.ShouldNotBeNull();
			parsedEvent.Meeting.Id.ShouldBe(7357282288);
			parsedEvent.Meeting.Uuid.ShouldBe("0AErJTQBTI2bSXKyAxIq5A==");

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
		public void MeetingParticipantJoinedBeforeHost()
		{
			var parsedEvent = ParseWebhookEvent<MeetingParticipantJoinedBeforeHostEvent>(Resource.meeting_participant_jbh_joined_webhook);

			parsedEvent.EventType.ShouldBe(ZoomNet.Models.Webhooks.EventType.MeetingParticipantJoinedBeforeHost);

			VerifyMeetingParticipantEvent(parsedEvent);

			parsedEvent.Participant.ShouldNotBeNull();
			parsedEvent.Participant.UserId.ShouldBe("iFxeBPYun6SAiWUzBcEkX");
			parsedEvent.Participant.DisplayName.ShouldBe("Jill Chill");
			parsedEvent.Participant.CustomerKey.ShouldBe("349589LkJyeW");
			parsedEvent.Participant.RegistrantId.ShouldBe("abcdefghij0-klmnopq23456");
		}

		[Fact]
		public void MeetingParticipantWaitingForHost()
		{
			var parsedEvent = ParseWebhookEvent<MeetingParticipantWaitingForHostEvent>(Resource.meeting_participant_jbh_waiting_webhook);

			parsedEvent.EventType.ShouldBe(ZoomNet.Models.Webhooks.EventType.MeetingParticipantWaitingForHost);

			VerifyMeetingParticipantEvent(parsedEvent);

			parsedEvent.Participant.ShouldNotBeNull();
			parsedEvent.Participant.UserId.ShouldBe("iFxeBPYun6SAiWUzBcEkX");
			parsedEvent.Participant.DisplayName.ShouldBe("Jill Chill");
			parsedEvent.Participant.CustomerKey.ShouldBe("349589LkJyeW");
		}

		[Fact]
		public void MeetingParticipantWaitingForHostLeft()
		{
			var parsedEvent = ParseWebhookEvent<MeetingParticipantWaitingForHostLeftEvent>(Resource.meeting_participant_jbh_waiting_left_webhook);

			parsedEvent.EventType.ShouldBe(ZoomNet.Models.Webhooks.EventType.MeetingParticipantWaitingForHostLeft);

			VerifyMeetingParticipantEvent(parsedEvent);

			parsedEvent.Participant.ShouldNotBeNull();
			parsedEvent.Participant.UserId.ShouldBe("iFxeBPYun6SAiWUzBcEkX");
			parsedEvent.Participant.DisplayName.ShouldBe("Jill Chill");
			parsedEvent.Participant.CustomerKey.ShouldBe("349589LkJyeW");
			parsedEvent.Participant.RegistrantId.ShouldBe("abcdefghij0-klmnopq23456");
		}

		[Fact]
		public void MeetingParticipantJoined()
		{
			var parsedEvent = ParseWebhookEvent<MeetingParticipantJoinedEvent>(Resource.meeting_participant_joined_webhook);

			parsedEvent.EventType.ShouldBe(ZoomNet.Models.Webhooks.EventType.MeetingParticipantJoined);

			VerifyMeetingParticipantEvent(parsedEvent);

			VerifyWebhookParticipant(parsedEvent.Participant);

			parsedEvent.JoinedOn.ShouldBe(timestamp);
		}

		[Fact]
		public void MeetingParticipantJoinedWaitingRoom()
		{
			var parsedEvent = ParseWebhookEvent<MeetingParticipantJoinedWaitingRoomEvent>(Resource.meeting_participant_joined_waiting_room_webhook);

			parsedEvent.EventType.ShouldBe(ZoomNet.Models.Webhooks.EventType.MeetingParticipantJoinedWaitingRoom);

			VerifyMeetingParticipantEvent(parsedEvent);

			VerifyWebhookParticipant(parsedEvent.Participant);

			parsedEvent.JoinedOn.ShouldBe(timestamp);
		}

		[Fact]
		public void MeetingParticipantLeft()
		{
			var parsedEvent = ParseWebhookEvent<MeetingParticipantLeftEvent>(Resource.meeting_participant_left_webhook);

			parsedEvent.EventType.ShouldBe(ZoomNet.Models.Webhooks.EventType.MeetingParticipantLeft);

			VerifyMeetingParticipantEvent(parsedEvent);

			VerifyWebhookParticipant(parsedEvent.Participant);

			parsedEvent.LeftOn.ShouldBe(timestamp);
			parsedEvent.LeaveReason.ShouldBe("Jill Chill left the meeting.<br>Reason: Host ended the meeting.");
		}

		[Fact]
		public void MeetingParticipantLeftWaitingRoom()
		{
			var parsedEvent = ParseWebhookEvent<MeetingParticipantLeftWaitingRoomEvent>(Resource.meeting_participant_left_waiting_room_webhook);

			parsedEvent.EventType.ShouldBe(ZoomNet.Models.Webhooks.EventType.MeetingParticipantLeftWaitingRoom);

			VerifyMeetingParticipantEvent(parsedEvent);

			VerifyWebhookParticipant(parsedEvent.Participant);

			parsedEvent.LeftOn.ShouldBe(timestamp);
		}

		[Fact]
		public void MeetingParticipantSentToWaitingRoom()
		{
			var parsedEvent = ParseWebhookEvent<MeetingParticipantSentToWaitingRoomEvent>(Resource.meeting_participant_put_in_waiting_room_webhook);

			parsedEvent.EventType.ShouldBe(ZoomNet.Models.Webhooks.EventType.MeetingParticipantSentToWaitingRoom);

			VerifyMeetingParticipantEvent(parsedEvent);

			VerifyWebhookParticipant(parsedEvent.Participant);

			parsedEvent.SentToWaitingRoomOn.ShouldBe(timestamp);
		}

		[Fact]
		public void MeetingParticipantRoleChanged()
		{
			var parsedEvent = ParseWebhookEvent<MeetingParticipantRoleChangedEvent>(Resource.meeting_participant_role_changed_webhook);

			parsedEvent.EventType.ShouldBe(ZoomNet.Models.Webhooks.EventType.MeetingParticipantRoleChanged);

			VerifyMeetingParticipantEvent(parsedEvent);

			VerifyWebhookParticipant(parsedEvent.Participant, customerKey: null, phoneNumber: null);

			parsedEvent.ChangedOn.ShouldBe(timestamp);
			parsedEvent.NewRole.ShouldBe(ParticipantRole.CoHost);
			parsedEvent.OldRole.ShouldBe(ParticipantRole.Host);
		}

		[Fact]
		public void MeetingParticipantJoinedBreakoutRoom()
		{
			var parsedEvent = ParseWebhookEvent<MeetingParticipantJoinedBreakoutRoomEvent>(Resource.meeting_participant_joined_breakout_room_webhook);

			parsedEvent.EventType.ShouldBe(ZoomNet.Models.Webhooks.EventType.MeetingParticipantJoinedBreakoutRoom);

			VerifyMeetingBreakoutRoomEvent(parsedEvent);

			VerifyBreakoutRoomParticipantInfo(parsedEvent.Participant);

			parsedEvent.JoinTime.ShouldBe(new DateTime(2021, 7, 13, 21, 45, 51, DateTimeKind.Utc));
		}

		[Fact]
		public void MeetingParticipantLeftBreakoutRoom()
		{
			var parsedEvent = ParseWebhookEvent<MeetingParticipantLeftBreakoutRoomEvent>(Resource.meeting_participant_left_breakout_room_webhook);

			parsedEvent.EventType.ShouldBe(ZoomNet.Models.Webhooks.EventType.MeetingParticipantLeftBreakoutRoom);

			VerifyMeetingBreakoutRoomEvent(parsedEvent);

			VerifyBreakoutRoomParticipantInfo(parsedEvent.Participant);

			parsedEvent.LeaveTime.ShouldBe(new DateTime(2021, 7, 13, 22, 50, 51, DateTimeKind.Utc));
			parsedEvent.LeaveReason.ShouldBe("Jill Chill left the meeting.<br>Reason: Host ended the meeting.");
		}

		[Fact]
		public void MeetingBreakoutRoomSharingStarted()
		{
			var parsedEvent = ParseWebhookEvent<MeetingBreakoutRoomSharingStartedEvent>(Resource.meeting_breakout_room_sharing_started_webhook);

			parsedEvent.EventType.ShouldBe(ZoomNet.Models.Webhooks.EventType.MeetingBreakoutRoomSharingStarted);

			VerifyMeetingBreakoutRoomEvent(parsedEvent);

			VerifyBreakoutRoomParticipantBasicInfo(parsedEvent.Participant);

			VerifyScreenshareDetails(parsedEvent.SharingDetails);
		}

		[Fact]
		public void MeetingBreakoutRoomSharingEnded()
		{
			var parsedEvent = ParseWebhookEvent<MeetingBreakoutRoomSharingEndedEvent>(Resource.meeting_breakout_room_sharing_ended_webhook);

			parsedEvent.EventType.ShouldBe(ZoomNet.Models.Webhooks.EventType.MeetingBreakoutRoomSharingEnded);

			VerifyMeetingBreakoutRoomEvent(parsedEvent);

			VerifyBreakoutRoomParticipantBasicInfo(parsedEvent.Participant);

			VerifyScreenshareDetails(parsedEvent.SharingDetails);
		}

		[Fact]
		public void MeetingSummaryCompleted()
		{
			var parsedEvent = ParseWebhookEvent<MeetingSummaryCompletedEvent>(Resource.meeting_summary_completed_webhook);

			parsedEvent.EventType.ShouldBe(ZoomNet.Models.Webhooks.EventType.MeetingSummaryCompleted);

			VerifyMeetingSummaryEvent(parsedEvent);

			parsedEvent.MeetingSummary.SummaryContent.ShouldBe("## Key takeaways\n- Mobile app performance issues are affecting user retention.\n- New onboarding flow received positive feedback from beta testers.");
			parsedEvent.MeetingSummary.SummaryDocUrl.ShouldBe("https://docs.example.com/doc/1aBcDeFgHiJkLmNoPqRsTu");
			parsedEvent.MeetingSummary.SummaryLastModifiedUserEmail.ShouldBeNull();
			parsedEvent.MeetingSummary.SummaryLastModifiedUserId.ShouldBeNull();
		}

		[Fact]
		public void MeetingSummaryDeleted()
		{
			var parsedEvent = ParseWebhookEvent<MeetingSummaryDeletedEvent>(Resource.meeting_summary_deleted_webhook);

			parsedEvent.EventType.ShouldBe(ZoomNet.Models.Webhooks.EventType.MeetingSummaryDeleted);

			VerifyMeetingSummaryEvent(parsedEvent);

			parsedEvent.Operator.ShouldBe(OperatorEmail);
			parsedEvent.OperatorId.ShouldBe(OperatorId);
		}

		[Fact]
		public void MeetingSummaryRecovered()
		{
			var parsedEvent = ParseWebhookEvent<MeetingSummaryRecoveredEvent>(Resource.meeting_summary_recovered_webhook);

			parsedEvent.EventType.ShouldBe(ZoomNet.Models.Webhooks.EventType.MeetingSummaryRecovered);

			VerifyMeetingSummaryEvent(parsedEvent);

			parsedEvent.Operator.ShouldBe(OperatorEmail);
			parsedEvent.OperatorId.ShouldBe(OperatorId);
		}

		[Fact]
		public void MeetingSummaryShared()
		{
			var parsedEvent = ParseWebhookEvent<MeetingSummarySharedEvent>(Resource.meeting_summary_shared_webhook);

			parsedEvent.EventType.ShouldBe(ZoomNet.Models.Webhooks.EventType.MeetingSummaryShared);

			VerifyMeetingSummaryEvent(parsedEvent);

			parsedEvent.Operator.ShouldBe(OperatorEmail);
			parsedEvent.OperatorId.ShouldBe(OperatorId);

			parsedEvent.ShareWithUsers.ShouldNotBeNull();
			parsedEvent.ShareWithUsers.Length.ShouldBe(1);
			parsedEvent.ShareWithUsers[0].Email.ShouldBe(UserEmail);
		}

		[Fact]
		public void MeetingSummaryTrashed()
		{
			var parsedEvent = ParseWebhookEvent<MeetingSummaryTrashedEvent>(Resource.meeting_summary_trashed_webhook);

			parsedEvent.EventType.ShouldBe(ZoomNet.Models.Webhooks.EventType.MeetingSummaryTrashed);

			VerifyMeetingSummaryEvent(parsedEvent);

			parsedEvent.Operator.ShouldBe(OperatorEmail);
			parsedEvent.OperatorId.ShouldBe(OperatorId);
		}

		[Fact]
		public void MeetingSummaryUpdated()
		{
			var parsedEvent = ParseWebhookEvent<MeetingSummaryUpdatedEvent>(Resource.meeting_summary_updated_webhook);

			parsedEvent.EventType.ShouldBe(ZoomNet.Models.Webhooks.EventType.MeetingSummaryUpdated);

			VerifyMeetingSummaryEvent(parsedEvent);

			parsedEvent.Operator.ShouldBe(OperatorEmail);
			parsedEvent.OperatorId.ShouldBe(OperatorId);

			parsedEvent.MeetingSummary.SummaryLastModifiedUserId.ShouldBe("Lfi0BlBQTM-bbktE9BRUvA");
			parsedEvent.MeetingSummary.SummaryLastModifiedUserEmail.ShouldBe("user@example.com");
			parsedEvent.MeetingSummary.SummaryContent.ShouldBe("## Key takeaways\n- Mobile app performance issues are affecting user retention.\n- New onboarding flow received positive feedback from beta testers.");
			parsedEvent.MeetingSummary.SummaryDocUrl.ShouldBe("https://docs.example.com/doc/1aBcDeFgHiJkLmNoPqRsTu");
		}

		[Fact]
		public void MeetingInvitationAccepted()
		{
			var parsedEvent = ParseWebhookEvent<MeetingInvitationAcceptedEvent>(Resource.meeting_invitation_accepted_webhook);

			parsedEvent.EventType.ShouldBe(ZoomNet.Models.Webhooks.EventType.MeetingInvitationAccepted);

			VerifyMeetingInvitationEvent(parsedEvent);

			parsedEvent.Operator.ShouldBe("dhill@example.com");
			parsedEvent.OperatorId.ShouldBe("Hy7YgA-cR4eU8EfWV00tPQ");
		}

		[Fact]
		public void MeetingInvitationDispatched()
		{
			var parsedEvent = ParseWebhookEvent<MeetingInvitationDispatchedEvent>(Resource.meeting_invitation_dispatched_webhook);

			parsedEvent.EventType.ShouldBe(ZoomNet.Models.Webhooks.EventType.MeetingInvitationDispatched);

			VerifyMeetingInvitationEvent(parsedEvent);

			parsedEvent.Operator.ShouldBe("dhill@example.com");
			parsedEvent.OperatorId.ShouldBe("Hy7YgA-cR4eU8EfWV00tPQ");
		}

		[Fact]
		public void MeetingInvitationRejected()
		{
			var parsedEvent = ParseWebhookEvent<MeetingInvitationRejectedEvent>(Resource.meeting_invitation_rejected_webhook);

			parsedEvent.EventType.ShouldBe(ZoomNet.Models.Webhooks.EventType.MeetingInvitationRejected);

			VerifyMeetingInvitationEvent(parsedEvent);

			parsedEvent.Operator.ShouldBe("dhill@example.com");
			parsedEvent.OperatorId.ShouldBe("Hy7YgA-cR4eU8EfWV00tPQ");
		}

		[Fact]
		public void MeetingInvitationTimeout()
		{
			var parsedEvent = ParseWebhookEvent<MeetingInvitationTimeoutEvent>(Resource.meeting_invitation_timeout_webhook);

			parsedEvent.EventType.ShouldBe(ZoomNet.Models.Webhooks.EventType.MeetingInvitationTimeout);

			VerifyMeetingInvitationEvent(parsedEvent);

			parsedEvent.Operator.ShouldBe("dhill@example.com");
			parsedEvent.OperatorId.ShouldBe("Hy7YgA-cR4eU8EfWV00tPQ");
		}

		[Fact]
		public void MeetingParticipantPhoneCalloutAccepted()
		{
			var parsedEvent = ParseWebhookEvent<MeetingParticipantPhoneCalloutAcceptedEvent>(Resource.meeting_participant_phone_callout_accepted_webhook);

			parsedEvent.EventType.ShouldBe(ZoomNet.Models.Webhooks.EventType.MeetingParticipantPhoneCalloutAccepted);

			VerifyMeetingParticipantPhoneCalloutEvent(parsedEvent);
		}

		[Fact]
		public void MeetingParticipantPhoneCalloutMissed()
		{
			var parsedEvent = ParseWebhookEvent<MeetingParticipantPhoneCalloutMissedEvent>(Resource.meeting_participant_phone_callout_missed_webhook);

			parsedEvent.EventType.ShouldBe(ZoomNet.Models.Webhooks.EventType.MeetingParticipantPhoneCalloutMissed);

			VerifyMeetingParticipantPhoneCalloutEvent(parsedEvent);
		}

		[Fact]
		public void MeetingParticipantPhoneCalloutRejected()
		{
			var parsedEvent = ParseWebhookEvent<MeetingParticipantPhoneCalloutRejectedEvent>(Resource.meeting_participant_phone_callout_rejected_webhook);

			parsedEvent.EventType.ShouldBe(ZoomNet.Models.Webhooks.EventType.MeetingParticipantPhoneCalloutRejected);

			VerifyMeetingParticipantPhoneCalloutEvent(parsedEvent);
		}

		[Fact]
		public void MeetingParticipantPhoneCalloutRinging()
		{
			var parsedEvent = ParseWebhookEvent<MeetingParticipantPhoneCalloutRingingEvent>(Resource.meeting_participant_phone_callout_ringing_webhook);

			parsedEvent.EventType.ShouldBe(ZoomNet.Models.Webhooks.EventType.MeetingParticipantPhoneCalloutRinging);

			VerifyMeetingParticipantPhoneCalloutEvent(parsedEvent);
		}

		[Fact]
		public void MeetingParticipantRoomSystemCalloutAccepted()
		{
			var parsedEvent = ParseWebhookEvent<MeetingParticipantRoomSystemCalloutAcceptedEvent>(Resource.meeting_participant_room_system_callout_accepted_webhook);

			parsedEvent.EventType.ShouldBe(ZoomNet.Models.Webhooks.EventType.MeetingParticipantRoomSystemCalloutAccepted);

			VerifyMeetingParticipantRoomSystemCalloutEvent(parsedEvent);
		}

		[Fact]
		public void MeetingParticipantRoomSystemCalloutFailed()
		{
			var parsedEvent = ParseWebhookEvent<MeetingParticipantRoomSystemCalloutFailedEvent>(Resource.meeting_participant_room_system_callout_failed_webhook);

			parsedEvent.EventType.ShouldBe(ZoomNet.Models.Webhooks.EventType.MeetingParticipantRoomSystemCalloutFailed);

			VerifyMeetingParticipantRoomSystemCalloutEvent(parsedEvent);

			parsedEvent.ReasonType.ShouldBe(MeetingRoomCalloutFailureReason.EncryptionFail);
		}

		[Fact]
		public void MeetingParticipantRoomSystemCalloutMissed()
		{
			var parsedEvent = ParseWebhookEvent<MeetingParticipantRoomSystemCalloutMissedEvent>(Resource.meeting_participant_room_system_callout_missed_webhook);

			parsedEvent.EventType.ShouldBe(ZoomNet.Models.Webhooks.EventType.MeetingParticipantRoomSystemCalloutMissed);

			VerifyMeetingParticipantRoomSystemCalloutEvent(parsedEvent);
		}

		[Fact]
		public void MeetingParticipantRoomSystemCalloutRejected()
		{
			var parsedEvent = ParseWebhookEvent<MeetingParticipantRoomSystemCalloutRejectedEvent>(Resource.meeting_participant_room_system_callout_rejected_webhook);

			parsedEvent.EventType.ShouldBe(ZoomNet.Models.Webhooks.EventType.MeetingParticipantRoomSystemCalloutRejected);

			VerifyMeetingParticipantRoomSystemCalloutEvent(parsedEvent);
		}

		[Fact]
		public void MeetingParticipantRoomSystemCalloutRinging()
		{
			var parsedEvent = ParseWebhookEvent<MeetingParticipantRoomSystemCalloutRingingEvent>(Resource.meeting_participant_room_system_callout_ringing_webhook);

			parsedEvent.EventType.ShouldBe(ZoomNet.Models.Webhooks.EventType.MeetingParticipantRoomSystemCalloutRinging);

			VerifyMeetingParticipantRoomSystemCalloutEvent(parsedEvent);
		}

		[Fact]
		public void MeetingAiCompanionAssetsDeleted()
		{
			var parsedEvent = ParseWebhookEvent<MeetingAiCompanionAssetsDeletedEvent>(Resource.meeting_ai_companion_assets_deleted_webhook);

			parsedEvent.EventType.ShouldBe(ZoomNet.Models.Webhooks.EventType.MeetingAiCompanionAssetsDeleted);

			VerifyMeetingAiCompanionEvent(parsedEvent);

			parsedEvent.DeletedAssets.ShouldNotBeNull();
			parsedEvent.DeletedAssets.Length.ShouldBe(1);
			parsedEvent.DeletedAssets[0].ShouldBe("transcripts");
		}

		[Fact]
		public void MeetingAiCompanionStarted()
		{
			var parsedEvent = ParseWebhookEvent<MeetingAiCompanionStartedEvent>(Resource.meeting_ai_companion_started_webhook);

			parsedEvent.EventType.ShouldBe(ZoomNet.Models.Webhooks.EventType.MeetingAiCompanionStarted);

			VerifyMeetingAiCompanionEvent(parsedEvent);

			parsedEvent.Questions.ShouldBe(true);
			parsedEvent.Summary.ShouldBe(false);
		}

		[Fact]
		public void MeetingAiCompanionStopped()
		{
			var parsedEvent = ParseWebhookEvent<MeetingAiCompanionStoppedEvent>(Resource.meeting_ai_companion_stopped_webhook);

			parsedEvent.EventType.ShouldBe(ZoomNet.Models.Webhooks.EventType.MeetingAiCompanionStopped);

			VerifyMeetingAiCompanionEvent(parsedEvent);

			parsedEvent.Questions.ShouldBe(false);
			parsedEvent.Summary.ShouldBe(true);
		}

		[Fact]
		public void MeetingAicTranscriptCompleted()
		{
			var parsedEvent = ParseWebhookEvent<MeetingAicTranscriptCompletedEvent>(Resource.meeting_aic_transcript_completed_webhook);

			parsedEvent.EventType.ShouldBe(ZoomNet.Models.Webhooks.EventType.MeetingAicTranscriptCompleted);

			parsedEvent.Timestamp.ShouldBe(eventTimestamp);
			parsedEvent.AccountId.ShouldBe("dzVA4QmMQfyISoRcpFO8CA");

			parsedEvent.MeetingTranscript.ShouldNotBeNull();
			parsedEvent.MeetingTranscript.Id.ShouldBe(97763643886);
			parsedEvent.MeetingTranscript.Uuid.ShouldBe("aDYlohsHRtCd4ii1uC2+hA==");
			parsedEvent.MeetingTranscript.HostId.ShouldBe("30R7kT7bTIKSNUFEuH_Qlg");
			parsedEvent.MeetingTranscript.Topic.ShouldBe("My Meeting");
			parsedEvent.MeetingTranscript.StartTime.ShouldBe(new DateTime(2019, 7, 15, 23, 24, 52, DateTimeKind.Utc));
			parsedEvent.MeetingTranscript.FileId.ShouldBe("97F060CE-B123-4A57-A9E1-CC241BC1C555");
			parsedEvent.MeetingTranscript.AttachType.ShouldBe("durable_transcript");
		}

		[Fact]
		public void MeetingChatMessageFileDownloaded()
		{
			var parsedEvent = ParseWebhookEvent<MeetingChatMessageFileDownloadedEvent>(Resource.meeting_chat_message_file_downloaded_webhook);

			parsedEvent.EventType.ShouldBe(ZoomNet.Models.Webhooks.EventType.MeetingChatMessageFileDownloaded);

			parsedEvent.Timestamp.ShouldBe(eventTimestamp);
			parsedEvent.AccountId.ShouldBe(AccountId);
			parsedEvent.Operator.ShouldBe("user@example.com");
			parsedEvent.OperatorId.ShouldBe(OperatorId);
			parsedEvent.MeetingId.ShouldBe(MeetingId);
			parsedEvent.MeetingUuid.ShouldBe(MeetingUuid);
			parsedEvent.HostAccountId.ShouldBe("lmnop12345abcdefghijk");

			VerifyChatMessageFile(parsedEvent.File, OwnerId, null);
		}

		[Fact]
		public void MeetingChatMessageFileSent()
		{
			var parsedEvent = ParseWebhookEvent<MeetingChatMessageFileSentEvent>(Resource.meeting_chat_message_file_sent_webhook);

			parsedEvent.EventType.ShouldBe(ZoomNet.Models.Webhooks.EventType.MeetingChatMessageFileSent);

			parsedEvent.Timestamp.ShouldBe(eventTimestamp);
			parsedEvent.AccountId.ShouldBe(AccountId);
			parsedEvent.MeetingId.ShouldBe(MeetingId);
			parsedEvent.MeetingUuid.ShouldBe(MeetingUuid);

			VerifyChatMessageFile(parsedEvent.File, null, "https://file.zoomdev.us/file/hsdAXySKRe2KgS-0YNeSXg");
			VerifyWebhookChatMessage(parsedEvent.Message, null, null);
			VerifyChatMessageSender(parsedEvent.Sender);
			VerifyChatMessageRecipient(parsedEvent.Recipient);
		}

		[Fact]
		public void MeetingChatMessageSent()
		{
			var parsedEvent = ParseWebhookEvent<MeetingChatMessageSentEvent>(Resource.meeting_chat_message_sent_webhook);

			parsedEvent.EventType.ShouldBe(ZoomNet.Models.Webhooks.EventType.MeetingChatMessageSent);

			parsedEvent.Timestamp.ShouldBe(eventTimestamp);
			parsedEvent.AccountId.ShouldBe(AccountId);
			parsedEvent.MeetingId.ShouldBe(MeetingId);
			parsedEvent.MeetingUuid.ShouldBe(MeetingUuid);

			VerifyWebhookChatMessage(
				parsedEvent.Message,
				"This is a test message",
				[
					"MS17RDk0QTY3QUQtQkFGQy04QTJFLTI2RUEtNkYxQjRBRTU1MTk5fQ==",
					"MS17NDQ0OEU5MjMtM0JFOS1CMDA1LTQ0NDAtQjdGOTU0Rjk5MTkyfQ=="
				]);
			VerifyChatMessageSender(parsedEvent.Sender);
			VerifyChatMessageRecipient(parsedEvent.Recipient);
		}

		[Fact]
		public void MeetingConvertedToWebinar()
		{
			var parsedEvent = ParseWebhookEvent<MeetingConvertedToWebinarEvent>(Resource.meeting_converted_to_webinar_webhook);

			parsedEvent.EventType.ShouldBe(ZoomNet.Models.Webhooks.EventType.MeetingConvertedToWebinar);

			parsedEvent.Timestamp.ShouldBe(eventTimestamp);
			parsedEvent.AccountId.ShouldBe(AccountId);
			parsedEvent.Operator.ShouldBe(OperatorEmail);
			parsedEvent.OperatorId.ShouldBe(OperatorId);

			VerifyWebinarInfo(parsedEvent.Webinar);
		}

		[Fact]
		public void MeetingDeviceTested()
		{
			var parsedEvent = ParseWebhookEvent<MeetingDeviceTestedEvent>(Resource.meeting_device_tested_webhook);

			parsedEvent.EventType.ShouldBe(ZoomNet.Models.Webhooks.EventType.MeetingDeviceTested);

			parsedEvent.Timestamp.ShouldBe(eventTimestamp);
			parsedEvent.AccountId.ShouldBe(AccountId);

			parsedEvent.Meeting.ShouldNotBeNull();
			parsedEvent.Meeting.Id.ShouldBe(MeetingId);
			parsedEvent.Meeting.Uuid.ShouldBe(MeetingUuid);
			parsedEvent.Meeting.Topic.ShouldBeNull();
			parsedEvent.Meeting.HostId.ShouldBeNull();

			parsedEvent.TestResult.ShouldNotBeNull();
			parsedEvent.TestResult.UserId.ShouldBe("1234567890");
			parsedEvent.TestResult.UserName.ShouldBe("JillChill");
			parsedEvent.TestResult.CameraStatus.ShouldBe(DeviceTestStatus.NotTested);
			parsedEvent.TestResult.SpeakerStatus.ShouldBe(DeviceTestStatus.NotWorking);
			parsedEvent.TestResult.MicrophoneStatus.ShouldBe(DeviceTestStatus.Working);
			parsedEvent.TestResult.OperatingSystem.ShouldBe("ios");
		}

		[Fact]
		public void MeetingRiskAlert()
		{
			var parsedEvent = ParseWebhookEvent<MeetingRiskAlertEvent>(Resource.meeting_risk_alert_webhook);

			parsedEvent.EventType.ShouldBe(ZoomNet.Models.Webhooks.EventType.MeetingRiskAlert);

			parsedEvent.Timestamp.ShouldBe(1620872887470.FromUnixTime(Internal.UnixTimePrecision.Milliseconds));
			parsedEvent.AccountId.ShouldBe("XTYf8rirS_W0emt2TlxsYQ");

			parsedEvent.Meeting.ShouldNotBeNull();
			parsedEvent.Meeting.Id.ShouldBe(MeetingId);
			parsedEvent.Meeting.Uuid.ShouldBe(MeetingUuid);
			parsedEvent.Meeting.HostId.ShouldBe(HostId);
			parsedEvent.Meeting.Topic.ShouldBe("My Meeting");
			parsedEvent.Meeting.Type.ShouldBe(MeetingType.Scheduled);
			parsedEvent.Meeting.StartTime.ShouldBe(timestamp);
			parsedEvent.Meeting.Timezone.ShouldBe(TimeZones.America_Los_Angeles);

			parsedEvent.ArmnDetails.ShouldNotBeNull();
			parsedEvent.ArmnDetails.PostPlatform.ShouldBe("twitter");
			parsedEvent.ArmnDetails.PostTime.ShouldBe(new DateTime(2021, 5, 15, 2, 28, 0, DateTimeKind.Utc));
			parsedEvent.ArmnDetails.PostUser.ShouldBe("user");
			parsedEvent.ArmnDetails.MeetingUrl.ShouldBe("https://example.com/j/82390661051");
			parsedEvent.ArmnDetails.SocialLink.ShouldBe("https://example.com/user/status/1372556404872069120");
			parsedEvent.ArmnDetails.RecommendedEnableSettings.ShouldNotBeNull();
			parsedEvent.ArmnDetails.RecommendedEnableSettings.Length.ShouldBe(2);
			parsedEvent.ArmnDetails.RecommendedEnableSettings.ShouldBeSubsetOf(new[] { RecommendedSetting.EnablePassword, RecommendedSetting.EnableWaitingRoom });
			parsedEvent.ArmnDetails.RecommendedDisableSettings.ShouldNotBeNull();
			parsedEvent.ArmnDetails.RecommendedDisableSettings.Length.ShouldBe(2);
			parsedEvent.ArmnDetails.RecommendedDisableSettings.ShouldBeSubsetOf(new[] { RecommendedSetting.EnableScreenShareHostOnly, RecommendedSetting.EnableMeetingChat });
		}

		[Fact]
		public void MeetingLiveStreamingStarted()
		{
			var parsedEvent = ParseWebhookEvent<MeetingLiveStreamStartedEvent>(Resource.meeting_live_streaming_started_webhook);

			parsedEvent.EventType.ShouldBe(ZoomNet.Models.Webhooks.EventType.MeetingLiveStreamStarted);

			VerifyMeetingLiveStreamEvent(parsedEvent);

			parsedEvent.StartedOn.ShouldBe(new DateTime(2021, 8, 2, 12, 22, 45, DateTimeKind.Utc));
		}

		[Fact]
		public void MeetingLiveStreamingStopped()
		{
			var parsedEvent = ParseWebhookEvent<MeetingLiveStreamStoppedEvent>(Resource.meeting_live_streaming_stopped_webhook);

			parsedEvent.EventType.ShouldBe(ZoomNet.Models.Webhooks.EventType.MeetingLiveStreamStopped);

			VerifyMeetingLiveStreamEvent(parsedEvent);

			parsedEvent.StoppedOn.ShouldBe(new DateTime(2021, 8, 2, 12, 22, 45, DateTimeKind.Utc));
		}

		[Fact]
		public void MeetingRegistrationApproved()
		{
			var parsedEvent = ParseWebhookEvent<MeetingRegistrationApprovedEvent>(Resource.meeting_registration_approved_webhook);

			parsedEvent.EventType.ShouldBe(ZoomNet.Models.Webhooks.EventType.MeetingRegistrationApproved);

			VerifyMeetingRegistrationEvent(parsedEvent);

			parsedEvent.Operator.ShouldBe(OperatorEmail);
			parsedEvent.OperatorId.ShouldBe(OperatorId);

			VerifyRegistrant(parsedEvent.Registrant);

			parsedEvent.Registrant.JoinUrl.ShouldBe("https://example.com");
			parsedEvent.Registrant.ParticipantPinCode.ShouldBe(380303);
		}

		[Fact]
		public void MeetingRegistrationCancelled()
		{
			var parsedEvent = ParseWebhookEvent<MeetingRegistrationCancelledEvent>(Resource.meeting_registration_cancelled_webhook);

			parsedEvent.EventType.ShouldBe(ZoomNet.Models.Webhooks.EventType.MeetingRegistrationCancelled);

			VerifyMeetingRegistrationEvent(parsedEvent);

			parsedEvent.Operator.ShouldBe(OperatorEmail);
			parsedEvent.OperatorId.ShouldBe(OperatorId);

			VerifyRegistrant(parsedEvent.Registrant);
		}

		[Fact]
		public void MeetingRegistrationCreated()
		{
			var parsedEvent = ParseWebhookEvent<MeetingRegistrationCreatedEvent>(Resource.meeting_registration_created_webhook);

			parsedEvent.EventType.ShouldBe(ZoomNet.Models.Webhooks.EventType.MeetingRegistrationCreated);

			VerifyMeetingRegistrationEvent(parsedEvent);

			VerifyRegistrantExtended(parsedEvent.Registrant, pinCode: 380303);

			parsedEvent.Registrant.Comments.ShouldBe("Looking forward to the meeting");
			parsedEvent.Registrant.CustomQuestions.ShouldNotBeNull();
			parsedEvent.Registrant.CustomQuestions.Length.ShouldBe(1);
			parsedEvent.Registrant.CustomQuestions[0].Key.ShouldBe("What do you hope to learn from this meeting?");
			parsedEvent.Registrant.CustomQuestions[0].Value.ShouldBe("Look forward to learning how you come up with new recipes and what other services you offer.");
		}

		[Fact]
		public void MeetingRegistrationDenied()
		{
			var parsedEvent = ParseWebhookEvent<MeetingRegistrationDeniedEvent>(Resource.meeting_registration_denied_webhook);

			parsedEvent.EventType.ShouldBe(ZoomNet.Models.Webhooks.EventType.MeetingRegistrationDenied);

			VerifyMeetingRegistrationEvent(parsedEvent);

			parsedEvent.Operator.ShouldBe(OperatorEmail);
			parsedEvent.OperatorId.ShouldBe(OperatorId);

			VerifyRegistrant(parsedEvent.Registrant);
		}

		[Fact]
		public void EndpointUrlValidation()
		{
			string plainToken = "qgg8vlvZRS6UYooatFL8Aw";

			var parsedEvent = ParseWebhookEvent<EndpointUrlValidationEvent>(Resource.endpoint_url_validation_webhook);

			parsedEvent.EventType.ShouldBe(ZoomNet.Models.Webhooks.EventType.EndpointUrlValidation);
			parsedEvent.Timestamp.ShouldBe(eventTimestamp);
			parsedEvent.PlainToken.ShouldBe(plainToken);

			string jsonResponse = parsedEvent.GenerateUrlValidationResponse("UB_rdTwhTruoYCMr6x6-MQ");
			var parsedResponse = System.Text.Json.Nodes.JsonNode.Parse(jsonResponse);

			parsedResponse["plainToken"]?.ToString().ShouldBe(plainToken);
			parsedResponse["encryptedToken"]?.ToString().ShouldBe("439d1ed256e8d5513d2acd195e0adc64bbbfeb6b795b9d1880534610b58c674c");
		}

		#endregion

		#region private methods

		/// <summary>
		/// Wrapper for webhook event parsing and casting to specified type.
		/// </summary>
		private static T ParseWebhookEvent<T>(string webhookBody)
			where T : ZoomNet.Models.Webhooks.Event
		{
			return (T)new ZoomNet.WebhookParser().ParseEventWebhook(webhookBody);
		}

		/// <summary>
		/// Verify <see cref="MeetingSummaryEvent"/> properties.
		/// </summary>
		private static void VerifyMeetingSummaryEvent(MeetingSummaryEvent parsedEvent)
		{
			parsedEvent.Timestamp.ShouldBe(eventTimestamp);
			parsedEvent.AccountId.ShouldBe("dzVA4QmMQfyISoRcpFO8CA");

			parsedEvent.MeetingSummary.ShouldNotBeNull();
			parsedEvent.MeetingSummary.MeetingId.ShouldBe(97763643886);
			parsedEvent.MeetingSummary.MeetingUuid.ShouldBe("aDYlohsHRtCd4ii1uC2+hA==");
			parsedEvent.MeetingSummary.MeetingStartTime.ShouldBe(new DateTime(2019, 7, 15, 23, 24, 52, DateTimeKind.Utc));
			parsedEvent.MeetingSummary.MeetingEndTime.ShouldBe(new DateTime(2019, 7, 15, 23, 30, 19, DateTimeKind.Utc));
			parsedEvent.MeetingSummary.MeetingHostEmail.ShouldBe(UserEmail);
			parsedEvent.MeetingSummary.MeetingHostId.ShouldBe("30R7kT7bTIKSNUFEuH_Qlg");
			parsedEvent.MeetingSummary.MeetingTopic.ShouldBe("My Meeting");
			parsedEvent.MeetingSummary.SummaryCreatedTime.ShouldBe(new DateTime(2019, 7, 15, 23, 31, 47, DateTimeKind.Utc));
			parsedEvent.MeetingSummary.SummaryStartTime.ShouldBe(new DateTime(2019, 7, 15, 23, 24, 52, DateTimeKind.Utc));
			parsedEvent.MeetingSummary.SummaryEndTime.ShouldBe(new DateTime(2019, 7, 15, 23, 30, 19, DateTimeKind.Utc));
			parsedEvent.MeetingSummary.SummaryLastModifiedTime.ShouldBe(new DateTime(2019, 7, 15, 23, 32, 19, DateTimeKind.Utc));
			parsedEvent.MeetingSummary.SummaryTitle.ShouldBe("Meeting Summary for my meeting");
		}

		/// <summary>
		/// Verify <see cref="MeetingParticipantEvent"/> properties.
		/// </summary>
		private static void VerifyMeetingParticipantEvent(MeetingParticipantEvent parsedEvent)
		{
			parsedEvent.Timestamp.ShouldBe(eventTimestamp);
			parsedEvent.AccountId.ShouldBe(AccountId);

			VerifyMeetingInfo(parsedEvent.Meeting);
		}

		/// <summary>
		/// Verify <see cref="MeetingInfo"/> properties.
		/// </summary>
		private static void VerifyMeetingInfo(MeetingInfo info)
		{
			info.ShouldNotBeNull();
			info.Id.ShouldBe(MeetingId);
			info.Uuid.ShouldBe(MeetingUuid);
			info.HostId.ShouldBe(HostId);
			info.Topic.ShouldBe("My Meeting");
			info.Type.ShouldBe(MeetingType.RecurringFixedTime);
			info.StartTime.ShouldBe(timestamp);
			info.Timezone.ShouldBe(TimeZones.America_Los_Angeles);
			info.Duration.ShouldBe(60);
		}

		/// <summary>
		/// Verify <see cref="WebhookParticipant"/> properties.
		/// </summary>
		private static void VerifyWebhookParticipant(
			WebhookParticipant info,
			string customerKey = "349589LkJyeW",
			string phoneNumber = "8615250064084",
			string email = UserEmail,
			string participantUserId = "iFxeBPYun6SAiWUzBcEkX")
		{
			info.ShouldNotBeNull();
			info.UserId.ShouldBe("iFxeBPYun6SAiWUzBcEkX");
			info.DisplayName.ShouldBe("Jill Chill");
			info.ParticipantId.ShouldBe("55555AAAiAAAAAiAiAiiAii");
			info.ParticipantUuid.ShouldBe("55555AAAiAAAAAiAiAiiAii");
			info.Email.ShouldBe(email);
			info.RegistrantId.ShouldBe("abcdefghij0-klmnopq23456");
			info.ParticipantUserId.ShouldBe(participantUserId);
			info.PhoneNumber.ShouldBe(phoneNumber);
			info.CustomerKey.ShouldBe(customerKey);
		}

		/// <summary>
		/// Verify <see cref="MeetingBreakoutRoomEvent"/> properties.
		/// </summary>
		private static void VerifyMeetingBreakoutRoomEvent(MeetingBreakoutRoomEvent parsedEvent)
		{
			parsedEvent.Timestamp.ShouldBe(eventTimestamp);
			parsedEvent.AccountId.ShouldBe(AccountId);

			VerifyBreakoutRoomMeetingInfo(parsedEvent.Meeting);
		}

		/// <summary>
		/// Verify <see cref="BreakoutRoomMeetingInfo"/> properties.
		/// </summary>
		private static void VerifyBreakoutRoomMeetingInfo(BreakoutRoomMeetingInfo info)
		{
			VerifyMeetingInfo(info);

			info.BreakoutRoomUuid.ShouldBe("FkQbpP2UR028mDrwzEahqw==");
		}

		/// <summary>
		/// Verify <see cref="BreakoutRoomParticipantInfo"/> properties.
		/// </summary>
		private static void VerifyBreakoutRoomParticipantInfo(BreakoutRoomParticipantInfo info)
		{
			info.ShouldNotBeNull();
			info.UserId.ShouldBe("31228928");
			info.ParentUserId.ShouldBe("1234567890");
			info.UserName.ShouldBe("Jill Chill");
			info.Id.ShouldBe("iFxeBPYun6SAiWUzBcEkX");
			info.ParticipantUuid.ShouldBe("55555AAAiAAAAAiAiAiiAii");
			info.Email.ShouldBe(UserEmail);
			info.RegistrantId.ShouldBe("abcdefghij0-klmnopq23456");
			info.ParticipantUserId.ShouldBe("rstuvwxyza789-cde");
			info.PhoneNumber.ShouldBe("8615250064084");
			info.CustomerKey.ShouldBe("349589LkJyeW");
		}

		/// <summary>
		/// Verify <see cref="BreakoutRoomParticipantBasicInfo"/> properties.
		/// </summary>
		private static void VerifyBreakoutRoomParticipantBasicInfo(BreakoutRoomParticipantBasicInfo info)
		{
			info.ShouldNotBeNull();
			info.UserId.ShouldBe("31228928");
			info.ParentUserId.ShouldBe("ABCDE12345");
			info.UserName.ShouldBe("JillChill");
			info.Id.ShouldBe("iFxeBPYun6SAiWUzBcEkX");
		}

		/// <summary>
		/// Verify <see cref="MeetingInvitationEvent"/> properties.
		/// </summary>
		private static void VerifyMeetingInvitationEvent(MeetingInvitationEvent parsedEvent)
		{
			parsedEvent.Timestamp.ShouldBe(eventTimestamp);
			parsedEvent.AccountId.ShouldBe("dzVA4QmMQfyISoRcpFO8CA");

			parsedEvent.Meeting.ShouldNotBeNull();
			parsedEvent.Meeting.Id.ShouldBe(91578204824);
			parsedEvent.Meeting.Uuid.ShouldBe(MeetingUuid);
			parsedEvent.Meeting.HostId.ShouldBe("ICuPoX4ERtikRcKqkVxunQ");
			parsedEvent.Meeting.Topic.ShouldBe("Jill Chill's Zoom Meeting");

			parsedEvent.Participant.ShouldNotBeNull();
			parsedEvent.Participant.Email.ShouldBe(UserEmail);
			parsedEvent.Participant.UserId.ShouldBe("rPwsQrpC6gPuw2zEJqw");
		}

		/// <summary>
		/// Verify <see cref="MeetingParticipantPhoneCalloutEvent"/> properties.
		/// </summary>
		private static void VerifyMeetingParticipantPhoneCalloutEvent(MeetingParticipantPhoneCalloutEvent parsedEvent)
		{
			parsedEvent.Timestamp.ShouldBe(eventTimestamp);
			parsedEvent.AccountId.ShouldBe("dzVA4QmMQfyISoRcpFO8CA");

			parsedEvent.Meeting.ShouldNotBeNull();
			parsedEvent.Meeting.Id.ShouldBe(713592696);
			parsedEvent.Meeting.Uuid.ShouldBe(MeetingUuid);
			parsedEvent.Meeting.HostId.ShouldBe("ICuPoX4ERtikRcKqkVxunQ");
			parsedEvent.Meeting.Topic.ShouldBeNull();

			parsedEvent.Participant.ShouldNotBeNull();
			parsedEvent.Participant.InviteeName.ShouldBe("Jill Chill");
			parsedEvent.Participant.PhoneNumber.ShouldBe(1800000000);
		}

		/// <summary>
		/// Verify <see cref="MeetingParticipantRoomSystemCalloutEvent"/> properties.
		/// </summary>
		private static void VerifyMeetingParticipantRoomSystemCalloutEvent(MeetingParticipantRoomSystemCalloutEvent parsedEvent)
		{
			parsedEvent.Timestamp.ShouldBe(eventTimestamp);
			parsedEvent.AccountId.ShouldBe("dzVA4QmMQfyISoRcpFO8CA");

			parsedEvent.JoinInvitation.ShouldNotBeNull();
			parsedEvent.JoinInvitation.Id.ShouldBe(713592696);
			parsedEvent.JoinInvitation.Uuid.ShouldBe(MeetingUuid);
			parsedEvent.JoinInvitation.HostId.ShouldBe("ICuPoX4ERtikRcKqkVxunQ");
			parsedEvent.JoinInvitation.MessageId.ShouldBe("atsXxhSEQWit9t+U02HXNQ==");
			parsedEvent.JoinInvitation.InviterName.ShouldBe("Jill Chill");
			parsedEvent.JoinInvitation.Topic.ShouldBeNull();

			parsedEvent.Participant.ShouldNotBeNull();
			parsedEvent.Participant.CallType.ShouldBe("h323");
			parsedEvent.Participant.DeviceIp.ShouldBe("10.100.111.237");
		}

		/// <summary>
		/// Verify <see cref="MeetingAiCompanionEvent"/> properties.
		/// </summary>
		private static void VerifyMeetingAiCompanionEvent(MeetingAiCompanionEvent parsedEvent)
		{
			parsedEvent.Timestamp.ShouldBe(1726142087123.FromUnixTime(Internal.UnixTimePrecision.Milliseconds));
			parsedEvent.AccountId.ShouldBe("dzVA4QmMQfyISoRcpFO8CA");

			parsedEvent.Meeting.ShouldNotBeNull();
			parsedEvent.Meeting.Id.ShouldBe(97763643886);
			parsedEvent.Meeting.Uuid.ShouldBe("aDYlohsHRtCd4ii1uC2+hA==");
			parsedEvent.Meeting.HostId.ShouldBe("30R7kT7bTIKSNUFEuH_Qlg");
		}

		/// <summary>
		/// Verify <see cref="ChatMessageFile"/> properties.
		/// </summary>
		private static void VerifyChatMessageFile(ChatMessageFile chatMessageFile, string ownerId, string downloadUrl)
		{
			chatMessageFile.ShouldNotBeNull();
			chatMessageFile.Id.ShouldBe("MS17RDk0QTY3QUQtQkFGQy04QTJFLTI2RUEtNkYxQjRBRTU1MTk5fQ==");
			chatMessageFile.Name.ShouldBe("example.jpg");
			chatMessageFile.OwnerId.ShouldBe(ownerId);
			chatMessageFile.Size.ShouldBe(3966);
			chatMessageFile.Type.ShouldBe("jpg");
			chatMessageFile.DownloadUrl.ShouldBe(downloadUrl);
		}

		/// <summary>
		/// Verify <see cref="WebhookChatMessage"/> properties.
		/// </summary>
		private static void VerifyWebhookChatMessage(WebhookChatMessage chatMessage, string content, string[] fileIds)
		{
			chatMessage.ShouldNotBeNull();
			chatMessage.Id.ShouldBe("MS17MDQ5NjE4QjYtRjk4Ny00REEwLUFBQUItMTg3QTY0RjU2MzhFfQ==");
			chatMessage.Timestamp.ShouldBe(new DateTime(2022, 1, 10, 7, 20, 10, DateTimeKind.Utc));
			chatMessage.Content.ShouldBe(content);

			if (fileIds == null)
			{
				chatMessage.FileIds.ShouldBeNull();
			}
			else
			{
				chatMessage.FileIds.Length.ShouldBe(fileIds.Length);
				chatMessage.FileIds.ShouldBeSubsetOf(fileIds);
			}
		}

		/// <summary>
		/// Verify chat message sender properties.
		/// </summary>
		private static void VerifyChatMessageSender(ChatMessageParty chatMessageParty)
		{
			chatMessageParty.ShouldNotBeNull();
			chatMessageParty.Name.ShouldBe("Jill Chill");
			chatMessageParty.Email.ShouldBe("dlp.user@example.com");
			chatMessageParty.SessionId.ShouldBe("26219520");
			chatMessageParty.PartyType.ShouldBe(ChatMessagePartyType.Host);
		}

		/// <summary>
		/// Verify chat message recipient properties.
		/// </summary>
		private static void VerifyChatMessageRecipient(ChatMessageParty chatMessageParty)
		{
			chatMessageParty.ShouldNotBeNull();
			chatMessageParty.Name.ShouldBe("John Smith");
			chatMessageParty.Email.ShouldBe("guest.user@example");
			chatMessageParty.SessionId.ShouldBe("38681600");
			chatMessageParty.PartyType.ShouldBe(ChatMessagePartyType.Guest);
		}

		/// <summary>
		/// Verify that meeting is a recurring meeting.
		/// </summary>
		private static void VerifyRecurringMeeting(
			Meeting meeting,
			string joinUrl = null,
			string password = null,
			string pmi = null,
			int? occurrenceDuration = null,
			string occurrenceStatus = null)
		{
			meeting.ShouldNotBeNull();
			meeting.ShouldBeOfType<RecurringMeeting>();

			var recurringMeeting = (RecurringMeeting)meeting;

			recurringMeeting.Id.ShouldBe(MeetingId);
			recurringMeeting.Uuid.ShouldBe(MeetingUuid);
			recurringMeeting.HostId.ShouldBe(HostId);
			recurringMeeting.Topic.ShouldBe("My Meeting");
			recurringMeeting.Type.ShouldBe(MeetingType.RecurringFixedTime);
			recurringMeeting.Timezone.ShouldBe(TimeZones.America_Los_Angeles);
			recurringMeeting.JoinUrl.ShouldBe(joinUrl);
			recurringMeeting.Password.ShouldBe(password);
			recurringMeeting.PersonalMeetingId.ShouldBe(pmi);

			VerifyMeetingOccurrences(recurringMeeting.Occurrences, occurrenceDuration, occurrenceStatus);
		}

		/// <summary>
		/// Verify meeting occurrences content.
		/// </summary>
		private static void VerifyMeetingOccurrences(MeetingOccurrence[] info, int? duration, string status)
		{
			info.ShouldNotBeNull();
			info.Length.ShouldBe(1);
			info[0].OccurrenceId.ShouldBe("ABCDE12345");
			info[0].StartTime.ShouldBe(timestamp);
			info[0].Duration.ShouldBe(duration);
			info[0].Status.ShouldBe(status);
		}

		/// <summary>
		/// Verify <see cref="MeetingLiveStreamEvent"/> properties.
		/// </summary>
		private static void VerifyMeetingLiveStreamEvent(MeetingLiveStreamEvent parsedEvent)
		{
			parsedEvent.Timestamp.ShouldBe(1627906965803.FromUnixTime(Internal.UnixTimePrecision.Milliseconds));
			parsedEvent.AccountId.ShouldBe("D8cJuqWVQ623CI4Q8yQK0Q");
			parsedEvent.Operator.ShouldBe(OperatorEmail);
			parsedEvent.OperatorId.ShouldBe(OperatorId);

			VerifyMeetingInfo(parsedEvent.Meeting);

			VerifyLiveStreamingInfo(parsedEvent.StreamingInfo);
		}

		/// <summary>
		/// Verify <see cref="LiveStreamingInfo"/> properties.
		/// </summary>
		private static void VerifyLiveStreamingInfo(LiveStreamingInfo streamingInfo)
		{
			streamingInfo.ShouldNotBeNull();
			streamingInfo.ServiceName.ShouldBe("Custom_Live_Streaming_Service");
			streamingInfo.Settings.ShouldNotBeNull();
			streamingInfo.Settings.Url.ShouldBe("https://example.com/livestream");
			streamingInfo.Settings.Key.ShouldBe("ABCDEFG12345HIJ6789");
			streamingInfo.Settings.PageUrl.ShouldBe("https://example.com/livestream/123");
			streamingInfo.Settings.Resolution.ShouldBe("1080p");
		}

		/// <summary>
		/// Verify <see cref="MeetingRegistrationEvent"/> properties.
		/// </summary>
		private static void VerifyMeetingRegistrationEvent(MeetingRegistrationEvent parsedEvent)
		{
			parsedEvent.Timestamp.ShouldBe(eventTimestamp);
			parsedEvent.AccountId.ShouldBe(AccountId);

			VerifyRecurringMeeting(parsedEvent.Meeting);
		}

		/// <summary>
		/// Verify <see cref="Registrant"/> properties.
		/// </summary>
		private static void VerifyRegistrant(Registrant registrant)
		{
			registrant.ShouldNotBeNull();
			registrant.Id.ShouldBe("iFxeBPYun6SAiWUzBcEkX");
			registrant.FirstName.ShouldBe("Jill");
			registrant.LastName.ShouldBe("Chill");
			registrant.Email.ShouldBe(UserEmail);
		}

		/// <summary>
		/// Verify all properties in <see cref="Registrant"/>.
		/// </summary>
		private static void VerifyRegistrantExtended(
			Registrant registrant,
			string joinUrl = "https://example.com/join",
			long? pinCode = null)
		{
			VerifyRegistrant(registrant);

			registrant.Address.ShouldBe("1800 Amphibious Blvd.");
			registrant.City.ShouldBe("Mountain View");
			registrant.Country.ShouldBe("US");
			registrant.Zip.ShouldBe("94045");
			registrant.State.ShouldBe("CA");
			registrant.Phone.ShouldBe("5550100");
			registrant.Industry.ShouldBe("Food");
			registrant.Organization.ShouldBe("Cooking Org");
			registrant.JobTitle.ShouldBe("Chef");
			registrant.PurchasingTimeFrame.ShouldBe(PurchasingTimeFrame.Between_1_and_3_months);
			registrant.RoleInPurchaseProcess.ShouldBe(RoleInPurchaseProcess.Influencer);
			registrant.NumberOfEmployees.ShouldBe(NumberOfEmployees.Between_0001_and_0020);
			registrant.Status.ShouldBe(RegistrantStatus.Approved);
			registrant.JoinUrl.ShouldBe(joinUrl);
			registrant.ParticipantPinCode.ShouldBe(pinCode);
		}

		/// <summary>
		/// Verify <see cref="MeetingSharingEvent"/> properties.
		/// </summary>
		private static void VerifyMeetingSharingEvent(MeetingSharingEvent parsedEvent)
		{
			parsedEvent.Timestamp.ShouldBe(1234566789900.FromUnixTime(Internal.UnixTimePrecision.Milliseconds));
			parsedEvent.AccountId.ShouldBe("EPeQtiABC000VYxHMA");

			parsedEvent.Participant.ShouldNotBeNull();
			parsedEvent.Participant.DisplayName.ShouldBe("Arya Arya");
			parsedEvent.Participant.Email.ShouldBeNullOrEmpty();
			parsedEvent.Participant.ParticipantId.ShouldBe("16778000");
			parsedEvent.Participant.UserId.ShouldBe("s0AAAASoSE1V8KIFOCYw");

			VerifyMeetingInfo(parsedEvent.Meeting);

			VerifyScreenshareDetails(parsedEvent.ScreenshareDetails);
		}

		/// <summary>
		/// Verify <see cref="ScreenshareDetails"/> properties.
		/// </summary>
		private static void VerifyScreenshareDetails(ScreenshareDetails screenshareDetails)
		{
			screenshareDetails.ShouldNotBeNull();
			screenshareDetails.ContentType.ShouldBe(ScreenshareContentType.Application);
			screenshareDetails.Date.ShouldBe(new DateTime(2021, 7, 13, 21, 55, 52, 0, DateTimeKind.Utc));
			screenshareDetails.SharingMethod.ShouldBe("in_meeting");
			screenshareDetails.Source.ShouldBe("dropbox");
			screenshareDetails.Link.ShouldBe("https://shared");
		}

		#endregion
	}
}
