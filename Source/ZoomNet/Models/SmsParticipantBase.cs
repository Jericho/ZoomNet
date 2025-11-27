using System.Text.Json.Serialization;

namespace ZoomNet.Models
{
	/// <summary>
	/// Base information about SMS participant (sender or receiver) common for API endpoints and webhook events.
	/// </summary>
	public abstract class SmsParticipantBase
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
	}
}
