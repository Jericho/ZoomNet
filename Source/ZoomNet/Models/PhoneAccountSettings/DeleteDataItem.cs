using System.Text.Json.Serialization;

namespace ZoomNet.Models.PhoneAccountSettings
{
	/// <summary>Delete data item information.</summary>
	public record DeleteDataItem
	{
		/// <summary>Gets or sets the retention duration where -1 means unlimited. See <see cref="DurationUnit"/> for the value unit.</summary>
		[JsonPropertyName("duration")]
		public int RetentionDuration { get; init; }

		/// <summary>Gets or sets the retention duration unit.</summary>
		[JsonPropertyName("time_unit")]
		public RetentionDurationUnit DurationUnit { get; init; }

		/// <summary>Gets or sets the data type.</summary>
		[JsonPropertyName("type")]
		public DeleteDataType Type { get; init; }
	}
}
