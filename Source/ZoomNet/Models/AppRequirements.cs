using System.Text.Json.Serialization;

namespace ZoomNet.Models
{
	/// <summary>
	/// App requirements.
	/// </summary>
	public class AppRequirements
	{
		/// <summary>Gets or sets the user roles required to authorize or add the app.</summary>
		[JsonPropertyName("userRole")] // documentation says there's an underscore between 'user' and 'role' but during testing I found that's not the case
		public string UserRole { get; set; }

		/// <summary>Gets or sets the minimum client version required for the app.</summary>
		[JsonPropertyName("min_client_version")]
		public string MinClientVersion { get; set; }

		/// <summary>Gets or sets the eligibility requirements for app.</summary>
		[JsonPropertyName("accountEligibility")] // documentation says there's an underscore between 'account' and 'eligibility' but during testing I found that's not the case
		public string AccountEligibility { get; set; }
	}
}
