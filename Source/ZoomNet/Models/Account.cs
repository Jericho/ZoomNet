using Newtonsoft.Json;
using System;

namespace ZoomNet.Models
{
	/// <summary>
	/// An account.
	/// </summary>
	public class Account
	{
		/// <summary>
		/// Gets or sets the account id.
		/// </summary>
		/// <value>
		/// The id.
		/// </value>
		[JsonProperty("id", NullValueHandling = NullValueHandling.Ignore)]
		public long Id { get; set; }

		/// <summary>
		/// Gets or sets the name of the account.
		/// </summary>
		/// <value>
		/// The user id.
		/// </value>
		[JsonProperty("account_name", NullValueHandling = NullValueHandling.Ignore)]
		public string Name { get; set; }

		/// <summary>
		/// Gets or sets the email address of the owner of the account.
		/// </summary>
		/// <value>
		/// The email address of the owner.
		/// </value>
		[JsonProperty("owner_email", NullValueHandling = NullValueHandling.Ignore)]
		public string OwnerEmailAddress { get; set; }

		/// <summary>
		/// Gets or sets the account type.
		/// </summary>
		/// <value>The meeting type.</value>
		//[JsonProperty(PropertyName = "account_type", NullValueHandling = NullValueHandling.Ignore)]
		//public AccountType Type { get; set; }

		/// <summary>
		/// Gets or sets the number of seats.
		/// </summary>
		/// <value>
		/// The user id.
		/// </value>
		[JsonProperty("seats", NullValueHandling = NullValueHandling.Ignore)]
		public int Seats { get; set; }

		/// <summary>
		/// Gets or sets the date and time when the subscription was started.
		/// </summary>
		/// <value>The meeting created time.</value>
		[JsonProperty(PropertyName = "subscription_start_time", NullValueHandling = NullValueHandling.Ignore)]
		public DateTime SubscriptionStart { get; set; }

		/// <summary>
		/// Gets or sets the date and time when the subscription will end.
		/// </summary>
		/// <value>The meeting created time.</value>
		[JsonProperty(PropertyName = "subscription_end_time", NullValueHandling = NullValueHandling.Ignore)]
		public DateTime SubscriptionEnd { get; set; }

		/// <summary>
		/// Gets or sets the date and time when the meeting was created.
		/// </summary>
		/// <value>The meeting created time.</value>
		[JsonProperty(PropertyName = "created_at", NullValueHandling = NullValueHandling.Ignore)]
		public DateTime CreatedOn { get; set; }
	}
}
