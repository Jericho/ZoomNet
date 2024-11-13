using System.Text.Json.Serialization;

namespace ZoomNet.Models
{
	/// <summary>
	/// App links.
	/// </summary>
	public class AppLinks
	{
		/// <summary>Gets or sets the link to the app's documentation.</summary>
		[JsonPropertyName("documentation_url")]
		public string Documentation { get; set; }

		/// <summary>Gets or sets the link to the app's privacy policy.</summary>
		[JsonPropertyName("privacy_policy_url")]
		public string PrivacyPolicy { get; set; }

		/// <summary>Gets or sets the link to the app's support.</summary>
		[JsonPropertyName("support_url")]
		public string Support { get; set; }

		/// <summary>Gets or sets the link to the app's terms of use.</summary>
		[JsonPropertyName("terms_of_use_url")]
		public string TermsOfUse { get; set; }
	}
}
