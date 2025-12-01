namespace ZoomNet.Models
{
	/// <summary>
	/// The status of the voicemail transcript.
	/// </summary>
	public enum VoicemailTranscriptStatus
	{
		/// <summary>Transcript is not available.</summary>
		NotAvailable = 0,

		/// <summary>Transcript is processing.</summary>
		Processing = 1,

		/// <summary>Transcript processed successfully.</summary>
		Completed = 2,

		/// <summary>Transcript is disabled.</summary>
		Disabled = 4,

		/// <summary>Transcript is enabled.</summary>
		Enabled = 5,

		/// <summary>Transcript web error.</summary>
		WebError = 9,

		/// <summary>Transcript download error.</summary>
		DownloadError = 11,

		/// <summary>Transcript upload error.</summary>
		UploadError = 12,

		/// <summary>Transcript web database error.</summary>
		WebDatabaseError = 13,

		/// <summary>Transcript BYOS (Bring your own storage) upload error.</summary>
		ByosUploadError = 14,

		/// <summary>Transcript duplicate processing request error.</summary>
		DuplicateRequestError = 409,

		/// <summary>Transcript unsupported media error.</summary>
		UnsupportedMediaError = 415,

		/// <summary>Transcript cannot be processed.</summary>
		CanNotBeProcessed = 422,

		/// <summary>Transcript server error.</summary>
		ServerError = 500,

		/// <summary>Transcript AISense after retry error.</summary>
		AiSenseAfterRetryError = 601,

		/// <summary>Transcript AISense upload file error.</summary>
		AiSenseUploadFileError = 602,

		/// <summary>Transcript AISense download file error.</summary>
		AiSenseDownloadFileError = 603,

		/// <summary>Transcript AISense error.</summary>
		AiSenseError = 999,
	}
}
