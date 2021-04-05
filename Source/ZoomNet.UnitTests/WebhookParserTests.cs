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
				'operator_id': '9lzIwvZTSOqjndWPbPqzuA',
				'object': {
					'uuid': '6yDZGNlQSV7qOjg5NxajHQ==',
					'id': 99995864943,
					'host_id': '9lzIwvZTSOqjndWPbPqzuA',
					'topic': 'ZoomNet Unit Testing: instant meeting',
					'type': 1,
					'duration': 60,
					'timezone': 'America/New_York',
					'join_url': 'https://zoom.us/j/99995864943?pwd=c21EQzg0SXY2dlNTOFF2TENpSm1aQT09',
					'password': 'PaSsWoRd',
					'settings': {
						'use_pmi': false,
						'alternative_hosts': ''
					}
				}
			},
			'event_ts': 1617628462392
		}";

		#endregion

		[Fact]
		public void MeetingCreated()
		{
			var meetingCreatedEvent = (MeetingCreatedEvent)WebhookParser.ParseEventWebhook(MEETING_CREATED_WEBHOOK);

			meetingCreatedEvent.EventType.ShouldBe(EventType.MeetingCreated);
			meetingCreatedEvent.Operator.ShouldBe("someone@example.com");
			meetingCreatedEvent.OperatorId.ShouldBe("9lzIwvZTSOqjndWPbPqzuA");
			meetingCreatedEvent.Meeting.ShouldNotBeNull();
			meetingCreatedEvent.Meeting.Uuid.ShouldBe("6yDZGNlQSV7qOjg5NxajHQ==");
			meetingCreatedEvent.Meeting.Id.ShouldBe(99995864943);
			meetingCreatedEvent.Meeting.HostId.ShouldBe("9lzIwvZTSOqjndWPbPqzuA");
			meetingCreatedEvent.Meeting.Topic.ShouldBe("ZoomNet Unit Testing: instant meeting");
			meetingCreatedEvent.Meeting.Type.ShouldBe(Models.MeetingType.Instant);
			meetingCreatedEvent.Meeting.JoinUrl.ShouldBe("https://zoom.us/j/99995864943?pwd=c21EQzg0SXY2dlNTOFF2TENpSm1aQT09");
			meetingCreatedEvent.Meeting.Password.ShouldBe("PaSsWoRd");
			meetingCreatedEvent.Meeting.Settings.ShouldNotBeNull();
			meetingCreatedEvent.Meeting.Settings.UsePmi.HasValue.ShouldBeTrue();
			meetingCreatedEvent.Meeting.Settings.UsePmi.Value.ShouldBeFalse();
			meetingCreatedEvent.Meeting.Settings.AlternativeHosts.ShouldBeEmpty();
		}
	}
}
