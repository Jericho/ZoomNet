using System.Text.Json.Serialization;

namespace ZoomNet.Models.Webhooks
{
	/// <summary>
	/// Represents an event related to user TSP account action (create/update/delete).
	/// </summary>
	public abstract class UserTspEvent : Event
	{
		/// <summary>
		/// Gets or sets the account id of the user who made action related to TSP account.
		/// </summary>
		[JsonPropertyName("account_id")]
		public string AccountId { get; set; }

		/// <summary>
		/// Gets or sets the email address of the user who made action related to TSP account.
		/// </summary>
		[JsonPropertyName("operator")]
		public string Operator { get; set; }

		/// <summary>
		/// Gets or sets id of the user who made action related to TSP account.
		/// </summary>
		[JsonPropertyName("operator_id")]
		public string OperatorId { get; set; }

		/// <summary>
		/// Gets or sets information about the affected user TSP account.
		/// </summary>
		[JsonPropertyName("object")]
		public UserTspAccount UserTspAccount { get; set; }
	}
}
