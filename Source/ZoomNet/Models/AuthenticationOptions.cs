using System.Text.Json.Serialization;

namespace ZoomNet.Models
{
	/// <summary>Authentication options.</summary>
	public record AuthenticationOptions
	{
		/// <summary>Gets or sets a value indicating whether this is the default authentication option.</summary>
		[JsonPropertyName("default_option")]
		public bool IsDefault { get; init; }

		/// <summary>Gets or sets the domains.</summary>
		[JsonPropertyName("domains")]
		public string Domains { get; init; }

		/// <summary>Gets or sets the unique identifier.</summary>
		[JsonPropertyName("id")]
		public string Id { get; init; }

		/// <summary>Gets or sets the name.</summary>
		[JsonPropertyName("name")]
		public string Name { get; init; }

		/// <summary>Gets or sets the authentication type.</summary>
		[JsonPropertyName("type")]
		public AuthenticationType Type { get; init; }

		/// <summary>Gets or sets a value indicating whether the authentication is visible.</summary>
		[JsonPropertyName("visible")]
		public bool Visible { get; init; }
	}
}
