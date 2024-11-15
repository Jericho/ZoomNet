using System.Text.Json.Serialization;

namespace ZoomNet.Models.CallHandlingSettings
{
	/// <summary>
	/// Base call handling sub-settings model.
	/// </summary>
	[JsonDerivedType(typeof(CallForwardingSubsettings))]
	[JsonDerivedType(typeof(CallHandlingSubsettings))]
	[JsonDerivedType(typeof(CustomHoursSubsettings))]
	[JsonDerivedType(typeof(HolidaySubsettings))]
	public abstract class CallHandlingSubsettingsBase
	{
		/// <summary>
		/// Gets the type of sub-setting.
		/// </summary>
		[JsonIgnore]
		public abstract CallHandlingSubsettingsType SubsettingType { get; }
	}
}
