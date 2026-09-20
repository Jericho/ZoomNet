using System.Text.Json.Serialization;

namespace ZoomNet.Models
{
	/// <summary>Meeting Resource.</summary>
	public class MeetingResource
	{
		/// <summary>Gets or sets the type of the meeting resource.</summary>
		[JsonPropertyName("resource_type")]
		public MeetingResourceType Type { get; set; }

		/// <summary>Gets or sets the ID of the meeting resource.</summary>
		[JsonPropertyName("resource_id")]
		public string Id { get; set; }

		/// <summary>Gets or sets the value indicating the permission level for users to access the whiteboard.</summary>
		[JsonPropertyName("permission_level")]
		public MeetingResourcePermissionLevel PermissionLevel { get; set; }
	}
}
