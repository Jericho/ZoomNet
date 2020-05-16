using Newtonsoft.Json;
using StrongGrid.Models;
using System;

namespace ZoomNet.Models
{
	/// <summary>
	/// A webinar.
	/// </summary>
	public abstract class Webinar
	{
		/// <summary>
		/// Gets or sets the unique id.
		/// </summary>
		/// <value>
		/// The unique id.
		/// </value>
		[JsonProperty("uuid", NullValueHandling = NullValueHandling.Ignore)]
		public string Uuid { get; set; }

		/// <summary>
		/// Gets or sets the webinar id, also known as the webinar number.
		/// </summary>
		/// <value>
		/// The id.
		/// </value>
		[JsonProperty("id", NullValueHandling = NullValueHandling.Ignore)]
		public long Id { get; set; }

		/// <summary>
		/// Gets or sets the ID of the user who is set as the host of the webinar.
		/// </summary>
		/// <value>
		/// The user id.
		/// </value>
		[JsonProperty("host_id", NullValueHandling = NullValueHandling.Ignore)]
		public string HostId { get; set; }

		/// <summary>
		/// Gets or sets the topic of the meeting.
		/// </summary>
		/// <value>
		/// The topic.
		/// </value>
		[JsonProperty("topic", NullValueHandling = NullValueHandling.Ignore)]
		public string Topic { get; set; }

		/// <summary>
		/// Gets or sets the webinar type.
		/// </summary>
		/// <value>The webinar type.</value>
		[JsonProperty(PropertyName = "type", NullValueHandling = NullValueHandling.Ignore)]
		public WebinarType Type { get; set; }

		/// <summary>
		/// Gets or sets the duration in minutes.
		/// </summary>
		/// <value>The duration in minutes.</value>
		[JsonProperty(PropertyName = "duration", NullValueHandling = NullValueHandling.Ignore)]
		public int Duration { get; set; }

		/// <summary>
		/// Gets or sets the date and time when the meeting was created.
		/// </summary>
		/// <value>The meeting created time.</value>
		[JsonProperty(PropertyName = "created_at", NullValueHandling = NullValueHandling.Ignore)]
		public DateTime CreatedOn { get; set; }

		/// <summary>
		/// Gets or sets the URL to join the webinar.
		/// </summary>
		/// <value>The join URL.</value>
		[JsonProperty(PropertyName = "join_url", NullValueHandling = NullValueHandling.Ignore)]
		public string JoinUrl { get; set; }
	}
}
