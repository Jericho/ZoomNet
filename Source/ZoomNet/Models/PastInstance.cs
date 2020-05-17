using Newtonsoft.Json;
using System;

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
		[JsonProperty("uuid", NullValueHandling = NullValueHandling.Ignore)]
		public string Uuid { get; set; }

		/// <summary>
		/// Gets or sets the date and time when the instance started.
		/// </summary>
		/// <value>The start time.</value>
		[JsonProperty(PropertyName = "start_time", NullValueHandling = NullValueHandling.Ignore)]
		public DateTime StartedOn { get; set; }
	}
}
