using Newtonsoft.Json;
using System;

namespace ZoomNet.Models
{
	/// <summary>
	/// Metrics of a Meeting.
	/// </summary>
	public class DashboardMeeting
	{
		/// <summary>
		/// Gets or sets the unique id.
		/// </summary>
		/// <value>
		/// The unique id.
		/// </value>
		[JsonProperty(PropertyName = "uuid")]
		public string Uuid { get; set; }

		/// <summary>
		/// Gets or sets the meeting id, also known as the meeting number.
		/// </summary>
		/// <value>
		/// The id.
		/// </value>
		[JsonProperty(PropertyName = "id")]
		public long Id { get; set; }

		/// <summary>
		/// Gets or sets the topic of the meeting.
		/// </summary>
		/// <value>
		/// The topic.
		/// </value>
		[JsonProperty(PropertyName = "topic")]
		public string Topic { get; set; }

		/// <summary>
		/// Gets or sets the display name of the meeting host.
		/// </summary>
		/// <value>
		/// The host display name.
		/// </value>
		[JsonProperty(PropertyName = "host")]
		public string Host { get; set; }

		/// <summary>
		/// Gets or sets the email address of the meeting host.
		/// </summary>
		/// <value>
		/// The host email address.
		/// </value>
		[JsonProperty(PropertyName = "email")]
		public string Email { get; set; }

		/// <summary>
		/// Gets or sets the license type of the user.
		/// </summary>
		/// <value>
		/// The users license type.
		/// </value>
		[JsonProperty(PropertyName = "user_type")]
		public string UserType { get; set; }

		/// <summary>
		/// Gets or sets the start time of the meeting.
		/// </summary>
		/// <value>
		/// The meeting start time.
		/// </value>
		[JsonProperty(PropertyName = "start_time")]
		public DateTime StartTime { get; set; }

		/// <summary>
		/// Gets or sets the end time of the meeting.
		/// </summary>
		/// <value>
		/// The meeting end time.
		/// </value>
		[JsonProperty(PropertyName = "end_time")]
		public DateTime EndTime { get; set; }

		/// <summary>
		/// Gets or sets the meeting duration.
		/// </summary>
		/// <value>
		/// The meeting duration.
		/// </value>
		[JsonProperty(PropertyName = "duration")]
		public string Duration { get; set; }

		/// <summary>
		/// Gets or sets the meeting participant count.
		/// </summary>
		/// <value>
		/// The meeting participant count.
		/// </value>
		[JsonProperty(PropertyName = "participants")]
		public int ParticipantCount { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether or not the PSTN was used in the meeting.
		/// </summary>
		/// <value>
		/// Indication whether or not the PSTN was used in the meeting.
		/// </value>
		[JsonProperty(PropertyName = "has_pstn")]
		public bool HasPstn { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether or not VoIP was used in the meeting.
		/// </summary>
		/// <value>
		/// Indication whether or not VoIP was used in the meeting.
		/// </value>
		[JsonProperty(PropertyName = "has_voip")]
		public bool HasVoip { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether or not <a href="https://support.zoom.us/hc/en-us/articles/202470795-3rd-Party-Audio-Conference">3rd party audio</a> was used in the meeting.
		/// </summary>
		/// <value>
		/// Indication whether or not 3rd Party audio was used.
		/// </value>
		[JsonProperty(PropertyName = "has_3rd_party_audio")]
		public bool Has3RdPartyAudio { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether or not video was used in the meeting.
		/// </summary>
		/// <value>
		/// Indication whether or not video was used in the meeting.
		/// </value>
		[JsonProperty(PropertyName = "has_video")]
		public bool HasVideo { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether or not screenshare feature was used in the meeting.
		/// </summary>
		/// <value>
		/// Indication whether or not screenshare feature was used in the meeting.
		/// </value>
		[JsonProperty(PropertyName = "has_screen_share")]
		public bool HasScreenShare { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether or not the recording feature was used in the meeting.
		/// </summary>
		/// <value>
		/// Indication whether or not the recording feature was used in the meeting.
		/// </value>
		[JsonProperty(PropertyName = "has_recording")]
		public bool HasRecording { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether or not someone joined the meeting using SIP.
		/// </summary>
		/// <value>
		/// Indication whether or not someone joined the meeting using SIP.
		/// </value>
		[JsonProperty(PropertyName = "has_sip")]
		public bool HasSip { get; set; }

		/// <summary>
		/// Gets or sets the number of Zoom Room participants in the meeting.
		/// </summary>
		/// <value>
		/// The number of Zoom Room participants in the meeting.
		/// </value>
		[JsonProperty(PropertyName = "in_room_participants")]
		public int InRoomParticipants { get; set; }

		/// <summary>
		/// Gets or sets the department of the host.
		/// </summary>
		/// <value>
		/// The department of the host.
		/// </value>
		[JsonProperty(PropertyName = "dept")]
		public string Department { get; set; }
	}
}
