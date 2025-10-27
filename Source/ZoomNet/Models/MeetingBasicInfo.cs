using System.Text.Json.Serialization;

namespace ZoomNet.Models
{
	/// <summary>
	/// Represents minimal information provided about the meeting.
	/// </summary>
	public class MeetingBasicInfo
	{
		/// <summary>
		/// Gets or sets the meeting id, also known as the meeting number.
		/// </summary>
		[JsonPropertyName("id")]
		/*
			This allows us to overcome the fact that "id" is sometimes a string and sometimes a number
			See: https://devforum.zoom.us/t/the-data-type-of-meetingid-is-inconsistent-in-webhook-documentation/70090
			Also, see: https://github.com/Jericho/ZoomNet/issues/228
		*/
		[JsonNumberHandling(JsonNumberHandling.AllowReadingFromString)]
		public long Id { get; set; }

		/// <summary>
		/// Gets or sets the unique id. Each meeting instance generates a meeting uuid.
		/// </summary>
		[JsonPropertyName("uuid")]
		public string Uuid { get; set; }

		/// <summary>
		/// Gets or sets the ID of the user who is set as the host of the meeting.
		/// </summary>
		[JsonPropertyName("host_id")]
		public string HostId { get; set; }

		/// <summary>
		/// Gets or sets the topic of the meeting.
		/// </summary>
		[JsonPropertyName("topic")]
		public string Topic { get; set; }
	}
}
