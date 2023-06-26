using Shouldly;
using System;
using System.Linq;
using Xunit;
using ZoomNet.Models;
using ZoomNet.Models.Webhooks;

namespace ZoomNet.UnitTests
{
	public class WebhookParserTests
	{
		#region FIELDS

		private const string MEETING_CREATED_WEBHOOK = @"
		{
			""event"": ""meeting.created"",
			""payload"": {
				""account_id"": ""VjZoEArIT5y-HlWxkV-tVA"",
				""operator"": ""someone@example.com"",
				""operator_id"": ""8lzIwvZTSOqjndWPbPqzuA"",
				""object"": {
					""uuid"": ""5yDZGNlQSV6qOjg4NxajHQ=="",
					""id"": 98884753832,
					""host_id"": ""8lzIwvZTSOqjndWPbPqzuA"",
					""topic"": ""ZoomNet Unit Testing: instant meeting"",
					""type"": 1,
							""duration"": 60,
					""timezone"": ""America/New_York"",
					""join_url"": ""https://zoom.us/j/98884753832?pwd=c21EQzg0SXY2dlNTOFF2TENpSm1aQT09"",
					""password"": ""PaSsWoRd"",
					""settings"": {
						""use_pmi"": false,
						""alternative_hosts"": """"
				}
			}
			},
			""event_ts"": 1617628462392
		}";

		private const string MEETING_DELETED_WEBHOOK = @"
		{
			""event"": ""meeting.deleted"",
			""payload"": {
				""account_id"": ""VjZoEArIT5y-HlWxkV-tVA"",
				""operator"": ""someone@example.com"",
				""operator_id"": ""8lzIwvZTSOqjndWPbPqzuA"",
				""object"": {
					""uuid"": ""5yDZGNlQSV6qOjg4NxajHQ=="",
					""id"": 98884753832,
					""host_id"": ""8lzIwvZTSOqjndWPbPqzuA"",
					""topic"": ""ZoomNet Unit Testing: instant meeting"",
					""type"": 1,
					""duration"": 60,
					""timezone"": ""America/New_York""
			}
			},
			""event_ts"": 1617628462764
		}";

		private const string MEETING_ENDED_WEBHOOK = @"
		{
			""event"": ""meeting.ended"",
			""event_ts"": 1626230691572,
			""payload"": {
				""account_id"": ""AAAAAABBBB"",
				""operator"": ""admin@example.com"",
				""operator_id"": ""z8yCxjabcdEFGHfp8uQ"",
				""operation"": ""single"",
				""object"": {
					""id"": ""1234567890"",
					""uuid"": ""4444AAAiAAAAAiAiAiiAii=="",
					""host_id"": ""x1yCzABCDEfg23HiJKl4mN"",
					""topic"": ""My Meeting"",
					""type"": 3,
					""start_time"": ""2021-07-13T21:44:51Z"",
					""timezone"": ""America/Los_Angeles"",
					""duration"": 60,
					""end_time"": ""2021-07-13T23:00:51Z""
				}
			}
		}";

		private const string MEETING_UPDATED_WEBHOOK = @"
		{
			""event"": ""meeting.updated"",
			""payload"": {
				""account_id"": ""VjZoEArIT5y-HlWxkV-tVA"",
				""operator"": ""someone@example.com"",
				""operator_id"": ""8lzIwvZTSOqjndWPbPqzuA"",
				""object"": {
					""id"": 94890226305,
					""topic"": ""ZoomNet Unit Testing: UPDATED scheduled meeting"",
					""settings"": { ""audio"": ""voip"" }
					},
					""old_object"": {
					""id"": 94890226305,
					""topic"": ""ZoomNet Unit Testing: scheduled meeting"",
					""settings"": { ""audio"": ""telephony"" }
				},
				""time_stamp"": 1617628464664
			},
			""event_ts"": 1617628464664
		}";

		private const string MEETING_STARTED_WEBHOOK = @"
		{
			""event"":""meeting.started"",
			""payload"": {
				""account_id"":""VjZoEArIT5y-HlWxkV-tVA"",
				""object"": {
					""duration"":0,
					""start_time"":""2021-04-21T14:49:04Z"",
					""timezone"":""America/New_York"",
					""topic"":""My Personal Meeting Room"",
					""id"":3479130610,
					""type"":4,
					""uuid"":""mOG8pEaFQqeDm6Vd/3xopA=="",
					""host_id"":""8lzIwvZTSOqjndWPbPqzuA""
				}
			},
			""event_ts"":1619016544371
		}";

		private const string MEETING_SHARING_STARTED_WEBHOOK = @"
		{
			""event"": ""meeting.sharing_started"",
			""event_ts"": 1234566789900,
			""payload"": {
				""object"": {
					""duration"": 60,
					""start_time"": ""2019-07-16T17:14:39Z"",
					""timezone"": ""America/Los_Angeles"",
					""topic"": ""My Meeting"",
					""id"": 6962400003,
					""type"": 2,
					""uuid"": ""4118UHIiRCAAAtBlDkcVyw=="",
					""host_id"": ""z8yCxTTTTSiw02QgCAp8uQ"",
					""participant"": {
						""id"": ""s0AAAASoSE1V8KIFOCYw"",
						""user_id"": ""16778000"",
						""user_name"": ""Arya Arya"",
						""sharing_details"": {
							""link_source"": ""in_meeting"",
							""file_link"": """",
							""source"": ""dropbox"",
							""date_time"": ""2019-07-16T17:19:11Z"",
							""content"": ""application""
						}
			}
		},
				""account_id"": ""EPeQtiABC000VYxHMA""
			}
	}";

