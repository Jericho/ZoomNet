using Newtonsoft.Json;
using System;

namespace ZoomNet.Models
{
	/// <summary>
	/// A meeting that occured in the past.
	/// </summary>
	public class PastMeeting
	{
		/// <summary>
		/// Gets or sets the unique id.
		/// </summary>
		/// <value>
		/// The unique id.
		/// </value>
		[JsonProperty("uuid", NullValueHandling = NullValueHandling.Ignore)]
		public string Uuid { get; set; }

		/// <summary>
		/// Gets or sets the meeting id, also known as the meeting number.
		/// </summary>
		/// <value>
		/// The id.
		/// </value>
		[JsonProperty("id", NullValueHandling = NullValueHandling.Ignore)]
		public long Id { get; set; }

		/// <summary>
		/// Gets or sets the ID of the user who is set as the host of the meeting.
		/// </summary>
		/// <value>
		/// The user id.
		/// </value>
		[JsonProperty("host_id", NullValueHandling = NullValueHandling.Ignore)]
		public string HostId { get; set; }

		/// <summary>
		/// Gets or sets the topic of the meeting.
		/// </summary>
		/// <value>
		/// The topic.
		/// </value>
		[JsonProperty("topic", NullValueHandling = NullValueHandling.Ignore)]
		public string Topic { get; set; }

		/// <summary>
		/// Gets or sets the meeting type.
		/// </summary>
		/// <value>The meeting type.</value>
		[JsonProperty(PropertyName = "type", NullValueHandling = NullValueHandling.Ignore)]
		public MeetingType Type { get; set; }

		/// <summary>
		/// Gets or sets the user display name.
		/// </summary>
		/// <value>The user display name.</value>
		[JsonProperty(PropertyName = "user_name", NullValueHandling = NullValueHandling.Ignore)]
		public string UserName { get; set; }

		/// <summary>
		/// Gets or sets the user email.
		/// </summary>
		/// <value>The user email.</value>
		[JsonProperty(PropertyName = "user_email", NullValueHandling = NullValueHandling.Ignore)]
		public string UserEmail { get; set; }

		/// <summary>
		/// Gets or sets the date and time when the meeting started.
		/// </summary>
		/// <value>The meeting start time.</value>
		[JsonProperty(PropertyName = "start_time", NullValueHandling = NullValueHandling.Ignore)]
		public DateTime StartedOn { get; set; }

		/// <summary>
		/// Gets or sets the date and time when the meeting ended.
		/// </summary>
		/// <value>The meeting end time.</value>
		[JsonProperty(PropertyName = "end_time", NullValueHandling = NullValueHandling.Ignore)]
		public DateTime EndedOn { get; set; }

		/// <summary>
		/// Gets or sets the meeting duration in minutes.
		/// </summary>
		/// <value>The meeting duration.</value>
		[JsonProperty(PropertyName = "duration", NullValueHandling = NullValueHandling.Ignore)]
		public long Duration { get; set; }

		/// <summary>
		/// Gets or sets the sum of meeting minutes from all participants.
		/// </summary>
		/// <value>The total meeting minutes.</value>
		[JsonProperty(PropertyName = "total_minutes", NullValueHandling = NullValueHandling.Ignore)]
		public long TotalMinutes { get; set; }

		/// <summary>
		/// Gets or sets the number of participants.
		/// </summary>
		/// <value>The number of participants.</value>
		[JsonProperty(PropertyName = "participants_count", NullValueHandling = NullValueHandling.Ignore)]
		public long ParticipantsCount { get; set; }
	}
}
