using Shouldly;
using System;
using Xunit;
using ZoomNet.Models;
using ZoomNet.Models.Webhooks;
using ZoomNet.UnitTests.Properties;

namespace ZoomNet.UnitTests.WebhookParser
{
	/// <summary>
	/// Unit tests that verify recording webhook events parsing.
	/// </summary>
	public partial class WebhookParserTests
	{
		#region private fields

		private const string DownloadToken = "abJhbGciOiJIUzUxMiJ9";

		#endregion

		#region tests

		[Fact]
		public void RecordingArchiveFilesCompleted()
		{
			var parsedEvent = ParseWebhookEvent<RecordingArchiveFilesCompletedEvent>(WebhooksResource.recording_archive_files_completed);

			VerifyRecordingEvent(parsedEvent, ZoomNet.Models.Webhooks.EventType.RecordingArchiveFilesCompleted);

			parsedEvent.DownloadToken.ShouldBe(DownloadToken);

			VerifyRecordedMeetingOrWebinarInfo(parsedEvent.Recording);

			parsedEvent.Recording.AccountName.ShouldBe("account_01");
			parsedEvent.Recording.Status.ShouldBe(RecordingArchiveFileStatus.Completed);
			parsedEvent.Recording.GroupId.ShouldBe("pvFIYKSDTum9iCDOOtQL4w,_FsqLyI0RlO6LVPeUVWi8g");
			parsedEvent.Recording.MeetingKind.ShouldBe(MeetingKind.Internal);
			parsedEvent.Recording.CompleteTime.ShouldBe(new DateTime(2021, 3, 12, 2, 57, 27, DateTimeKind.Utc));
			parsedEvent.Recording.DurationInSeconds.ShouldBe(3602);
			parsedEvent.Recording.IsBreakoutRoom.ShouldNotBeNull();
			parsedEvent.Recording.IsBreakoutRoom.Value.ShouldBeFalse();
			parsedEvent.Recording.ParentMeetingUuid.ShouldBeNull();
			parsedEvent.Recording.TotalSize.ShouldBe(3328371);
			parsedEvent.Recording.FilesCount.ShouldBe(2);
			parsedEvent.Recording.Files.ShouldNotBeNull();
			parsedEvent.Recording.Files.Length.ShouldBe(2);

			var audioFile = parsedEvent.Recording.Files[0];

			audioFile.ShouldNotBeNull();
			audioFile.Id.ShouldBe("ed6c2f27-2ae7-42f4-b3d0-835b493e4fa8");
			audioFile.FileType.ShouldBe(RecordingFileType.Audio);
			audioFile.FileExtension.ShouldBe(RecordingFileExtension.M4A);
			audioFile.Size.ShouldBe(1547842);
			audioFile.DownloadUrl.ShouldBe("https://example.com/recording/download/Qg75t7xZBtEbAkjdlgbfdngBBBB");
			audioFile.Status.ShouldBe(RecordingArchiveFileStatus.Completed);
			audioFile.ContentType.ShouldBe(RecordingContentType.AudioOnly);
			audioFile.Individual.ShouldBeTrue();
			audioFile.ParticipantEmail.ShouldBe(UserEmail);
			audioFile.ParticipantJoinTime.ShouldBe(new DateTime(2021, 3, 12, 2, 7, 27, DateTimeKind.Utc));
			audioFile.ParticipantLeaveTime.ShouldBe(new DateTime(2021, 3, 12, 2, 12, 27, DateTimeKind.Utc));
			audioFile.EncryptionFingerprint.ShouldBe("2123150b921fb1babda81c6156d2711659d37b5d3cbe6957e22e51fbb87e7a87");
			audioFile.StorageLocation.ShouldBe("US");

			var videoFile = parsedEvent.Recording.Files[1];

			videoFile.ShouldNotBeNull();
			videoFile.Id.ShouldBe("388ffb46-1541-460d-8447-4624451a1db7");
			videoFile.FileType.ShouldBe(RecordingFileType.Video);
			videoFile.FileExtension.ShouldBe(RecordingFileExtension.MP4);
			videoFile.Size.ShouldBe(1780529);
			videoFile.DownloadUrl.ShouldBe("https://example.com/recording/download/Qg75t7xZBtEbAkjdlgbfdngCCCC");
			videoFile.Status.ShouldBe(RecordingArchiveFileStatus.Completed);
			videoFile.ContentType.ShouldBe(RecordingContentType.SharedScreenWithSpeakerView);
			videoFile.Individual.ShouldBeTrue();
			videoFile.ParticipantEmail.ShouldBe("thill@example.com");
			videoFile.ParticipantJoinTime.ShouldBe(new DateTime(2021, 3, 12, 2, 7, 27, DateTimeKind.Utc));
			videoFile.ParticipantLeaveTime.ShouldBe(new DateTime(2021, 3, 12, 2, 12, 27, DateTimeKind.Utc));
			videoFile.EncryptionFingerprint.ShouldBe("abf85f0fe6a4db3cdd8c37e505e1dd18a34d9696170a14b5bc6395677472cf43");
			videoFile.StorageLocation.ShouldBe("US");
		}