		private const string MEETING_SERVICE_ISSUE_WEBHOOK = @"
		{
			""event"": ""meeting.alert"",
			""event_ts"": 1626230691572,
			""payload"": {
				""account_id"": ""AAAAAABBBB"",
				""object"": {
					""id"": 1234567890,
					""uuid"": ""4444AAAiAAAAAiAiAiiAii=="",
					""host_id"": ""x1yCzABCDEfg23HiJKl4mN"",
					""topic"": ""My Meeting"",
					""type"": 2,
					""join_time"": ""2021-07-13T21:44:51Z"",
					""timezone"": ""America/Los_Angeles"",
					""duration"": 60,
					""issues"": [
						""Unstable audio quality""
					]
				}
			}
		}";

		private const string WEBINAR_ENDED_WEBHOOK = @"
		{
			""event"": ""webinar.ended"",
			""event_ts"": 1626230691572,
			""payload"": {
				""account_id"": ""AAAAAABBBB"",
				""object"": {
					""id"": ""1234567890"",
					""uuid"": ""4444AAAiAAAAAiAiAiiAii=="",
					""host_id"": ""x1yCzABCDEfg23HiJKl4mN"",
					""topic"": ""My Webinar"",
					""type"": 5,
					""start_time"": ""2021-07-13T21:44:51Z"",
					""timezone"": ""America/Los_Angeles"",
					""duration"": 60,
					""end_time"": ""2021-07-13T23:00:51Z""
				}
			}
		}";

