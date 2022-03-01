using Newtonsoft.Json;

namespace ZoomNet.Models
{
	/// <summary>
	/// Custom attribute.
	/// </summary>
	public class CustomAttribute
	{
		/// <summary>Gets or sets the unique identifier.</summary>
		[JsonProperty("key")]
		public string Key { get; set; }

		/// <summary>Gets or sets the name.</summary>
		[JsonProperty("name")]
		public string Name { get; set; }

		/// <summary>Gets or sets the value.</summary>
		[JsonProperty("value")]
		public string Value { get; set; }
	}
}
