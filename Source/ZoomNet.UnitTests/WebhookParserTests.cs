using Shouldly;
using System;
using System.Linq;
using Xunit;
using ZoomNet.Models;
using ZoomNet.Models.Webhooks;
using ZoomNet.UnitTests.Properties;

namespace ZoomNet.UnitTests
{
	/// <summary>
	/// Unit tests that verify webhook events parsing.
	/// </summary>
	public class WebhookParserTests
	{
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

			parsedEvent.Meeting.ShouldNotBeNull();
			parsedEvent.Meeting.Uuid.ShouldBe("5yDZGNlQSV6qOjg4NxajHQ==");
			parsedEvent.Meeting.Id.ShouldBe(98884753832);
			parsedEvent.Meeting.HostId.ShouldBe("8lzIwvZTSOqjndWPbPqzuA");
			parsedEvent.Meeting.Topic.ShouldBe("ZoomNet Unit Testing: instant meeting");
			parsedEvent.Meeting.Type.ShouldBe(MeetingType.Instant);
			parsedEvent.Meeting.Timezone.ShouldBe(TimeZones.America_New_York);
			parsedEvent.Meeting.JoinUrl.ShouldBe("https://zoom.us/j/98884753832?pwd=c21EQzg0SXY2dlNTOFF2TENpSm1aQT09");
			parsedEvent.Meeting.Password.ShouldBe("PaSsWoRd");
			parsedEvent.Meeting.Settings.ShouldNotBeNull();
			parsedEvent.Meeting.Settings.UsePmi.HasValue.ShouldBeTrue();
			parsedEvent.Meeting.Settings.UsePmi.Value.ShouldBeFalse();
			parsedEvent.Meeting.Settings.AlternativeHosts.ShouldBeEmpty();
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
			parsedEvent.Timestamp.ShouldBe(1626230691572.FromUnixTime(Internal.UnixTimePrecision.Milliseconds));
			parsedEvent.AccountId.ShouldBe("AAAAAABBBB");
			parsedEvent.Operator.ShouldBe("admin@example.com");
			parsedEvent.OperatorId.ShouldBe("z8yCxjabcdEFGHfp8uQ");
			parsedEvent.Operation.ShouldBe("single");

			VerifyRecurringMeeting(parsedEvent.Meeting);
		}

		[Fact]
		public void MeetingRecovered()
		{
			var parsedEvent = ParseWebhookEvent<MeetingRecoveredEvent>(Resource.meeting_recovered_webhook);

			parsedEvent.EventType.ShouldBe(ZoomNet.Models.Webhooks.EventType.MeetingRecovered);
			parsedEvent.Timestamp.ShouldBe(1626230691572.FromUnixTime(Internal.UnixTimePrecision.Milliseconds));
			parsedEvent.AccountId.ShouldBe("AAAAAABBBB");
			parsedEvent.Operator.ShouldBe("admin@example.com");
			parsedEvent.OperatorId.ShouldBe("z8yCxjabcdEFGHfp8uQ");
			parsedEvent.Operation.ShouldBe("single");

			VerifyRecurringMeeting(parsedEvent.Meeting);
		}

		[Fact]
		public void MeetingEnded()
		{
			var parsedEvent = ParseWebhookEvent<MeetingEndedEvent>(Resource.meeting_ended_webhook);

			parsedEvent.EventType.ShouldBe(ZoomNet.Models.Webhooks.EventType.MeetingEnded);
			parsedEvent.Timestamp.ShouldBe(1626230691572.FromUnixTime(Internal.UnixTimePrecision.Milliseconds));
			parsedEvent.AccountId.ShouldBe("AAAAAABBBB");

			parsedEvent.Meeting.ShouldNotBeNull();
			parsedEvent.Meeting.Uuid.ShouldBe("4444AAAiAAAAAiAiAiiAii==");
			parsedEvent.Meeting.Id.ShouldBe(1234567890);
			parsedEvent.Meeting.HostId.ShouldBe("x1yCzABCDEfg23HiJKl4mN");
			parsedEvent.Meeting.Topic.ShouldBe("My Meeting");
			parsedEvent.Meeting.Type.ShouldBe(MeetingType.RecurringNoFixedTime);
			parsedEvent.Meeting.Timezone.ShouldBe(TimeZones.America_Los_Angeles);
			parsedEvent.Meeting.JoinUrl.ShouldBeNull();
			parsedEvent.Meeting.Password.ShouldBeNull();
			parsedEvent.Meeting.Settings.ShouldBeNull();
		}

