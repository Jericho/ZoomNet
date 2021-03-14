using Newtonsoft.Json.Linq;
using Pathoschild.Http.Client;
using System.Threading;
using System.Threading.Tasks;
using ZoomNet.Utilities;

namespace ZoomNet.Resources
{
	/// <summary>
	/// Allows you to notify Zoom that you comply with the policy which requires you to handle
	/// user's data in accordance to the user's preference after the user uninstalls your app.
	/// </summary>
	/// <seealso cref="ZoomNet.Resources.IDataCompliance" />
	/// <remarks>
	/// See <a href="https://marketplace.zoom.us/docs/api-reference/data-compliance/">Zoom documentation</a> for more information.
	/// </remarks>
	public class DataCompliance : IDataCompliance
	{
		private readonly Pathoschild.Http.Client.IClient _client;

		/// <summary>
		/// Initializes a new instance of the <see cref="DataCompliance" /> class.
		/// </summary>
		/// <param name="client">The HTTP client.</param>
		internal DataCompliance(Pathoschild.Http.Client.IClient client)
		{
			_client = client;
		}

		/// <summary>
		/// Notify Zoom that you comply with the policy which requires you to handle user's
		/// data in accordance to the user's preference after the user uninstalls your app.
		/// </summary>
		/// <param name="clientId">Your app's Client Id.</param>
		/// <param name="secret">Your app's secret.</param>
		/// <param name="userId">The Zoom user's id who uninstalled your app.</param>
		/// <param name="accountId">The account Id.</param>
		/// <param name="deauthorizationEventReceived">This object represents the payload of the webhook event sent by Zoom in your Deauthorization Endpoint Url after a user uninstalls your app. The values of the parameters in this object should be the same as that of the deauthorization event that you receive.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>
		/// The async task.
		/// </returns>
		public Task NotifyAsync(string clientId, string secret, string userId, long accountId, object deauthorizationEventReceived, CancellationToken cancellationToken = default)
		{
			var data = new JObject();
			data.AddPropertyIfValue("client_id", clientId);
			data.AddPropertyIfValue("user_id", userId);
			data.AddPropertyIfValue("account_id", accountId);
			data.AddPropertyIfValue("deauthorization_event_received", deauthorizationEventReceived);
			data.AddPropertyIfValue("compliance_completed", "true");

			var request = _client
				.PostAsync("oauth/data/compliance")
				.WithBasicAuthentication(clientId, secret)
				.WithJsonBody(data)
				.WithCancellationToken(cancellationToken);

			// This endpoint relies on clientId+secret for authentication. It does not need tokens.
			request.Filters.Remove<OAuthTokenHandler>();
			request.Filters.Remove<JwtTokenHandler>();

			return request
				.AsMessage();
		}
	}
}
