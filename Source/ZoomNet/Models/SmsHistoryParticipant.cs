using System.Text.Json.Serialization;

namespace ZoomNet.Models
{
	/// <summary>
	/// The user who sent it/who received the SMS.
	/// </summary>
	public class SmsHistoryParticipant
	{
		/// <summary>
		/// Gets or sets sender's/receiver's name.
		/// </summary>
		[JsonPropertyName("display_name")]
		public string DisplayName { get; set; }

		/// <summary>
		/// Gets or sets sender's/receiver's phone number.
		/// </summary>
		[JsonPropertyName("phone_number")]
		public string PhoneNumber { get; set; }

		/// <summary>
		/// Gets or sets owner.
		/// </summary>
		[JsonPropertyName("owner")]
		public SmsParticipantOwner Owner { get; set; }
	}
}
