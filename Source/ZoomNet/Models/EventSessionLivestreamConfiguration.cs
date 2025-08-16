using System;
using System.Text.Json.Serialization;

namespace ZoomNet.Models
{
	/// <summary>
	/// Livestream configuration for a session.
	/// </summary>
	public class EventSessionLivestreamConfiguration
	{
		/// <summary>Gets or sets the stream key ID.</summary>
		[JsonPropertyName("stream_key_id")]
		public string StreamKeyId { get; set; }

		/// <summary>Gets or sets the stream key name.</summary>
		[JsonPropertyName("stream_key_name")]
		public string StreamKeyName { get; set; }

		/// <summary>Gets or sets the stream key description.</summary>
		[JsonPropertyName("stream_key_description")]
		public string StreamKeyDescription { get; set; }

		/// <summary>Gets or sets the type of stream key.</summary>
		[JsonPropertyName("stream_key_type")]
		public string StreamKeyType { get; set; }

		/// <summary>Gets or sets the stream key.</summary>
		[JsonPropertyName("stream_key")]
		public string StreamKey { get; set; }

		/// <summary>Gets or sets the stream url.</summary>
		[JsonPropertyName("stream_url")]
		public string StreamUrl { get; set; }

		/// <summary>Gets or sets the stream backup url.</summary>
		[JsonPropertyName("stream_backup_url")]
		public string StreamBackupUrl { get; set; }

		/// <summary>Gets or sets the effective time of the stream key when it is valid.</summary>
		[JsonPropertyName("effective_time")]
		public DateTime EffectiveTime { get; set; }

		/// <summary>Gets or sets the expiration time of the stream key.</summary>
		[JsonPropertyName("expiration_time")]
		public DateTime ExpirationTime { get; set; }
	}
}
