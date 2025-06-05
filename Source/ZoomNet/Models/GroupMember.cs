using System.Text.Json.Serialization;

namespace ZoomNet.Models
{
	/// <summary>
	/// Group member.
	/// </summary>
	public class GroupMember
	{
		/// <summary>
		/// Gets or sets the member's unique identifier.
		/// </summary>
		[JsonPropertyName("id")]
		public string Id { get; set; }

		/// <summary>
		/// Gets or sets the member's email address.
		/// </summary>
		[JsonPropertyName("email")]
		public string EmailAddress { get; set; }

		/// <summary>
		/// Gets or sets the member's first name.
		/// </summary>
		[JsonPropertyName("first_name")]
		public string FirstName { get; set; }

		/// <summary>
		/// Gets or sets the member's last name.
		/// </summary>
		[JsonPropertyName("last_name")]
		public string LastName { get; set; }

		/// <summary>
		/// Gets or sets the member's type.
		/// </summary>
		[JsonPropertyName("type")]
		public GroupMemberType Type { get; set; }
	}
}
