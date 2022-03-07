using System.Text.Json.Serialization;

namespace ZoomNet.Models
{
	/// <summary>
	/// Panelist.
	/// </summary>
	public class Panelist
	{
		/// <summary>
		/// Gets or sets the panelist id.
		/// </summary>
		/// <value>
		/// The id.
		/// </value>
		[JsonPropertyName("id")]
		public string Id { get; set; }

		/// <summary>
		/// Gets or sets the panelist's email address.
		/// </summary>
		[JsonPropertyName("email")]
		public string Email { get; set; }

		/// <summary>
		/// Gets or sets the panelist's full name.
		/// </summary>
		[JsonPropertyName("name")]
		public string FullName { get; set; }

		/// <summary>
		/// Gets or sets the panelist's join URL.
		/// </summary>
		[JsonPropertyName("join_url")]
		public string JoinUrl { get; set; }
	}
}
