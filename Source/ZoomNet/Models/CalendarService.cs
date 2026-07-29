using System;
using System.Text.Json.Serialization;

namespace ZoomNet.Models
{
	/// <summary>
	/// Calendar service record.
	/// </summary>
	public class CalendarService
	{
		/// <summary>
		/// Gets or sets the id of the calendar service.
		/// </summary>
		[JsonPropertyName("calendar_service_id")]
		public string Id { get; set; }

		/// <summary>
		/// Gets or sets the name of the calendar service.
		/// </summary>
		[JsonPropertyName("calendar_service_name")]
		public string Name { get; set; }

		/// <summary>
		/// Gets or sets the total number of calendar resources.
		/// </summary>
		[JsonPropertyName("calendar_resource_total_number")]
		public int ResourcesCount { get; set; }

		/// <summary>
		/// Gets or sets the total number of assigned calendar resources.
		/// </summary>
		[JsonPropertyName("calendar_resource_assigned_number")]
		public int AssignedResourcesCount { get; set; }

		/// <summary>
		/// Gets or sets the date and time when the calendar service was added.
		/// </summary>
		[JsonPropertyName("added_date_time")]
		public DateTime AddedOn { get; set; }

		/// <summary>
		/// Gets or sets the date and time when the calendar service was last synced.
		/// </summary>
		[JsonPropertyName("latest_synced_date_time")]
		public DateTime LastSynchronizedOn { get; set; }
	}
}
