using System.Text.Json.Serialization;

namespace ZoomNet.Models
{
	/// <summary>
	/// Base plan usage.
	/// </summary>
	public class BillingBasePlanUsage : BillingPlanUsage
	{
		/// <summary>
		/// Gets or sets the number of active hosts under the base plan.
		/// </summary>
		[JsonPropertyName("active_hosts")]
		public int ActiveHostsCount { get; set; }
	}
}
