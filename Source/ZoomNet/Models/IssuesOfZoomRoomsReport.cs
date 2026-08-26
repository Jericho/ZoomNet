using System;
using System.Text.Json.Serialization;

namespace ZoomNet.Models
{
	/// <summary>Top issues across Zoom rooms for the given date range.</summary>
	public class IssuesOfZoomRoomsReport
	{
		/// <summary>Gets or sets the start date for this report.</summary>
		[JsonPropertyName("from")]
		public DateTime From { get; set; }

		/// <summary>Gets or sets the collection of Zoom room issues.</summary>
		[JsonPropertyName("issues")]
		public IssuesOfZoomRooms[] Issues { get; set; }

		/// <summary>Gets or sets the end date for this report.</summary>
		[JsonPropertyName("to")]
		public DateTime To { get; set; }

		/// <summary>Gets or sets the number of all records available across pages.</summary>
		[JsonPropertyName("total_records")]
		public int TotalRecords { get; set; }
	}
}
