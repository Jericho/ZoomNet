using Newtonsoft.Json;
using System;

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
		[JsonProperty(PropertyName = "id")]
		public string Id { get; set; }

		/// <summary>
		/// Gets or sets the participant ID.
		/// </summary>
		/// <value>
		/// The participant ID.
		/// </value>
		[JsonProperty(PropertyName = "user_id")]
		public string UserId { get; set; }

		/// <summary>
		/// Gets or sets the participant display name.
		/// </summary>
		/// <value>
		/// The participant display name.
		/// </value>
		[JsonProperty(PropertyName = "user_name")]
		public string UserName { get; set; }

		/// <summary>
		/// Gets or sets the type of device using which the participant joined the meeting.
		/// </summary>
		/// <value>
		/// The type of device using which the participant joined the meeting.
		/// </value>
		[JsonProperty(PropertyName = "device")]
		public string Device { get; set; }

		/// <summary>
		/// Gets or sets the participant’s IP address.
		/// </summary>
		/// <value>
		/// The participant’s IP address.
		/// </value>
		[JsonProperty(PropertyName = "ip_address")]
		public string IpAddress { get; set; }

		/// <summary>
		/// Gets or sets the participant’s location.
		/// </summary>
		/// <value>
		/// The participant’s location.
		/// </value>
		[JsonProperty(PropertyName = "location")]
		public string Location { get; set; }

		/// <summary>
		/// Gets or sets the participant’s network type.
		/// </summary>
		/// <value>
		/// The participant’s network type.
		/// </value>
		[JsonProperty(PropertyName = "network_type")]
		public string NetworkType { get; set; }

		/// <summary>
		/// Gets or sets the type of microphone that participant used during the meeting.
		/// </summary>
		/// <value>
		/// The type of microphone that participant used during the meeting.
		/// </value>
		[JsonProperty(PropertyName = "microphone")]
		public string Microphone { get; set; }

		/// <summary>
		/// Gets or sets the type of speaker participant used during the meeting.
		/// </summary>
		/// <value>
		/// The type of speaker participant used during the meeting.
		/// </value>
		[JsonProperty(PropertyName = "speaker")]
		public string Speaker { get; set; }

		/// <summary>
		/// Gets or sets the data center where participant’s meeting data is stored.
		/// </summary>
		/// <value>
		/// The data center where participant’s meeting data is stored.
		/// </value>
		[JsonProperty(PropertyName = "data_center")]
		public string DataCenter { get; set; }

		/// <summary>
		/// Gets or sets the participant connection type.
		/// </summary>
		/// <value>
		/// The participant connection type.
		/// </value>
		[JsonProperty(PropertyName = "connection_type")]
		public string ConnectionType { get; set; }

		/// <summary>
		/// Gets or sets the time at which participant joined the meeting.
		/// </summary>
		/// <value>
		/// The time at which participant joined the meeting.
		/// </value>
		[JsonProperty(PropertyName = "join_time")]
		public DateTime JoinTime { get; set; }

		/// <summary>
		/// Gets or sets the time at which a participant left the meeting.
		/// </summary>
		/// <value>
		/// The time at which a participant left the meeting. For live meetings this field will only be returned if a participant has left the ongoing meeting.
		/// </value>
		[JsonProperty(PropertyName = "leave_time", NullValueHandling = NullValueHandling.Ignore)]
		public DateTime? LeaveTime { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether or not a user selected to share an iPhone/iPad application during the screen-share.
		/// </summary>
		/// <value>
		/// Indicates whether or not a user selected to share an iPhone/iPad application during the screen-share.
		/// </value>
		[JsonProperty(PropertyName = "share_application")]
		public bool ShareApplication { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether or not a user selected to share their desktop during the screen-share.
		/// </summary>
		/// <value>
		/// Indicates whether or not a user selected to share their desktop during the screen-share.
		/// </value>
		[JsonProperty(PropertyName = "share_desktop")]
		public bool ShareDesktop { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether or not a user selected to share their white-board during the screen-share.
		/// </summary>
		/// <value>
		/// Indicates whether or not a user selected to share their white-board during the screen-share.
		/// </value>
		[JsonProperty(PropertyName = "share_whiteboard")]
		public bool ShareWhiteboard { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether or not recording was used during the meeting.
		/// </summary>
		/// <value>
		/// Indicates whether or not recording was used during the meeting.
		/// </value>
		[JsonProperty(PropertyName = "recording")]
		public bool Recording { get; set; }

		/// <summary>
		/// Gets or sets the name of participant’s PC.
		/// </summary>
		/// <value>
		/// The name of participant’s PC.
		/// </value>
		[JsonProperty(PropertyName = "pc_name")]
		public string PcName { get; set; }

		/// <summary>
		/// Gets or sets the participant’s PC domain.
		/// </summary>
		/// <value>
		/// The participant’s PC domain.
		/// </value>
		[JsonProperty(PropertyName = "domain")]
		public string Domain { get; set; }

		/// <summary>
		/// Gets or sets the participant’s MAC address.
		/// </summary>
		/// <value>
		/// The participant’s MAC address.
		/// </value>
		[JsonProperty(PropertyName = "mac_addr")]
		public string MacAddress { get; set; }

		/// <summary>
		/// Gets or sets the participant’s hard disk ID.
		/// </summary>
		/// <value>
		/// The participant’s hard disk ID.
		/// </value>
		[JsonProperty(PropertyName = "harddisk_id")]
		public string HardDiskId { get; set; }

		/// <summary>
		/// Gets or sets the participant’s Zoom Client version.
		/// </summary>
		/// <value>
		/// The participant’s Zoom Client version.
		/// </value>
		[JsonProperty(PropertyName = "version")]
		public string Version { get; set; }

		/// <summary>
		/// Gets or sets the Possible reasons for why participant left the meeting.
		/// </summary>
		/// <value>
		/// The Possible reasons for why participant left the meeting.
		/// </value>
		[JsonProperty(PropertyName = "leave_reason")]
		public string LeaveReason { get; set; }

		/// <summary>
		/// Gets or sets the Email address of the participant.
		/// </summary>
		/// <value>
		/// The Email address of the participant.
		/// </value>
		[JsonProperty(PropertyName = "email")]
		public string Email { get; set; }
	}
}
