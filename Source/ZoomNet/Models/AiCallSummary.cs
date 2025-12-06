using System;
using System.Text.Json.Serialization;

namespace ZoomNet.Models
{
	/// <summary>
	/// AI phone call summary.
	/// </summary>
	public class AiCallSummary
	{
		/// <summary>
		/// Gets or sets the id of AI call summary.
		/// </summary>
		[JsonPropertyName("ai_call_summary_id")]
		public string Id { get; set; }

		/// <summary>
		/// Gets or sets the call id.
		/// </summary>
		[JsonPropertyName("call_id")]
		public string CallId { get; set; }

		/// <summary>
		/// Gets or sets the owner's user id.
		/// </summary>
		[JsonPropertyName("user_id")]
		public string UserId { get; set; }

		/// <summary>
		/// Gets or sets the created date and time.
		/// </summary>
		[JsonPropertyName("created_time")]
		public DateTime CreatedOn { get; set; }

		/// <summary>
		/// Gets or sets the modified date and time.
		/// </summary>
		[JsonPropertyName("modified_time")]
		public DateTime? ModifiedOn { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether the call summary was edited.
		/// </summary>
		[JsonPropertyName("edited")]
		public bool IsModified { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether the call summary was deleted.
		/// </summary>
		[JsonPropertyName("deleted")]
		public bool IsDeleted { get; set; }

		/// <summary>
		/// Gets or sets the list of call log ids.
		/// </summary>
		[JsonPropertyName("call_log_ids")]
		public string[] CallLogIds { get; set; }
	}
}
