using System.Text.Json.Serialization;

namespace ZoomNet.Models
{
	/// <summary>
	/// Group administrator.
	/// </summary>
	public class GroupAdministrator
	{
		/// <summary>
		/// Gets or sets the administrator's email address.
		/// </summary>
		[JsonPropertyName("email")]
		public string EmailAddress { get; set; }

		/// <summary>
		/// Gets or sets the administrator's name.
		/// </summary>
		[JsonPropertyName("name")]
		public string Name { get; set; }
	}
}
