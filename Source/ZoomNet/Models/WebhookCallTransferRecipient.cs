using System.Text.Json.Serialization;

namespace ZoomNet.Models
{
	/// <summary>
	/// The recipient who received a warm/blind transfer call from the webhook event owner.
	/// </summary>
	public class WebhookCallTransferRecipient
	{
		/// <summary>
		/// Gets or sets the recipient's extension number.
		/// </summary>
		[JsonPropertyName("extension_number")]
		public long ExtensionNumber { get; set; }

		/// <summary>
		/// Gets or sets the name of the recipient.
		/// </summary>
		[JsonPropertyName("name")]
		public string Name { get; set; }

		/// <summary>
		/// Gets or sets the recipient's phone number in E.164 format.
		/// </summary>
		[JsonPropertyName("phone_number")]
		public string PhoneNumber { get; set; }
	}
}
