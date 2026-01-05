using System;
using System.Text.Json.Serialization;

namespace ZoomNet.Models
{
	/// <summary>
	/// Payload model for <see cref="Webhooks.ContactCenterAssetUpdatedEvent"/>.
	/// </summary>
	public class WebhookAssetUpdated
	{
		/// <summary>
		/// Gets or sets the asset's id.
		/// </summary>
		[JsonPropertyName("asset_id")]
		public string Id { get; set; }

		/// <summary>
		/// Gets or sets the date and time when the asset is updated.
		/// </summary>
		[JsonPropertyName("date_time_ms")]
		public DateTime UpdatedOn { get; set; }

		/// <summary>
		/// Gets or sets the id of the user that last modified this data.
		/// </summary>
		[JsonPropertyName("modified_by")]
		public string ModifiedBy { get; set; }

		/// <summary>
		/// Gets or sets the information about the updated asset.
		/// </summary>
		[JsonPropertyName("updates")]
		public WebhookAssetUpdates Updates { get; set; }
	}
}
