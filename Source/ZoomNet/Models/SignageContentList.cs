using System;
using System.Text.Json.Serialization;

namespace ZoomNet.Models
{
	/// <summary>Content list.</summary>
	public record SignageContentList
	{
		/// <summary>Gets or sets the action.</summary>
		[JsonPropertyName("action")]
		public SignageContentListAction Action { get; init; }

		/// <summary>Gets or sets the number contents.</summary>
		[JsonPropertyName("contents")]
		public SignageContentItem[] Contents { get; init; }

		/// <summary>Gets or sets the display end time for the content list in GMT.</summary>
		[JsonPropertyName("end_time")]
		public DateTime EndTime { get; init; }

		/// <summary>Gets or sets the unique identifier of the content list.</summary>
		[JsonPropertyName("id")]
		public string Id { get; init; }

		/// <summary>Gets or sets the name of the content list.</summary>
		[JsonPropertyName("name")]
		public string Name { get; init; }

		/// <summary>Gets or sets the display start time for the content list in GMT.</summary>
		[JsonPropertyName("start_time")]
		public DateTime StartTime { get; init; }
	}
}
