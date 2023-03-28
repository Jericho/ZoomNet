using System.Text.Json.Serialization;

namespace ZoomNet.Models
{
	/// <summary>
	/// Call transfer/forward information.
	/// </summary>
	public class CallLogTransferInfo
	{
		/// <summary>Gets or sets the extension number.</summary>
		[JsonPropertyName("extension_number")]
		public string ExtensionNumber { get; set; }

		/// <summary>Gets or sets the extension type.</summary>
		[JsonPropertyName("extension_type")]
		public CallLogTransferInfoExtensionType? ExtensionType { get; set; }

		/// <summary>Gets or sets the location.</summary>
		[JsonPropertyName("location")]
		public string Location { get; set; }

		/// <summary>Gets or sets the name.</summary>
		[JsonPropertyName("name")]
		public string Name { get; set; }

		/// <summary>Gets or sets the number type.</summary>
		[JsonPropertyName("number_type")]
		public CallLogTransferInfoNumberType? NumberType { get; set; }

		/// <summary>Gets or sets the phone number.</summary>
		[JsonPropertyName("phone_number")]
		public string PhoneNumber { get; set; }
	}
}
