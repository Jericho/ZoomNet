using Newtonsoft.Json;
using System;

namespace ZoomNet.Models
{
	/// <summary>
	/// Details of a screensharing by a meeting participant.
	/// </summary>
	public class ScreenshareSharingDetail
	{
		/// <summary>
		/// Gets or sets the type of content shared. Allowed values: application, whiteboard, desktop.
		/// </summary>
		[JsonProperty(PropertyName = "content")]
		public string Content { get; set; }

		/// <summary>
		/// Gets or sets the method of sharing for dropbox integration. Allowed values: application, whiteboard, desktop.
		/// </summary>
		[JsonProperty(PropertyName = "link_source")]
		public string DropboxSharingMethod { get; set; }

		/// <summary>
		/// Gets or sets the link using which the file was shared via dropbox integration.
		/// </summary>
		[JsonProperty(PropertyName = "file_link")]
		public string Link { get; set; }

		/// <summary>
		/// Gets or sets the date and time during which the screenshare happened.
		/// </summary>
		[JsonProperty(PropertyName = "date_time")]
		public DateTime Date { get; set; }
	}
}
