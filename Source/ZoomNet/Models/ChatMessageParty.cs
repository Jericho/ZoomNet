namespace ZoomNet.Models
{
	/// <summary>Chat message party (i.e. sender or recipient) information.</summary>
	public record ChatMessageParty
	{
		/// <summary>Gets or sets message sender or recipient name.</summary>
		public string Name { get; init; }

		/// <summary>Gets or sets message sender or recipient email.</summary>
		/// <remarks>Recipient email is available only if it is a private message.</remarks>
		public string Email { get; init; }

		/// <summary>Gets or sets message sender or recipient session id.</summary>
		public string SessionId { get; init; }

		/// <summary>Gets or sets message sender or recipient type.</summary>
		public ChatMessagePartyType PartyType { get; init; }
	}
}
