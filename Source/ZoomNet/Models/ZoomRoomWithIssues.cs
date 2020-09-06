using Newtonsoft.Json;

namespace ZoomNet.Models
{
	/// <summary>
	/// Zoom room with issues.
	/// </summary>
	public class ZoomRoomWithIssues
	{
		/// <summary>
		/// Gets or sets the Zoom room id.
		/// </summary>
		/// <value>The Zoom room id.</value>
		[JsonProperty(PropertyName = "id")]
		public string Id { get; set; }

		/// <summary>
		/// Gets or sets the Zoom room name.
		/// </summary>
		/// <value>The Zoom room name.</value>
		[JsonProperty(PropertyName = "room_name")]
		public string Name { get; set; }

		/// <summary>
		/// Gets or sets the count of issues in the Zoom room.
		/// </summary>
		/// <value>The count of issues in the Zoom room.</value>
		[JsonProperty(PropertyName = "issues_count")]
		public int IssuesCount { get; set; }
	}
}
