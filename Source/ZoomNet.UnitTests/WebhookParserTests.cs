using Shouldly;
using Xunit;
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

		#endregion

		[Fact]
		public void MeetingCreated()
		{
			var parsedEvent = (MeetingCreatedEvent)WebhookParser.ParseEventWebhook(MEETING_CREATED_WEBHOOK);

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
			var parsedEvent = (MeetingDeletedEvent)WebhookParser.ParseEventWebhook(MEETING_DELETED_WEBHOOK);

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
	}
}
