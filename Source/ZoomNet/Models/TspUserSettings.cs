using System.Text.Json.Serialization;

namespace ZoomNet.Models
{
	/// <summary>
	/// TSP user settings.
	/// </summary>
	public class TspUserSettings
	{
		/// <summary>Gets or sets a value indicating whether call out or not.</summary>
		[JsonPropertyName("call_out")]
		public bool CallOut { get; set; }

		/// <summary>Gets or sets the call out countries/regions.</summary>
		[JsonPropertyName("call_out_countries")]
		public string[] CallOutCountries { get; set; }

		/// <summary>Gets or sets a value indicating whether to show the international numbers link on the invitation email.</summary>
		[JsonPropertyName("show_international_numbers_link")]
		public bool ShowInternationalNumbersInInvitation { get; set; }
	}
}
