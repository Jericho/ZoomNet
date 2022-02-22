using Newtonsoft.Json;

namespace ZoomNet.Models
{
	/// <summary>
	/// Survey details.
	/// </summary>
	public class SurveyDetails
	{
		/// <summary>
		/// Gets or sets a value indicating whether to allow participants to anonymously answer survey questions.
		/// </summary>
		[JsonProperty("anonymous")]
		public bool AllowAnonymous { get; set; }

		/// <summary>
		/// Gets or sets the survey questions.
		/// </summary>
		[JsonProperty(PropertyName = "questions")]
		public SurveyQuestion[] Questions { get; set; }
	}
}