		[Fact]
		public void MeetingUpdated()
		{
			var parsedEvent = ParseWebhookEvent<MeetingUpdatedEvent>(Resource.meeting_updated_webhook);

			parsedEvent.EventType.ShouldBe(ZoomNet.Models.Webhooks.EventType.MeetingUpdated);
			parsedEvent.Timestamp.ShouldBe(1617628464664.FromUnixTime(Internal.UnixTimePrecision.Milliseconds));
			parsedEvent.Operator.ShouldBe("someone@example.com");
			parsedEvent.OperatorId.ShouldBe("8lzIwvZTSOqjndWPbPqzuA");
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

			parsedEvent.Meeting.ShouldNotBeNull();
			parsedEvent.Meeting.ShouldBeOfType<InstantMeeting>();
			var parsedMeeting = (InstantMeeting)parsedEvent.Meeting;
			parsedMeeting.Id.ShouldBe(3479130610);
			parsedMeeting.Topic.ShouldBe("My Personal Meeting Room");
			parsedMeeting.Uuid.ShouldBe("mOG8pEaFQqeDm6Vd/3xopA==");
			parsedMeeting.HostId.ShouldBe("8lzIwvZTSOqjndWPbPqzuA");
			parsedMeeting.Timezone.ShouldBe(TimeZones.America_New_York);
			parsedMeeting.Type.ShouldBe(MeetingType.Personal);
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
			parsedEvent.Timestamp.ShouldBe(1626230691572.FromUnixTime(Internal.UnixTimePrecision.Milliseconds));
			parsedEvent.AccountId.ShouldBe("AAAAAABBBB");

			parsedEvent.Issues.ShouldNotBeNull();
			parsedEvent.Issues.ShouldBe(new[] { "Unstable audio quality" });

			VerifyScheduledMeeting(parsedEvent.Meeting);
		}

		[Fact]
		public void RecordingCompleted()
		{
			var parsedEvent = ParseWebhookEvent<RecordingCompletedEvent>(Resource.recording_completed_webhook);

			parsedEvent.EventType.ShouldBe(ZoomNet.Models.Webhooks.EventType.RecordingCompleted);
			parsedEvent.Timestamp.ShouldBe(1626230691572.FromUnixTime(Internal.UnixTimePrecision.Milliseconds));
			parsedEvent.DownloadToken.ShouldBe("abJhbGciOiJIUzUxMiJ9.eyJpc3MiOiJodHRwczovL2V2ZW50Lnpvb20udXMiLCJhY2NvdW50SWQiOiJNdDZzdjR1MFRBeVBrd2dzTDJseGlBIiwiYXVkIjoiaHR0cHM6Ly9vYXV0aC56b29tLnVzIiwibWlkIjoieFp3SEc0c3BRU2VuekdZWG16dnpiUT09IiwiZXhwIjoxNjI2MTM5NTA3LCJ1c2VySWQiOiJEWUhyZHBqclMzdWFPZjdkUGtrZzh3In0.a6KetiC6BlkDhf1dP4KBGUE1bb2brMeraoD45yhFx0eSSSTFdkHQnsKmlJQ-hdo9Zy-4vQw3rOxlyoHv583JyZ");
			parsedEvent.AccountId.ShouldBe("AAAAAABBBB");

			parsedEvent.Recording.ShouldNotBeNull();
			parsedEvent.Recording.Id.ShouldBe(1234567890);
			parsedEvent.Recording.Uuid.ShouldBe("4444AAAiAAAAAiAiAiiAii==");
			parsedEvent.Recording.HostId.ShouldBe("x1yCzABCDEfg23HiJKl4mN");
			parsedEvent.Recording.AccountId.ShouldBe("x1yCzABCDEfg23HiJKl4mN");
			parsedEvent.Recording.Topic.ShouldBe("My Personal Recording");
			parsedEvent.Recording.StartTime.ShouldBe(new DateTime(2021, 7, 13, 21, 44, 51, DateTimeKind.Utc));
			parsedEvent.Recording.Password.ShouldBe("132456");
			parsedEvent.Recording.Duration.ShouldBe(60);
			parsedEvent.Recording.ShareUrl.ShouldBe("https://example.com");
			parsedEvent.Recording.Type.ShouldBe(RecordingType.PersonnalMeeting);
			parsedEvent.Recording.TotalSize.ShouldBe(3328371);
			parsedEvent.Recording.FilesCount.ShouldBe(2);
			parsedEvent.Recording.PlayPasscode.ShouldBe("yNYIS408EJygs7rE5vVsJwXIz4-VW7MH");
			parsedEvent.Recording.RecordingFiles.ShouldNotBeNull();

			var audioFile = parsedEvent.Recording.RecordingFiles
				.FirstOrDefault(f => f.ContentType == RecordingContentType.AudioOnly);

			audioFile.ShouldNotBeNull();
			audioFile.Id.ShouldBe("ed6c2f27-2ae7-42f4-b3d0-835b493e4fa8");
			audioFile.MeetingId.ShouldBe("098765ABCD");
			audioFile.StartTime.ShouldBe(new DateTime(2021, 3, 23, 22, 14, 57, 0, DateTimeKind.Utc));
			audioFile.EndTime.ShouldBe(new DateTime(2021, 3, 23, 23, 15, 41, 0, DateTimeKind.Utc));
			audioFile.ContentType.ShouldBe(RecordingContentType.AudioOnly);
			audioFile.FileType.ShouldBe(RecordingFileType.Audio);
			audioFile.Size.ShouldBe(246560);
			audioFile.FileExtension.ShouldBe(RecordingFileExtension.M4A);
			audioFile.PlayUrl.ShouldBe("https://example.com/recording/play/Qg75t7xZBtEbAkjdlgbfdngBBBB");
			audioFile.DownloadUrl.ShouldBe("https://example.com/recording/download/Qg75t7xZBtEbAkjdlgbfdngBBBB");
			audioFile.Status.ShouldBe(RecordingStatus.Completed);

			var videoFile = parsedEvent.Recording.RecordingFiles
				.FirstOrDefault(f => f.ContentType == RecordingContentType.SharedScreenWithSpeakerView);

			videoFile.ShouldNotBeNull();
			videoFile.Id.ShouldBe("388ffb46-1541-460d-8447-4624451a1db7");
			videoFile.MeetingId.ShouldBe("098765ABCD");
			videoFile.StartTime.ShouldBe(new DateTime(2021, 3, 23, 22, 14, 57, 0, DateTimeKind.Utc));
			videoFile.EndTime.ShouldBe(new DateTime(2021, 3, 23, 23, 15, 41, 0, DateTimeKind.Utc));
			videoFile.ContentType.ShouldBe(RecordingContentType.SharedScreenWithSpeakerView);
			videoFile.FileType.ShouldBe(RecordingFileType.Video);
			videoFile.Size.ShouldBe(282825);
			videoFile.FileExtension.ShouldBe(RecordingFileExtension.MP4);
			videoFile.PlayUrl.ShouldBe("https://example.com/recording/play/Qg75t7xZBtEbAkjdlgbfdngCCCC");
			videoFile.DownloadUrl.ShouldBe("https://example.com/recording/download/Qg75t7xZBtEbAkjdlgbfdngCCCC");
			videoFile.Status.ShouldBe(RecordingStatus.Completed);

			parsedEvent.Recording.ParticipantAudioFiles.ShouldNotBeNull();
			parsedEvent.Recording.ParticipantAudioFiles.ShouldHaveSingleItem();
			var participantAudioFile = parsedEvent.Recording.ParticipantAudioFiles.First();
			participantAudioFile.ShouldNotBeNull();
			participantAudioFile.Id.ShouldBe("ed6c2f27-2ae7-42f4-b3d0-835b493e4fa8");
			participantAudioFile.StartTime.ShouldBe(new DateTime(2021, 3, 23, 22, 14, 57, 0, DateTimeKind.Utc));
			participantAudioFile.EndTime.ShouldBe(new DateTime(2021, 3, 23, 23, 15, 41, 0, DateTimeKind.Utc));
			participantAudioFile.FileType.ShouldBe(RecordingFileType.Audio);
			participantAudioFile.Size.ShouldBe(246560);
			participantAudioFile.FileExtension.ShouldBe(RecordingFileExtension.MP4);
			participantAudioFile.PlayUrl.ShouldBe("https://example.com/recording/play/Qg75t7xZBtEbAkjdlgbfdngAAAA");
			participantAudioFile.DownloadUrl.ShouldBe("https://example.com/recording/download/Qg75t7xZBtEbAkjdlgbfdngAAAA");
			participantAudioFile.Status.ShouldBe(RecordingStatus.Completed);
		}

