using System.Text.Json.Serialization;

namespace ZoomNet.Models.PhoneAccountSettings
{
	/// <summary>
	/// Timezone information.
	/// </summary>
	public class TimezoneInfo
	{
		/// <summary>
		/// Gets or sets the timezone id.
		/// </summary>
		[JsonPropertyName("id")]
		public TimeZones Id { get; set; }

		/// <summary>
		/// Gets or sets the timezone name.
		/// </summary>
		[JsonPropertyName("name")]
		public string Name { get; set; }
	}
}