		[Fact]
		public void RecordingBatchDeleted()
		{
			var parsedEvent = ParseWebhookEvent<RecordingBatchDeletedEvent>(WebhooksResource.recording_batch_deleted);

			VerifyRecordingEvent(parsedEvent, ZoomNet.Models.Webhooks.EventType.RecordingBatchDeleted);

			parsedEvent.Operator.ShouldBe(OperatorEmail);
			parsedEvent.OperatorId.ShouldBe(OperatorId);

			VerifyRecordingsBatch(parsedEvent.Meetings);
		}

		[Fact]
		public void RecordingBatchRecovered()
		{
			var parsedEvent = ParseWebhookEvent<RecordingBatchRecoveredEvent>(WebhooksResource.recording_batch_recovered);

			VerifyRecordingEvent(parsedEvent, ZoomNet.Models.Webhooks.EventType.RecordingBatchRecovered);

			parsedEvent.Operator.ShouldBe(OperatorEmail);
			parsedEvent.OperatorId.ShouldBe(OperatorId);

			VerifyRecordingsBatch(parsedEvent.Meetings);
		}

		[Fact]
		public void RecordingBatchTrashed()
		{
			var parsedEvent = ParseWebhookEvent<RecordingBatchTrashedEvent>(WebhooksResource.recording_batch_trashed);

			VerifyRecordingEvent(parsedEvent, ZoomNet.Models.Webhooks.EventType.RecordingBatchTrashed);

			parsedEvent.Operator.ShouldBe(OperatorEmail);
			parsedEvent.OperatorId.ShouldBe(OperatorId);
			parsedEvent.Operation.ShouldBe(RecordingsBatchTrashOperationType.TrashUserRecordings);

			parsedEvent.MeetingUuids.ShouldNotBeNull();
			parsedEvent.MeetingUuids.Length.ShouldBe(2);
			parsedEvent.MeetingUuids.ShouldBeSubsetOf(new[] { "atsXxhSEQWit9t+U02HXNQ==", "3KvSP3SHRvyalrGRqZ5+2w==" });
		}

		[Fact]
		public void RecordingCloudStorageUsageUpdated()
		{
			var parsedEvent = ParseWebhookEvent<RecordingCloudStorageUsageUpdatedEvent>(WebhooksResource.recording_cloud_storage_usage_updated);

			VerifyRecordingEvent(parsedEvent, ZoomNet.Models.Webhooks.EventType.RecordingCloudStorageUsageUpdated);

			parsedEvent.StorageUsage.ShouldNotBeNull();
			parsedEvent.StorageUsage.FreeStorage.ShouldBe("1.65 GB");
			parsedEvent.StorageUsage.PlanStorage.ShouldBe("40 GB");
			parsedEvent.StorageUsage.PlanType.ShouldBe("cmr_monthly_commitment_40");
			parsedEvent.StorageUsage.StorageUsed.ShouldBe("38.35 GB");
			parsedEvent.StorageUsage.StorageUsedPercentage.ShouldBe(95);
			parsedEvent.StorageUsage.StorageExceed.ShouldBe("0");
			parsedEvent.StorageUsage.MaxExceedDate.ShouldBe("2023-03-01");
		}

