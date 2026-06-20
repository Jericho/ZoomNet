using System;
using System.Text.Json.Serialization;

namespace ZoomNet.Models
{
	/// <summary>Custom emoji.</summary>
	public class CustomEmoji
	{
		/// <summary>Gets or sets the custom emoji id.</summary>
		[JsonPropertyName("file_id")]
		public string Id { get; set; }

		/// <summary>Gets or sets the custom emoji's name.</summary>
		[JsonPropertyName("name")]
		public string Name { get; set; }

		/// <summary>Gets or sets the custom emoji's user id.</summary>
		[JsonPropertyName("user_id")]
		public string UserId { get; set; }

		/// <summary>Gets or sets the custom emoji's member id.</summary>
		[JsonPropertyName("member_id")]
		public string MemberId { get; set; }

		/// <summary>Gets or sets the custom emoji's user name.</summary>
		[JsonPropertyName("user_name")]
		public string UserName { get; set; }

		/// <summary>Gets or sets the custom emoji's user email.</summary>
		[JsonPropertyName("user_email")]
		public string UserEmail { get; set; }

		/// <summary>Gets or sets the custom emoji's date added.</summary>
		[JsonPropertyName("date_added")]
		public DateTime DateAdded { get; set; }
	}
}
