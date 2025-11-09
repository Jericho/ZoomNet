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
		[JsonPropertyName("uuid")]
		public string Uuid { get; set; }

		/// <summary>
		/// Gets or sets the webinar id, also known as the webinar number.
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
		/// Gets or sets the ID of the user who is set as the host of the webinar.
		/// </summary>
		[JsonPropertyName("host_id")]
		public string HostId { get; set; }

		/// <summary>
		/// Gets or sets the email address of the host of the webinar.
		/// </summary>
		[JsonPropertyName("host_email")]
		public string HostEmail { get; set; }

		/// <summary>
		/// Gets or sets the topic of the webinar.
		/// </summary>
		[JsonPropertyName("topic")]
		public string Topic { get; set; }

		/// <summary>
		/// Gets or sets the webinar type.
		/// </summary>
		[JsonPropertyName("type")]
		public WebinarType Type { get; set; }

		/// <summary>
		/// Gets or sets the duration in minutes.
		/// </summary>
		[JsonPropertyName("duration")]
		public int Duration { get; set; }

		/// <summary>
		/// Gets or sets the webinar agenda.
		/// </summary>
		[JsonPropertyName("agenda")]
		public string Agenda { get; set; }

		/// <summary>
		/// Gets or sets the date and time when the webinar was created.
		/// </summary>
		[JsonPropertyName("created_at")]
		public DateTime CreatedOn { get; set; }

		/// <summary>
		/// Gets or sets the URL to join the webinar.
		/// </summary>
		[JsonPropertyName("join_url")]
		public string JoinUrl { get; set; }

		/// <summary>
		/// Gets or sets the URL for the host to start the webinar.
		/// </summary>
		[JsonPropertyName("start_url")]
		public string StartUrl { get; set; }

		/// <summary>
		/// Gets or sets the tracking fields.
		/// </summary>
		[JsonPropertyName("tracking_fields")]
		[JsonConverter(typeof(TrackingFieldsConverter))]
		public KeyValuePair<string, string>[] TrackingFields { get; set; } = Array.Empty<KeyValuePair<string, string>>();

		/// <summary>
		/// Gets or sets the webinar settings.
		/// </summary>
		[JsonPropertyName("settings")]
		public WebinarSettings Settings { get; set; }

		/// <summary>
		/// Gets or sets the webinar password.
		/// </summary>
		[JsonPropertyName("password")]
		public string Password { get; set; }
	}
}
