using System.Text.Json.Serialization;

namespace ZoomNet.Models
{
	/// <summary>
	/// Call transfer/forward information.
	/// </summary>
	public class CallLogOwnerInfo
	{
		/// <summary>Gets or sets the extension number.</summary>
		[JsonPropertyName("extension_number")]
		public int ExtensionNumber { get; set; }

		/// <summary>Gets or sets the owner ID.</summary>
		[JsonPropertyName("id")]
		public string Id { get; set; }

		/// <summary>Gets or sets the name.</summary>
		[JsonPropertyName("name")]
		public string Name { get; set; }

		/// <summary>Gets or sets the owner type.</summary>
		[JsonPropertyName("type")]
		public CallLogOwnerType? Type { get; set; }
	}
}
