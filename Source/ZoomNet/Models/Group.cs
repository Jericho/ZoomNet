using Newtonsoft.Json;

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
		[JsonProperty("id", NullValueHandling = NullValueHandling.Ignore)]
		public string Id { get; set; }

		/// <summary>
		/// Gets or sets the name of the group.
		/// </summary>
		[JsonProperty(PropertyName = "name")]
		public string Name { get; set; }

		/// <summary>
		/// Gets or sets the number of member in this group.
		/// </summary>
		[JsonProperty(PropertyName = "total_members")]
		public long NumberOfMembers { get; set; }
	}
}
