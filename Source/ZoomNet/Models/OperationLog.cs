using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ZoomNet.Models
{
	/// <summary>
	/// Represents a log entry for an operation performed in Zoom.
	/// </summary>
	public class OperationLog
	{
		/// <summary>Gets or sets the action.</summary>
		[JsonPropertyName("action")]
		public string Action { get; set; }

		/// <summary>Gets or sets the operation type.</summary>
		[JsonPropertyName("category_type")]
		public ReportPhoneOperationsLogType CategoryType { get; set; }

		/// <summary>Gets or sets the operation detail.</summary>
		[JsonPropertyName("operation_detail")]
		public string OperationDetail { get; set; }

		/// <summary>Gets or sets the Operator.</summary>
		[JsonPropertyName("operator")]
		public string Operator { get; set; }

		/// <summary>Gets or sets the timestamp.</summary>
		[JsonPropertyName("time")]
		public DateTime Time { get; set; }
	}
}
