using System.Text.Json.Serialization;

namespace ZoomNet.Models
{
	/// <summary>Call transfer/forward information.</summary>
	public class CallLogSite
	{
		/// <summary>Gets or sets the Id.</summary>
		[JsonPropertyName("id")]
		public string Id { get; set; }

		/// <summary>Gets or sets the name.</summary>
		[JsonPropertyName("name")]
		public string Name { get; set; }
	}
}
