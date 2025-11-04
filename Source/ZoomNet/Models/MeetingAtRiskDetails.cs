using System;
using System.Text.Json.Serialization;

namespace ZoomNet.Models
{
	/// <summary>
	/// Information about the at-risk meeting.
	/// </summary>
	public class MeetingAtRiskDetails
	{
		/// <summary>
		/// Gets or sets the meeting's url.
		/// </summary>
		[JsonPropertyName("meeting_url")]
		public string MeetingUrl { get; set; }

		/// <summary>
		/// Gets or sets the post's social media platform (e.g. twitter, reddit, facebook, etc).
		/// </summary>
		[JsonPropertyName("post_platform")]
		public string PostPlatform { get; set; }

		/// <summary>
		/// Gets or sets the post's timestamp.
		/// </summary>
		[JsonPropertyName("post_time")]
		public DateTime PostTime { get; set; }

		/// <summary>
		/// Gets or sets the user who created the social media post.
		/// </summary>
		[JsonPropertyName("post_user")]
		public string PostUser { get; set; }

		/// <summary>
		/// Gets or sets the link to the social media post.
		/// </summary>
		[JsonPropertyName("social_link")]
		public string SocialLink { get; set; }

		/// <summary>
		/// Gets or sets the recommended meeting settings to disable.
		/// </summary>
		[JsonPropertyName("recommended_disable_settings")]
		public RecommendedSetting[] RecommendedDisableSettings { get; set; }

		/// <summary>
		/// Gets or sets the recommended meeting settings to enable.
		/// </summary>
		[JsonPropertyName("recommended_enable_settings")]
		public RecommendedSetting[] RecommendedEnableSettings { get; set; }
	}
}
