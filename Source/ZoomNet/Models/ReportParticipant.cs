using System;
using System.Text.Json.Serialization;

namespace ZoomNet.Models
{
	/// <summary>
	/// Metrics of a participant.
	/// </summary>
	public class ReportParticipant : Participant
	{
		/// <summary>
		/// Gets or sets the participant ID.
		/// </summary>
		/// <value>
		/// The participant ID.
		/// </value>
		[JsonPropertyName("user_id")]
		public string UserId { get; set; }

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
		/// Gets or sets a value of the duration the participant.
		/// </summary>
		/// <value>
		/// Indicates how long a participant has participated.
		/// </value>
		[JsonPropertyName("duration")]
		public int? Duration { get; set; }

		/// <summary>
		/// Gets or sets the CustomerKey of the participant.
		/// </summary>
		/// <value>
		/// The CustomerKey of the participant.  This is another identifier with a max length of 15 characters.
		/// </value>
		[JsonPropertyName("customer_key")]
		public string CustomerKey { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether a failover occurred during the meeting.
		/// </summary>
		/// <value>
		/// Indicates whether or not failover occurred during the meeting.
		/// </value>
		[JsonPropertyName("failover")]
		public bool Failover { get; set; }
	}
}
