using System;
using System.Text.Json.Serialization;

namespace ZoomNet.Models
{
	/// <summary>
	/// Base information about recording file.
	/// </summary>
	public abstract class RecordingFileBase
	{
		private static readonly RecordingContentType[] _screenShareContentTypes =
		{
			RecordingContentType.SharedScreenWithSpeakerViewClosedCaptioned,
			RecordingContentType.SharedScreenWithSpeakerView,
			RecordingContentType.SharedScreenWithGalleryView,
			RecordingContentType.SharedScreen
		};

		/// <summary>
		/// Gets or sets the file unique id.
		/// </summary>
		[JsonPropertyName("id")]
		public string Id { get; set; }

		/// <summary>
		/// Gets or sets the file name.
		/// </summary>
		[JsonPropertyName("file_name")]
		public string FileName { get; set; }

		/// <summary>
		/// Gets or sets the file extension.
		/// </summary>
		[JsonPropertyName("file_extension")]
		public RecordingFileExtension FileExtension { get; set; }

		/// <summary>
		/// Gets or sets the file size in bytes.
		/// </summary>
		[JsonPropertyName("file_size")]
		public long Size { get; set; }

		/// <summary>
		/// Gets or sets the file type.
		/// </summary>
		[JsonPropertyName("file_type")]
		public RecordingFileType FileType { get; set; }

		/// <summary>
		/// Gets or sets the file path to the on-premise account recording.
		/// </summary>
		/// <remarks>
		/// This field is returned only for Zoom On-Premise accounts.
		/// </remarks>
		[JsonPropertyName("file_path")]
		public string FilePath { get; set; }

		/// <summary>
		/// Gets or sets the file content type.
		/// </summary>
		[JsonPropertyName("recording_type")]
		public RecordingContentType ContentType { get; set; }

		/// <summary>
		/// Gets or sets the URL which can be used to download the file.
		/// </summary>
		[JsonPropertyName("download_url")]
		public string DownloadUrl { get; set; }

		/// <summary>
		/// Gets a value indicating whether the recording file represents a screensharing session.
		/// </summary>
		/// <remarks>A value of <see langword="true"/> indicates that the file is a video recording specifically
		/// identified as screenshare content. Use this property to distinguish between regular video recordings and those
		/// that capture screensharing activity.</remarks>
		public bool IsScreenshare => FileType is RecordingFileType.Video && Array.Exists(_screenShareContentTypes, t => t == ContentType);

		/// <summary>
		/// Gets a value indicating whether this file represents the primary video recording.
		/// </summary>
		public bool IsPrimaryVideo => FileType is RecordingFileType.Video && !IsScreenshare;
	}
}
