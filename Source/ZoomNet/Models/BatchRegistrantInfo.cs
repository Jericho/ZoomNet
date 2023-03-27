using System.Text.Json.Serialization;

namespace ZoomNet.Models
{
	/// <summary>
	/// Batch registrants containing array of registrants.
	/// </summary>
	public class BatchRegistrantInfo
	{
		/// <summary>
		/// Gets or sets an array of Batch Registrant.
		/// </summary>
		[JsonPropertyName("registrants")]
		public BatchRegistrant[] Registrants { get; set; }
	}
}
