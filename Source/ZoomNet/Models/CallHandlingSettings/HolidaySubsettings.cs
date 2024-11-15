using System;
using System.Text.Json.Serialization;

namespace ZoomNet.Models.CallHandlingSettings
{
	/// <summary>
	/// Call handling holiday subsettings.
	/// </summary>
	public class HolidaySubsettings : CallHandlingSubsettingsBase
	{
		/// <summary>
		/// Gets or sets the holiday's start date and time in yyyy-MM-dd'T'HH:mm:ss'Z' format.
		/// </summary>
		[JsonPropertyName("from")]
		public DateTime? From { get; set; }

		/// <summary>
		/// Gets or sets the holiday's end date and time in yyyy-MM-dd'T'HH:mm:ss'Z' format.
		/// </summary>
		[JsonPropertyName("to")]
		public DateTime? To { get; set; }

		/// <summary>
		/// Gets or sets the name of the holiday.
		/// </summary>
		[JsonPropertyName("name")]
		public string Name { get; set; }

		/// <summary>
		/// Gets or sets the holiday's ID.
		/// </summary>
		[JsonPropertyName("holiday_id")]
		public string HolidayId { get; set; }

		/// <summary>
		/// Gets the type of sub-setting.
		/// </summary>
		[JsonIgnore]
		public override CallHandlingSubsettingsType SubsettingType => CallHandlingSubsettingsType.Holiday;
	}
}