		[Fact]
		public void RecordingCompleted()
		{
			var parsedEvent = ParseWebhookEvent<RecordingCompletedEvent>(WebhooksResource.recording_completed);

			VerifyRecordingEvent(parsedEvent, ZoomNet.Models.Webhooks.EventType.RecordingCompleted);

			parsedEvent.DownloadToken.ShouldBe(DownloadToken);

			VerifyRecordedMeetingOrWebinarInfo(parsedEvent.Recording);

			parsedEvent.Recording.AccountId.ShouldBe("x1yCzABCDEfg23HiJKl4mN");
			parsedEvent.Recording.HostEmail.ShouldBe(UserEmail);
			parsedEvent.Recording.Password.ShouldBe("132456");
			parsedEvent.Recording.ShareUrl.ShouldBe("https://example.com");
			parsedEvent.Recording.TotalSize.ShouldBe(3328371);
			parsedEvent.Recording.FilesCount.ShouldBe(2);
			parsedEvent.Recording.AutoDelete.ShouldNotBeNull();
			parsedEvent.Recording.AutoDelete.Value.ShouldBeTrue();
			parsedEvent.Recording.AutoDeleteDate.ShouldBe("2028-07-12");
			parsedEvent.Recording.OnPremise.ShouldNotBeNull();
			parsedEvent.Recording.OnPremise.Value.ShouldBeFalse();
			parsedEvent.Recording.PlayPasscode.ShouldBe("yNYIS408EJygs7rE5vVsJwXIz4-VW7MH");

			parsedEvent.Recording.RecordingFiles.ShouldNotBeNull();
			parsedEvent.Recording.RecordingFiles.Length.ShouldBe(2);

			VerifyAudioRecordingFile(parsedEvent.Recording.RecordingFiles[0]);
			VerifyVideoRecordingFile(parsedEvent.Recording.RecordingFiles[1]);

			parsedEvent.Recording.ParticipantAudioFiles.ShouldNotBeNull();
			parsedEvent.Recording.ParticipantAudioFiles.ShouldHaveSingleItem();

			VerifyParticipantAudioFile(parsedEvent.Recording.ParticipantAudioFiles[0]);
		}

		[Fact]
		public void RecordingDeleted()
		{
			var parsedEvent = ParseWebhookEvent<RecordingDeletedEvent>(WebhooksResource.recording_deleted);

			VerifyRecordingEvent(parsedEvent, ZoomNet.Models.Webhooks.EventType.RecordingDeleted);

			parsedEvent.Operator.ShouldBe(OperatorEmail);
			parsedEvent.OperatorId.ShouldBe(OperatorId);

			VerifyRecordedMeetingOrWebinarInfo(parsedEvent.Recording);

			parsedEvent.Recording.AccountId.ShouldBe("x1yCzABCDEfg23HiJKl4mN");
			parsedEvent.Recording.TotalSize.ShouldBe(3328371);
			parsedEvent.Recording.ShareUrl.ShouldBe("https://example.com");
			parsedEvent.Recording.FilesCount.ShouldBe(2);

			parsedEvent.Recording.RecordingFiles.ShouldNotBeNull();
			parsedEvent.Recording.RecordingFiles.Length.ShouldBe(2);

			VerifyAudioRecordingFile(parsedEvent.Recording.RecordingFiles[0]);
			VerifyVideoRecordingFile(parsedEvent.Recording.RecordingFiles[1]);

			parsedEvent.Recording.ParticipantAudioFiles.ShouldNotBeNull();
			parsedEvent.Recording.ParticipantAudioFiles.ShouldHaveSingleItem();

			VerifyParticipantAudioFile(parsedEvent.Recording.ParticipantAudioFiles[0]);
		}

		[Fact]
		public void RecordingPaused()
		{
			var parsedEvent = ParseWebhookEvent<RecordingPausedEvent>(WebhooksResource.recording_paused);

			VerifyRecordingProgressEvent(parsedEvent, ZoomNet.Models.Webhooks.EventType.RecordingPaused, new DateTime(2021, 3, 23, 23, 15, 41, DateTimeKind.Utc));
		}

