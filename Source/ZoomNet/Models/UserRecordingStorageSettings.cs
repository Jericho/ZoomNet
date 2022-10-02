using System.Text.Json.Serialization;

namespace ZoomNet.Models
{
	/// <summary>
	/// User recording storage settings.
	/// </summary>
	public class UserRecordingStorageSettings
	{
		/// <summary>
		/// Gets or sets the list of countries where user's recorded content such as meetings, webinars, and phone recordings, as well as voicemail, transcripts, and custom greeting prompts can be stored.
		/// </summary>
		[JsonPropertyName("allowed_values")]
		public Country[] AllowedCountries { get; set; }

		/// <summary>
		/// Gets or sets the country where user's recorded content such as meetings, webinars, and phone recordings, as well as voicemail, transcripts, and custom greeting prompts are stored.
		/// </summary>
		[JsonPropertyName("value")]
		public Country Country { get; set; }
	}
}
