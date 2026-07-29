using System;
using System.Text.Json.Serialization;

namespace ZoomNet.Models
{
	/// <summary>Represents a content item in the Zoom Rooms Digital Signage content library.</summary>
	public class DigitalSignageContentItem
	{
		/// <summary>Gets or sets the content item ID.</summary>
		[JsonPropertyName("content_id")]
		public string Id { get; set; }

		/// <summary>Gets or sets the type of the content item.</summary>
		[JsonPropertyName("content_type")]
		public DigitalSignageContentItemType Type { get; set; }

		/// <summary>Gets or sets the name of the content item.</summary>
		[JsonPropertyName("content_name")]
		public string Name { get; set; }

		/// <summary>Gets or sets the URL of the content item.</summary>
		[JsonPropertyName("content_url")]
		public string Url { get; set; }

		/// <summary>Gets or sets a value indicating whether the content item expires.</summary>
		[JsonPropertyName("expires")]
		public bool Expires { get; set; }

		/// <summary>Gets or sets the expiration date of the content item.</summary>
		[JsonPropertyName("expiration_date")]
		public DateTime? ExpirationDate { get; set; }

		/// <summary>Gets or sets the folder ID of the content item.</summary>
		[JsonPropertyName("folder_id")]
		public string FolderId { get; set; }

		/// <summary>Gets or sets the download URL of the content item.</summary>
		[JsonPropertyName("download_url")]
		public string DownloadUrl { get; set; }

		/// <summary>Gets or sets the time-to-live of the download URL.</summary>
		[JsonPropertyName("download_url_ttl")]
		public long DownloadUrlTimeToLive { get; set; }
	}
}
