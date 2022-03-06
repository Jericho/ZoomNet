using System.Text.Json.Serialization;

namespace ZoomNet.Models
{
	/// <summary>
	/// Group.
	/// </summary>
	public class Group
	{
		/// <summary>
		/// Gets or sets the group id.
		/// </summary>
		/// <value>
		/// The id.
		/// </value>
		[JsonPropertyName("id")]
		public string Id { get; set; }

		/// <summary>
		/// Gets or sets the name of the group.
		/// </summary>
		[JsonPropertyName("name")]
		public string Name { get; set; }

		/// <summary>
		/// Gets or sets the number of member in this group.
		/// </summary>
		[JsonPropertyName("total_members")]
		public long NumberOfMembers { get; set; }
	}
}
