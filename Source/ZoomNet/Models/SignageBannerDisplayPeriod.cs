using System.Text.Json.Serialization;

namespace ZoomNet.Models
{
	/// <summary>
	/// Allows you to define the number of minutes before or after the scheduled meeting time you would like the content to display.
	/// </summary>
	public class SignageBannerDisplayPeriod
	{
		/// <summary>
		/// Gets or sets the number of minutes before the meeting starts.
		/// </summary>
		[JsonPropertyName("start_displaying_content")]
		public int StartDisplayingContent { get; set; }

		/// <summary>
		/// Gets or sets the number of minutes after the meeting ends.
		/// </summary>
		[JsonPropertyName("stop_displaying_content")]
		public int StopDisplayingContent { get; set; }
	}
}
