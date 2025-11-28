using System.Runtime.Serialization;

namespace ZoomNet.Models
{
	/// <summary>Reasons why a meeting (or webinar) transcript is not available for download.</summary>
	public enum TranscriptRestrictionReason
	{
		/// <summary>The transcript is available.</summary>
		[EnumMember(Value = "")]
		Available,

		/// <summary>The transcript has been deleted or moved to trash and is no longer available.</summary>
		[EnumMember(Value = "DELETED_OR_TRASHED")]
		DeletedOrTrashed,

		/// <summary>The transcript format is not supported for download.</summary>
		[EnumMember(Value = "UNSUPPORTED")]
		Unsupported,

		/// <summary>No transcript data exists for the meeting.</summary>
		[EnumMember(Value = "NO_TRANSCRIPT_DATA")]
		NoData,

		/// <summary>The transcript is still being processed and not yet ready for download.</summary>
		[EnumMember(Value = "NOT_READY")]
		NotReady,
	}
}
