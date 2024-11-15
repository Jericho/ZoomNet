using System;
using System.Text.Json.Serialization;

namespace ZoomNet.Models.CallHandlingSettings
{
	/// <summary>
	/// Custom hours child subsettings.
	/// </summary>
	public class CustomHoursChildSubsettings
	{
		/// <summary>
		/// Gets or sets custom hours start time in HH:mm format.
		/// </summary>
		[JsonPropertyName("from")]
		public string From { get; set; }

		/// <summary>
		/// Gets or sets custom hours end time in HH:mm format.
		/// </summary>
		[JsonPropertyName("to")]
		public string To { get; set; }

		/// <summary>
		/// Gets or sets the type of custom hours.
		/// </summary>
		[JsonPropertyName("type")]
		public CustomHoursType? Type { get; set; }

		/// <summary>
		/// Gets or sets the day of the week.
		/// </summary>
		[JsonPropertyName("weekday")]
		public DayOfWeek? Weekday { get; set; }
	}
}
