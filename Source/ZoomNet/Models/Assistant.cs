using Newtonsoft.Json;

namespace ZoomNet.Models
{
	/// <summary>
	/// Assistant.
	/// </summary>
	public class Assistant
	{
		/// <summary>
		/// Gets or sets the assistant id.
		/// </summary>
		/// <value>
		/// The id.
		/// </value>
		[JsonProperty("id", NullValueHandling = NullValueHandling.Ignore)]
		public string Id { get; set; }

		/// <summary>
		/// Gets or sets the assistant's email address.
		/// </summary>
		[JsonProperty(PropertyName = "email")]
		public string Email { get; set; }
	}
}