		private const string RECORDING_COMPLETED_WEBHOOK = @"
		{
		  ""event"": ""recording.completed"",
		  ""event_ts"": 1626230691572,
		  ""payload"": {
		    ""account_id"": ""AAAAAABBBB"",
		    ""object"": {
		      ""id"": 1234567890,
		      ""uuid"": ""4444AAAiAAAAAiAiAiiAii=="",
		      ""host_id"": ""x1yCzABCDEfg23HiJKl4mN"",
		      ""account_id"": ""x1yCzABCDEfg23HiJKl4mN"",
		      ""topic"": ""My Personal Recording"",
		      ""type"": 4,
		      ""start_time"": ""2021-07-13T21:44:51Z"",
		      ""password"": ""132456"",
		      ""timezone"": ""America/Los_Angeles"",
		      ""host_email"": ""jchill@example.com"",
		      ""duration"": 60,
		      ""share_url"": ""https://example.com"",
		      ""total_size"": 3328371,
		      ""recording_count"": 2,
		      ""on_prem"": false,
		      ""recording_play_passcode"": ""yNYIS408EJygs7rE5vVsJwXIz4-VW7MH"",
		      ""recording_files"": [
		        {
		          ""id"": ""ed6c2f27-2ae7-42f4-b3d0-835b493e4fa8"",
		          ""meeting_id"": ""098765ABCD"",
		          ""recording_start"": ""2021-03-23T22:14:57Z"",
		          ""recording_end"": ""2021-03-23T23:15:41Z"",
		          ""recording_type"": ""audio_only"",
		          ""file_type"": ""M4A"",
		          ""file_size"": 246560,
		          ""file_extension"": ""M4A"",
		          ""play_url"": ""https://example.com/recording/play/Qg75t7xZBtEbAkjdlgbfdngBBBB"",
		          ""download_url"": ""https://example.com/recording/download/Qg75t7xZBtEbAkjdlgbfdngBBBB"",
		          ""status"": ""completed""
		        },
		        {
		          ""id"": ""388ffb46-1541-460d-8447-4624451a1db7"",
		          ""meeting_id"": ""098765ABCD"",
		          ""recording_start"": ""2021-03-23T22:14:57Z"",
		          ""recording_end"": ""2021-03-23T23:15:41Z"",
		          ""recording_type"": ""shared_screen_with_speaker_view"",
		          ""file_type"": ""MP4"",
		          ""file_size"": 282825,
		          ""file_extension"": ""MP4"",
		          ""play_url"": ""https://example.com/recording/play/Qg75t7xZBtEbAkjdlgbfdngCCCC"",
		          ""download_url"": ""https://example.com/recording/download/Qg75t7xZBtEbAkjdlgbfdngCCCC"",
		          ""status"": ""completed""
		        }
		      ],
		      ""participant_audio_files"": [
		        {
		          ""id"": ""ed6c2f27-2ae7-42f4-b3d0-835b493e4fa8"",
		          ""recording_start"": ""2021-03-23T22:14:57Z"",
		          ""recording_end"": ""2021-03-23T23:15:41Z"",
		          ""file_type"": ""M4A"",
		          ""file_name"": ""MyRecording"",
		          ""file_size"": 246560,
		          ""file_extension"": ""MP4"",
		          ""play_url"": ""https://example.com/recording/play/Qg75t7xZBtEbAkjdlgbfdngAAAA"",
		          ""download_url"": ""https://example.com/recording/download/Qg75t7xZBtEbAkjdlgbfdngAAAA"",
		          ""status"": ""completed""
		        }
		      ]
		    }
		  },
		  ""download_token"": ""abJhbGciOiJIUzUxMiJ9.eyJpc3MiOiJodHRwczovL2V2ZW50Lnpvb20udXMiLCJhY2NvdW50SWQiOiJNdDZzdjR1MFRBeVBrd2dzTDJseGlBIiwiYXVkIjoiaHR0cHM6Ly9vYXV0aC56b29tLnVzIiwibWlkIjoieFp3SEc0c3BRU2VuekdZWG16dnpiUT09IiwiZXhwIjoxNjI2MTM5NTA3LCJ1c2VySWQiOiJEWUhyZHBqclMzdWFPZjdkUGtrZzh3In0.a6KetiC6BlkDhf1dP4KBGUE1bb2brMeraoD45yhFx0eSSSTFdkHQnsKmlJQ-hdo9Zy-4vQw3rOxlyoHv583JyZ""
		}";

		#endregion

		[Fact]
		public void MeetingCreated()
		{
			var parsedEvent = (MeetingCreatedEvent)new WebhookParser().ParseEventWebhook(MEETING_CREATED_WEBHOOK);

			parsedEvent.EventType.ShouldBe(EventType.MeetingCreated);
			parsedEvent.Operator.ShouldBe("someone@example.com");
			parsedEvent.OperatorId.ShouldBe("8lzIwvZTSOqjndWPbPqzuA");
			parsedEvent.Meeting.ShouldNotBeNull();
			parsedEvent.Meeting.Uuid.ShouldBe("5yDZGNlQSV6qOjg4NxajHQ==");
			parsedEvent.Meeting.Id.ShouldBe(98884753832);
			parsedEvent.Meeting.HostId.ShouldBe("8lzIwvZTSOqjndWPbPqzuA");
			parsedEvent.Meeting.Topic.ShouldBe("ZoomNet Unit Testing: instant meeting");
			parsedEvent.Meeting.Type.ShouldBe(Models.MeetingType.Instant);
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
			var parsedEvent = (MeetingDeletedEvent)new WebhookParser().ParseEventWebhook(MEETING_DELETED_WEBHOOK);

			parsedEvent.EventType.ShouldBe(EventType.MeetingDeleted);
			parsedEvent.Operator.ShouldBe("someone@example.com");
			parsedEvent.OperatorId.ShouldBe("8lzIwvZTSOqjndWPbPqzuA");
			parsedEvent.Meeting.ShouldNotBeNull();
			parsedEvent.Meeting.Uuid.ShouldBe("5yDZGNlQSV6qOjg4NxajHQ==");
			parsedEvent.Meeting.Id.ShouldBe(98884753832);
			parsedEvent.Meeting.HostId.ShouldBe("8lzIwvZTSOqjndWPbPqzuA");
			parsedEvent.Meeting.Topic.ShouldBe("ZoomNet Unit Testing: instant meeting");
			parsedEvent.Meeting.Type.ShouldBe(Models.MeetingType.Instant);
			parsedEvent.Meeting.JoinUrl.ShouldBeNull();
			parsedEvent.Meeting.Password.ShouldBeNull();
			parsedEvent.Meeting.Settings.ShouldBeNull();
		}

