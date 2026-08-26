using System.Text.Json.Serialization;

namespace ZoomNet.Models;

/// <summary>Represents a calling plan.</summary>
public class CallingPlan
{
	/// <summary>Gets or sets the billing account ID.</summary>
	[JsonPropertyName("billing_account_id")]
	public string BillingAccountId { get; set; }

	/// <summary>Gets or sets the name of the billing account.</summary>
	[JsonPropertyName("billing_account_name")]
	public string BillingAccountName { get; set; }

	/// <summary>Gets or sets the type of the calling plan.</summary>
	[JsonPropertyName("type")]
	public CallingPlanType Type { get; set; }
}
