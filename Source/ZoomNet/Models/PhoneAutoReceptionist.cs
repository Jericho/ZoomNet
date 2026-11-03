using System.Text.Json.Serialization;

namespace ZoomNet.Models;

/// <summary>A Zoom Phone auto receptionist returned by the directory listing.</summary>
public class PhoneAutoReceptionist
{
	/// <summary>Gets or sets the auto receptionist ID.</summary>
	[JsonPropertyName("id")]
	public string Id { get; set; }

	/// <summary>Gets or sets the auto receptionist name.</summary>
	[JsonPropertyName("name")]
	public string Name { get; set; }

	/// <summary>Gets or sets the extension number, including site-prefixed numbers.</summary>
	[JsonPropertyName("extension_number")]
	public long ExtensionNumber { get; set; }
}
