using System.Runtime.Serialization;

namespace ZoomNet.Models
{
	/// <summary>
	/// Role in purchase process.
	/// </summary>
	public enum RoleInPurchaseProcess
	{
		/// <summary>Unknown</summary>
		[EnumMember(Value = "")]
		Unknown,

		/// <summary>Decision Maker</summary>
		[EnumMember(Value = "Decision Maker")]
		Decision_Maker,

		/// <summary>Evaluator/Recommender</summary>
		[EnumMember(Value = "Evaluator/Recommender")]
		Evaluator_or_Recommender,

		/// <summary>Influencer</summary>
		[EnumMember(Value = "Influencer")]
		Influencer,

		/// <summary>Not Involved</summary>
		[EnumMember(Value = "Not involved")]
		Not_Involved,
	}
}
