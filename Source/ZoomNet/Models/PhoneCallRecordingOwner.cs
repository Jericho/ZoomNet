using System.Text.Json.Serialization;

namespace ZoomNet.Models
{
	/// <summary>
	/// Phone call owner.
	/// </summary>
	public class PhoneCallRecordingOwner
	{
		/// <summary>Gets or sets the extension number.</summary>
		/// <value>The extension number associated with the call number.</value>
		[JsonPropertyName("extension_number")]
		public int ExtensionNumber { get; set; }

		/// <summary>Gets or sets the owner ID.</summary>
		/// <value>The owner's ID.</value>
		[JsonPropertyName("id")]
		public string Id { get; set; }

		/// <summary>Gets or sets the owner name.</summary>
		/// <value>Name of the owner.</value>
		[JsonPropertyName("name")]
		public string Name { get; set; }

		/// <summary>Gets or sets the owner type.</summary>
		/// <value>The owner type: user or call queue.</value>
		[JsonPropertyName("type")]
		[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
		public PhoneCallRecordingOwnerType? Type { get; set; }

		/// <summary>Gets or sets the extension status.</summary>
		/// <value>The extension status: inactive or deleted.</value>
		[JsonPropertyName("extension_status")]
		[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
		public PhoneCallRecordingOwnerExtensionStatus? ExtensionStatus { get; set; }
	}
}
