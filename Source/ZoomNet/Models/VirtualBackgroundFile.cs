using Newtonsoft.Json;

namespace ZoomNet.Models
{
	/// <summary>
	/// Virtual background file.
	/// </summary>
	public class VirtualBackgroundFile
	{
		/// <summary>Gets or sets the unique identifier.</summary>
		[JsonProperty(PropertyName = "id")]
		public string Id { get; set; }

		/// <summary>Gets or sets a value indicating whether this file is the default virtual background.</summary>
		[JsonProperty(PropertyName = "is_default")]
		public bool IsDefault { get; set; }

		/// <summary>Gets or sets name.</summary>
		[JsonProperty(PropertyName = "name")]
		public string Name { get; set; }

		/// <summary>Gets or sets the size.</summary>
		[JsonProperty(PropertyName = "size")]
		public string Size { get; set; }

		/// <summary>Gets or sets the type.</summary>
		[JsonProperty(PropertyName = "type")]
		public VirtualBackgroundType Type { get; set; }
	}
}
