using System;
using System.Text.Json.Serialization;

namespace ZoomNet.Models
{
	/// <summary>
	/// Information about recording archive file.
	/// </summary>
	public class RecordingArchiveFile : RecordingFileBase
	{
		/// <summary>
		/// Gets or sets the archived file's encryption fingerprint, using the SHA256 hash algorithm.
		/// </summary>
		[JsonPropertyName("encryption_fingerprint")]
		public string EncryptionFingerprint { get; set; }

		/// <summary>
		/// Gets or sets the archived file's processing status (completed/processing/failed).
		/// </summary>
		[JsonPropertyName("status")]
		public RecordingArchiveFileStatus Status { get; set; }

		/// <summary>
		/// Gets or sets the region where the file is stored.
		/// </summary>
		/// <remarks>
		/// This field is returned only if 'Enable Distributed Compliance Archiving' feature is enabled.
		/// </remarks>
		[JsonPropertyName("storage_location")]
		public string StorageLocation { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether the archived file is an individual or entire meeting file.
		/// </summary>
		[JsonPropertyName("individual")]
		public bool Individual { get; set; }

		/// <summary>
		/// Gets or sets the individual recording file's participant email address.
		/// </summary>
		/// <remarks>
		/// This field is returned only in case of <see cref="Individual"/> is TRUE.
		/// If the participant is not a part of the host's account, then empty value is returned.
		/// </remarks>
		[JsonPropertyName("participant_email")]
		public string ParticipantEmail { get; set; }

		/// <summary>
		/// Gets or sets the join time for the generated recording file.
		/// </summary>
		/// <remarks>
		/// If <see cref="Individual"/> is TRUE, then this is the recording file's participant join time.
		/// Otherwise, this is the join time for the archiving gateway.
		/// </remarks>
		[JsonPropertyName("participant_join_time")]
		public DateTime ParticipantJoinTime { get; set; }

		/// <summary>
		/// Gets or sets the leave time for the generated recording file.
		/// </summary>
		/// <remarks>
		/// If <see cref="Individual"/> is TRUE, then this is the recording file's participant leave time.
		/// Otherwise, this is the leave time for the archiving gateway.
		/// </remarks>
		[JsonPropertyName("participant_leave_time")]
		public DateTime ParticipantLeaveTime { get; set; }

		/// <summary>
		/// Gets or sets the number of TXT or JSON file messages.
		/// </summary>
		/// <remarks>
		/// This field is returned only in case of <see cref="RecordingFileExtension.TXT"/> or
		/// <see cref="RecordingFileExtension.JSON"/>.
		/// </remarks>
		[JsonPropertyName("number_of_messages")]
		public int? NumberOfMessages { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether to auto delete the archived file.
		/// </summary>
		/// <remarks>
		/// The 'Tag Archiving Files for Deletion' feature must be enabled in OP.
		/// </remarks>
		[JsonPropertyName("auto_delete")]
		public bool? AutoDelete { get; set; }
	}
}
