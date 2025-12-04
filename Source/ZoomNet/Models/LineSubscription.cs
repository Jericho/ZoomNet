using System.Text.Json.Serialization;

namespace ZoomNet.Models
{
	/// <summary>
	/// Line subscription information.
	/// </summary>
	public class LineSubscription
	{
		/// <summary>
		/// Gets or sets the display name.
		/// </summary>
		[JsonPropertyName("display_name")]
		public string DisplayName { get; set; }

		/// <summary>
		/// Gets or sets the extension number.
		/// </summary>
		[JsonPropertyName("extension_number")]
		public long ExtensionNumber { get; set; }

		/// <summary>
		/// Gets or sets the phone number.
		/// </summary>
		[JsonPropertyName("phone_number")]
		public string PhoneNumber { get; set; }
	}
}
