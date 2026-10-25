using System.Text.Json.Serialization;

namespace ZoomNet.Models
{
	/// <summary>Information about Zoom user and related TSP account.</summary>
	public record UserTspAccount
	{
		/// <summary>Gets or sets the user's email address.</summary>
		[JsonPropertyName("email")]
		public string Email { get; init; }

		/// <summary>Gets or sets the user id.</summary>
		[JsonPropertyName("id")]
		public string Id { get; init; }

		/// <summary>Gets or sets information about the user's TSP account.</summary>
		[JsonPropertyName("tsp_credentials")]
		public TspAccount Account { get; init; }
	}
}
