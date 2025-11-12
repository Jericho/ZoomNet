using System.Text.Json.Serialization;

namespace ZoomNet.Models
{
	/// <summary>
	/// Information about recording cloud storage usage.
	/// </summary>
	public class RecordingStorageUsage
	{
		/// <summary>
		/// Gets or sets the recording free storage.
		/// </summary>
		[JsonPropertyName("free_storage")]
		public string FreeStorage { get; set; }

		/// <summary>
		/// Gets or sets the recording plan storage.
		/// </summary>
		[JsonPropertyName("plan_storage")]
		public string PlanStorage { get; set; }

		/// <summary>
		/// Gets or sets the recording plan type.
		/// </summary>
		[JsonPropertyName("plan_type")]
		public string PlanType { get; set; }

		/// <summary>
		/// Gets or sets the amount of storage exceed.
		/// </summary>
		[JsonPropertyName("storage_exceed")]
		public string StorageExceed { get; set; }

		/// <summary>
		/// Gets or sets the amount of storage used.
		/// </summary>
		[JsonPropertyName("storage_used")]
		public string StorageUsed { get; set; }

		/// <summary>
		/// Gets or sets the percentage of storage used.
		/// </summary>
		[JsonPropertyName("storage_used_percentage")]
		public int StorageUsedPercentage { get; set; }

		/// <summary>
		/// Gets or sets the maximum date that overage storage.
		/// </summary>
		[JsonPropertyName("max_exceed_date")]
		public string MaxExceedDate { get; set; }
	}
}
