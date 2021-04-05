using Newtonsoft.Json;
using ZoomNet.Utilities;

namespace ZoomNet.Models.Webhooks
{
	/// <summary>
	/// This event is triggered when a meeting is deleted.
	/// </summary>
	public class MeetingDeletedEvent : Event
	{
		/// <summary>
		/// Gets or sets the email address of the user who deleted the meeting.
		/// </summary>
		[JsonProperty(PropertyName = "operator")]
		public string Operator { get; set; }

		/// <summary>
		/// Gets or sets the user ID of the operator who deleted the meeting.
		/// </summary>
		[JsonProperty(PropertyName = "operator_id")]
		public string OperatorId { get; set; }

		/// <summary>
		/// Gets or sets the meeting object.
		/// </summary>
		[JsonProperty(PropertyName = "object")]
		[JsonConverter(typeof(MeetingConverter))]
		public Meeting Meeting { get; set; }

		//"object": {
		//  "type": "object",
		//  "description": "Meeting object",
		//  "properties": {
		//    "id": {
		//      "type": "integer",
		//      "description": "Unique identifier of the meeting in \"long\" format(represented as int64 data type in JSON), also known as the meeting number.",
		//      "format": "int64"
		//    },
		//    "uuid": {
		//      "type": "string",
		//      "description": "Unique Meeting ID. Each meeting instance will generate its own meeting UUID."
		//    },
		//    "host_id": {
		//      "type": "string",
		//      "description": "ID of the user who is set as the host of the meeting."
		//    },
		//    "topic": {
		//      "type": "string",
		//      "description": "Meeting Topic"
		//    },
		//    "type": {
		//      "description": "Meeting Types:<br> `1` - Instant Meeting<br> `2` - Scheduled Meeting<br> `3` - Recurring Meeting with no fixed time.<br>`4` - Personal Meeting. In `meeting_created` event, type`4` is only returned when PMI is assigned to a user for the first time as that generates a Meeting URL with Personal Meeting ID.<br>\n<br>`8` - Recurring Meeting with a fixed time. ",
		//      "type": "integer"
		//    },
		//    "start_time": {
		//      "type": "string",
		//      "description": "Meeting start time.",
		//      "format": "date-time"
		//    },
		//    "timezone": {
		//      "type": "string",
		//      "description": "Timezone to format the meeting start time."
		//    },
		//    "duration": {
		//      "description": "Scheduled meeting duration.",
		//      "type": "integer"
		//    },
		//    "occurrences": {
		//      "type": "array",
		//      "description": "Array of occurence objects. Only returned for meeting `type` with a value of 8, i.e. if the meeting is a recurrence meeting with fixed time.",
		//      "items": {
		//        "type": "object",
		//        "properties": {
		//          "occurrence_id": {
		//            "description": "Occurrence ID",
		//            "type": "string"
		//          },
		//          "start_time": {
		//            "type": "string",
		//            "description": "Start time",
		//            "format": "date-time"
		//          }
		//        }
		//      }
		//    },
		//    "password": {
		//      "type": "string",
		//      "description": "Meeting password."
		//    }
		//  }
		//},
	}
}
