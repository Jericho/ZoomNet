using System.Runtime.Serialization;

namespace ZoomNet.Models
{
	/// <summary>
	/// Enumeration to indicate the type of contact.
	/// </summary>
	public enum ContactType
	{
		/// <summary>
		/// Contacts from the user's organization.
		/// </summary>
		[EnumMember(Value = "company")]
		Internal,

		/// <summary>
		/// External contacts.
		/// </summary>
		[EnumMember(Value = "external")]
		External
	}
}
