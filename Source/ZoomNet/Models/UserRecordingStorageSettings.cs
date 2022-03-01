using Newtonsoft.Json;

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
		[JsonProperty(PropertyName = "allowed_values")]
		public Country[] AllowedCountries { get; set; }

		/// <summary>
		/// Gets or sets the country where user's recorded content such as meetings, webinars, and phone recordings, as well as voicemail, transcripts, and custom greeting prompts are stored.
		/// </summary>
		[JsonProperty(PropertyName = "value")]
		public Country Country { get; set; }
	}
}
