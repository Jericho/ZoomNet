using System;
using System.Net;
using System.Net.Http;
using System.Text.Json.Serialization;
using ZoomNet.Json;

namespace ZoomNet.Models
{
	/// <summary>
	/// An API log.
	/// </summary>
	public class ApiLog
	{
		/// <summary>Gets or sets the URL pattern.</summary>
		[JsonPropertyName("url_pattern")]
		public string UrlPattern { get; set; }

		/// <summary>Gets or sets the time of the API log.</summary>
		[JsonPropertyName("time")]
		[JsonConverter(typeof(EpochConverter))]
		public DateTime Date { get; set; }

		/// <summary>Gets or sets the status of the call.</summary>
		[JsonPropertyName("http_status")]
		public HttpStatusCode Status { get; set; }

		/// <summary>Gets or sets the HTTP method of the call.</summary>
		[JsonPropertyName("method")]
		public HttpMethod Method { get; set; }

		/// <summary>Gets or sets the trace ID of the call.</summary>
		[JsonPropertyName("trace_id")]
		public string TraceId { get; set; }
	}
}
