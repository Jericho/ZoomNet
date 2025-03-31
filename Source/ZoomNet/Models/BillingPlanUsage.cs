using System.Text.Json.Serialization;

namespace ZoomNet.Models
{
	/// <summary>
	/// Plan usage.
	/// </summary>
	public class BillingPlanUsage
	{
		/// <summary>
		/// Gets or sets the plan's number of hosts.
		/// </summary>
		[JsonPropertyName("hosts")]
		public int HostsCount { get; set; }

		/// <summary>
		/// Gets or sets the plan's type.
		/// </summary>
		[JsonPropertyName("type")]
		public string Type { get; set; }

		/// <summary>
		/// Gets or sets the plan's total usage number.
		/// </summary>
		[JsonPropertyName("usage")]
		public int Usage { get; set; }

		/// <summary>
		/// Gets or sets the plan's total number of pending licenses.
		/// </summary>
		[JsonPropertyName("pending")]
		public int PendingLicensesCount { get; set; }
	}
}
