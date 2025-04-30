using System.Text.Json.Serialization;

namespace ZoomNet.Models
{
	/// <summary>
	/// A workspace.
	/// </summary>
	public class Workspace
	{
		/// <summary>Gets or sets the workspace id.</summary>
		[JsonPropertyName("id")]
		public string Id { get; set; }

		/// <summary>Gets or sets the name of the workspace.</summary>
		[JsonPropertyName("workspace_name")]
		public string Name { get; set; }

		/// <summary>Gets or sets the unique identifier of the location.</summary>
		[JsonPropertyName("location_id")]
		public string LocationId { get; set; }

		/// <summary>Gets or sets the type of the workspace.</summary>
		[JsonPropertyName("workspace_type")]
		public WorkspaceType Type { get; set; }
	}
}
