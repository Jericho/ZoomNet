using System.Runtime.Serialization;

namespace ZoomNet.Models
{
	/// <summary>
	/// Enumeration to indicate a skill category type.
	/// </summary>
	public enum ContactCenterSkillType
	{
		/// <summary>Text.</summary>
		[EnumMember(Value = "text")]
		Text,

		/// <summary>Proficiency.</summary>
		[EnumMember(Value = "proficiency")]
		Proficiency,
	}
}
