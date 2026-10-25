using System.Text.Json.Serialization;

namespace ZoomNet.Models
{
	/// <summary>An event coeditor.</summary>
	public record EventCoEditor
	{
		/// <summary>Gets or sets the email address of the coeditor.</summary>
		[JsonPropertyName("email")]
		public string Email { get; init; }

		/// <summary>Gets or sets the permissions groups.</summary>
		[JsonPropertyName("permission_groups")]
		public EventCoEditorPermissionGroup[] Permissions { get; init; }
	}
}
