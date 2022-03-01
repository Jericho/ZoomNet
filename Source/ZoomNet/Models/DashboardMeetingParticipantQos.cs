using Newtonsoft.Json;
using System;

namespace ZoomNet.Models
{
	/// <summary>
	/// Metrics of the quality of service experienced by a meeting participant.
	/// </summary>
	public class DashboardMeetingParticipantQos
	{
		/// <summary>
		/// Gets or sets participant ID.
		/// </summary>
		/// <value>
		/// The participant id.
		/// </value>
		[JsonProperty(PropertyName = "user_id")]
		public string UserId { get; set; }

		/// <summary>
		/// Gets or sets participant display name.
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
		/// Gets or sets participant's IP address.
		/// </summary>
		/// <value>
		/// The participant's IP address.
		/// </value>
		[JsonProperty(PropertyName = "ip_address")]
		public string IpAddress { get; set; }

		/// <summary>
		/// Gets or sets participant's location.
		/// </summary>
		/// <value>
		/// The participant's location.
		/// </value>
		[JsonProperty(PropertyName = "location")]
		public string Location { get; set; }

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
		/// Gets or sets the name of participant's PC.
		/// </summary>
		/// <value>
		/// The name of participant's PC.
		/// </value>
		[JsonProperty(PropertyName = "pc_name")]
		public string PcName { get; set; }

		/// <summary>
		/// Gets or sets the participant's PC domain.
		/// </summary>
		/// <value>
		/// The participant's PC domain.
		/// </value>
		[JsonProperty(PropertyName = "domain")]
		public string Domain { get; set; }

		/// <summary>
		/// Gets or sets the participant's MAC address.
		/// </summary>
		/// <value>
		/// The participant's MAC address.
		/// </value>
		[JsonProperty(PropertyName = "mac_addr")]
		public string MacAddress { get; set; }

		/// <summary>
		/// Gets or sets the participant's hard disk ID.
		/// </summary>
		/// <value>
		/// The participant's hard disk ID.
		/// </value>
		[JsonProperty(PropertyName = "harddisk_id")]
		public string HardDiskId { get; set; }

		/// <summary>
		/// Gets or sets the participant's Zoom Client version.
		/// </summary>
		/// <value>
		/// The participant's Zoom Client version.
		/// </value>
		[JsonProperty(PropertyName = "version")]
		public string Version { get; set; }

		/// <summary>
		/// Gets or sets the collection of quality of service data.
		/// </summary>
		/// <value>
		/// The quality of service data available for the meeting.
		/// </value>
		[JsonProperty(PropertyName = "user_qos")]
		public DashboardParticipantQos[] QualityOfServiceData { get; set; }
	}
}
