using System.Runtime.Serialization;

namespace ZoomNet.Models
{
	/// <summary>
	/// Interpretation sign language.
	/// </summary>
	public enum InterpretationSignLanguage
	{
		/// <summary>American Sign Language.</summary>
		[EnumMember(Value = "ASE")]
		American,

		/// <summary>Chinese sign language.</summary>
		[EnumMember(Value = "CSL")]
		Chinese,

		/// <summary>French sign language.</summary>
		[EnumMember(Value = "FSL")]
		French,

		/// <summary>German sign language.</summary>
		[EnumMember(Value = "GSG")]
		German,

		/// <summary>Japanese sign language.</summary>
		[EnumMember(Value = "JSL")]
		Japanese,

		/// <summary>Russian sign language.</summary>
		[EnumMember(Value = "RSL")]
		Russian,

		/// <summary>Brazilian sign language.</summary>
		[EnumMember(Value = "BZS")]
		Brazilian,

		/// <summary>Spanish sign language.</summary>
		[EnumMember(Value = "SSP")]
		Spanish,

		/// <summary>Mexican sign language.</summary>
		[EnumMember(Value = "MFS")]
		Mexican,

		/// <summary>British sign language.</summary>
		[EnumMember(Value = "BFI")]
		British,
	}
}
