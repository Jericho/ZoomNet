using Newtonsoft.Json;
using System;

namespace ZoomNet.Models
{
	/// <summary>
	/// Details of a screensharing by a participant.
	/// </summary>
	public class ScreenshareDetails
	{
		/// <summary>
		/// Gets or sets the type of content shared. Allowed values: application, whiteboard, desktop.
		/// </summary>
		[JsonProperty(PropertyName = "content")]
		public ScreenshareContentType ContentType { get; set; }

		/// <summary>
		/// Gets or sets the source.
		/// </summary>
		[JsonProperty(PropertyName = "source")]
		public string Source { get; set; }

		/// <summary>
		/// Gets or sets the method of sharing for dropbox integration. Allowed values: deep_link, in_meeting.
		/// </summary>
		[JsonProperty(PropertyName = "link_source")]
		public string SharingMethod { get; set; }

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
