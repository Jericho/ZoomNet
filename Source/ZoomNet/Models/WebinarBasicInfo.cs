using System.Text.Json.Serialization;

namespace ZoomNet.Models
{
	/// <summary>
	/// Provides basic information about webinar.
	/// </summary>
	public class WebinarBasicInfo
	{
		/// <summary>
		/// Gets or sets the webinar id, also known as the webinar number.
		/// </summary>
		[JsonPropertyName("id")]
		/*
			This allows us to overcome the fact that "id" is sometimes a string and sometimes a number
			See: https://devforum.zoom.us/t/the-data-type-of-meetingid-is-inconsistent-in-webhook-documentation/70090
			Also, see: https://github.com/Jericho/ZoomNet/issues/228
		*/
		[JsonNumberHandling(JsonNumberHandling.AllowReadingFromString)]
		public long Id { get; set; }

		/// <summary>
		/// Gets or sets the webinar's unique id. Each webinar instance generates a webinar uuid.
		/// </summary>
		[JsonPropertyName("uuid")]
		public string Uuid { get; set; }
	}
}
