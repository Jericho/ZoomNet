using System.Text.Json.Serialization;

namespace ZoomNet.Models
{
	/// <summary>
	/// Call transfer/forward information.
	/// </summary>
	public class CallLogSite
	{
		/// <summary>
		/// Gets or sets the Id.
		/// </summary>
		/// <value>Target site in which the phone number was assigned.</value>
		[JsonPropertyName("id")]
		public string Id { get; set; }

		/// <summary>
		/// Gets or sets the name.
		/// </summary>
		/// <value>Name of the site where the phone number is assigned.</value>
		[JsonPropertyName("name")]
		public string Name { get; set; }
	}
}
