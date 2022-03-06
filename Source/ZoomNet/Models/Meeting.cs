using System;
using System.Text.Json.Serialization;

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
		[JsonPropertyName("uuid")]
		public string Uuid { get; set; }

		/// <summary>
		/// Gets or sets the meeting id, also known as the meeting number.
		/// </summary>
		/// <value>
		/// The id.
		/// </value>
		[JsonPropertyName("id")]
		public long Id { get; set; }

		/// <summary>
		/// Gets or sets the ID of the user who is set as the host of the meeting.
		/// </summary>
		/// <value>
		/// The user id.
		/// </value>
		[JsonPropertyName("host_id")]
		public string HostId { get; set; }

		/// <summary>
		/// Gets or sets the topic of the meeting.
		/// </summary>
		/// <value>
		/// The topic.
		/// </value>
		[JsonPropertyName("topic")]
		public string Topic { get; set; }

		/// <summary>
		/// Gets or sets the meeting type.
		/// </summary>
		/// <value>The meeting type.</value>
		[JsonPropertyName("type")]
		public MeetingType Type { get; set; }

		/// <summary>
		/// Gets or sets the status.
		/// </summary>
		/// <value>
		/// The status.
		/// </value>
		[JsonPropertyName("status")]
		public MeetingStatus? Status { get; set; }

		/// <summary>
		/// Gets or sets the meeting description.
		/// </summary>
		/// <value>Meeting description.</value>
		[JsonPropertyName("agenda")]
		public string Agenda { get; set; }

		/// <summary>
		/// Gets or sets the date and time when the meeting was created.
		/// </summary>
		/// <value>The meeting created time.</value>
		[JsonPropertyName("created_at")]
		public DateTime CreatedOn { get; set; }

		/// <summary>
		/// Gets or sets the URL for the host to start the meeting.
		/// </summary>
		/// <value>The start URL.</value>
		[JsonPropertyName("start_url")]
		public string StartUrl { get; set; }

		/// <summary>
		/// Gets or sets the URL to join the meeting.
		/// </summary>
		/// <value>The join URL.</value>
		[JsonPropertyName("join_url")]
		public string JoinUrl { get; set; }

		/// <summary>
		/// Gets or sets the password to join the meeting.
		/// Password may only contain the following characters: [a-z A-Z 0-9 @ - _ *].
		/// Max of 10 characters.
		/// </summary>
		/// <value>Password to join the meeting. Password may only contain the following characters: [a-z A-Z 0-9 @ - _ *]. Max of 10 characters.</value>
		[JsonPropertyName("password")]
		public string Password { get; set; }

		/// <summary>
		/// Gets or sets the H.323/SIP room system password.
		/// </summary>
		/// <value>
		/// The h.323 password.
		/// </value>
		[JsonPropertyName("h323_password")]
		public string H323Password { get; set; }

		/// <summary>
		/// Gets or sets the password to join the phone session.
		/// </summary>
		/// <value>
		/// The pstn password.
		/// </value>
		[JsonPropertyName("pstn_password")]
		public string PstnPassword { get; set; }

		/// <summary>
		/// Gets or Sets the meeting settings.
		/// </summary>
		[JsonPropertyName("settings")]
		public MeetingSettings Settings { get; set; }
	}
}
