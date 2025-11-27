using System.Text.Json.Serialization;

namespace ZoomNet.Models
{
	/// <summary>
	/// Opt status for each pair of sender's phone number and receiver's phone number.
	/// </summary>
	public class SmsCampaignNumbersOptStatus
	{
		/// <summary>
		/// Gets or sets the end user's phone number in E.164 format that sends the Opt-in or Opt-out keyword to the Zoom Phone number.
		/// </summary>
		[JsonPropertyName("consumer_phone_number")]
		public string ConsumerPhoneNumber { get; set; }

		/// <summary>
		/// Gets or sets the Zoom user's phone number in E.164 format that receives the Opt-in or Opt-out keyword from the end user.
		/// </summary>
		[JsonPropertyName("zoom_phone_user_number")]
		public string ZoomPhoneUserNumber { get; set; }

		/// <summary>
		/// Gets or sets the Opt-In message of the sms campaign bound to <see cref="ZoomPhoneUserNumber"/>.
		/// If <see cref="OptInStatus"/> is <see cref="SmsOptInStatus.NewSession"/> you must send the Opt-In message as the first message in the SMS session and then wait for the Opt-In response.
		/// </summary>
		[JsonPropertyName("opt_in_message")]
		public string OptInMessage { get; set; }

		/// <summary>
		/// Gets or sets the opt status.
		/// </summary>
		[JsonPropertyName("opt_status")]
		public SmsOptStatus? OptStatus { get; set; }

		/// <summary>
		/// Gets or sets the opt-in status between <see cref="ZoomPhoneUserNumber"/> and <see cref="ConsumerPhoneNumber"/>.
		/// </summary>
		[JsonPropertyName("opt_in_status")]
		public SmsOptInStatus? OptInStatus { get; set; }
	}
}
