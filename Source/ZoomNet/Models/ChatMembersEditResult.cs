namespace ZoomNet.Models
{
	using System;
	using System.Text.Json.Serialization;
	using ZoomNet.Json;

	/// <summary>
	/// Chat channel invitation result.
	/// </summary>
	public class ChatMembersEditResult
	{
		/// <summary>
		/// Gets or sets the user ids of the members added to the channel.
		/// The user ids of those who are not from the same account will be omitted from the list.
		/// </summary>
		[JsonPropertyName("ids")]
		[JsonConverter(typeof(DelimitedStringConverter))]
		public string[] Ids { get; set; }

		/// <summary>
		/// Gets or sets the member IDs of the members added to the channel.
		/// </summary>
		[JsonPropertyName("member_ids")]
		[JsonConverter(typeof(DelimitedStringConverter))]
		public string[] MemberIds { get; set; }

		/// <summary>
		/// Gets or sets the date and time when the member(s) are added to the channel.
		/// </summary>
		[JsonPropertyName("added_at")]
		public DateTime AddedAt { get; set; }
	}
}
