using System.Text.Json.Serialization;

namespace ZoomNet.Models
{
	/// <summary>
	/// Billing recording plan usage info.
	/// </summary>
	public class BillingRecordingPlanUsage
	{
		/// <summary>
		/// Gets or sets the amount of free storage for recording.
		/// </summary>
		[JsonPropertyName("free_storage")]
		public string FreeStorage { get; set; }

		/// <summary>
		/// Gets or sets the amount of free storage used.
		/// </summary>
		[JsonPropertyName("free_storage_usage")]
		public string FreeStorageUsage { get; set; }

		/// <summary>
		/// Gets or sets the amount of free storage for recording.
		/// </summary>
		[JsonPropertyName("plan_storage")]
		public string PlanStorage { get; set; }

		/// <summary>
		/// Gets or sets the amount of free storage used.
		/// </summary>
		[JsonPropertyName("plan_storage_usage")]
		public string PlanStorageUsage { get; set; }

		/// <summary>
		/// Gets or sets the ???.
		/// </summary>
		[JsonPropertyName("plan_storage_exceed")]
		public string PlanStorageExceed { get; set; }

		/// <summary>
		/// Gets or sets the date when the plan exceeds maximum storage..
		/// </summary>
		[JsonPropertyName("max_exceed_date")]
		public string MaxExceededOn { get; set; }

		/// <summary>
		/// Gets or sets the base plan's type.
		/// </summary>
		[JsonPropertyName("type")]
		public string Type { get; set; }
	}
}
