using System.Text.Json.Serialization;

namespace ZoomNet.Models
{
	/// <summary>An exhibitor.</summary>
	public record EventExhibitor
	{
		/// <summary>Gets or sets the sessions associated with the exhibitor or sponsor. The value is an array of sessionIds.</summary>
		[JsonPropertyName("associated_sessions")]
		public string[] AssociatedSessions { get; init; }

		/// <summary>Gets or sets the contact's email address.</summary>
		[JsonPropertyName("contact_email")]
		public string ContactEmailAddress { get; init; }

		/// <summary>Gets or sets the contact's full name.</summary>
		[JsonPropertyName("contact_name")]
		public string ContactName { get; init; }

		/// <summary>Gets or sets the exhibitor's description.</summary>
		[JsonPropertyName("description")]
		public string Description { get; init; }

		/// <summary>Gets or sets the exhibitor ID.</summary>
		[JsonPropertyName("exhibitor_id")]
		public string Id { get; init; }

		/// <summary>Gets or sets the link to the Facebook page.</summary>
		[JsonPropertyName("facebook_url")]
		public string FacebookUrl { get; init; }

		/// <summary>Gets or sets the link to the Instagram profile.</summary>
		[JsonPropertyName("instagram_url")]
		public string InstagramUrl { get; init; }

		/// <summary>Gets or sets a value indicating whether the exhibitor is a sponsor of the event.</summary>
		[JsonPropertyName("is_sponsor")]
		public bool IsSponsor { get; init; }

		/// <summary>Gets or sets the link to the LinkedIn profile.</summary>
		[JsonPropertyName("linkedin_url")]
		public string LinkedInUrl { get; init; }

		/// <summary>Gets or sets the name of the exhibitor.</summary>
		[JsonPropertyName("name")]
		public string Name { get; init; }

		/// <summary>Gets or sets the privacy policy link.</summary>
		[JsonPropertyName("privacy_policy")]
		public string PrivacyPolicyUrl { get; init; }

		/// <summary>Gets or sets the sponsor tier ID for a particular event. This field only applies to a sponsor.</summary>
		[JsonPropertyName("tier_id")]
		public string SponsorTier { get; init; }

		/// <summary>Gets or sets the link to the Twitter profile.</summary>
		[JsonPropertyName("twitter_url")]
		public string TwitterUrl { get; init; }

		/// <summary>Gets or sets the website's URL.</summary>
		[JsonPropertyName("website")]
		public string WebsiteUrl { get; init; }

		/// <summary>Gets or sets the link to the YouTube profile.</summary>
		[JsonPropertyName("youtube_url")]
		public string YoutubeUrl { get; init; }
	}
}