		[Fact]
		public void RecordingRecovered()
		{
			var parsedEvent = ParseWebhookEvent<RecordingRecoveredEvent>(WebhooksResource.recording_recovered);

			VerifyRecordingEvent(parsedEvent, ZoomNet.Models.Webhooks.EventType.RecordingRecovered);

			parsedEvent.DownloadToken.ShouldBe(DownloadToken);
			parsedEvent.Operator.ShouldBe(OperatorEmail);
			parsedEvent.OperatorId.ShouldBe(OperatorId);

			VerifyRecordedMeetingOrWebinarInfo(parsedEvent.Recording);

			parsedEvent.Recording.AccountId.ShouldBe("x1yCzABCDEfg23HiJKl4mN");
			parsedEvent.Recording.TotalSize.ShouldBe(3328371);
			parsedEvent.Recording.ShareUrl.ShouldBe("https://example.com");
			parsedEvent.Recording.FilesCount.ShouldBe(2);

			parsedEvent.Recording.RecordingFiles.ShouldNotBeNull();
			parsedEvent.Recording.RecordingFiles.Length.ShouldBe(2);

			VerifyAudioRecordingFile(parsedEvent.Recording.RecordingFiles[0]);
			VerifyVideoRecordingFile(parsedEvent.Recording.RecordingFiles[1]);

			parsedEvent.Recording.ParticipantAudioFiles.ShouldNotBeNull();
			parsedEvent.Recording.ParticipantAudioFiles.ShouldHaveSingleItem();

			VerifyParticipantAudioFile(parsedEvent.Recording.ParticipantAudioFiles[0]);
		}

		[Fact]
		public void RecordingRegistrationApproved()
		{
			var parsedEvent = ParseWebhookEvent<RecordingRegistrationApprovedEvent>(WebhooksResource.recording_registration_approved);

			VerifyRecordingEvent(parsedEvent, ZoomNet.Models.Webhooks.EventType.RecordingRegistrationApproved);

			parsedEvent.Operator.ShouldBe(OperatorEmail);
			parsedEvent.OperatorId.ShouldBe(OperatorId);

			VerifyRecordedMeetingOrWebinarInfo(parsedEvent.SessionInfo);

			VerifyRegistrant(parsedEvent.Registrant);
		}

		[Fact]
		public void RecordingRegistrationCreated()
		{
			var parsedEvent = ParseWebhookEvent<RecordingRegistrationCreatedEvent>(WebhooksResource.recording_registration_created);

			VerifyRecordingEvent(parsedEvent, ZoomNet.Models.Webhooks.EventType.RecordingRegistrationCreated);

			VerifyRecordedMeetingOrWebinarInfo(parsedEvent.SessionInfo);

			VerifyRegistrantExtended(parsedEvent.Registrant, joinUrl: null);

			parsedEvent.Registrant.Comments.ShouldBe("Looking forward to the webinar");
			parsedEvent.Registrant.CustomQuestions.ShouldNotBeNull();
			parsedEvent.Registrant.CustomQuestions.Length.ShouldBe(1);
			parsedEvent.Registrant.CustomQuestions[0].Key.ShouldBe("What do you hope to learn from this webinar?");
			parsedEvent.Registrant.CustomQuestions[0].Value.ShouldBe("Look forward to learning how you come up with new recipes and what other services you offer.");
		}

		[Fact]
		public void RecordingRegistrationDenied()
		{
			var parsedEvent = ParseWebhookEvent<RecordingRegistrationDeniedEvent>(WebhooksResource.recording_registration_denied);

			VerifyRecordingEvent(parsedEvent, ZoomNet.Models.Webhooks.EventType.RecordingRegistrationDenied);

			VerifyRecordedMeetingOrWebinarInfo(parsedEvent.SessionInfo);

			VerifyRegistrant(parsedEvent.Registrant);
		}

