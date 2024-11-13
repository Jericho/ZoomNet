using System.Text.Json;
using ZoomNet.Models;
using ZoomNet.Models.CallHandlingSettings;

namespace ZoomNet.Json
{
	/// <summary>
	/// Converts a <see cref="Webinar">webinar</see> to or from JSON.
	/// </summary>
	/// <seealso cref="ZoomNetJsonConverter{T}"/>
	internal class CallHandlingSubsettingsConverter : ZoomNetJsonConverter<CallHandlingSubsettingsBase>
	{
		public override void Write(Utf8JsonWriter writer, CallHandlingSubsettingsBase value, JsonSerializerOptions options)
		{
			if (value == null)
			{
				writer.WriteNullValue();
			}
			else
			{
				switch (value.SubsettingType)
				{
					case CallHandlingSubsettingsType.CallForwarding:
						Serialize(writer, value, typeof(CallForwardingSubsettings), options, null);
						break;
					case CallHandlingSubsettingsType.Holiday:
						Serialize(writer, value, typeof(HolidaySubsettings), options, null);
						break;
					case CallHandlingSubsettingsType.CallHandling:
						Serialize(writer, value, typeof(CallHandlingSubsettings), options, null);
						break;
					case CallHandlingSubsettingsType.CustomHours:
						Serialize(writer, value, typeof(CustomHoursSubsettings), options, null);
						break;
					default:
						throw new JsonException($"{value.SubsettingType} is an unknown subsettings type");
				};
			}
		}
	}
}
