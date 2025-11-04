using System.Text.Json.Serialization;

namespace ZoomNet.Models
{
	/// <summary>
	/// Represents information about the participant invited to join a meeting through phone (call out).
	/// </summary>
	public class InvitedPhoneParticipant
	{
		/// <summary>
		/// Gets or sets the user's name to display in the meeting.
		/// </summary>
		[JsonPropertyName("invitee_name")]
		public string InviteeName { get; set; }

		/// <summary>
		/// Gets or sets the user's phone number.
		/// </summary>
		[JsonPropertyName("phone_number")]
		public long PhoneNumber { get; set; }
	}
}