		[Fact]
		public void RecordingRenamed()
		{
			var parsedEvent = ParseWebhookEvent<RecordingRenamedEvent>(WebhooksResource.recording_renamed);

			VerifyRecordingEvent(parsedEvent, ZoomNet.Models.Webhooks.EventType.RecordingRenamed);

			parsedEvent.Operator.ShouldBe(OperatorEmail);
			parsedEvent.OperatorId.ShouldBe(OperatorId);

			parsedEvent.UpdatedOn.ShouldBe(1628705277325.FromUnixTime(Internal.UnixTimePrecision.Milliseconds));
			parsedEvent.Id.ShouldBe(MeetingId);
			parsedEvent.Uuid.ShouldBe(MeetingUuid);
			parsedEvent.HostId.ShouldBe(HostId);
			parsedEvent.Type.ShouldBe(RecordingType.PersonnalMeeting);
			parsedEvent.OldTitle.ShouldBe("My Old Personal Recording");
			parsedEvent.NewTitle.ShouldBe("My New Personal Recording");
		}

		[Fact]
		public void RecordingResumed()
		{
			var parsedEvent = ParseWebhookEvent<RecordingResumedEvent>(WebhooksResource.recording_resumed);

			VerifyRecordingProgressEvent(parsedEvent, ZoomNet.Models.Webhooks.EventType.RecordingResumed);
		}

		[Fact]
		public void RecordingStarted()
		{
			var parsedEvent = ParseWebhookEvent<RecordingStartedEvent>(WebhooksResource.recording_started);

			VerifyRecordingProgressEvent(parsedEvent, ZoomNet.Models.Webhooks.EventType.RecordingStarted);
		}

		[Fact]
		public void RecordingStopped()
		{
			var parsedEvent = ParseWebhookEvent<RecordingStoppedEvent>(WebhooksResource.recording_stopped);

			VerifyRecordingProgressEvent(parsedEvent, ZoomNet.Models.Webhooks.EventType.RecordingStopped, new DateTime(2021, 3, 23, 23, 15, 41, DateTimeKind.Utc));
		}

		[Fact]
		public void RecordingTranscriptCompleted()
		{
			var parsedEvent = ParseWebhookEvent<RecordingTranscriptCompletedEvent>(WebhooksResource.recording_transcript_completed);

			VerifyRecordingEvent(parsedEvent, ZoomNet.Models.Webhooks.EventType.RecordingTranscriptCompleted);

			parsedEvent.DownloadToken.ShouldBe(DownloadToken);

			VerifyRecordedMeetingOrWebinarInfo(parsedEvent.Recording);

			parsedEvent.Recording.AccountId.ShouldBe("x1yCzABCDEfg23HiJKl4mN");
			parsedEvent.Recording.HostEmail.ShouldBe(UserEmail);
			parsedEvent.Recording.Password.ShouldBe("123456");
			parsedEvent.Recording.ShareUrl.ShouldBe("https://example.com");
			parsedEvent.Recording.TotalSize.ShouldBe(529758);
			parsedEvent.Recording.FilesCount.ShouldBe(1);

			parsedEvent.Recording.RecordingFiles.ShouldNotBeNull();
			parsedEvent.Recording.RecordingFiles.ShouldHaveSingleItem();

			VerifyTranscriptRecordingFile(parsedEvent.Recording.RecordingFiles[0]);

			parsedEvent.Recording.ParticipantAudioFiles.ShouldBeNull();
		}

		[Fact]
		public void RecordingTrashed()
		{
			var parsedEvent = ParseWebhookEvent<RecordingTrashedEvent>(WebhooksResource.recording_trashed);

			VerifyRecordingEvent(parsedEvent, ZoomNet.Models.Webhooks.EventType.RecordingTrashed);

			parsedEvent.DownloadToken.ShouldBe(DownloadToken);
			parsedEvent.Operator.ShouldBe(OperatorEmail);
			parsedEvent.OperatorId.ShouldBe(OperatorId);

			VerifyRecordedMeetingOrWebinarInfo(parsedEvent.Recording);

			parsedEvent.Recording.AccountId.ShouldBe("x1yCzABCDEfg23HiJKl4mN");
			parsedEvent.Recording.TotalSize.ShouldBe(3328371);
			parsedEvent.Recording.ShareUrl.ShouldBe("https://example.com");
			parsedEvent.Recording.FilesCount.ShouldBe(2);

			parsedEvent.Recording.RecordingFiles.ShouldNotBeNull();
			parsedEvent.Recording.RecordingFiles.Length.ShouldBe(2);

			VerifyAudioRecordingFile(parsedEvent.Recording.RecordingFiles[0]);
			VerifyVideoRecordingFile(parsedEvent.Recording.RecordingFiles[1]);

			parsedEvent.Recording.ParticipantAudioFiles.ShouldNotBeNull();
			parsedEvent.Recording.ParticipantAudioFiles.ShouldHaveSingleItem();

			VerifyParticipantAudioFile(parsedEvent.Recording.ParticipantAudioFiles[0]);
		}

