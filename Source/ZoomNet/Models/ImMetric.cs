using Newtonsoft.Json;

namespace ZoomNet.Models
{
	/// <summary>
	/// Metric for chat room usage.
	/// </summary>
	public class ImMetric
	{
		/// <summary>
		/// Gets or sets the user id.
		/// </summary>
		/// <value>The user id.</value>
		[JsonProperty(PropertyName = "user_id")]
		public string UserId { get; set; }

		/// <summary>
		/// Gets or sets the user display name.
		/// </summary>
		/// <value>The user display name.</value>
		[JsonProperty(PropertyName = "user_name")]
		public string UserName { get; set; }

		/// <summary>
		/// Gets or sets the user email.
		/// </summary>
		/// <value>The user email.</value>
		[JsonProperty(PropertyName = "email")]
		public string Email { get; set; }

		/// <summary>
		/// Gets or sets the total number of messages sent by the user.
		/// </summary>
		/// <value>The total number of messages sent by the user.</value>
		[JsonProperty(PropertyName = "total_send")]
		public int TotalSend { get; set; }

		/// <summary>
		/// Gets or sets total number of messages received by the user.
		/// </summary>
		/// <value>The total number of messages received by the user.</value>
		[JsonProperty(PropertyName = "total_receive")]
		public int TotalReceive { get; set; }

		/// <summary>
		/// Gets or sets total number of messages sent by the user in channels.
		/// </summary>
		/// <value>The total number of messages sent by the user in channels.</value>
		[JsonProperty(PropertyName = "group_send")]
		public int GroupSend { get; set; }

		/// <summary>
		/// Gets or sets total number of messages received by the user in channels.
		/// </summary>
		/// <value>The total number of messages received by the user in channels.</value>
		[JsonProperty(PropertyName = "group_receive")]
		public int GroupReceive { get; set; }

		/// <summary>
		/// Gets or sets total number of instant meeting calls made by the user.
		/// </summary>
		/// <value>The total number of instant meeting calls made by the user.</value>
		[JsonProperty(PropertyName = "calls_send")]
		public int CallsSend { get; set; }

		/// <summary>
		/// Gets or sets total number of instant meeting calls received by the user.
		/// </summary>
		/// <value>The total number of instant meeting calls received by the user.</value>
		[JsonProperty(PropertyName = "calls_receive")]
		public int CallReceive { get; set; }

		/// <summary>
		/// Gets or sets total number of files sent by the user.
		/// </summary>
		/// <value>The total number of files sent by the user.</value>
		[JsonProperty(PropertyName = "files_send")]
		public int FilesSend { get; set; }

		/// <summary>
		/// Gets or sets total number of files received by the user.
		/// </summary>
		/// <value>The total number of files received by the user.</value>
		[JsonProperty(PropertyName = "files_receive")]
		public int FilesReceive { get; set; }

		/// <summary>
		/// Gets or sets total number of images sent by the user.
		/// </summary>
		/// <value>The total number of images sent by the user.</value>
		[JsonProperty(PropertyName = "images_send")]
		public int ImagesSend { get; set; }

		/// <summary>
		/// Gets or sets total number of images received by the user.
		/// </summary>
		/// <value>The total number of images received by the user.</value>
		[JsonProperty(PropertyName = "images_receive")]
		public int ImagesReceive { get; set; }

		/// <summary>
		/// Gets or sets total number of voice files sent by the user.
		/// </summary>
		/// <value>The total number of voice files sent by the user.</value>
		[JsonProperty(PropertyName = "voice_send")]
		public int VoiceSend { get; set; }

		/// <summary>
		/// Gets or sets total number of voice files received by the user.
		/// </summary>
		/// <value>The total number of voice files received by the user.</value>
		[JsonProperty(PropertyName = "voice_receive")]
		public int VoiceReceive { get; set; }

		/// <summary>
		/// Gets or sets total number of video files sent by the user.
		/// </summary>
		/// <value>The total number of video files sent by the user.</value>
		[JsonProperty(PropertyName = "videos_send")]
		public int VideosSend { get; set; }

		/// <summary>
		/// Gets or sets total number of video files received by the user. .
		/// </summary>
		/// <value>The total number of video files received by the user. </value>
		[JsonProperty(PropertyName = "videos_receive")]
		public int VideosReceive { get; set; }

		/// <summary>
		/// Gets or sets total number of emojis sent by the user.
		/// </summary>
		/// <value>The total number of emojis sent by the user.</value>
		[JsonProperty(PropertyName = "emoji_send")]
		public int EmojiSend { get; set; }

		/// <summary>
		/// Gets or sets total number of emojis received by the user.
		/// </summary>
		/// <value>The total number of emojis received by the user.</value>
		[JsonProperty(PropertyName = "emoji_receive")]
		public int EmojiReceive { get; set; }
	}
}
