using System.Text.Json.Serialization;

namespace ZoomNet.Models
{
	/// <summary>Settings for the Questions and Answers for webinar.</summary>
	public class QuestionsAndAnswersSettings
	{
		/// <summary>Gets or sets a value indicating whether to allow submitting questions.</summary>
		[JsonPropertyName("allow_submit_questions")]
		public bool AllowSubmitQuestions { get; set; }

		/// <summary>Gets or sets a value indicating whether to allow anonymous questions.</summary>
		[JsonPropertyName("allow_anonymous_questions")]
		public bool AllowAnonymousQuestions { get; set; }

		/// <summary>Gets or sets a value indicating whether you want attendees to be able to view answered questions only or view all questions.</summary>
		[JsonPropertyName("answer_questions")]
		public ViewQuestionsType ViewdQuestions { get; set; }

		/// <summary>Gets or sets a value indicating whether to allow attendees to comment.</summary>
		[JsonPropertyName("attendees_can_comment")]
		public bool AttendeesCanComment { get; set; }

		/// <summary>Gets or sets a value indicating whether to allow attendees to upvote questions.</summary>
		[JsonPropertyName("attendees_can_upvote")]
		public bool AttendeesCanUpvote { get; set; }

		/// <summary>Gets or sets a value indicating whether to allow auto-reply to attendees.</summary>
		/// <remarks>Auto-reply is only available for simulive webinars.</remarks>
		[JsonPropertyName("allow_auto_reply")]
		public bool AllowAutoReply { get; set; }

		/// <summary>Gets or sets the text to be included in the automatic response.</summary>
		[JsonPropertyName("auto_reply_text")]
		public string AutoReplyText { get; set; }

		/// <summary>Gets or sets a value indicating whether to enable the Questions and Answers feature.</summary>
		[JsonPropertyName("enable")]
		public bool Enabled { get; set; }
	}
}
