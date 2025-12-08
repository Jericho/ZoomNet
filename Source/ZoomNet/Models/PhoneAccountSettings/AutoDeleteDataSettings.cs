using System.Text.Json.Serialization;

namespace ZoomNet.Models.PhoneAccountSettings
{
	/// <summary>
	/// Settings that allow Zoom to automatically delete data after the retention duration has elapsed.
	/// </summary>
	public class AutoDeleteDataSettings : SettingsGroupBase
	{
		/// <summary>
		/// Gets or sets the deletion policy.
		/// </summary>
		[JsonPropertyName("delete_type")]
		public DeleteDataPolicy? DeleteType { get; set; }

		/// <summary>
		/// Gets or sets information about delete data items.
		/// </summary>
		[JsonPropertyName("items")]
		public DeleteDataItem[] Items { get; set; }
	}
}
