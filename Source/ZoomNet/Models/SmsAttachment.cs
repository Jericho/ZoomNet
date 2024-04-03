using System.Text.Json.Serialization;

namespace ZoomNet.Models
{
	/// <summary>
	/// SMS attachment file information.
	/// </summary>
	public class SmsAttachment
	{
		/// <summary>
		/// Gets or sets the download link for the media file.
		/// </summary>
		[JsonPropertyName("download_url")]
		public string DownloadUrl { get; set; }

		/// <summary>
		/// Gets or sets the media file ID.
		/// </summary>
		[JsonPropertyName("id")]
		public string Id { get; set; }

		/// <summary>
		/// Gets or sets the file name.
		/// </summary>
		[JsonPropertyName("name")]
		public string Name { get; set; }

		/// <summary>
		/// Gets or sets the file size.
		/// </summary>
		[JsonPropertyName("size")]
		public int Size { get; set; }

		/// <summary>
		/// Gets or sets the file type.
		/// </summary>
		[JsonPropertyName("type")]
		public SmsAttachmentType Type { get; set; }
	}
}
