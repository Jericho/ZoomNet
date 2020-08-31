using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZoomNet.Models
{
	/// <summary>
	/// Zoom Room.
	/// </summary>
	public class ZoomRoom
	{
		/// <summary>
		/// Gets or sets the room id.
		/// </summary>
		/// <value>
		/// The room id.
		/// </value>
		public string Id { get; set; }

		/// <summary>
		/// Gets or sets the room name.
		/// </summary>
		/// <value>
		/// The room name.
		/// </value>
		public string Name { get; set; }

		/// <summary>
		/// Gets or sets the Zoom calendar name.
		/// </summary>
		/// <value>
		/// The Zoom calendar name.
		/// </value>
		public string CalendarName { get; set; }

		/// <summary>
		/// Gets or sets the Zoom room email.
		/// </summary>
		/// <value>
		/// The Zoom room email.
		/// </value>
		public string Email { get; set; }

		/// <summary>
		/// Gets or sets the Zoom room email type.
		/// </summary>
		/// <value>
		/// Zoom room email type.
		/// </value>
		public string AccountType { get; set; }

		/// <summary>
		/// Gets or sets the Zoom room status.
		/// </summary>
		/// <value>
		/// The Zoom room status.
		/// </value>
		public string Status { get; set; }

		/// <summary>
		/// Gets or sets the Zoom room device IP.
		/// </summary>
		/// <value>
		/// The Zoom room device IP.
		/// </value>
		public string DeviceIp { get; set; }

		/// <summary>
		/// Gets or sets the Zoom room camera.
		/// </summary>
		/// <value>
		/// The Zoom room camera.
		/// </value>
		public string Camera { get; set; }

		/// <summary>
		/// Gets or sets the Zoom room microphone.
		/// </summary>
		/// <value>
		/// The Zoom room microphone.
		/// </value>
		public string Microphone { get; set; }

		/// <summary>
		/// Gets or sets the Zoom room speaker.
		/// </summary>
		/// <value>
		/// The Zoom room speaker.
		/// </value>
		public string Speaker { get; set; }

		/// <summary>
		/// Gets or sets the last start time of the Zoom room.
		/// </summary>
		/// <value>
		/// The last start time of the Zoom room.
		/// </value>
		public string LastStartTime { get; set; }

		/// <summary>
		/// Gets or sets the Zoom room location.
		/// </summary>
		/// <value>
		/// The Zoom room location.
		/// </value>
		public string Location { get; set; }

		/// <summary>
		/// Gets or sets the health value.
		/// </summary>
		/// <value>
		/// The health value.
		/// </value>
		public string Health { get; set; }

		/// <summary>
		/// Gets or sets Zoom room issues.
		/// </summary>
		/// <value>
		/// Zoom room issues.
		/// </value>
		public string[] Issues { get; set; }
	}
}
