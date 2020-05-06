using Microsoft.Extensions.Logging;

namespace ZoomNet.Utilities
{
	/// <summary>
	/// Options for the Zoom client.
	/// </summary>
	public class ZoomClientOptions
	{
		/// <summary>
		/// Gets or sets the log levels for successful calls (HTTP status code in the 200-299 range).
		/// </summary>
		public LogLevel LogLevelSuccessfulCalls { get; set; }

		/// <summary>
		/// Gets or sets the log levels for failed calls (HTTP status code outside of the 200-299 range).
		/// </summary>
		public LogLevel LogLevelFailedCalls { get; set; }
	}
}
