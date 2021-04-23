using Newtonsoft.Json;

namespace ZoomNet.Models
{
	/// <summary>
	/// Sharing details.
	/// </summary>
	public class ScreenshareSharingDetails : SharingDetails
	{
		/// <summary>
		/// Gets or sets the sharing details.
		/// </summary>
		/// <value>
		/// An array of sharing and recording details.
		/// </value>
		[JsonProperty(PropertyName = "details")]
		public ScreenshareSharingDetail Details { get; set; }
	}
}
