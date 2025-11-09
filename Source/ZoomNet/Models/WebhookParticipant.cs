using System.Text.Json.Serialization;

namespace ZoomNet.Models
{
	/// <summary>
	/// Participant (for webhooks).
	/// </summary>
	/// <remarks>
	/// This class is used to parse the participant information included in webhook JSON payloads.
	/// It is very similar to the <see cref="Participant"/> class but there are a few notable differences.
	///
	/// For instance:
	/// - the JSON payload uses "email" instead of "user_email".
	/// - the JSON payload uses "user_name" instead of "name".
	/// - the JSON payload uses "email" instead of "user_email".
	/// - the JSON payload includes a "user_id" property which contains the participant identifier and a "id" property which contains the UserId.
	/// </remarks>
	public class WebhookParticipant
	{
		/// <summary>
		/// Gets or sets the unique participant identifier.
		/// </summary>
		/// <remarks>
		/// This is unique for each meeting participant and is valid only for that meeting.
		/// </remarks>
		[JsonPropertyName("user_id")]
		public string ParticipantId { get; set; }

		/// <summary>
		/// Gets or sets the participant's display name.
		/// </summary>
		[JsonPropertyName("user_name")]
		public string DisplayName { get; set; }

		/// <summary>
		/// Gets or sets the user id of the participant.
		/// </summary>
		/// <remarks>
		/// Same as the User Id used in the Users API if participant joined the meeting by logging in.
		/// If the participant joins the meeting without logging into Zoom this returns empty string.
		/// Use <see cref="ParticipantUserId"/> instead as this property is going to be deprecated.
		/// </remarks>
		[JsonPropertyName("id")]
		public string UserId { get; set; }

		/// <summary>
		/// Gets or sets the participant's email address.
		/// </summary>
		/// <remarks>
		/// Has value only if the participant joined the meeting by logging into Zoom.
		/// If the participant is not a part of the host's account this returns empty string.
		/// </remarks>
		[JsonPropertyName("email")]
		public string Email { get; set; }

		/// <summary>
		/// Gets or sets the participant's SDK identifier.
		/// </summary>
		[JsonPropertyName("customer_key")]
		public string CustomerKey { get; set; }

		/// <summary>
		/// Gets or sets the participant's universally unique id.
		/// </summary>
		/// <remarks>
		/// Same as the User Id used in the Users API if participant joined the meeting by logging in.
		/// If the participant joins the meeting without logging into Zoom this returns empty string.
		/// The value is blank for external users.
		/// </remarks>
		[JsonPropertyName("participant_user_id")]
		public string ParticipantUserId { get; set; }

		/// <summary>
		/// Gets or sets the participant's meeting universally unique id.
		/// </summary>
		/// <remarks>
		/// This value is assigned to a participant upon joining a meeting and is only valid for the duration of the meeting.
		/// This value does not change when the participant joins/leaves a breakout room.
		/// </remarks>
		[JsonPropertyName("participant_uuid")]
		public string ParticipantUuid { get; set; }

		/// <summary>
		/// Gets or sets phone number of participant who joined via PSTN.
		/// </summary>
		[JsonPropertyName("phone_number")]
		public string PhoneNumber { get; set; }

		/// <summary>
		/// Gets or sets the participant's registrant id.
		/// </summary>
		/// <remarks>
		/// A host or a user with admin permissions can require registration for Zoom meetings.
		/// </remarks>
		[JsonPropertyName("registrant_id")]
		public string RegistrantId { get; set; }
	}
}
