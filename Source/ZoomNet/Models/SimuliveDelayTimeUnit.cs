using System.ComponentModel;
using System.Runtime.Serialization;

namespace ZoomNet.Models
{
	/// <summary>Simulive delay time unit.</summary>
	public enum SimuliveDelayTimeUnit
	{
		/// <summary>Seconds.</summary>
		[EnumMember(Value = "second")]
		Seconds,

		/// <summary>Minutes.</summary>
		[EnumMember(Value = "minute")]
		Minutes,

		/// <summary>This is a bogus value to match the data in sample JSON in the Zoom API documentation. It is not a valid value and should never be used.</summary>
		[EnumMember(Value = "second or minute")]
		[EditorBrowsable(EditorBrowsableState.Never)]
		Bogus,
	}
}
