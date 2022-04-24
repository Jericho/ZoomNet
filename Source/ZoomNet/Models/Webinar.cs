using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using ZoomNet.Json;

namespace ZoomNet.Models
{
	/// <summary>
	/// A webinar.
	/// </summary>
	public abstract class Webinar
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
		/// Gets or sets the webinar id, also known as the webinar number.
		/// </summary>
		/// <value>
		/// The id.
		/// </value>
		[JsonPropertyName("id")]
		public long Id { get; set; }

		/// <summary>
		/// Gets or sets the ID of the user who is set as the host of the webinar.
		/// </summary>
		/// <value>
		/// The user id.
		/// </value>
		[JsonPropertyName("host_id")]
		public string HostId { get; set; }

		/// <summary>
		/// Gets or sets the email address of the host of the webinar.
		/// </summary>
		/// <value>
		/// The user id.
		/// </value>
		[JsonPropertyName("host_email")]
		public string HostEmail { get; set; }

		/// <summary>
		/// Gets or sets the topic of the meeting.
		/// </summary>
		/// <value>
		/// The topic.
		/// </value>
		[JsonPropertyName("topic")]
		public string Topic { get; set; }

		/// <summary>
		/// Gets or sets the webinar type.
		/// </summary>
		/// <value>The webinar type.</value>
		[JsonPropertyName("type")]
		public WebinarType Type { get; set; }

		/// <summary>
		/// Gets or sets the duration in minutes.
		/// </summary>
		/// <value>The duration in minutes.</value>
		[JsonPropertyName("duration")]
		public int Duration { get; set; }

		/// <summary>
		/// Gets or sets the webinar agenda.
		/// </summary>
		/// <value>The agenda.</value>
		[JsonPropertyName("agenda")]
		public string Agenda { get; set; }

		/// <summary>
		/// Gets or sets the date and time when the meeting was created.
		/// </summary>
		/// <value>The meeting created time.</value>
		[JsonPropertyName("created_at")]
		public DateTime CreatedOn { get; set; }

		/// <summary>
		/// Gets or sets the URL to join the webinar.
		/// </summary>
		/// <value>The join URL.</value>
		[JsonPropertyName("join_url")]
		public string JoinUrl { get; set; }

		/// <summary>
		/// Gets or sets the URL for the host to start the meeting.
		/// </summary>
		/// <value>The start URL.</value>
		[JsonPropertyName("start_url")]
		public string StartUrl { get; set; }

		/// <summary>
		/// Gets or sets the tracking fields.
		/// </summary>
		/// <value>The tracking fields.</value>
		[JsonPropertyName("tracking_fields")]
		[JsonConverter(typeof(TrackingFieldsConverter))]
		public KeyValuePair<string, string>[] TrackingFields { get; set; }

		/// <summary>
		/// Gets or sets the webinar settings.
		/// </summary>
		/// <value>The webinar settings.</value>
		[JsonPropertyName("settings")]
		public WebinarSettings Settings { get; set; }

		/// <summary>
		/// Gets or sets the webinar password.
		/// </summary>
		/// <value>The password.</value>
		[JsonPropertyName("password")]
		public string Password { get; set; }
	}
}
