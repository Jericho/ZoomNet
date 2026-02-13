using System.Runtime.Serialization;

namespace ZoomNet.Models
{
	/// <summary>
	/// Specify an action for the content list.
	/// </summary>
	public enum SignageContentListAction
	{
		/// <summary>Add another content list.</summary>
		[EnumMember(Value = "add")]
		Add,

		/// <summary>Update existing content list.</summary>
		[EnumMember(Value = "update")]
		Update,

		/// <summary>Delete content list.</summary>
		[EnumMember(Value = "delete")]
		Delete,
	}
}
