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
	}
}
