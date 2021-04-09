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
	public class WebhookParser
	{
		#region PROPERTIES

		/// <summary>
		/// The name of the HTTP header where Zoom stores the verification token.
		/// </summary>
		public const string AUTHORIZATION_HEADER_NAME = "authorization";

		#endregion

		#region PUBLIC METHODS

		/// <summary>
		/// Parses the event webhook asynchronously.
		/// </summary>
		/// <param name="stream">The stream.</param>
		/// <returns>An <see cref="Event" />.</returns>
		public async Task<Event> ParseEventWebhookAsync(Stream stream)
		{
			string requestBody;
			using (var streamReader = new StreamReader(stream))
			{
				requestBody = await streamReader.ReadToEndAsync().ConfigureAwait(false);
			}

			return ParseEventWebhook(requestBody);
		}

		/// <summary>
		/// Parses the event webhook.
		/// </summary>
		/// <param name="requestBody">The content submitted by Zoom's WebHook.</param>
		/// <returns>An <see cref="Event" />.</returns>
		public Event ParseEventWebhook(string requestBody)
		{
			var webHookEvent = JsonConvert.DeserializeObject<Event>(requestBody, new WebHookEventConverter());
			return webHookEvent;
		}

		#endregion
	}
}