		[Fact]
		public void WebinarEnded()
		{
			var parsedEvent = ParseWebhookEvent<WebinarEndedEvent>(Resource.webinar_ended_webhook);

			parsedEvent.EventType.ShouldBe(ZoomNet.Models.Webhooks.EventType.WebinarEnded);
			parsedEvent.Timestamp.ShouldBe(1626230691572.FromUnixTime(Internal.UnixTimePrecision.Milliseconds));
			parsedEvent.AccountId.ShouldBe("AAAAAABBBB");

			parsedEvent.Webinar.ShouldNotBeNull();
			parsedEvent.Webinar.ShouldBeOfType<ScheduledWebinar>();
			var parsedWebinar = (ScheduledWebinar)parsedEvent.Webinar;
			parsedWebinar.Uuid.ShouldBe("4444AAAiAAAAAiAiAiiAii==");
			parsedWebinar.Id.ShouldBe(1234567890);
			parsedWebinar.HostId.ShouldBe("x1yCzABCDEfg23HiJKl4mN");
			parsedWebinar.Topic.ShouldBe("My Webinar");
			parsedWebinar.Type.ShouldBe(WebinarType.Regular);
			parsedWebinar.StartTime.ShouldBe(new DateTime(2021, 7, 13, 21, 44, 51, DateTimeKind.Utc));
			parsedWebinar.Timezone.ShouldBe(TimeZones.America_Los_Angeles);
			parsedWebinar.Duration.ShouldBe(60);
			parsedWebinar.JoinUrl.ShouldBeNull();
			parsedWebinar.Password.ShouldBeNull();
			parsedWebinar.Settings.ShouldBeNull();
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

			parsedEvent.Operator.ShouldBe("admin@example.com");
			parsedEvent.OperatorId.ShouldBe("z8yCxjabcdEFGHfp8uQ");
		}

