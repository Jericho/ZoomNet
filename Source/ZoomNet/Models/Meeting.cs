using Newtonsoft.Json;
using ZoomNet.Models;
using System;

namespace ZoomNet.Models
{
	/// <summary>
	/// A meeting.
	/// </summary>
	public abstract class Meeting
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
		/// Gets or sets the status.
		/// </summary>
		/// <value>
		/// The status.
		/// </value>
		[JsonProperty("status", NullValueHandling = NullValueHandling.Ignore)]
		public MeetingStatus Status { get; set; }

		/// <summary>
		/// Gets or sets the meeting description.
		/// </summary>
		/// <value>Meeting description.</value>
		[JsonProperty(PropertyName = "agenda", NullValueHandling = NullValueHandling.Ignore)]
		public string Agenda { get; set; }

		/// <summary>
		/// Gets or sets the date and time when the meeting was created.
		/// </summary>
		/// <value>The meeting created time.</value>
		[JsonProperty(PropertyName = "created_at", NullValueHandling = NullValueHandling.Ignore)]
		public DateTime CreatedOn { get; set; }

		/// <summary>
		/// Gets or sets the URL for the host to start the meeting.
		/// </summary>
		/// <value>The start URL.</value>
		[JsonProperty(PropertyName = "start_url", NullValueHandling = NullValueHandling.Ignore)]
		public string StartUrl { get; set; }

		/// <summary>
		/// Gets or sets the URL to join the meeting.
		/// </summary>
		/// <value>The join URL.</value>
		[JsonProperty(PropertyName = "join_url", NullValueHandling = NullValueHandling.Ignore)]
		public string JoinUrl { get; set; }

		/// <summary>
		/// Gets or sets the password to join the meeting.
		/// Password may only contain the following characters: [a-z A-Z 0-9 @ - _ *].
		/// Max of 10 characters.
		/// </summary>
		/// <value>Password to join the meeting. Password may only contain the following characters: [a-z A-Z 0-9 @ - _ *]. Max of 10 characters.</value>
		[JsonProperty(PropertyName = "password", NullValueHandling = NullValueHandling.Ignore)]
		public string Password { get; set; }

		/// <summary>
		/// Gets or sets the H.323/SIP room system password.
		/// </summary>
		/// <value>
		/// The h.323 password.
		/// </value>
		[JsonProperty("h323_password", NullValueHandling = NullValueHandling.Ignore)]
		public string H323Password { get; set; }

		/// <summary>
		/// Gets or sets the password to join the phone session.
		/// </summary>
		/// <value>
		/// The pstn password.
		/// </value>
		[JsonProperty("pstn_password", NullValueHandling = NullValueHandling.Ignore)]
		public string PstnPassword { get; set; }

		/// <summary>
		/// Gets or Sets the meeting settings.
		/// </summary>
		[JsonProperty(PropertyName = "settings", NullValueHandling = NullValueHandling.Ignore)]
		public MeetingSettings Settings { get; set; }
	}
}
