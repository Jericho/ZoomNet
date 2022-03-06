using System;
using System.Text.Json.Serialization;

namespace ZoomNet.Models
{
	/// <summary>
	/// A meeting or webinar instance that occured in the past.
	/// </summary>
	public class PastInstance
	{
		/// <summary>
		/// Gets or sets the uuid.
		/// </summary>
		/// <value>
		/// The uuid.
		/// </value>
		[JsonPropertyName("uuid")]
		public string Uuid { get; set; }

		/// <summary>
		/// Gets or sets the date and time when the instance started.
		/// </summary>
		/// <value>The start time.</value>
		[JsonPropertyName("start_time")]
		public DateTime StartedOn { get; set; }
	}
}
