using System.Text.Json.Serialization;

namespace ZoomNet.Models
{
	/// <summary>
	/// United plan usage.
	/// </summary>
	public class BillingUnitedPlanUsage : BillingPlanUsage
	{
		/// <summary>
		/// Gets or sets the plan's name.
		/// </summary>
		[JsonPropertyName("name")]
		public string Name { get; set; }
	}
}
