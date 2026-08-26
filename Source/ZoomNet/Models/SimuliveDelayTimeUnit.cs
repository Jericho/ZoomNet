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
	}
}
