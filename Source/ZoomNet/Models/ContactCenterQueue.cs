using System;
using System.Text.Json.Serialization;

namespace ZoomNet.Models
{
	/// <summary>
	/// A Contact Center queue.
	/// </summary>
	public class ContactCenterQueue
	{
		/// <summary>Gets or sets the number of assigned agents.</summary>
		[JsonPropertyName("agents_count")]
		public int AgentsCount { get; set; }

		/// <summary>Gets or sets the unique identifier.</summary>
		[JsonPropertyName("cc_queue_id")]
		public string Id { get; set; }

		/// <summary>Gets or sets the queue's channel.</summary>
		[JsonPropertyName("channel")]
		public ContactCenterQueueChannel Channel { get; set; }

		/// <summary>Gets or sets the date and time when the queue was last modified.</summary>
		[JsonPropertyName("last_modified_time")]
		public DateTime ModifiedOn { get; set; }

		/// <summary>Gets or sets the ID of the user that last modified the queue.</summary>
		[JsonPropertyName("modified_by")]
		public string ModifiedBy { get; set; }

		/// <summary>Gets or sets the queue's name.</summary>
		[JsonPropertyName("queue_name")]
		public string Name { get; set; }

		/// <summary>Gets or sets the number of assigned supervisors.</summary>
		[JsonPropertyName("supervisors_count")]
		public int SupervisorsCount { get; set; }
	}
}
