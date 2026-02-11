using System.Text.Json.Serialization;

namespace ZoomNet.Models
{
	/// <summary>
	/// AI phone call summary detail.
	/// </summary>
	public class AiCallSummaryDetail : AiCallSummary
	{
		/// <summary>Gets or sets the recap of the call summary.</summary>
		[JsonPropertyName("call_summary")]
		public string Summary { get; set; }

		/// <summary>
		/// Gets or sets the call summary rate.
		/// </summary>
		[JsonPropertyName("call_summary_rate")]
		public CallSummaryRate Rate { get; set; }

		/// <summary>Gets or sets the detailed version of the call summary.</summary>
		[JsonPropertyName("detailed_summary")]
		public string DetailedSummary { get; set; }

		/// <summary>Gets or sets the next step in the call summary.</summary>
		[JsonPropertyName("next_steps")]
		public string NextStep { get; set; }

		/// <summary>Gets or sets the transcription language ID.</summary>
		[JsonPropertyName("transcript_language")]
		public string TranscriptLanguage { get; set; }

		/// <summary>Gets a value indicating whether the entity is marked as deleted.</summary>
		public new bool IsDeleted => false;
	}
}
