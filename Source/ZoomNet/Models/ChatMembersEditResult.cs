namespace ZoomNet.Models
{
	using System;
	using System.Diagnostics;
	using System.Text.Json.Serialization;

	/// <summary>
	/// Chat channel invitation result.
	/// </summary>
	public class ChatMembersEditResult
	{
		/// <summary>
		/// Gets or sets the user ids of the members added to the channel.
		/// The user ids of those who are not from the same account will be omitted from the list.
		/// </summary>
		[JsonIgnore]
		public string[] Ids
		{
			get => CommaSeparatedIds?.Split(',') ?? [];
			set => CommaSeparatedIds = string.Join(",", value ?? []);
		}

		/// <summary>
		/// Gets or sets the member IDs of the members added to the channel.
		/// </summary>
		[JsonIgnore]
		public string[] MemberIds
		{
			get => CommaSeparatedMemberIds?.Split(',') ?? [];
			set => CommaSeparatedMemberIds = string.Join(",", value ?? []);
		}

		/// <summary>
		/// Gets or sets the comma-separated user ids of the members added to the channel.
		/// The user ids of those who are not from the same account will be omitted from the list.
		/// </summary>
		[JsonPropertyName("ids")]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public string CommaSeparatedIds { get; set; }

		/// <summary>
		/// Gets or sets the comma-separated member IDs of the members added to the channel.
		/// </summary>
		[JsonPropertyName("member_ids")]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public string CommaSeparatedMemberIds { get; set; }

		/// <summary>
		/// Gets or sets the date and time when the member(s) are added to the channel.
		/// </summary>
		[JsonPropertyName("added_at")]
		public DateTime AddedAt { get; set; }
	}
}
