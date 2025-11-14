using System.Text.Json.Serialization;

namespace ZoomNet.Models;

/// <summary>
/// Represents a calling plan.
/// </summary>
public class CallingPlan
{
	/// <summary>
	/// Gets or sets the type of the calling plan.
	/// </summary>
	/// <value>
	/// The type of the calling plan.
	/// </value>
	[JsonPropertyName("type")]
	public CallingPlanType Type { get; set; }

	/// <summary>
	/// Gets or sets the billing account ID.
	/// </summary>
	/// <value>The billing account ID.</value>
	[JsonPropertyName("billing_account_id")]
	public string BillingAccountId { get; set; }

	/// <summary>
	/// Gets or sets the name of the billing account.
	/// </summary>
	/// <value>
	/// The name of the billing account.
	/// </value>
	[JsonPropertyName("billing_account_name")]
	public string BillingAccountName { get; set; }
}
