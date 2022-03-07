using System.Runtime.Serialization;

namespace ZoomNet.Models
{
	/// <summary>
	/// Streaming service.
	/// </summary>
	public enum StreamingService
	{
		/// <summary>Facebook</summary>
		[EnumMember(Value = "facebook")]
		Facebook,

		/// <summary>Workplace by Facebook</summary>
		[EnumMember(Value = "workplace_by_facebook")]
		WorkplaceByFacebook,

		/// <summary>YouTube</summary>
		[EnumMember(Value = "youtube")]
		YouTube,

		/// <summary>custom live streaming service</summary>
		[EnumMember(Value = "custom_live_streaming_service")]
		Custom
	}
}
