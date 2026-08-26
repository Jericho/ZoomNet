using System;
using System.Text.Json.Serialization;

namespace ZoomNet.Models
{
	/// <summary>Metrics of the quality of service experienced by a meeting participant.</summary>
	public class DashboardMeetingParticipantQos
	{
		/// <summary>Gets or sets the type of device using which the participant joined the meeting.</summary>
		[JsonPropertyName("device")]
		public string Device { get; set; }

		/// <summary>Gets or sets the participant's PC domain.</summary>
		[JsonPropertyName("domain")]
		public string Domain { get; set; }

		/// <summary>Gets or sets the participant's hard disk ID.</summary>
		[JsonPropertyName("harddisk_id")]
		public string HardDiskId { get; set; }

		/// <summary>Gets or sets participant's IP address.</summary>
		[JsonPropertyName("ip_address")]
		public string IpAddress { get; set; }

		/// <summary>Gets or sets the time at which participant joined the meeting.</summary>
		[JsonPropertyName("join_time")]
		public DateTime JoinTime { get; set; }

		/// <summary>Gets or sets the time at which a participant left the meeting.</summary>
		[JsonPropertyName("leave_time")]
		public DateTime? LeaveTime { get; set; }

		/// <summary>Gets or sets participant's location.</summary>
		[JsonPropertyName("location")]
		public string Location { get; set; }

		/// <summary>Gets or sets the participant's MAC address.</summary>
		[JsonPropertyName("mac_addr")]
		public string MacAddress { get; set; }

		/// <summary>Gets or sets the name of participant's PC.</summary>
		[JsonPropertyName("pc_name")]
		public string PcName { get; set; }

		/// <summary>Gets or sets participant ID.</summary>
		[JsonPropertyName("user_id")]
		public string UserId { get; set; }

		/// <summary>Gets or sets participant display name.</summary>
		[JsonPropertyName("user_name")]
		public string UserName { get; set; }

		/// <summary>Gets or sets the collection of quality of service data.</summary>
		[JsonPropertyName("user_qos")]
		public DashboardParticipantQos[] QualityOfServiceData { get; set; }

		/// <summary>Gets or sets the participant's Zoom Client version.</summary>
		[JsonPropertyName("version")]
		public string Version { get; set; }
	}
}
