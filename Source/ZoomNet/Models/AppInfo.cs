using System.Text.Json.Serialization;

namespace ZoomNet.Models
{
	/// <summary>
	/// A marketplace app.
	/// </summary>
	public class AppInfo
	{
		/// <summary>Gets or sets the app id.</summary>
		[JsonPropertyName("app_id")]
		public string Id { get; set; }

		/// <summary>Gets or sets the name of the app.</summary>
		[JsonPropertyName("app_name")]
		public string Name { get; set; }

		/// <summary>Gets or sets the status of the app.</summary>
		[JsonPropertyName("app_status")]
		public AppStatus Status { get; set; }

		/// <summary>Gets or sets the type of the app.</summary>
		[JsonPropertyName("app_type")]
		public AppType Type { get; set; }

		/// <summary>Gets or sets the app's usage categorization.</summary>
		[JsonPropertyName("app_usage")]
		public AppCategorization Categorization { get; set; }

		/// <summary>Gets or sets the app's description.</summary>
		[JsonPropertyName("app_description")]
		public string Description { get; set; }

		/// <summary>Gets or sets the app's links.</summary>
		[JsonPropertyName("app_links")]
		public AppLinks Links { get; set; }

		/// <summary>Gets or sets the app's permissions.</summary>
		[JsonPropertyName("app_permissions")]
		public AppPermission[] Permissions { get; set; }

		/// <summary>Gets or sets the app's requirements.</summary>
		//[JsonPropertyName("app_requirements")]
		//public AppRequirements Requirements { get; set; }

		/// <summary>Gets or sets the app's scopes.</summary>
		[JsonPropertyName("app_scopes")]
		public string[] Scopes { get; set; }
	}
}
