using System.Text.Json.Serialization;

namespace ZoomNet.Models
{
	/// <summary>
	/// An event coeditor.
	/// </summary>
	public class EventCoEditor
	{
		/// <summary>
		/// Gets or sets the email address of the coeditor.
		/// </summary>
		[JsonPropertyName("email")]
		public string Email { get; set; }

		/// <summary>
		/// Gets or sets the permissions groups.
		/// </summary>
		[JsonPropertyName("permission_groups")]
		public EventCoEditorPermissionGroup[] Permissions { get; set; }
	}
}
