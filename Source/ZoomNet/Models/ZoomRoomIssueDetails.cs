using System;
using System.Text.Json.Serialization;

namespace ZoomNet.Models
{
	/// <summary>
	/// Details of an issue for a Zoom room.
	/// </summary>
	public class ZoomRoomIssueDetails
	{
		/// <summary>
		/// Gets or sets the issue type.<br/>
		/// The value of the this field could be one of the following:<br/>
		/// * `Room Controller disconnected`<br/>
		/// * `Room Controller connected`<br/>
		/// * `Selected camera has disconnected`<br/>
		/// * `Selected camera is reconnected`<br/>
		/// * `Selected microphone has disconnected`<br/>
		/// * `Selected microphone is reconnected`<br/>
		/// * `Selected speaker has disconnected`<br/>
		/// * `Selected speaker is reconnected`<br/>
		/// * `Zoom room is offline`<br/>
		/// * `Zoom room is online`<br/>
		/// * `High CPU usage is detected`<br/>
		/// * `Low bandwidth network is detected`<br/>
		/// * `{name} battery is low`<br/>
		/// * `{name} battery is normal`<br/>
		/// * `{name} disconnected`<br/>
		/// * `{name} connected`<br/>
		/// * `{name} is not charging`<br/><br/>
		/// Possible values for {name}: <br/>
		/// * Zoom Rooms Computer<br/>
		/// * Controller<br/>
		/// * Scheduling Display.<br/>
		/// </summary>
		/// <value>The issue type.</value>
		[JsonPropertyName("issue")]
		public IssueType IssueType { get; set; }

		/// <summary>
		/// Gets or sets the time the issue appeared.
		/// </summary>
		/// <value>The time the issue appeared.</value>
		[JsonPropertyName("time")]
		public DateTime Time { get; set; }
	}
}