		#endregion

		#region private methods

		/// <summary>
		/// Verify <see cref="RecordingEvent"/> properties.
		/// </summary>
		private static void VerifyRecordingEvent(RecordingEvent parsedEvent, ZoomNet.Models.Webhooks.EventType eventType)
		{
			parsedEvent.EventType.ShouldBe(eventType);
			parsedEvent.Timestamp.ShouldBe(eventTimestamp);
			parsedEvent.AccountId.ShouldBe(AccountId);
		}

		/// <summary>
		/// Verify <see cref="RecordedMeetingOrWebinarInfo"/> properties.
		/// </summary>
		private static void VerifyRecordedMeetingOrWebinarInfo(RecordedMeetingOrWebinarInfo info)
		{
			info.ShouldNotBeNull();
			info.Id.ShouldBe(MeetingId);
			info.Uuid.ShouldBe(MeetingUuid);
			info.HostId.ShouldBe(HostId);
			info.Topic.ShouldBe("My Personal Recording");
			info.Type.ShouldBe(RecordingType.PersonnalMeeting);
			info.StartTime.ShouldBe(timestamp);
			info.Timezone.ShouldBe(TimeZones.America_Los_Angeles);
			info.Duration.ShouldBe(60);
		}

		/// <summary>
		/// Verify recordings batch.
		/// </summary>
		private static void VerifyRecordingsBatch(RecordingsBatch[] batch)
		{
			batch.ShouldNotBeNull();
			batch.ShouldHaveSingleItem();
			batch[0].MeetingUuid.ShouldBe(MeetingUuid);
			batch[0].RecordingFileIds.ShouldNotBeNull();
			batch[0].RecordingFileIds.Length.ShouldBe(2);
			batch[0].RecordingFileIds.ShouldBeSubsetOf(new[] { "ed6c2f27-2ae7-42f4-b3d0-835b493e4fa8", "388ffb46-1541-460d-8447-4624451a1db7" });
		}

		/// <summary>
		/// Verify <see cref="RecordingProgressEvent"/> properties.
		/// </summary>
		private static void VerifyRecordingProgressEvent(RecordingProgressEvent parsedEvent, ZoomNet.Models.Webhooks.EventType eventType, DateTime? endTime = null)
		{
			VerifyRecordingEvent(parsedEvent, eventType);

			VerifyRecordedMeetingOrWebinarInfo(parsedEvent.SessionInfo);

			parsedEvent.StartTime.ShouldBe(new DateTime(2021, 3, 23, 22, 14, 57, DateTimeKind.Utc));
			parsedEvent.EndTime.ShouldBe(endTime);
		}

		/// <summary>
		/// Verify audio file properties.
		/// </summary>
		private static void VerifyAudioRecordingFile(RecordingFile file)
		{
			file.ShouldNotBeNull();
			file.Id.ShouldBe("ed6c2f27-2ae7-42f4-b3d0-835b493e4fa8");
			file.MeetingId.ShouldBe("098765ABCD");
			file.StartTime.ShouldBe(new DateTime(2021, 3, 23, 22, 14, 57, 0, DateTimeKind.Utc));
			file.EndTime.ShouldBe(new DateTime(2021, 3, 23, 23, 15, 41, 0, DateTimeKind.Utc));
			file.ContentType.ShouldBe(RecordingContentType.AudioOnly);
			file.FileType.ShouldBe(RecordingFileType.Audio);
			file.Size.ShouldBe(246560);
			file.FileExtension.ShouldBe(RecordingFileExtension.M4A);
			file.PlayUrl.ShouldBe("https://example.com/recording/play/Qg75t7xZBtEbAkjdlgbfdngBBBB");
			file.DownloadUrl.ShouldBe("https://example.com/recording/download/Qg75t7xZBtEbAkjdlgbfdngBBBB");
			file.Status.ShouldBe(RecordingStatus.Completed);
		}

