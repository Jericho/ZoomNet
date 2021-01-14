using Newtonsoft.Json;

namespace ZoomNet.Models
{
	public class Location
	{
		/// <summary>
		/// Gets or sets the location id.
		/// </summary>
		[JsonProperty("id")]
		public string Id { get; set; }

		/// <summary>
		/// Gets or sets the name.
		/// </summary>
		[JsonProperty("name")]
		public string Name { get; set; }

		/// <summary>
		/// Gets or sets the unique identifier of the parent location.
		/// For instance, if a Zoom Room is located in Floor 1 of Building A, the location of Building A will be the parent location of Floor 1 and the parent_location_id of Floor 1 will be the ID of Building A.
		/// The value of parent_location_id of the top-level location (country) is the Account ID of the Zoom account.
		/// </summary>
		[JsonProperty(PropertyName = "parent_location_id")]
		public string ParentId { get; set; }

		/// <summary>
		/// Gets or sets the type.
		/// </summary>
		[JsonProperty("type")]
		public LocationType Type { get; set; }
	}
}
