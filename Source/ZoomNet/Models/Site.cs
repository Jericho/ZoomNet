using System.Text.Json.Serialization;

namespace ZoomNet.Models;

/// <summary>
/// Site information to which a user belongs to.
/// </summary>
public class Site
{
	/// <summary>
	/// Gets or sets the Id.
	/// </summary>
	[JsonPropertyName("id")]
	public string Id { get; set; }

	/// <summary>
	/// Gets or sets the name.
	/// </summary>
	[JsonPropertyName("name")]
	public string Name { get; set; }
}
