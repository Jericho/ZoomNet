using Newtonsoft.Json;
using System;

namespace ZoomNet.Models
{
	/// <summary>
	/// Metrics of a participant.
	/// </summary>
	public class ReportParticipant
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
		[JsonProperty(PropertyName = "name")]
		public string UserName { get; set; }

		/// <summary>
		/// Gets or sets the Email address of the participant.
		/// </summary>
		/// <value>
		/// The Email address of the participant.
		/// </value>
		[JsonProperty(PropertyName = "user_email")]
		public string Email { get; set; }

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
		/// Gets or sets a value of the duration the participant.
		/// </summary>
		/// <value>
		/// Indicates how long a participant has participated.
		/// </value>
		[JsonProperty(PropertyName = "duration", NullValueHandling = NullValueHandling.Ignore)]
		public int? Duration { get; set; }

		/// <summary>
		/// Gets or sets the RegistrantID of the participant.
		/// </summary>
		/// <value>
		/// The RegistrantID of the participant.  Only returned if registrant_id is included in the include_fields query parameter.
		/// </value>
		[JsonProperty(PropertyName = "registrant_id")]
		public string RegistrantID { get; set; }

		/// <summary>
		/// Gets or sets the CustomerKey of the participant.
		/// </summary>
		/// <value>
		/// The CustomerKey of the participant.  This is another identifier with a max length of 15 characters.
		/// </value>
		[JsonProperty(PropertyName = "customer_key")]
		public string CustomerKey { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether a failover occurred during the meeting.
		/// </summary>
		/// <value>
		/// Indicates whether or not failover occurred during the meeting.
		/// </value>
		[JsonProperty(PropertyName = "failover")]
		public bool Failover { get; set; }
	}
}