		[Fact]
		public void MeetingEnded()
		{
			var parsedEvent = (MeetingEndedEvent)new WebhookParser().ParseEventWebhook(MEETING_ENDED_WEBHOOK);

			parsedEvent.EventType.ShouldBe(EventType.MeetingEnded);
			parsedEvent.Timestamp.ShouldBe(1626230691572.FromUnixTime(Internal.UnixTimePrecision.Milliseconds));
			parsedEvent.Meeting.ShouldNotBeNull();
			parsedEvent.Meeting.Uuid.ShouldBe("4444AAAiAAAAAiAiAiiAii==");
			parsedEvent.Meeting.Id.ShouldBe(1234567890);
			parsedEvent.Meeting.HostId.ShouldBe("x1yCzABCDEfg23HiJKl4mN");
			parsedEvent.Meeting.Topic.ShouldBe("My Meeting");
			parsedEvent.Meeting.Type.ShouldBe(MeetingType.RecurringNoFixedTime);
			parsedEvent.Meeting.JoinUrl.ShouldBeNull();
			parsedEvent.Meeting.Password.ShouldBeNull();
			parsedEvent.Meeting.Settings.ShouldBeNull();
		}

		[Fact]
		public void MeetingUpdated()
		{
			var parsedEvent = (MeetingUpdatedEvent)new WebhookParser().ParseEventWebhook(MEETING_UPDATED_WEBHOOK);

			parsedEvent.EventType.ShouldBe(EventType.MeetingUpdated);
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
			var parsedEvent = (MeetingStartedEvent)new WebhookParser().ParseEventWebhook(MEETING_STARTED_WEBHOOK);

			parsedEvent.EventType.ShouldBe(EventType.MeetingStarted);
			parsedEvent.AccountId.ShouldBe("VjZoEArIT5y-HlWxkV-tVA");
			parsedEvent.Timestamp.ShouldBe(new DateTime(2021, 4, 21, 14, 49, 4, 371, DateTimeKind.Utc));

			parsedEvent.Meeting.GetType().ShouldBe(typeof(InstantMeeting));
			var parsedMeeting = (InstantMeeting)parsedEvent.Meeting;
			parsedMeeting.Id.ShouldBe(3479130610);
			parsedMeeting.Topic.ShouldBe("My Personal Meeting Room");
			parsedMeeting.Uuid.ShouldBe("mOG8pEaFQqeDm6Vd/3xopA==");
			parsedMeeting.HostId.ShouldBe("8lzIwvZTSOqjndWPbPqzuA");
		}

		[Fact]
		public void MeetingSharingStarted()
		{
			var parsedEvent = (MeetingSharingStartedEvent)new WebhookParser().ParseEventWebhook(MEETING_SHARING_STARTED_WEBHOOK);

			parsedEvent.EventType.ShouldBe(EventType.MeetingSharingStarted);
			parsedEvent.AccountId.ShouldBe("EPeQtiABC000VYxHMA");
			parsedEvent.Timestamp.ShouldBe(new DateTime(2009, 2, 13, 23, 13, 9, 900, DateTimeKind.Utc));

			parsedEvent.Participant.ShouldNotBeNull();
			parsedEvent.Participant.DisplayName.ShouldBe("Arya Arya");
			parsedEvent.Participant.Email.ShouldBeNullOrEmpty();
			parsedEvent.Participant.ParticipantId.ShouldBe("16778000");
			parsedEvent.Participant.UserId.ShouldBe("s0AAAASoSE1V8KIFOCYw");

			parsedEvent.ScreenshareDetails.ShouldNotBeNull();
			parsedEvent.ScreenshareDetails.ContentType.ShouldBe(ScreenshareContentType.Application);
			parsedEvent.ScreenshareDetails.Date.ShouldBe(new DateTime(2019, 7, 16, 17, 19, 11, 0, DateTimeKind.Utc));
			parsedEvent.ScreenshareDetails.SharingMethod.ShouldBe("in_meeting");
			parsedEvent.ScreenshareDetails.Source.ShouldBe("dropbox");
			parsedEvent.ScreenshareDetails.Link.ShouldBe("");

			parsedEvent.Meeting.GetType().ShouldBe(typeof(ScheduledMeeting));
			var parsedMeeting = (ScheduledMeeting)parsedEvent.Meeting;
			parsedMeeting.Id.ShouldBe(6962400003);
			parsedMeeting.Topic.ShouldBe("My Meeting");
			parsedMeeting.Uuid.ShouldBe("4118UHIiRCAAAtBlDkcVyw==");
			parsedMeeting.HostId.ShouldBe("z8yCxTTTTSiw02QgCAp8uQ");
			parsedMeeting.StartTime.ShouldBe(new DateTime(2019, 7, 16, 17, 14, 39, 0, DateTimeKind.Utc));
			parsedMeeting.Duration.ShouldBe(60);
			parsedMeeting.Timezone.ShouldBe("America/Los_Angeles");
		}

