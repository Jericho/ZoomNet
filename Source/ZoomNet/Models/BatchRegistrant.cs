using System.Text.Json.Serialization;

namespace ZoomNet.Models
{
	/// <summary>
	/// Registant.
	/// </summary>
	public class BatchRegistrant
	{
		/// <summary>
		/// Gets or sets registant's email.
		/// </summary>
		[JsonPropertyName("email")]
		public string Email { get; set; }

		/// <summary>
		/// Gets or sets registant's first name.
		/// </summary>
		[JsonPropertyName("first_name")]
		public string FirstName { get; set; }

		/// <summary>
		/// Gets or sets registant's last name.
		/// </summary>
		[JsonPropertyName("last_name")]
		public string LastName { get; set; }

		/// <summary>
		/// Gets or sets the registrant id.
		/// </summary>
		/// <value>
		/// The id.
		/// </value>
		[JsonPropertyName("registrant_id")]
		public string Id { get; set; }

		/// <summary>
		/// Gets or sets the URL for this registrant to join the meeting or webinar.
		/// </summary>
		[JsonPropertyName("join_url")]
		public string JoinUrl { get; set; }
	}
}
