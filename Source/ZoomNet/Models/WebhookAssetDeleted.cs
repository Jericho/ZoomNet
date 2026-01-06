using System;
using System.Text.Json.Serialization;

namespace ZoomNet.Models
{
	/// <summary>
	/// Payload model for <see cref="Webhooks.ContactCenterAssetDeletedEvent"/>.
	/// </summary>
	public class WebhookAssetDeleted
	{
		/// <summary>
		/// Gets or sets the asset's id.
		/// </summary>
		[JsonPropertyName("asset_id")]
		public string Id { get; set; }

		/// <summary>
		/// Gets or sets the asset's name.
		/// </summary>
		[JsonPropertyName("asset_name")]
		public string Name { get; set; }

		/// <summary>
		/// Gets or sets the date and time when the asset was deleted.
		/// </summary>
		[JsonPropertyName("date_time_ms")]
		public DateTime DeletedOn { get; set; }

		/// <summary>
		/// Gets or sets the id of the user that last modified this data.
		/// </summary>
		[JsonPropertyName("modified_by")]
		public string ModifiedBy { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether the asset is archived.
		/// </summary>
		[JsonPropertyName("archived")]
		public bool Archived { get; set; }

		/// <summary>
		/// Gets or sets the date and time when this data was archived.
		/// </summary>
		[JsonPropertyName("archived_time")]
		public DateTime? ArchivedTime { get; set; }
	}
}
