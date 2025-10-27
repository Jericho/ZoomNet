using System.Text.Json.Serialization;

namespace ZoomNet.Models
{
	/// <summary>
	/// Represents information about the participant invited to join a meeting or video call.
	/// </summary>
	public class InvitedParticipant
	{
		/// <summary>
		/// Gets or sets the email address of the user who received the meeting invitation.
		/// </summary>
		/// <remarks>
		/// If the participant is not part of the host's account the value can be empty string.
		/// </remarks>
		[JsonPropertyName("email")]
		public string Email { get; set; }

		/// <summary>
		/// Gets or sets id of the user who received the meeting invitation.
		/// </summary>
		[JsonPropertyName("participant_user_id")]
		public string UserId { get; set; }
	}
}
