using System.Text.Json.Serialization;

namespace ZoomNet.Models
{
	/// <summary>
	/// Room tag.
	/// </summary>
	public class RoomTag
	{
		/// <summary>
		/// Gets or sets the tag id.
		/// </summary>
		[JsonPropertyName("id")]
		public string Id { get; set; }

		/// <summary>
		/// Gets or sets the name of the Zoom Room Tag.
		/// </summary>
		[JsonPropertyName("name")]
		public string Name { get; set; }

		/// <summary>
		/// Gets or sets the short description of the Zoom Room Tag.
		/// </summary>
		[JsonPropertyName("description")]
		public string Description { get; set; }

		/// <summary>
		/// Gets or sets the  number of Zoom Rooms associated with this Tag.
		/// </summary>
		[JsonPropertyName("num_of_rooms")]
		public int NumberOfRooms { get; set; }
	}
}
