using Newtonsoft.Json;

namespace ZoomNet.Models
{
	/// <summary>
	/// Authentication options.
	/// </summary>
	public class AuthenticationOptions
	{
		/// <summary>
		/// Gets or sets the unique identifier.
		/// </summary>
		[JsonProperty(PropertyName = "id")]
		public string Id { get; set; }

		/// <summary>
		/// Gets or sets the name.
		/// </summary>
		[JsonProperty(PropertyName = "name")]
		public string Name { get; set; }

		/// <summary>
		/// Gets or sets the authentication type.
		/// </summary>
		[JsonProperty(PropertyName = "type")]
		public AuthenticationType Type { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether the authentication is visible.
		/// </summary>
		[JsonProperty(PropertyName = "visible")]
		public bool Visible { get; set; }

		/// <summary>
		/// Gets or sets the domains.
		/// </summary>
		[JsonProperty(PropertyName = "domains")]
		public string Domains { get; set; }
	}
}
