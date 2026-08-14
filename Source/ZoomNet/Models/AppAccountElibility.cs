using System.Text.Json.Serialization;

namespace ZoomNet.Models
{
	/// <summary>
	/// Represents the eligibility requirements for an app.
	/// </summary>
	public class AppAccountElibility
	{
		/// <summary>Gets or sets the types of account.</summary>
		[JsonPropertyName("account_types")]
		public string[] AccountTypes { get; set; }

		/// <summary>Gets or sets the premium events.</summary>
		[JsonPropertyName("premium_events")]
		public AppPremiumEvent[] PremiumEvents { get; set; }
	}
}
