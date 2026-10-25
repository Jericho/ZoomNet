using System.Text.Json.Serialization;

namespace ZoomNet.Models
{
	/// <summary>Group administrator.</summary>
	public record GroupAdministrator
	{
		/// <summary>Gets or sets the administrator's email address.</summary>
		[JsonPropertyName("email")]
		public string EmailAddress { get; init; }

		/// <summary>Gets or sets the administrator's name.</summary>
		[JsonPropertyName("name")]
		public string Name { get; init; }
	}
}
