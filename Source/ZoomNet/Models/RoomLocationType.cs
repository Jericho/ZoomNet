using System.Runtime.Serialization;

namespace ZoomNet.Models
{
	/// <summary>
	/// Enumeration to indicate room location type.
	/// </summary>
	public enum RoomLocationType
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
		Floor,

		/// <summary>
		/// Room.
		/// </summary>
		[EnumMember(Value = "room")]
		Room
	}
}
