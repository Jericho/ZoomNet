using System.Runtime.Serialization;

namespace ZoomNet.Models
{
	/// <summary>
	/// Enumeration to indicate room device type.
	/// </summary>
	public enum RoomSensorType
	{
		/// <summary>Carbon dioxide.</summary>
		[EnumMember(Value = "CO2")]
		CarbonDioxide,

		/// <summary>Temperature.</summary>
		[EnumMember(Value = "TEMPERATURE")]
		Temperature,

		/// <summary>Real time people count.</summary>
		[EnumMember(Value = "REAL_TIME_PEOPLE_COUNT")]
		RealTimePeopleCount,

		/// <summary>Humidity.</summary>
		[EnumMember(Value = "HUMIDITY")]
		Humidity,

		/// <summary>Volatile organic compound.</summary>
		[EnumMember(Value = "VOC")]
		VolatileOrganicCompound
	}
}
