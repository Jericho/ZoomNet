using Newtonsoft.Json;
using System;

namespace ZoomNet.Models
{
	/// <summary>
	/// A meeting instance that occured in the past.
	/// </summary>
	public class PastMeetingInstance
	{
		/// <summary>
		/// Gets or sets the meeting uuid.
		/// </summary>
		/// <value>
		/// The uuid.
		/// </value>
		[JsonProperty("uuid", NullValueHandling = NullValueHandling.Ignore)]
		public string Uuid { get; set; }

		/// <summary>
		/// Gets or sets the date and time when the meeting instance started.
		/// </summary>
		/// <value>The meeting start time.</value>
		[JsonProperty(PropertyName = "start_time", NullValueHandling = NullValueHandling.Ignore)]
		public DateTime StartedOn { get; set; }
	}
}
