using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Runtime.Serialization;

namespace ZoomNet.Models
{
	/// <summary>
	/// Enumeration to indicate the type of a room.
	/// </summary>
	[JsonConverter(typeof(StringEnumConverter))]
	public enum RoomType
	{
		/// <summary>
		/// Country.
		/// </summary>
		[EnumMember(Value = "country")]
		Country,

		/// <summary>
		/// State.
		/// </summary>
		[EnumMember(Value = "States")]
		State,

		/// <summary>
		/// City.
		/// </summary>
		[EnumMember(Value = "city")]
		City,

		/// <summary>
		/// Campus.
		/// </summary>
		[EnumMember(Value = "campus")]
		Campus,

		/// <summary>
		/// Building.
		/// </summary>
		[EnumMember(Value = "building")]
		Building,

		/// <summary>
		/// Floor.
		/// </summary>
		[EnumMember(Value = "floor")]
		Floor
	}
}
