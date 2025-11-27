using System.Text.Json.Serialization;

namespace ZoomNet.Models
{
	/// <summary>
	/// Phone call owner.
	/// </summary>
	public class PhoneCallRecordingOwner
	{
		/// <summary>
		/// Gets or sets the extension number.
		/// </summary>
		[JsonPropertyName("extension_number")]
		public int ExtensionNumber { get; set; }

		/// <summary>
		/// Gets or sets the owner ID.
		/// </summary>
		[JsonPropertyName("id")]
		public string Id { get; set; }

		/// <summary>
		/// Gets or sets the owner name.
		/// </summary>
		[JsonPropertyName("name")]
		public string Name { get; set; }

		/// <summary>
		/// Gets or sets the owner type.
		/// </summary>
		[JsonPropertyName("type")]
		[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
		public PhoneCallRecordingOwnerType? Type { get; set; }

		/// <summary>
		/// Gets or sets the extension status.
		/// </summary>
		[JsonPropertyName("extension_status")]
		[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
		public PhoneCallRecordingOwnerExtensionStatus? ExtensionStatus { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether the owner has the access permission to the recording.
		/// </summary>
		[JsonPropertyName("has_access_permission")]
		public bool? HasAccessPermission { get; set; }
	}
}
