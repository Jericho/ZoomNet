using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using ZoomNet.Json;

namespace ZoomNet.Models
{
	/// <summary>A meeting.</summary>
	public abstract class Meeting : MeetingBasicInfo
	{
		/// <summary>Gets or sets the meeting description.</summary>
		[JsonPropertyName("agenda")]
		public string Agenda { get; set; }

		/// <summary>Gets or sets the ID of the user who scheduled this meeting on behalf of the host.</summary>
		[JsonPropertyName("assistant_id")]
		public string AssistantId { get; set; }

		/// <summary>Gets or sets the date and time when the meeting was created.</summary>
		[JsonPropertyName("created_at")]
		public DateTime CreatedOn { get; set; }

		/// <summary>Gets or sets the encrypted passcode for third party endpoints (H323/SIP).</summary>
		[JsonPropertyName("encrypted_password")]
		public string EncryptedPassword { get; set; }

		/// <summary>Gets or sets the H.323/SIP room system password.</summary>
		[JsonPropertyName("h323_password")]
		public string H323Password { get; set; }

		/// <summary>Gets or sets the email address of the meeting host.</summary>
		[JsonPropertyName("host_email")]
		public string HostEmail { get; set; }

		/// <summary>Gets or sets the URL to join the meeting.</summary>
		[JsonPropertyName("join_url")]
		public string JoinUrl { get; set; }

		/// <summary>Gets or sets the password to join the meeting. Password may only contain the following characters: [a-z A-Z 0-9 @ - _ *]. Max of 10 characters.</summary>
		[JsonPropertyName("password")]
		public string Password { get; set; }

		/// <summary>Gets or sets the password to join the phone session.</summary>
		[JsonPropertyName("pstn_password")]
		public string PstnPassword { get; set; }

		/// <summary>Gets or Sets the meeting settings.</summary>
		[JsonPropertyName("settings")]
		public MeetingSettings Settings { get; set; }

		/// <summary>Gets or sets the URL for the host to start the meeting.</summary>
		[JsonPropertyName("start_url")]
		public string StartUrl { get; set; }

		/// <summary>Gets or sets the status.</summary>
		[JsonPropertyName("status")]
		public MeetingStatus? Status { get; set; }

		/// <summary>Gets or sets the timezone.</summary>
		[JsonPropertyName("timezone")]
		public TimeZones Timezone { get; set; }

		/// <summary>Gets or sets the tracking fields.</summary>
		[JsonPropertyName("tracking_fields")]
		[JsonConverter(typeof(TrackingFieldsConverter))]
		public KeyValuePair<string, string>[] TrackingFields { get; set; } = Array.Empty<KeyValuePair<string, string>>();

		/// <summary>Gets or sets the meeting type.</summary>
		[JsonPropertyName("type")]
		public MeetingType Type { get; set; }
	}
}
