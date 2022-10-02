using System.Text.Json.Serialization;

namespace ZoomNet.Models
{
	/// <summary>
	/// A user's Tol=free and fee-base toll call settings.
	/// </summary>
	public class AudioConferencingCallSettings
	{
		/// <summary>
		/// Gets or sets a value indicating whether webinar attendees can dial in through the account's Toll-free and Fee-based Toll Call phone numbers.
		/// </summary>
		/// <remarks>
		/// This feature is only available in version 5.2.2 and higher.
		/// </remarks>
		[JsonPropertyName("allow_webinar_attendees_dial")]
		public bool WebinarAttendeesCanDialIn { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether the user has the Toll-free and Fee-based Toll Call setting enabled.
		/// </summary>
		[JsonPropertyName("enable")]
		public bool Enabled { get; set; }

		/// <summary>
		/// Gets or sets the user's Toll-free and Fee-based Toll Call phone number information.
		/// </summary>
		[JsonPropertyName("numbers")]
		public AudioConferencingPhoneNumberInformation[] Numbers { get; set; }
	}
}
