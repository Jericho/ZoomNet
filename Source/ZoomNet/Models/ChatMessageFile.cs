using System.Text.Json.Serialization;

namespace ZoomNet.Models
{
	/// <summary>
	/// Chat message file information as received in meeting or webhook chat message file webhook events.
	/// </summary>
	public class ChatMessageFile
	{
		/// <summary>
		/// Gets or sets the live meeting or webinar chat file uuid, in base64 encoded format.
		/// </summary>
		[JsonPropertyName("file_id")]
		public string Id { get; set; }

		/// <summary>
		/// Gets or sets the live meeting or webinar chat file name.
		/// </summary>
		[JsonPropertyName("file_name")]
		public string Name { get; set; }

		/// <summary>
		/// Gets or sets the live meeting or webinar chat file size, in bytes.
		/// </summary>
		[JsonPropertyName("file_size")]
		public long Size { get; set; }

		/// <summary>
		/// Gets or sets the live meeting or webinar chat file type.
		/// </summary>
		[JsonPropertyName("file_type")]
		public string Type { get; set; }

		/// <summary>
		/// Gets or sets id of the user who uploaded the chat file.
		/// The value is blank for external users.
		/// </summary>
		/// <remarks>
		/// Available only in <see cref="Webhooks.MeetingChatMessageFileDownloadedEvent"/> and
		/// <see cref="Webhooks.WebinarChatMessageFileDownloadedEvent"/>.
		/// </remarks>
		[JsonPropertyName("file_owner_id")]
		public string OwnerId { get; set; }

		/// <summary>
		/// Gets or sets the live meeting or webinar chat file download URL.
		/// </summary>
		/// <remarks>
		/// Available only in <see cref="Webhooks.MeetingChatMessageFileSentEvent"/> and
		/// <see cref="Webhooks.WebinarChatMessageFileSentEvent"/>.
		/// </remarks>
		[JsonPropertyName("download_url")]
		public string DownloadUrl { get; set; }
	}
}
