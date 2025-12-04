using System.Text.Json.Serialization;

namespace ZoomNet.Models.Webhooks
{
	/// <summary>
	/// This event is triggered the first time a new desk phone or device is registered.
	/// </summary>
	public class PhoneDeviceRegistrationEvent : Event
	{
		/// <summary>
		/// Gets or sets the user's account id.
		/// </summary>
		[JsonPropertyName("account_id")]
		public string AccountId { get; set; }

		/// <summary>
		/// Gets or sets registered device information.
		/// </summary>
		[JsonPropertyName("object")]
		public WebhookDeviceRegistration Device { get; set; }
	}
}
