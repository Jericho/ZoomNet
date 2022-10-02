using System.Text.Json.Serialization;

namespace ZoomNet.Models
{
	/// <summary>
	/// Survey.
	/// </summary>
	public class Survey
	{
		/// <summary>
		/// Gets or sets the link to the third party meeting survey.
		/// </summary>
		[JsonPropertyName("third_party_survey")]
		public string ThirdPartySurveyLink { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether the survey will be displayed in the attendee's browser.
		/// </summary>
		[JsonPropertyName("show_in_the_browser")]
		public bool ShowInBrowser { get; set; }

		/// <summary>
		/// Gets or sets the information about the customized survey.
		/// </summary>
		[JsonPropertyName("custom_survey")]
		public SurveyDetails Details { get; set; }
	}
}
