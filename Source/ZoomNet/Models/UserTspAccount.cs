using System.Text.Json.Serialization;

namespace ZoomNet.Models
{
	/// <summary>
	/// Information about Zoom user and related TSP account.
	/// </summary>
	public class UserTspAccount
	{
		/// <summary>
		/// Gets or sets the user id.
		/// </summary>
		[JsonPropertyName("id")]
		public string Id { get; set; }

		/// <summary>
		/// Gets or sets the user's email address.
		/// </summary>
		[JsonPropertyName("email")]
		public string Email { get; set; }

		/// <summary>
		/// Gets or sets information about the user's TSP account.
		/// </summary>
		[JsonPropertyName("tsp_credentials")]
		public TspAccount Account { get; set; }
	}
}
