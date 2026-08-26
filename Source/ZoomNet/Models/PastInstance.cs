using System;
using System.Text.Json.Serialization;

namespace ZoomNet.Models
{
	/// <summary>A meeting or webinar instance that occured in the past.</summary>
	public class PastInstance
	{
		/// <summary>Gets or sets the date and time when the instance started.</summary>
		[JsonPropertyName("start_time")]
		public DateTime StartedOn { get; set; }

		/// <summary>Gets or sets the uuid.</summary>
		[JsonPropertyName("uuid")]
		public string Uuid { get; set; }
	}
}
