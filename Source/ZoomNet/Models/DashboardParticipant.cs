using System;
using System.Text.Json.Serialization;

namespace ZoomNet.Models
{
	/// <summary>
	/// Metrics of a participant.
	/// </summary>
	public class DashboardParticipant
	{
		/// <summary>
		/// Gets or sets the Universally unique identifier of the participant.
		/// </summary>
		/// <value>
		/// The Universally unique identifier of the participant.<br/>
		/// It is the same as the User ID of the participant if the participant joins the meeting by logging into Zoom<br/>
		/// If the participant joins the meeting without logging in the value of this field will be blank.
		/// </value>
		[JsonPropertyName("id")]
		public string Id { get; set; }

		/// <summary>
		/// Gets or sets the participant ID.
		/// </summary>
		/// <value>
		/// The participant ID.
		/// </value>
		[JsonPropertyName("user_id")]
		public string UserId { get; set; }

		/// <summary>
		/// Gets or sets the participant display name.
		/// </summary>
		/// <value>
		/// The participant display name.
		/// </value>
		[JsonPropertyName("user_name")]
		public string UserName { get; set; }

		/// <summary>
		/// Gets or sets the device(s) used by the participant to join the meeting.
		/// </summary>
		/// <value>
		/// The type of device used by the participant to join the meeting.
		/// </value>
		[JsonPropertyName("device")]
		public string Devices { get; set; }

		/// <summary>
		/// Gets or sets the device operation system.
		/// </summary>
		[JsonPropertyName("os")]
		public string OperatingSystem { get; set; }

		/// <summary>
		/// Gets or sets the device operation system version.
		/// </summary>
		[JsonPropertyName("os_version")]
		public string OperatingSystemVersion { get; set; }

		/// <summary>
		/// Gets or sets the participant's IP address.
		/// </summary>
		/// <value>
		/// The participant's IP address.
		/// </value>
		[JsonPropertyName("ip_address")]
		public string IpAddress { get; set; }

		/// <summary>
		/// Gets or sets the participant's location.
		/// </summary>
		/// <value>
		/// The participant's location.
		/// </value>
		[JsonPropertyName("location")]
		public string Location { get; set; }

		/// <summary>
		/// Gets or sets the participant's network type.
		/// </summary>
		/// <value>
		/// The participant's network type.
		/// </value>
		[JsonPropertyName("network_type")]
		public ParticipantNetwork NetworkType { get; set; }

		/// <summary>
		/// Gets or sets the type of microphone that participant used during the meeting.
		/// </summary>
		/// <value>
		/// The type of microphone that participant used during the meeting.
		/// </value>
		[JsonPropertyName("microphone")]
		public string Microphone { get; set; }

		/// <summary>
		/// Gets or sets the type of speaker participant used during the meeting.
		/// </summary>
		/// <value>
		/// The type of speaker participant used during the meeting.
		/// </value>
		[JsonPropertyName("speaker")]
		public string Speaker { get; set; }

		/// <summary>
		/// Gets or sets the data center where participant's meeting data is stored.
		/// </summary>
		/// <value>
		/// The data center where participant's meeting data is stored.
		/// </value>
		[JsonPropertyName("data_center")]
		public string DataCenter { get; set; }

		/// <summary>
		/// Gets or sets the data center where participant's meeting data is stored.
		/// This field includes a semicolon-separated list of HTTP Tunnel (HT), Cloud Room Connector (CRC), and Real-Time Web Gateway (RWG) location information.
		/// </summary>
		[JsonPropertyName("full_data_center")]
		public string DataCenterFullName { get; set; }

		/// <summary>
		/// Gets or sets the participant connection type.
		/// </summary>
		/// <value>
		/// The participant connection type.
		/// </value>
		[JsonPropertyName("connection_type")]
		public string ConnectionType { get; set; }

		/// <summary>
		/// Gets or sets the time at which participant joined the meeting.
		/// </summary>
		/// <value>
		/// The time at which participant joined the meeting.
		/// </value>
		[JsonPropertyName("join_time")]
		public DateTime JoinTime { get; set; }