		/// <summary>
		/// Verify video file properties.
		/// </summary>
		private static void VerifyVideoRecordingFile(RecordingFile file)
		{
			file.ShouldNotBeNull();
			file.Id.ShouldBe("388ffb46-1541-460d-8447-4624451a1db7");
			file.MeetingId.ShouldBe("098765ABCD");
			file.StartTime.ShouldBe(new DateTime(2021, 3, 23, 22, 14, 57, 0, DateTimeKind.Utc));
			file.EndTime.ShouldBe(new DateTime(2021, 3, 23, 23, 15, 41, 0, DateTimeKind.Utc));
			file.ContentType.ShouldBe(RecordingContentType.SharedScreenWithSpeakerView);
			file.FileType.ShouldBe(RecordingFileType.Video);
			file.Size.ShouldBe(282825);
			file.FileExtension.ShouldBe(RecordingFileExtension.MP4);
			file.PlayUrl.ShouldBe("https://example.com/recording/play/Qg75t7xZBtEbAkjdlgbfdngCCCC");
			file.DownloadUrl.ShouldBe("https://example.com/recording/download/Qg75t7xZBtEbAkjdlgbfdngCCCC");
			file.Status.ShouldBe(RecordingStatus.Completed);
		}

		/// <summary>
		/// Verify transcript recording file properties.
		/// </summary>
		private static void VerifyTranscriptRecordingFile(RecordingFile file)
		{
			file.ShouldNotBeNull();
			file.Id.ShouldBe("bd5b3ea6-7dd6-484d-9125-b297e0a2f80a");
			file.MeetingId.ShouldBe("098765ABCD");
			file.StartTime.ShouldBe(new DateTime(2021, 3, 23, 22, 14, 57, 0, DateTimeKind.Utc));
			file.EndTime.ShouldBe(new DateTime(2021, 3, 23, 23, 15, 41, 0, DateTimeKind.Utc));
			file.ContentType.ShouldBe(RecordingContentType.AudioTranscript);
			file.FileType.ShouldBe(RecordingFileType.Transcript);
			file.Size.ShouldBe(142);
			file.FileExtension.ShouldBe(RecordingFileExtension.VTT);
			file.PlayUrl.ShouldBe("https://example.com/recording/play/Qg75t7xZBtEbAkjdlgbfdngBBBB");
			file.DownloadUrl.ShouldBe("https://example.com/recording/download/Qg75t7xZBtEbAkjdlgbfdngBBBB");
			file.Status.ShouldBe(RecordingStatus.Completed);
		}

		/// <summary>
		/// Verify participant audio file properties.
		/// </summary>
		private static void VerifyParticipantAudioFile(RecordingFile file)
		{
			file.ShouldNotBeNull();
			file.Id.ShouldBe("2af3b4b2-d5ad-4b5b-9be2-81b429206b27");
			file.StartTime.ShouldBe(new DateTime(2021, 3, 23, 22, 14, 57, 0, DateTimeKind.Utc));
			file.EndTime.ShouldBe(new DateTime(2021, 3, 23, 23, 15, 41, 0, DateTimeKind.Utc));
			file.FileType.ShouldBe(RecordingFileType.Audio);
			file.FileName.ShouldBe("MyRecording");
			file.Size.ShouldBe(246560);
			file.FileExtension.ShouldBe(RecordingFileExtension.M4A);
			file.PlayUrl.ShouldBe("https://example.com/recording/play/Qg75t7xZBtEbAkjdlgbfdngAAAA");
			file.DownloadUrl.ShouldBe("https://example.com/recording/download/Qg75t7xZBtEbAkjdlgbfdngAAAA");
			file.Status.ShouldBe(RecordingStatus.Completed);
		}

		#endregion
	}
}