		[Fact]
		public void MeetingServiceIssue()
		{
			var parsedEvent = (MeetingServiceIssueEvent)new WebhookParser().ParseEventWebhook(MEETING_SERVICE_ISSUE_WEBHOOK);

			parsedEvent.EventType.ShouldBe(EventType.MeetingServiceIssue);
			parsedEvent.AccountId.ShouldBe("AAAAAABBBB");
			parsedEvent.Timestamp.ShouldBe(new DateTime(2021, 7, 14, 2, 44, 51, 572, DateTimeKind.Utc));
			parsedEvent.Issues.ShouldBe(new[] { "Unstable audio quality" });

			parsedEvent.Meeting.GetType().ShouldBe(typeof(ScheduledMeeting));
			var parsedMeeting = (ScheduledMeeting)parsedEvent.Meeting;
			parsedMeeting.Id.ShouldBe(1234567890L);
			parsedMeeting.Topic.ShouldBe("My Meeting");
			parsedMeeting.Uuid.ShouldBe("4444AAAiAAAAAiAiAiiAii==");
			parsedMeeting.HostId.ShouldBe("x1yCzABCDEfg23HiJKl4mN");
			parsedMeeting.Duration.ShouldBe(60);
			parsedMeeting.Timezone.ShouldBe("America/Los_Angeles");
		}

		[Fact]
		public void RecordingCompleted()
		{
			var parsedEvent = (RecordingCompletedEvent)new WebhookParser().ParseEventWebhook(RECORDING_COMPLETED_WEBHOOK);

			parsedEvent.EventType.ShouldBe(EventType.RecordingCompleted);
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
			participantAudioFile.PlayUrl.ShouldBe("https://example.com/recording/play/Qg75t7xZBtEbAkjdlgbfdngAAAA");
			participantAudioFile.DownloadUrl.ShouldBe("https://example.com/recording/download/Qg75t7xZBtEbAkjdlgbfdngAAAA");
			participantAudioFile.Status.ShouldBe(RecordingStatus.Completed);
		}

		[Fact]
		public void WebinarEnded()
		{
			var parsedEvent = (WebinarEndedEvent)new WebhookParser().ParseEventWebhook(WEBINAR_ENDED_WEBHOOK);

			parsedEvent.EventType.ShouldBe(EventType.WebinarEnded);
			parsedEvent.Timestamp.ShouldBe(1626230691572.FromUnixTime(Internal.UnixTimePrecision.Milliseconds));
			parsedEvent.Webinar.ShouldNotBeNull();
			parsedEvent.Webinar.Uuid.ShouldBe("4444AAAiAAAAAiAiAiiAii==");
			parsedEvent.Webinar.Id.ShouldBe(1234567890);
			parsedEvent.Webinar.HostId.ShouldBe("x1yCzABCDEfg23HiJKl4mN");
			parsedEvent.Webinar.Topic.ShouldBe("My Webinar");
			parsedEvent.Webinar.Type.ShouldBe(WebinarType.Regular);
			parsedEvent.Webinar.JoinUrl.ShouldBeNull();
			parsedEvent.Webinar.Password.ShouldBeNull();
			parsedEvent.Webinar.Settings.ShouldBeNull();
		}
	}
}
