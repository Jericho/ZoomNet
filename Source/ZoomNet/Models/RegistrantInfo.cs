using Newtonsoft.Json;
using System;

namespace ZoomNet.Models
{
	/// <summary>
	/// Registrant info.
	/// </summary>
	public class RegistrantInfo
	{
		/// <summary>
		/// Gets or sets the registrant id.
		/// </summary>
		/// <value>
		/// The id.
		/// </value>
		[JsonProperty("registrant_id")]
		public string Id { get; set; }

		/// <summary>
		/// Gets or sets the webinar id.
		/// </summary>
		[JsonProperty(PropertyName = "id")]
		public long WebinarId { get; set; }

		/// <summary>
		/// Gets or sets the start time.
		/// </summary>
		[JsonProperty(PropertyName = "start_time")]
		public DateTime StartTime { get; set; }

		/// <summary>
		/// Gets or sets the URL for this registrant to join the webinar.
		/// </summary>
		[JsonProperty(PropertyName = "join_url")]
		public string JoinUrl { get; set; }
	}
}
