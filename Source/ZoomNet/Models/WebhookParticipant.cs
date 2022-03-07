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
		/// Same as the User Id used in the Users API if p[articipant oined the meeting by logging in.
		/// </remarks>
		[JsonPropertyName("id")]
		public string UserId { get; set; }

		/// <summary>
		/// Gets or sets the participant's email address.
		/// </summary>
		[JsonPropertyName("email")]
		public string Email { get; set; }
	}
}