		[Fact]
		public void MeetingSummaryRecovered()
		{
			var parsedEvent = ParseWebhookEvent<MeetingSummaryRecoveredEvent>(Resource.meeting_summary_recovered_webhook);

			parsedEvent.EventType.ShouldBe(ZoomNet.Models.Webhooks.EventType.MeetingSummaryRecovered);

			VerifyMeetingSummaryEvent(parsedEvent);

			parsedEvent.Operator.ShouldBe("admin@example.com");
			parsedEvent.OperatorId.ShouldBe("z8yCxjabcdEFGHfp8uQ");
		}

		[Fact]
		public void MeetingSummaryShared()
		{
			var parsedEvent = ParseWebhookEvent<MeetingSummarySharedEvent>(Resource.meeting_summary_shared_webhook);

			parsedEvent.EventType.ShouldBe(ZoomNet.Models.Webhooks.EventType.MeetingSummaryShared);

			VerifyMeetingSummaryEvent(parsedEvent);

			parsedEvent.Operator.ShouldBe("admin@example.com");
			parsedEvent.OperatorId.ShouldBe("z8yCxjabcdEFGHfp8uQ");

			parsedEvent.ShareWithUsers.ShouldNotBeNull();
			parsedEvent.ShareWithUsers.Length.ShouldBe(1);
			parsedEvent.ShareWithUsers[0].Email.ShouldBe("jchill@example.com");
		}

		[Fact]
		public void MeetingSummaryTrashed()
		{
			var parsedEvent = ParseWebhookEvent<MeetingSummaryTrashedEvent>(Resource.meeting_summary_trashed_webhook);

			parsedEvent.EventType.ShouldBe(ZoomNet.Models.Webhooks.EventType.MeetingSummaryTrashed);

			VerifyMeetingSummaryEvent(parsedEvent);

			parsedEvent.Operator.ShouldBe("admin@example.com");
			parsedEvent.OperatorId.ShouldBe("z8yCxjabcdEFGHfp8uQ");
		}

		[Fact]
		public void MeetingSummaryUpdated()
		{
			var parsedEvent = ParseWebhookEvent<MeetingSummaryUpdatedEvent>(Resource.meeting_summary_updated_webhook);

			parsedEvent.EventType.ShouldBe(ZoomNet.Models.Webhooks.EventType.MeetingSummaryUpdated);

			VerifyMeetingSummaryEvent(parsedEvent);

			parsedEvent.Operator.ShouldBe("admin@example.com");
			parsedEvent.OperatorId.ShouldBe("z8yCxjabcdEFGHfp8uQ");

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

			parsedEvent.Timestamp.ShouldBe(1626230691572.FromUnixTime(Internal.UnixTimePrecision.Milliseconds));
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

			parsedEvent.Timestamp.ShouldBe(1626230691572.FromUnixTime(Internal.UnixTimePrecision.Milliseconds));
			parsedEvent.AccountId.ShouldBe("AAAAAABBBB");
			parsedEvent.Operator.ShouldBe("user@example.com");
			parsedEvent.OperatorId.ShouldBe("z8yCxjabcdEFGHfp8uQ");
			parsedEvent.MeetingId.ShouldBe(1234567890);
			parsedEvent.MeetingUuid.ShouldBe("4444AAAiAAAAAiAiAiiAii==");
			parsedEvent.HostAccountId.ShouldBe("lmnop12345abcdefghijk");

			VerifyChatMessageFile(parsedEvent.File, "rw3dIsBRTpOyMOJmeKgdaQ", null);
		}

