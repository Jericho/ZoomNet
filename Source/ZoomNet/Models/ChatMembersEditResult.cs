using System;
using System.Text.Json.Serialization;

namespace ZoomNet.Models
{
	/// <summary>
	/// Chat channel invitation result.
	/// </summary>
	public class ChatMembersEditResult
	{
		/// <summary>
		/// Gets or sets the comma-separated user IDs of the members added to the channel.
		/// The user IDs of those who are not from the same account will be omitted from the list.
		/// </summary>
		[JsonPropertyName("ids")]
		public string Ids { get; set; }

		/// <summary>
		/// Gets or sets the comma-separated member IDs of the members added to the channel.
		/// </summary>
		[JsonPropertyName("member_ids")]
		public string MemberIds { get; set; }

		/// <summary>
		/// Gets or sets the date and time when the member(s) are added to the channel.
		/// </summary>
		[JsonPropertyName("added_at")]
		public DateTime AddedAt { get; set; }
	}
}
