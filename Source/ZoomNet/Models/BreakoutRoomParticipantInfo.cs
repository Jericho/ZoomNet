using System.Text.Json.Serialization;

namespace ZoomNet.Models
{
	/// <summary>
	/// Breakout room participant information as received in webhook events related to breakout room joining and leaving.
	/// </summary>
	public class BreakoutRoomParticipantInfo : BreakoutRoomParticipantBasicInfo
	{
		/// <summary>
		/// Gets or sets the participant's email address.
		/// Returns only if the participant joined the meeting by logging into Zoom.
		/// </summary>
		[JsonPropertyName("email")]
		public string Email { get; set; }

		/// <summary>
		/// Gets or sets the participant's SDK identifier. This value can be alphanumeric.
		/// </summary>
		[JsonPropertyName("customer_key")]
		public string CustomerKey { get; set; }

		/// <summary>
		/// Gets or sets the participant's unique ID.
		/// - If meeting registration was not required and the participant joined by logging into Zoom,
		///   this value is the same as the userId field used in the Get a user API.
		/// - If registration was required for the meeting and the participant joined the meeting by logging into Zoom,
		///   this value is the same as the id value in the List meeting registrants API response.
		/// - If participant joins without logging into Zoom, this returns an empty value.
		/// This value returns blank for external users.
		/// </summary>
		[JsonPropertyName("participant_user_id")]
		public string ParticipantUserId { get; set; }

		/// <summary>
		/// Gets or sets the participant's UUID for this specific meeting and any breakout rooms created in this meeting.
		/// This value is assigned to a participant when they join a meeting, and is only valid for the duration of that meeting.
		/// </summary>
		[JsonPropertyName("participant_uuid")]
		public string ParticipantUuid { get; set; }

		/// <summary>
		/// Gets or sets phone number of the participant joined via PSTN.
		/// </summary>
		[JsonPropertyName("phone_number")]
		public string PhoneNumber { get; set; }

		/// <summary>
		/// Gets or sets the participant's registrant ID.
		/// A host or a user with administrative permissions can require registration for Zoom meetings.
		/// </summary>
		[JsonPropertyName("registrant_id")]
		public string RegistrantId { get; set; }
	}
}
