using Shouldly;
using System;
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
			'event': 'meeting.created',
			'payload': {
				'account_id': 'VjZoEArIT5y-HlWxkV-tVA',
				'operator': 'someone@example.com',
				'operator_id': '8lzIwvZTSOqjndWPbPqzuA',
				'object': {
					'uuid': '5yDZGNlQSV6qOjg4NxajHQ==',
					'id': 98884753832,
					'host_id': '8lzIwvZTSOqjndWPbPqzuA',
					'topic': 'ZoomNet Unit Testing: instant meeting',
					'type': 1,
					'duration': 60,
					'timezone': 'America/New_York',
					'join_url': 'https://zoom.us/j/98884753832?pwd=c21EQzg0SXY2dlNTOFF2TENpSm1aQT09',
					'password': 'PaSsWoRd',
					'settings': {
						'use_pmi': false,
						'alternative_hosts': ''
					}
				}
			},
			'event_ts': 1617628462392
		}";

		private const string MEETING_DELETED_WEBHOOK = @"
		{
			'event': 'meeting.deleted',
			'payload': {
				'account_id': 'VjZoEArIT5y-HlWxkV-tVA',
				'operator': 'someone@example.com',
				'operator_id': '8lzIwvZTSOqjndWPbPqzuA',
				'object': {
					'uuid': '5yDZGNlQSV6qOjg4NxajHQ==',
					'id': 98884753832,
					'host_id': '8lzIwvZTSOqjndWPbPqzuA',
					'topic': 'ZoomNet Unit Testing: instant meeting',
					'type': 1,
					'duration': 60,
					'timezone': 'America/New_York'
				}
			},
			'event_ts': 1617628462764
		}";

		private const string MEETING_UPDATED_WEBHOOK = @"
		{
			'event': 'meeting.updated',
			'payload': {
				'account_id': 'VjZoEArIT5y-HlWxkV-tVA',
				'operator': 'someone@example.com',
				'operator_id': '8lzIwvZTSOqjndWPbPqzuA',
				'object': {
					'id': 94890226305,
					'topic': 'ZoomNet Unit Testing: UPDATED scheduled meeting',
					'settings': { 'audio': 'voip' }
				},
				'old_object': {
					'id': 94890226305,
					'topic': 'ZoomNet Unit Testing: scheduled meeting',
					'settings': { 'audio': 'telephony' }
				},
				'time_stamp': 1617628464664
			},
			'event_ts': 1617628464664
		}";

		private const string MEETING_STARTED_WEBHOOK = @"
		{
			'event':'meeting.started',
			'payload': {
				'account_id':'VjZoEArIT5y-HlWxkV-tVA',
				'object': {
					'duration':0,
					'start_time':'2021-04-21T14:49:04Z',
					'timezone':'America/New_York',
					'topic':'My Personal Meeting Room',
					'id':'3479130610',
					'type':4,
					'uuid':'mOG8pEaFQqeDm6Vd/3xopA==',
					'host_id':'8lzIwvZTSOqjndWPbPqzuA'
				}
			},
			'event_ts':1619016544371
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
		public void MeetingUpdated()
		{
			var parsedEvent = (MeetingUpdatedEvent)new WebhookParser().ParseEventWebhook(MEETING_UPDATED_WEBHOOK);

			parsedEvent.EventType.ShouldBe(EventType.MeetingUpdated);
			parsedEvent.Operator.ShouldBe("someone@example.com");
			parsedEvent.OperatorId.ShouldBe("8lzIwvZTSOqjndWPbPqzuA");
			parsedEvent.ModifiedFields.ShouldNotBeNull();
			parsedEvent.ModifiedFields.Length.ShouldBe(3);
			parsedEvent.ModifiedFields[0].FieldName.ShouldBe("id");
			parsedEvent.ModifiedFields[0].OldValue.ShouldBe(94890226305);
			parsedEvent.ModifiedFields[0].NewValue.ShouldBe(94890226305);
			parsedEvent.ModifiedFields[1].FieldName.ShouldBe("topic");
			parsedEvent.ModifiedFields[1].OldValue.ShouldBe("ZoomNet Unit Testing: scheduled meeting");
			parsedEvent.ModifiedFields[1].NewValue.ShouldBe("ZoomNet Unit Testing: UPDATED scheduled meeting");
			parsedEvent.ModifiedFields[2].FieldName.ShouldBe("settings");
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
	}
}
