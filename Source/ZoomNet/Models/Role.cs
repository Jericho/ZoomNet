using Newtonsoft.Json;

namespace ZoomNet.Models
{
	/// <summary>
	/// Role.
	/// </summary>
	public class Role
	{
		/// <summary>
		/// Gets or sets the unique identifier.
		/// </summary>
		[JsonProperty("id", NullValueHandling = NullValueHandling.Ignore)]
		public string Id { get; set; }

		/// <summary>
		/// Gets or sets the name of the role.
		/// </summary>
		[JsonProperty(PropertyName = "name")]
		public string Name { get; set; }

		/// <summary>
		/// Gets or sets the description.
		/// </summary>
		[JsonProperty(PropertyName = "description")]
		public string Description { get; set; }

		/// <summary>
		/// Gets or sets the number of members in this role.
		/// </summary>
		[JsonProperty(PropertyName = "total_members")]
		public long MembersCount { get; set; }
	}
}
