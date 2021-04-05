using Newtonsoft.Json;
using System.IO;
using System.Threading.Tasks;
using ZoomNet.Models.Webhooks;
using ZoomNet.Utilities;

namespace ZoomNet
{
	/// <summary>
	/// Allows parsing of information posted from Zoom.
	/// </summary>
	public static class WebhookParser
	{
		#region PUBLIC METHODS

		/// <summary>
		/// Parses the event webhook asynchronously.
		/// </summary>
		/// <param name="stream">The stream.</param>
		/// <returns>An <see cref="Event"/>.</returns>
		public static async Task<Event> ParseEventWebhookAsync(Stream stream)
		{
			string requestBody;
			using (var streamReader = new StreamReader(stream))
			{
				requestBody = await streamReader.ReadToEndAsync().ConfigureAwait(false);
			}

			var webHookEvents = ParseEventWebhook(requestBody);
			return webHookEvents;
		}

		/// <summary>
		/// Parses the event webhook.
		/// </summary>
		/// <param name="requestBody">The content submitted by Zoom's WebHook.</param>
		/// <returns>An <see cref="Event"/>.</returns>
		public static Event ParseEventWebhook(string requestBody)
		{
			var webHookEvent = JsonConvert.DeserializeObject<Event>(requestBody, new WebHookEventConverter());
			return webHookEvent;
		}

		#endregion
	}
}
