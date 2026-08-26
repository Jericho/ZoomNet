using System;
using System.Text.Json.Serialization;

namespace ZoomNet.Models
{
	/// <summary>Report with client feedback metrics for a given range.</summary>
	public class ClientFeedbackMetricsReport
	{
		/// <summary>Gets or sets the collection of client feedback metrics.</summary>
		[JsonPropertyName("client_feedbacks")]
		public ClientFeedbackMetrics[] ClientFeedbacks { get; set; }

		/// <summary>Gets or sets the start date for this report.</summary>
		[JsonPropertyName("from")]
		public DateTime From { get; set; }

		/// <summary>Gets or sets the end date for this report.</summary>
		[JsonPropertyName("to")]
		public DateTime To { get; set; }

		/// <summary>Gets or sets the number of all records available across pages.</summary>
		[JsonPropertyName("total_records")]
		public int TotalRecords { get; set; }
	}
}
