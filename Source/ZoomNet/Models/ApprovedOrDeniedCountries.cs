using System.Text.Json.Serialization;

namespace ZoomNet.Models
{
	/// <summary>Approved or Denied Countries.</summary>
	public class ApprovedOrDeniedCountries
	{
		/// <summary>Gets or sets the list of approved countries or regions.</summary>
		[JsonPropertyName("approved_list")]
		public string[] Approved { get; set; }

		/// <summary>Gets or sets the list of denied countries or regions.</summary>
		[JsonPropertyName("denied_list")]
		public string[] Denied { get; set; }

		/// <summary>Gets or sets a value indicating whether the feature is enabled.</summary>
		[JsonPropertyName("enable")]
		public bool Enabled { get; set; }

		/// <summary>Gets or sets the value indicating the method used.</summary>
		[JsonPropertyName("method")]
		public string Method { get; set; }
	}
}