		/// <summary>
		/// Gets or sets the time at which a participant left the meeting.
		/// </summary>
		/// <value>
		/// The time at which a participant left the meeting. For live meetings this field will only be returned if a participant has left the ongoing meeting.
		/// </value>
		[JsonPropertyName("leave_time")]
		public DateTime? LeaveTime { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether or not a user selected to share an iPhone/iPad application during the screen-share.
		/// </summary>
		/// <value>
		/// Indicates whether or not a user selected to share an iPhone/iPad application during the screen-share.
		/// </value>
		[JsonPropertyName("share_application")]
		public bool ShareApplication { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether or not a user selected to share their desktop during the screen-share.
		/// </summary>
		/// <value>
		/// Indicates whether or not a user selected to share their desktop during the screen-share.
		/// </value>
		[JsonPropertyName("share_desktop")]
		public bool ShareDesktop { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether or not a user selected to share their white-board during the screen-share.
		/// </summary>
		/// <value>
		/// Indicates whether or not a user selected to share their white-board during the screen-share.
		/// </value>
		[JsonPropertyName("share_whiteboard")]
		public bool ShareWhiteboard { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether or not recording was used during the meeting.
		/// </summary>
		/// <value>
		/// Indicates whether or not recording was used during the meeting.
		/// </value>
		[JsonPropertyName("recording")]
		public bool Recording { get; set; }

		/// <summary>
		/// Gets or sets the name of participant's PC.
		/// </summary>
		/// <value>
		/// The name of participant's PC.
		/// </value>
		[JsonPropertyName("pc_name")]
		public string PcName { get; set; }

		/// <summary>
		/// Gets or sets the participant's PC domain.
		/// </summary>
		/// <value>
		/// The participant's PC domain.
		/// </value>
		[JsonPropertyName("domain")]
		public string Domain { get; set; }

		/// <summary>
		/// Gets or sets the participant's MAC address.
		/// </summary>
		/// <value>
		/// The participant's MAC address.
		/// </value>
		[JsonPropertyName("mac_addr")]
		public string MacAddress { get; set; }

		/// <summary>
		/// Gets or sets the participant's hard disk ID.
		/// </summary>
		/// <value>
		/// The participant's hard disk ID.
		/// </value>
		[JsonPropertyName("harddisk_id")]
		public string HardDiskId { get; set; }

		/// <summary>
		/// Gets or sets the participant's Zoom Client version.
		/// </summary>
		/// <value>
		/// The participant's Zoom Client version.
		/// </value>
		[JsonPropertyName("version")]
		public string Version { get; set; }

		/// <summary>
		/// Gets or sets the Possible reasons for why participant left the meeting.
		/// </summary>
		/// <value>
		/// The Possible reasons for why participant left the meeting.
		/// </value>
		[JsonPropertyName("leave_reason")]
		public string LeaveReason { get; set; }

		/// <summary>
		/// Gets or sets the Email address of the participant.
		/// </summary>
		/// <value>
		/// The Email address of the participant.
		/// </value>
		[JsonPropertyName("email")]
		public string Email { get; set; }

		/// <summary>
		/// Gets or sets the participant's unique registrant ID.
		/// </summary>
		/// <remarks>
		/// This field does not return if the type query parameter is the live value.
		/// </remarks>
		[JsonPropertyName("registrant_id")]
		public string RegistrantId { get; set; }

		/// <summary>
		/// Gets or sets the participant's audio quality.
		/// </summary>
		/// <remarks>
		/// The API only returns this value when the Meeting quality scores and network alerts
		/// on Dashboard setting is enabled in the Zoom Web Portal and the Show meeting quality
		/// score and network alerts on Dashboard option is selected in Account Settings.
		/// </remarks>
		[JsonPropertyName("audio_quality")]
		public QualityType AudioQuality { get; set; }

		/// <summary>
		/// Gets or sets the participant's video quality.
		/// </summary>
		/// <remarks>
		/// The API only returns this value when the Meeting quality scores and network alerts
		/// on Dashboard setting is enabled in the Zoom Web Portal and the Show meeting quality
		/// score and network alerts on Dashboard option is selected in Account Settings.
		/// </remarks>
		[JsonPropertyName("video_quality")]
		public QualityType VideoQuality { get; set; }

		/// <summary>
		/// Gets or sets the participant's screen share quality.
		/// </summary>
		/// <remarks>
		/// The API only returns this value when the Meeting quality scores and network alerts
		/// on Dashboard setting is enabled in the Zoom Web Portal and the Show meeting quality
		/// score and network alerts on Dashboard option is selected in Account Settings.
		/// </remarks>
		[JsonPropertyName("screen_share_quality")]
		public QualityType ScreenShareQuality { get; set; }

		/// <summary>
		/// Gets or sets a participant identifier.
		/// </summary>
		/// <remarks>
		/// This value can be numbers or characters, up to a maximum length of 15 characters.
		/// </remarks>
		[JsonPropertyName("customer_key")]
		public string CustomerKey { get; set; }

		/// <summary>
		/// Gets or sets the meeting participant's SIP (Session Initiation Protocol) Contact header URI.
		/// </summary>
		/// <remarks>
		/// The API only returns this response when the participant joins a meeting via SIP.
		/// </remarks>
		[JsonPropertyName("sip_uri")]
		public string SipUri { get; set; }

		/// <summary>
		/// Gets or sets the meeting participant's SIP From header URI.
		/// </summary>
		/// <remarks>
		/// The API only returns this response when the participant joins a meeting via SIP.
		/// </remarks>
		[JsonPropertyName("from_sip_uri")]
		public string FromSipUri { get; set; }

		/// <summary>
		/// Gets or sets the participant's role.
		/// </summary>
		[JsonPropertyName("role")]
		public string Role { get; set; }
	}
}
