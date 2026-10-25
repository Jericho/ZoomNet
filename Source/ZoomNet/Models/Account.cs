using System;
using System.Text.Json.Serialization;

namespace ZoomNet.Models
{
	/// <summary>An account.</summary>
	public record Account
	{
		/// <summary>Gets or sets the name of the account.</summary>
		[JsonPropertyName("account_name")]
		public string Name { get; init; }

		/// <summary>Gets or sets the account number.</summary>
		[JsonPropertyName("account_number")]
		public string Number { get; init; }

		/// <summary>Gets or sets the account type.</summary>
		[JsonPropertyName("account_type")]
		public string Type { get; init; }

		/// <summary>Gets or sets the date and time when the meeting was created.</summary>
		[JsonPropertyName("created_at")]
		public DateTime CreatedOn { get; init; }

		/// <summary>Gets or sets the account id.</summary>
		[JsonPropertyName("id")]
		public long Id { get; init; }

		/// <summary>Gets or sets the email address of the owner of the account.</summary>
		[JsonPropertyName("owner_email")]
		public string OwnerEmailAddress { get; init; }

		/// <summary>Gets or sets the number of seats.</summary>
		[JsonPropertyName("seats")]
		public int Seats { get; init; }

		/// <summary>Gets or sets the date and time when the subscription will end.</summary>
		[JsonPropertyName("subscription_end_time")]
		public DateTime SubscriptionEnd { get; init; }

		/// <summary>Gets or sets the date and time when the subscription was started.</summary>
		[JsonPropertyName("subscription_start_time")]
		public DateTime SubscriptionStart { get; init; }
	}
}
