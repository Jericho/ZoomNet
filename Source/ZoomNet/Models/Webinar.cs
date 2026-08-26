using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using ZoomNet.Json;

namespace ZoomNet.Models
{
	/// <summary>A webinar.</summary>
	public abstract class Webinar
	{
		/// <summary>Gets or sets the webinar agenda.</summary>
		[JsonPropertyName("agenda")]
		public string Agenda { get; set; }

		/// <summary>Gets or sets the date and time when the webinar was created.</summary>
		[JsonPropertyName("created_at")]
		public DateTime CreatedOn { get; set; }

		/// <summary>Gets or sets how the webinar was created.</summary>
		[JsonPropertyName("creation_source")]
		public MeetingCreationSource CreationSource { get; set; }

		/// <summary>Gets or sets the duration in minutes.</summary>
		[JsonPropertyName("duration")]
		public int Duration { get; set; }

		/// <summary>Gets or sets the encrypted passcode for third party endpoints (H.323/SIP).</summary>
		[JsonPropertyName("encrypted_passcode")]
		public string EncryptedPasscode { get; set; }

		/// <summary>Gets or sets the H.323/SIP room system passcode.</summary>
		[JsonPropertyName("h323_passcode")]
		public string H323Passcode { get; set; }

		/// <summary>Gets or sets the email address of the host of the webinar.</summary>
		[JsonPropertyName("host_email")]
		public string HostEmail { get; set; }

		/// <summary>Gets or sets the ID of the user who is set as the host of the webinar.</summary>
		[JsonPropertyName("host_id")]
		public string HostId { get; set; }

		/// <summary>Gets or sets the webinar id, also known as the webinar number.</summary>
		[JsonPropertyName("id")]
		/*
			This allows us to overcome the fact that "id" is sometimes a string and sometimes a number
			See: https://devforum.zoom.us/t/the-data-type-of-meetingid-is-inconsistent-in-webhook-documentation/70090
			Also, see: https://github.com/Jericho/ZoomNet/issues/228
		*/
		[JsonNumberHandling(JsonNumberHandling.AllowReadingFromString)]
		public long Id { get; set; }

		/// <summary>Gets or sets a value indicating whether the webinar is simulive.</summary>
		[JsonPropertyName("is_simulive")]
		public bool? IsSimulive { get; set; }

		/// <summary>Gets or sets the URL to join the webinar.</summary>
		[JsonPropertyName("join_url")]
		public string JoinUrl { get; set; }

		/// <summary>Gets or sets the webinar password.</summary>
		[JsonPropertyName("password")]
		public string Password { get; set; }

		/// <summary>Gets or sets the record file ID for simulive webinars.</summary>
		[JsonPropertyName("record_file_id")]
		public string RecordFileId { get; set; }

		/// <summary>Gets or sets the webinar registration URL.</summary>
		/// <remarks>This field is only returned for webinars that have enabled registration.</remarks>
		[JsonPropertyName("registration_url")]
		public string RegistrationUrl { get; set; }

		/// <summary>Gets or sets the webinar settings.</summary>
		[JsonPropertyName("settings")]
		public WebinarSettings Settings { get; set; }

		/// <summary>Gets or sets the simulive delay start settings.</summary>
		[JsonPropertyName("simulive_delay_start")]
		public SimuliveDelayStart SimuliveDelayStart { get; set; }

		/// <summary>Gets or sets the URL for the host to start the webinar.</summary>
		[JsonPropertyName("start_url")]
		public string StartUrl { get; set; }

		/// <summary>Gets or sets the webinar template ID.</summary>
		[JsonPropertyName("template_id")]
		public string TemplateId { get; set; }

		/// <summary>Gets or sets the topic of the webinar.</summary>
		[JsonPropertyName("topic")]
		public string Topic { get; set; }

		/// <summary>Gets or sets the tracking fields.</summary>
		[JsonPropertyName("tracking_fields")]
		[JsonConverter(typeof(TrackingFieldsConverter))]
		public KeyValuePair<string, string>[] TrackingFields { get; set; } = Array.Empty<KeyValuePair<string, string>>();

		/// <summary>Gets or sets a value indicating whether to transition to a live webinar after the simulive webinar ends.</summary>
		/// <remarks>The host must be present at the time of transition.</remarks>
		[JsonPropertyName("transition_to_live")]
		public bool? TransitionToLive { get; set; }

		/// <summary>Gets or sets the webinar type.</summary>
		[JsonPropertyName("type")]
		public WebinarType Type { get; set; }

		/// <summary>Gets or sets the unique id.</summary>
		[JsonPropertyName("uuid")]
		public string Uuid { get; set; }
	}
}