		[Fact]
		public void MeetingChatMessageFileSent()
		{
			var parsedEvent = ParseWebhookEvent<MeetingChatMessageFileSentEvent>(Resource.meeting_chat_message_file_sent_webhook);

			parsedEvent.EventType.ShouldBe(ZoomNet.Models.Webhooks.EventType.MeetingChatMessageFileSent);

			parsedEvent.Timestamp.ShouldBe(1626230691572.FromUnixTime(Internal.UnixTimePrecision.Milliseconds));
			parsedEvent.AccountId.ShouldBe("AAAAAABBBB");
			parsedEvent.MeetingId.ShouldBe(1234567890);
			parsedEvent.MeetingUuid.ShouldBe("4444AAAiAAAAAiAiAiiAii==");

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

			parsedEvent.Timestamp.ShouldBe(1626230691572.FromUnixTime(Internal.UnixTimePrecision.Milliseconds));
			parsedEvent.AccountId.ShouldBe("AAAAAABBBB");
			parsedEvent.MeetingId.ShouldBe(1234567890);
			parsedEvent.MeetingUuid.ShouldBe("4444AAAiAAAAAiAiAiiAii==");

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
		public void MeetingConvertedToWebinar()
		{
			var parsedEvent = ParseWebhookEvent<MeetingConvertedToWebinarEvent>(Resource.meeting_converted_to_webinar_webhook);

			parsedEvent.EventType.ShouldBe(ZoomNet.Models.Webhooks.EventType.MeetingConvertedToWebinar);

			parsedEvent.Timestamp.ShouldBe(1626230691572.FromUnixTime(Internal.UnixTimePrecision.Milliseconds));
			parsedEvent.AccountId.ShouldBe("AAAAAABBBB");
			parsedEvent.Operator.ShouldBe("admin@example.com");
			parsedEvent.OperatorId.ShouldBe("z8yCxjabcdEFGHfp8uQ");

			parsedEvent.Webinar.ShouldNotBeNull();
			parsedEvent.Webinar.Id.ShouldBe(1234567890);
			parsedEvent.Webinar.Uuid.ShouldBe("4444AAAiAAAAAiAiAiiAii==");
			parsedEvent.Webinar.HostId.ShouldBe("x1yCzABCDEfg23HiJKl4mN");
			parsedEvent.Webinar.Topic.ShouldBe("My Meeting Converted");
			parsedEvent.Webinar.Type.ShouldBe(WebinarType.Regular);
			parsedEvent.Webinar.StartTime.ShouldBe(new DateTime(2021, 7, 13, 21, 44, 51, DateTimeKind.Utc));
			parsedEvent.Webinar.Duration.ShouldBe(60);
			parsedEvent.Webinar.Timezone.ShouldBe(TimeZones.America_Los_Angeles);
		}

		[Fact]
		public void MeetingDeviceTested()
		{
			var parsedEvent = ParseWebhookEvent<MeetingDeviceTestedEvent>(Resource.meeting_device_tested_webhook);

			parsedEvent.EventType.ShouldBe(ZoomNet.Models.Webhooks.EventType.MeetingDeviceTested);

			parsedEvent.Timestamp.ShouldBe(1626230691572.FromUnixTime(Internal.UnixTimePrecision.Milliseconds));
			parsedEvent.AccountId.ShouldBe("AAAAAABBBB");

			parsedEvent.Meeting.ShouldNotBeNull();
			parsedEvent.Meeting.Id.ShouldBe(1234567890);
			parsedEvent.Meeting.Uuid.ShouldBe("4444AAAiAAAAAiAiAiiAii==");
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
			parsedEvent.Meeting.Id.ShouldBe(1234567890);
			parsedEvent.Meeting.Uuid.ShouldBe("4444AAAiAAAAAiAiAiiAii==");
			parsedEvent.Meeting.HostId.ShouldBe("x1yCzABCDEfg23HiJKl4mN");
			parsedEvent.Meeting.Topic.ShouldBe("My Meeting");
			parsedEvent.Meeting.Type.ShouldBe(MeetingType.Scheduled);
			parsedEvent.Meeting.StartTime.ShouldBe(new DateTime(2021, 7, 13, 21, 44, 51, DateTimeKind.Utc));
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
			parsedEvent.Timestamp.ShouldBe(1627906965803.FromUnixTime(Internal.UnixTimePrecision.Milliseconds));
			parsedEvent.AccountId.ShouldBe("D8cJuqWVQ623CI4Q8yQK0Q");
			parsedEvent.Operator.ShouldBe("admin@example.com");
			parsedEvent.OperatorId.ShouldBe("z8yCxjabcdEFGHfp8uQ");

			VerifyScheduledMeeting(parsedEvent.Meeting);

			parsedEvent.StartedOn.ShouldBe(new DateTime(2021, 8, 2, 12, 22, 45, DateTimeKind.Utc));

			VerifyLiveStreamingInfo(parsedEvent.StreamingInfo);
		}

		[Fact]
		public void MeetingLiveStreamingStopped()
		{
			var parsedEvent = ParseWebhookEvent<MeetingLiveStreamStoppedEvent>(Resource.meeting_live_streaming_stopped_webhook);

			parsedEvent.EventType.ShouldBe(ZoomNet.Models.Webhooks.EventType.MeetingLiveStreamStopped);
			parsedEvent.Timestamp.ShouldBe(1627906965803.FromUnixTime(Internal.UnixTimePrecision.Milliseconds));
			parsedEvent.AccountId.ShouldBe("D8cJuqWVQ623CI4Q8yQK0Q");
			parsedEvent.Operator.ShouldBe("admin@example.com");
			parsedEvent.OperatorId.ShouldBe("z8yCxjabcdEFGHfp8uQ");

			VerifyScheduledMeeting(parsedEvent.Meeting);

			parsedEvent.StoppedOn.ShouldBe(new DateTime(2021, 8, 2, 12, 22, 45, DateTimeKind.Utc));

			VerifyLiveStreamingInfo(parsedEvent.StreamingInfo);
		}

		[Fact]
		public void MeetingRegistrationApproved()
		{
			var parsedEvent = ParseWebhookEvent<MeetingRegistrationApprovedEvent>(Resource.meeting_registration_approved_webhook);

			parsedEvent.EventType.ShouldBe(ZoomNet.Models.Webhooks.EventType.MeetingRegistrationApproved);

			VerifyMeetingRegistrationEvent(parsedEvent);

			parsedEvent.Operator.ShouldBe("admin@example.com");
			parsedEvent.OperatorId.ShouldBe("z8yCxjabcdEFGHfp8uQ");

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

			parsedEvent.Operator.ShouldBe("admin@example.com");
			parsedEvent.OperatorId.ShouldBe("z8yCxjabcdEFGHfp8uQ");

			VerifyRegistrant(parsedEvent.Registrant);
		}

		[Fact]
		public void MeetingRegistrationCreated()
		{
			var parsedEvent = ParseWebhookEvent<MeetingRegistrationCreatedEvent>(Resource.meeting_registration_created_webhook);

			parsedEvent.EventType.ShouldBe(ZoomNet.Models.Webhooks.EventType.MeetingRegistrationCreated);

			VerifyMeetingRegistrationEvent(parsedEvent);

			VerifyRegistrant(parsedEvent.Registrant);

			parsedEvent.Registrant.Address.ShouldBe("1800 Amphibious Blvd.");
			parsedEvent.Registrant.City.ShouldBe("Mountain View");
			parsedEvent.Registrant.Country.ShouldBe("US");
			parsedEvent.Registrant.Zip.ShouldBe("94045");
			parsedEvent.Registrant.State.ShouldBe("CA");
			parsedEvent.Registrant.Phone.ShouldBe("5550100");
			parsedEvent.Registrant.Industry.ShouldBe("Food");
			parsedEvent.Registrant.Organization.ShouldBe("Cooking Org");
			parsedEvent.Registrant.JobTitle.ShouldBe("Chef");
			parsedEvent.Registrant.PurchasingTimeFrame.ShouldBe(PurchasingTimeFrame.Between_1_and_3_months);
			parsedEvent.Registrant.RoleInPurchaseProcess.ShouldBe(RoleInPurchaseProcess.Influencer);
			parsedEvent.Registrant.NumberOfEmployees.ShouldBe(NumberOfEmployees.Between_0001_and_0020);
			parsedEvent.Registrant.Comments.ShouldBe("Looking forward to the meeting");
			parsedEvent.Registrant.CustomQuestions.ShouldNotBeNull();
			parsedEvent.Registrant.CustomQuestions.Length.ShouldBe(1);
			parsedEvent.Registrant.CustomQuestions[0].Key.ShouldBe("What do you hope to learn from this meeting?");
			parsedEvent.Registrant.CustomQuestions[0].Value.ShouldBe("Look forward to learning how you come up with new recipes and what other services you offer.");
			parsedEvent.Registrant.Status.ShouldBe(RegistrantStatus.Approved);
			parsedEvent.Registrant.JoinUrl.ShouldBe("https://example.com/join");
			parsedEvent.Registrant.ParticipantPinCode.ShouldBe(380303);
		}

		[Fact]
		public void MeetingRegistrationDenied()
		{
			var parsedEvent = ParseWebhookEvent<MeetingRegistrationDeniedEvent>(Resource.meeting_registration_denied_webhook);

			parsedEvent.EventType.ShouldBe(ZoomNet.Models.Webhooks.EventType.MeetingRegistrationDenied);

			VerifyMeetingRegistrationEvent(parsedEvent);

			parsedEvent.Operator.ShouldBe("admin@example.com");
			parsedEvent.OperatorId.ShouldBe("z8yCxjabcdEFGHfp8uQ");

			VerifyRegistrant(parsedEvent.Registrant);
		}

		#endregion

		#region private methods

		/// <summary>
		/// Wrapper for webhook event parsing and casting to specified type.
		/// </summary>
		private static T ParseWebhookEvent<T>(string webhookBody)
			where T : ZoomNet.Models.Webhooks.Event
		{
			return (T)new WebhookParser().ParseEventWebhook(webhookBody);
		}

		/// <summary>
		/// Verify <see cref="MeetingSummaryEvent"/> properties.
		/// </summary>
		private static void VerifyMeetingSummaryEvent(MeetingSummaryEvent parsedEvent)
		{
			parsedEvent.Timestamp.ShouldBe(1626230691572.FromUnixTime(Internal.UnixTimePrecision.Milliseconds));
			parsedEvent.AccountId.ShouldBe("dzVA4QmMQfyISoRcpFO8CA");

			parsedEvent.MeetingSummary.ShouldNotBeNull();
			parsedEvent.MeetingSummary.MeetingId.ShouldBe(97763643886);
			parsedEvent.MeetingSummary.MeetingUuid.ShouldBe("aDYlohsHRtCd4ii1uC2+hA==");
			parsedEvent.MeetingSummary.MeetingStartTime.ShouldBe(new DateTime(2019, 7, 15, 23, 24, 52, DateTimeKind.Utc));
			parsedEvent.MeetingSummary.MeetingEndTime.ShouldBe(new DateTime(2019, 7, 15, 23, 30, 19, DateTimeKind.Utc));
			parsedEvent.MeetingSummary.MeetingHostEmail.ShouldBe("jchill@example.com");
			parsedEvent.MeetingSummary.MeetingHostId.ShouldBe("30R7kT7bTIKSNUFEuH_Qlg");
			parsedEvent.MeetingSummary.MeetingTopic.ShouldBe("My Meeting");
			parsedEvent.MeetingSummary.SummaryCreatedTime.ShouldBe(new DateTime(2019, 7, 15, 23, 31, 47, DateTimeKind.Utc));
			parsedEvent.MeetingSummary.SummaryStartTime.ShouldBe(new DateTime(2019, 7, 15, 23, 24, 52, DateTimeKind.Utc));
			parsedEvent.MeetingSummary.SummaryEndTime.ShouldBe(new DateTime(2019, 7, 15, 23, 30, 19, DateTimeKind.Utc));
			parsedEvent.MeetingSummary.SummaryLastModifiedTime.ShouldBe(new DateTime(2019, 7, 15, 23, 32, 19, DateTimeKind.Utc));
			parsedEvent.MeetingSummary.SummaryTitle.ShouldBe("Meeting Summary for my meeting");
		}

		/// <summary>
		/// Verify <see cref="MeetingBreakoutRoomEvent"/> properties.
		/// </summary>
		private static void VerifyMeetingBreakoutRoomEvent(MeetingBreakoutRoomEvent parsedEvent)
		{
			parsedEvent.Timestamp.ShouldBe(1626230691572.FromUnixTime(Internal.UnixTimePrecision.Milliseconds));
			parsedEvent.AccountId.ShouldBe("AAAAAABBBB");

			VerifyBreakoutRoomMeetingInfo(parsedEvent.Meeting);
		}

		/// <summary>
		/// Verify <see cref="BreakoutRoomMeetingInfo"/> properties.
		/// </summary>
		private static void VerifyBreakoutRoomMeetingInfo(BreakoutRoomMeetingInfo info)
		{
			info.ShouldNotBeNull();
			info.Id.ShouldBe(1234567890);
			info.Uuid.ShouldBe("4444AAAiAAAAAiAiAiiAii==");
			info.BreakoutRoomUuid.ShouldBe("FkQbpP2UR028mDrwzEahqw==");
			info.HostId.ShouldBe("x1yCzABCDEfg23HiJKl4mN");
			info.Topic.ShouldBe("My Meeting");
			info.Type.ShouldBe(MeetingType.RecurringFixedTime);
			info.StartTime.ShouldBe(new DateTime(2021, 7, 13, 21, 44, 51, DateTimeKind.Utc));
			info.Timezone.ShouldBe(TimeZones.America_Los_Angeles);
			info.Duration.ShouldBe(60);
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
			info.Email.ShouldBe("jchill@example.com");
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
			parsedEvent.Timestamp.ShouldBe(1626230691572.FromUnixTime(Internal.UnixTimePrecision.Milliseconds));
			parsedEvent.AccountId.ShouldBe("dzVA4QmMQfyISoRcpFO8CA");

			parsedEvent.Meeting.ShouldNotBeNull();
			parsedEvent.Meeting.Id.ShouldBe(91578204824);
			parsedEvent.Meeting.Uuid.ShouldBe("4444AAAiAAAAAiAiAiiAii==");
			parsedEvent.Meeting.HostId.ShouldBe("ICuPoX4ERtikRcKqkVxunQ");
			parsedEvent.Meeting.Topic.ShouldBe("Jill Chill's Zoom Meeting");

			parsedEvent.Participant.ShouldNotBeNull();
			parsedEvent.Participant.Email.ShouldBe("jchill@example.com");
			parsedEvent.Participant.UserId.ShouldBe("rPwsQrpC6gPuw2zEJqw");
		}

		/// <summary>
		/// Verify <see cref="MeetingParticipantPhoneCalloutEvent"/> properties.
		/// </summary>
		private static void VerifyMeetingParticipantPhoneCalloutEvent(MeetingParticipantPhoneCalloutEvent parsedEvent)
		{
			parsedEvent.Timestamp.ShouldBe(1626230691572.FromUnixTime(Internal.UnixTimePrecision.Milliseconds));
			parsedEvent.AccountId.ShouldBe("dzVA4QmMQfyISoRcpFO8CA");

			parsedEvent.Meeting.ShouldNotBeNull();
			parsedEvent.Meeting.Id.ShouldBe(713592696);
			parsedEvent.Meeting.Uuid.ShouldBe("4444AAAiAAAAAiAiAiiAii==");
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
			parsedEvent.Timestamp.ShouldBe(1626230691572.FromUnixTime(Internal.UnixTimePrecision.Milliseconds));
			parsedEvent.AccountId.ShouldBe("dzVA4QmMQfyISoRcpFO8CA");

			parsedEvent.JoinInvitation.ShouldNotBeNull();
			parsedEvent.JoinInvitation.Id.ShouldBe(713592696);
			parsedEvent.JoinInvitation.Uuid.ShouldBe("4444AAAiAAAAAiAiAiiAii==");
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
		/// Verify that meeting is a scheduled meeting.
		/// </summary>
		private static void VerifyScheduledMeeting(Meeting meeting)
		{
			meeting.ShouldNotBeNull();
			meeting.ShouldBeOfType<ScheduledMeeting>();

			var scheduledMeeting = (ScheduledMeeting)meeting;

			scheduledMeeting.Id.ShouldBe(1234567890L);
			scheduledMeeting.Topic.ShouldBe("My Meeting");
			scheduledMeeting.Uuid.ShouldBe("4444AAAiAAAAAiAiAiiAii==");
			scheduledMeeting.HostId.ShouldBe("x1yCzABCDEfg23HiJKl4mN");
			scheduledMeeting.StartTime.ShouldBe(new DateTime(2021, 7, 13, 21, 44, 51, 0, DateTimeKind.Utc));
			scheduledMeeting.Duration.ShouldBe(60);
			scheduledMeeting.Timezone.ShouldBe(TimeZones.America_Los_Angeles);
			scheduledMeeting.Type.ShouldBe(MeetingType.Scheduled);
		}

		/// <summary>
		/// Verify that meeting is a recurring meeting.
		/// </summary>
		private static void VerifyRecurringMeeting(Meeting meeting)
		{
			meeting.ShouldNotBeNull();
			meeting.ShouldBeOfType<RecurringMeeting>();

			var recurringMeeting = (RecurringMeeting)meeting;

			recurringMeeting.Id.ShouldBe(1234567890);
			recurringMeeting.Uuid.ShouldBe("4444AAAiAAAAAiAiAiiAii==");
			recurringMeeting.HostId.ShouldBe("x1yCzABCDEfg23HiJKl4mN");
			recurringMeeting.Topic.ShouldBe("My Meeting");
			recurringMeeting.Type.ShouldBe(MeetingType.RecurringFixedTime);
			recurringMeeting.Timezone.ShouldBe(TimeZones.America_Los_Angeles);
			recurringMeeting.Occurrences.ShouldNotBeNull();
			recurringMeeting.Occurrences.Length.ShouldBe(1);
			recurringMeeting.Occurrences[0].OccurrenceId.ShouldBe("ABCDE12345");
			recurringMeeting.Occurrences[0].StartTime.ShouldBe(new DateTime(2021, 7, 13, 21, 44, 51, DateTimeKind.Utc));
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
			parsedEvent.Timestamp.ShouldBe(1626230691572.FromUnixTime(Internal.UnixTimePrecision.Milliseconds));
			parsedEvent.AccountId.ShouldBe("AAAAAABBBB");

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
			registrant.Email.ShouldBe("jchill@example.com");
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

			VerifyScheduledMeeting(parsedEvent.Meeting);

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

		public class VerifySignature
		{
			[Fact]
			public void Simple()
			{
				// Arrange
				var requestBody = "{\"payload\":{\"plainToken\":\"xys8n8PGS7mAU0m5-YJjRA\"},\"event_ts\":1720705455858,\"event\":\"endpoint.url_validation\"}";
				var secretToken = "4fv1RkqGQUq5sWbEz6hA5A";
				var signature = "v0=93a1a675965ceb9c5a50c5dfb31f20e50f763be37a54ef74cd2d16a1a8e5c0d6";
				var timestamp = "1720705455";

				var parser = new WebhookParser();

				// Act
				var result = parser.VerifySignature(requestBody, secretToken, signature, timestamp);

				//Assert
				result.ShouldBeTrue();
			}

			[Fact]
			public void Topic_contains_non_ASCII_characters()
			{
				// Arange
				var requestBody = "{\"event\":\"meeting.started\",\"payload\":{\"account_id\":\"VjZoEArIT5y-HlWxkV-tVA\",\"object\":{\"duration\":60,\"start_time\":\"2024-07-11T14:12:55Z\",\"timezone\":\"America/New_York\",\"topic\":\"Test \\uD83D\\uDE92\\uD83D\\uDE92 ? - ’ - – \\uD83D\\uDE97 HOLA\",\"id\":\"85393847045\",\"type\":2,\"uuid\":\"jUh5o3dKQIytvcsfTtKBlg==\",\"host_id\":\"8lzIwvZTSOqjndWPbPqzuA\"}},\"event_ts\":1720707175904}";
				var secretToken = "4fv1RkqGQUq5sWbEz6hA5A";
				var signature = "v0=1a14e79349318fa1bead50ebbd3c185ae078e182d3bbd30ab8010fcb7f4357c7";
				var timestamp = "1720707175";

				var parser = new WebhookParser();

				// Act
				var result = parser.VerifySignature(requestBody, secretToken, signature, timestamp);

				//Assert
				result.ShouldBeTrue();
			}
		}
	}
}
