using Newtonsoft.Json;

namespace ZoomNet.Models
{
	/// <summary>
	/// Top issues from Zoom rooms.
	/// </summary>
	public class IssuesOfZoomRooms
	{
		/// <summary>
		/// Gets or sets the Issue Name.<br/>
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
		/// <value>The Issue name.</value>
		[JsonProperty(PropertyName = "issue_name")]
		public string IssueName { get; set; }

		/// <summary>
		/// Gets or sets the count of Zoom rooms where the issue appeared.
		/// </summary>
		/// <value>The count of Zoom rooms where the issue appeared.</value>
		[JsonProperty(PropertyName = "zoom_rooms_count")]
		public int ZoomRoomsCount { get; set; }
	}
}
