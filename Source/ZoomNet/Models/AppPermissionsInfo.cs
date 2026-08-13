using System.Text.Json.Serialization;

namespace ZoomNet.Models
{
	/// <summary>
	/// App permissions info.
	/// </summary>
	public class AppPermissionsInfo
	{
		/// <summary>Gets or sets the group.</summary>
		[JsonPropertyName("group")]
		public string Group { get; set; }

		/// <summary>Gets or sets the group message.</summary>
		[JsonPropertyName("groupMessage")]
		public string GroupMessage { get; set; }

		/// <summary>Gets or sets the permissions.</summary>
		[JsonPropertyName("permissions")]
		[JsonConverter(typeof(Json.AppPermissionsConverter))]
		public string[] Permissions { get; set; }

		/// <summary>Gets or sets the title.</summary>
		[JsonPropertyName("title")]
		public string Title { get; set; }
	}
}
