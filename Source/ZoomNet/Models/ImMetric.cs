using System.Text.Json.Serialization;

namespace ZoomNet.Models
{
	/// <summary>Metric for chat room usage.</summary>
	public class ImMetric
	{
		/// <summary>Gets or sets total number of instant meeting calls received by the user.</summary>
		[JsonPropertyName("calls_receive")]
		public int CallReceive { get; set; }

		/// <summary>Gets or sets total number of instant meeting calls made by the user.</summary>
		[JsonPropertyName("calls_send")]
		public int CallsSend { get; set; }

		/// <summary>Gets or sets the user email.</summary>
		[JsonPropertyName("email")]
		public string Email { get; set; }

		/// <summary>Gets or sets total number of emojis received by the user.</summary>
		[JsonPropertyName("emoji_receive")]
		public int EmojiReceive { get; set; }

		/// <summary>Gets or sets total number of emojis sent by the user.</summary>
		[JsonPropertyName("emoji_send")]
		public int EmojiSend { get; set; }

		/// <summary>Gets or sets total number of files received by the user.</summary>
		[JsonPropertyName("files_receive")]
		public int FilesReceive { get; set; }

		/// <summary>Gets or sets total number of files sent by the user.</summary>
		[JsonPropertyName("files_send")]
		public int FilesSend { get; set; }

		/// <summary>Gets or sets total number of messages received by the user in channels.</summary>
		[JsonPropertyName("group_receive")]
		public int GroupReceive { get; set; }

		/// <summary>Gets or sets total number of messages sent by the user in channels.</summary>
		[JsonPropertyName("group_send")]
		public int GroupSend { get; set; }

		/// <summary>Gets or sets total number of images received by the user.</summary>
		[JsonPropertyName("images_receive")]
		public int ImagesReceive { get; set; }

		/// <summary>Gets or sets total number of images sent by the user.</summary>
		[JsonPropertyName("images_send")]
		public int ImagesSend { get; set; }

		/// <summary>Gets or sets total number of messages received by the user.</summary>
		[JsonPropertyName("total_receive")]
		public int TotalReceive { get; set; }

		/// <summary>Gets or sets the total number of messages sent by the user.</summary>
		[JsonPropertyName("total_send")]
		public int TotalSend { get; set; }

		/// <summary>Gets or sets the user id.</summary>
		[JsonPropertyName("user_id")]
		public string UserId { get; set; }

		/// <summary>Gets or sets the user display name.</summary>
		[JsonPropertyName("user_name")]
		public string UserName { get; set; }

		/// <summary>Gets or sets total number of video files received by the user. .</summary>
		[JsonPropertyName("videos_receive")]
		public int VideosReceive { get; set; }

		/// <summary>Gets or sets total number of video files sent by the user.</summary>
		[JsonPropertyName("videos_send")]
		public int VideosSend { get; set; }

		/// <summary>Gets or sets total number of voice files received by the user.</summary>
		[JsonPropertyName("voice_receive")]
		public int VoiceReceive { get; set; }

		/// <summary>Gets or sets total number of voice files sent by the user.</summary>
		[JsonPropertyName("voice_send")]
		public int VoiceSend { get; set; }
	}
}
