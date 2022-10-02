using System;
using System.Text.Json.Serialization;

namespace ZoomNet.Models
{
	/// <summary>
	/// Top issues across Zoom rooms for the given date range.
	/// </summary>
	public class IssuesOfZoomRoomsReport
	{
		/// <summary>
		/// Gets or sets the start date for this report.
		/// </summary>
		/// <value>The start date for this report.</value>
		[JsonPropertyName("from")]
		public DateTime From { get; set; }

		/// <summary>
		/// Gets or sets the end date for this report.
		/// </summary>
		/// <value>The end date for this report.</value>
		[JsonPropertyName("to")]
		public DateTime To { get; set; }

		/// <summary>
		/// Gets or sets the number of all records available across pages.
		/// </summary>
		/// <value>The number of all records available across pages.</value>
		[JsonPropertyName("total_records")]
		public int TotalRecords { get; set; }

		/// <summary>
		/// Gets or sets the collection of Zoom room issues.
		/// </summary>
		/// <value>The collection of Zoom room issues.</value>
		[JsonPropertyName("issues")]
		public IssuesOfZoomRooms[] Issues { get; set; }
	}
}
