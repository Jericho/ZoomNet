using System;
using System.Text.Json.Serialization;

namespace ZoomNet.Models
{
	/// <summary>Participant.</summary>
	public class Participant
	{
		/// <summary>Gets or sets the participant's duration.</summary>
		[JsonPropertyName("duration")]
		public int? Duration { get; set; }

		/// <summary>Gets or sets a value indicating whether happened during the meeting.</summary>
		[JsonPropertyName("failover")]
		public bool? Failover { get; set; }

		/// <summary>
		/// Gets or sets the universal unique identifier of the participant.
		/// It is the same as the User ID of the participant if the participant joins the meeting by logging into Zoom.
		/// If the participant joins the meeting without logging in, the value of this field will be blank.
		/// </summary>
		[JsonPropertyName("id")]
		public string Uuid { get; set; }

		/// <summary>Gets or sets the participant's join time.</summary>
		[JsonPropertyName("join_time")]
		public DateTime? JoinTime { get; set; }

		/// <summary>Gets or sets the participant's leave time.</summary>
		[JsonPropertyName("leave_time")]
		public DateTime? LeaveTime { get; set; }

		/// <summary>Gets or sets the participant's display name.</summary>
		[JsonPropertyName("name")]
		public string DisplayName { get; set; }

		/// <summary>
		/// Gets or sets the participant's unique registrant ID.
		/// This field only returns if you pass the registrant_id value for the include_fields query parameter.
		/// </summary>
		/// <remarks>This field does not return if the type query parameter is the live value.</remarks>
		[JsonPropertyName("registrant_id")]
		public string RegistrantId { get; set; }

		/// <summary>Gets or sets the participant's status.</summary>
		[JsonPropertyName("status")]
		public ParticipantStatus? Status { get; set; }

		/// <summary>
		/// Gets or sets the email address of the user.
		/// If the participant is not part of the host's account, this returns an empty string value, with some exceptions.
		/// See Email address display rules for details.
		/// </summary>
		[JsonPropertyName("user_email")]
		public string Email { get; set; }

		/// <summary>Gets or sets the participant's ID. This is a unique ID assigned to the participant joining a meeting and is valid for that meeting only.</summary>
		[JsonPropertyName("user_id")]
		public string UserId { get; set; }
	}
}
