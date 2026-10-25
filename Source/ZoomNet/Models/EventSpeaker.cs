using System.Text.Json.Serialization;

namespace ZoomNet.Models
{
	/// <summary>A speaker.</summary>
	public record EventSpeaker
	{
		/// <summary>Gets or sets the biography of the speaker.</summary>
		[JsonPropertyName("biography")]
		public string Biography { get; init; }

		/// <summary>Gets or sets the name of the speaker's company.</summary>
		[JsonPropertyName("company_name")]
		public string CompanyName { get; init; }

		/// <summary>Gets or sets the speaker's company website.</summary>
		[JsonPropertyName("company_website")]
		public string CompanyWebsite { get; init; }

		/// <summary>Gets or sets the email address of the speaker.</summary>
		[JsonPropertyName("email")]
		public string Email { get; init; }

		/// <summary>Gets or sets a value indicating whether the speaker is featured in the event detail page.</summary>
		[JsonPropertyName("featured_in_event_detail_page")]
		public bool FeaturedInEventDetailPage { get; init; }

		/// <summary>Gets or sets a value indicating whether the speaker is featured in the event lobby.</summary>
		[JsonPropertyName("featured_in_lobby")]
		public bool FeaturedInLobby { get; init; }

		/// <summary>Gets or sets the job title the speaker.</summary>
		[JsonPropertyName("job_title")]
		public string JobTitle { get; init; }

		/// <summary>Gets or sets the link to the LinkedIn profile.</summary>
		[JsonPropertyName("linkedin_url")]
		public string LinkedInUrl { get; init; }

		/// <summary>Gets or sets the name of the speaker.</summary>
		[JsonPropertyName("name")]
		public string Name { get; init; }

		/// <summary>Gets or sets the unique identifier of the speaker.</summary>
		[JsonPropertyName("speaker_id")]
		public string Id { get; init; }

		/// <summary>Gets or sets the link to the Twitter profile.</summary>
		[JsonPropertyName("twitter_url")]
		public string TwitterUrl { get; init; }

		/// <summary>Gets or sets a value indicating whether the speaker is visible in the event detail page.</summary>
		[JsonPropertyName("visible_in_event_detail_page")]
		public bool VisibleInEventDetailPage { get; init; }

		/// <summary>Gets or sets a value indicating whether the speaker is visible in the event lobby.</summary>
		[JsonPropertyName("visible_in_lobby")]
		public bool VisibleInLobby { get; init; }

		/// <summary>Gets or sets the link to the YouTube profile.</summary>
		[JsonPropertyName("youtube_url")]
		public string YoutubeUrl { get; init; }
	}
}
