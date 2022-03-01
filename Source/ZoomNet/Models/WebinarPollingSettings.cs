using Newtonsoft.Json;

namespace ZoomNet.Models
{
	/// <summary>
	/// Webinar polling settings.
	/// </summary>
	public class WebinarPollingSettings
	{
		/// <summary>Gets or sets a value indicating whether to allow host to create advanced polls and quizzes.</summary>
		/// <remarks>Advanced polls and quizzes include single choice, multiple choice, drop down, matching, short answer, long answer, rank order, and fill-in-the-blank questions. Hosts can also set the correct answers for quizzes they create.</remarks>
		[JsonProperty(PropertyName = "advanced_polls")]
		public bool AllowAdvancedPolls { get; set; }

		/// <summary>Gets or sets a value indicating whether to allow the host to add polls before or during a webinar.</summary>
		[JsonProperty(PropertyName = "enable")]
		public bool AllowPolls { get; set; }
	}
}
