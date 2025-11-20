using System.Runtime.Serialization;

namespace ZoomNet.Models
{
	/// <summary>
	/// Phone call connection type.
	/// </summary>
	public enum PhoneCallConnectionType
	{
		/// <summary>
		/// From phone (off-net).
		/// </summary>
		[EnumMember(Value = "pstn_off_net")]
		PstnOffNet,

		/// <summary>
		/// Voice over IP.
		/// </summary>
		[EnumMember(Value = "voip")]
		Voip,

		/// <summary>
		/// PSTN over net.
		/// </summary>
		[EnumMember(Value = "pstn_on_net")]
		PstnOnNet,

		/// <summary>
		/// From contact center.
		/// </summary>
		[EnumMember(Value = "contact_center")]
		ContactCenter,

		/// <summary>
		/// Bring your own PBX.
		/// </summary>
		[EnumMember(Value = "byop")]
		Byop,
	}
}
