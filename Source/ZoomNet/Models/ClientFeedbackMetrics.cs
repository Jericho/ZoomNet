using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZoomNet.Models
{
	/// <summary>
	/// Metrics for a feedback item.
	/// </summary>
	public class ClientFeedbackMetrics
	{
		/// <summary>
		/// Gets or sets the feedback id.
		/// </summary>
		/// <value>The feedback id.</value>
		[JsonProperty(PropertyName = "feedback_id")]
		public string FeedbackId { get; set; }

		/// <summary>
		/// Gets or sets the feedback name.
		/// </summary>
		/// <value>The feedback name.</value>
		[JsonProperty(PropertyName = "feedback_name")]
		public string FeebackName { get; set; }

		/// <summary>
		/// Gets or sets the participant count.
		/// </summary>
		/// <value>The number of participants that upvoted the feedback.</value>
		[JsonProperty(PropertyName = "participants_count")]
		public int ParticipantsCount { get; set; }
	}
}
