using System.Text.Json.Serialization;

namespace ZoomNet.Models
{
	/// <summary>
	/// Information about the channel setting for a Contact Center user.
	/// </summary>
	public class ContactCenterUserChannelSettings
	{
		/// <summary>Gets or sets the maximum number of concurrent email conversations that can be assigned to a user.</summary>
		[JsonPropertyName("concurrent_email_capacity")]
		public int MaxConcurrentEmailConversations { get; set; }

		/// <summary>Gets or sets the maximum number of concurrent messaging conversations that can be assigned to a user.</summary>
		[JsonPropertyName("concurrent_message_capacity")]
		public int MaxConcurrentMessagingConversations { get; set; }

		/// <summary>Gets or sets the information about the setting's multi-channel engagements.</summary>
		[JsonPropertyName("multi_channel_engagements")]
		public ContactCenterUserMultiChannelSettings MultiChannelEngagementSettings { get; set; }
	}
}
