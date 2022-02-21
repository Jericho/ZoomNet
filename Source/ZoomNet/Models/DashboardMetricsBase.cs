using System;
using System.Text.Json.Serialization;

namespace ZoomNet.Models
{
	/// <summary>
	/// Base model for metrics returned by Dashboards client for meetings and webinars.
	/// </summary>
	public class DashboardMetricsBase
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
		/// Gets or sets the topic of the meeting.
		/// </summary>
		/// <value>
		/// The topic.
		/// </value>
		[JsonPropertyName("topic")]
		public string Topic { get; set; }

		/// <summary>
		/// Gets or sets the display name of the meeting host.
		/// </summary>
		/// <value>
		/// The host display name.
		/// </value>
		[JsonPropertyName("host")]
		public string Host { get; set; }

		/// <summary>
		/// Gets or sets the email address of the meeting host.
		/// </summary>
		/// <value>
		/// The host email address.
		/// </value>
		[JsonPropertyName("email")]
		public string Email { get; set; }

		/// <summary>
		/// Gets or sets the license type of the user.
		/// </summary>
		/// <value>
		/// The users license type.
		/// </value>
		[JsonPropertyName("user_type")]
		public string UserType { get; set; }

		/// <summary>
		/// Gets or sets the start time of the meeting.
		/// </summary>
		/// <value>
		/// The meeting start time.
		/// </value>
		[JsonPropertyName("start_time")]
		public DateTime StartTime { get; set; }

		/// <summary>
		/// Gets or sets the end time of the meeting.
		/// </summary>
		/// <value>
		/// The meeting end time.
		/// </value>
		[JsonPropertyName("end_time")]
		public DateTime? EndTime { get; set; }

		/// <summary>
		/// Gets or sets the meeting duration.
		/// </summary>
		/// <value>
		/// The meeting duration.
		/// </value>
		[JsonPropertyName("duration")]
		public string Duration { get; set; }

		/// <summary>
		/// Gets or sets the meeting participant count.
		/// </summary>
		/// <value>
		/// The meeting participant count.
		/// </value>
		[JsonPropertyName("participants")]
		public int ParticipantCount { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether or not the PSTN was used in the meeting.
		/// </summary>
		/// <value>
		/// Indication whether or not the PSTN was used in the meeting.
		/// </value>
		[JsonPropertyName("has_pstn")]
		public bool HasPstn { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether or not VoIP was used in the meeting.
		/// </summary>
		/// <value>
		/// Indication whether or not VoIP was used in the meeting.
		/// </value>
		[JsonPropertyName("has_voip")]
		public bool HasVoip { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether or not <a href="https://support.zoom.us/hc/en-us/articles/202470795-3rd-Party-Audio-Conference">3rd party audio</a> was used in the meeting.
		/// </summary>
		/// <value>
		/// Indication whether or not 3rd Party audio was used.
		/// </value>
		[JsonPropertyName("has_3rd_party_audio")]
		public bool Has3RdPartyAudio { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether or not video was used in the meeting.
		/// </summary>
		/// <value>
		/// Indication whether or not video was used in the meeting.
		/// </value>
		[JsonPropertyName("has_video")]
		public bool HasVideo { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether or not screenshare feature was used in the meeting.
		/// </summary>
		/// <value>
		/// Indication whether or not screenshare feature was used in the meeting.
		/// </value>
		[JsonPropertyName("has_screen_share")]
		public bool HasScreenShare { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether or not the recording feature was used in the meeting.
		/// </summary>
		/// <value>
		/// Indication whether or not the recording feature was used in the meeting.
		/// </value>
		[JsonPropertyName("has_recording")]
		public bool HasRecording { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether or not someone joined the meeting using SIP.
		/// </summary>
		/// <value>
		/// Indication whether or not someone joined the meeting using SIP.
		/// </value>
		[JsonPropertyName("has_sip")]
		public bool HasSip { get; set; }

		/// <summary>
		/// Gets or sets the department of the host.
		/// </summary>
		/// <value>
		/// The department of the host.
		/// </value>
		[JsonPropertyName("dept")]
		public string Department { get; set; }
	}
}
