using System.Runtime.Serialization;

namespace ZoomNet.Models
{
	/// <summary>
	/// Enumeration to indicate the role granted by a ticket.
	/// </summary>
	public enum EventTicketRole
	{
		/// <summary>Attendee.</summary>
		[EnumMember(Value = "ATTENDEE")]
		Attendee,

		/// <summary>Speaker.</summary>
		[EnumMember(Value = "SPEAKER")]
		Speaker,

		/// <summary>Alternative host.</summary>
		[EnumMember(Value = "ALTERNATIVE_HOST")]
		AlternativeHost,

		/// <summary>Panelist.</summary>
		[EnumMember(Value = "PANELIST")]
		Panelist,

		/// <summary>Interpreter.</summary>
		[EnumMember(Value = "INTERPRETER")]
		Interpreter,

		/// <summary>Sponsor.</summary>
		[EnumMember(Value = "SPONSOR")]
		Sponsor,

		/// <summary>Expo booth owner.</summary>
		[EnumMember(Value = "EXPO_BOOTH_OWNER")]
		ExpoBoothOwner,

		/// <summary>Moderator.</summary>
		[EnumMember(Value = "MODERATOR")]
		Moderator,

		/// <summary>Guest.</summary>
		[EnumMember(Value = "GUEST")]
		Guest,

		/// <summary>Original host.</summary>
		[EnumMember(Value = "ORIGINAL_HOST")]
		OriginalHost
	}
}
