using Pathoschild.Http.Client;
using System;
using System.Text.Json.Nodes;
using System.Threading;
using System.Threading.Tasks;
using ZoomNet.Models.Webhooks;
using ZoomNet.Utilities;

namespace ZoomNet.Resources
{
	/// <summary>
	/// Allows you to notify Zoom that you comply with the policy which requires you to handle
	/// user's data in accordance to the user's preference after the user uninstalls your app.
	/// </summary>
	/// <seealso cref="ZoomNet.Resources.IDataCompliance" />
	/// <remarks>
	/// This resource can only be used when you connect to Zoom using OAuth. It cannot be used with a Jwt connection.
	/// See <a href="https://marketplace.zoom.us/docs/api-reference/data-compliance/">Zoom documentation</a> for more information.
	/// </remarks>
	[Obsolete("The Data Compliance API is deprecated")]
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
		/// <param name="userId">The Zoom user's id who uninstalled your app.</param>
		/// <param name="accountId">The account Id.</param>
		/// <param name="deauthorizationEventReceived">This object represents the payload of the webhook event sent by Zoom in your Deauthorization Endpoint Url after a user uninstalls your app. The values of the parameters in this object should be the same as that of the deauthorization event that you receive.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>
		/// The async task.
		/// </returns>
		[Obsolete("The Data Compliance API is deprecated")]
		public Task NotifyAsync(string userId, long accountId, AppDeauthorizedEvent deauthorizationEventReceived, CancellationToken cancellationToken = default)
		{
			// Prepare the request (but do not dispatch it yet)
			var request = _client
				.PostAsync("oauth/data/compliance")
				.WithCancellationToken(cancellationToken);

			// Figure out the client id and secret for authentication purposes
			var tokenHandler = ((ZoomRetryCoordinator)((FluentClient)_client).RequestCoordinator).TokenHandler;
			string secret;
			string clientId;
			switch (tokenHandler.ConnectionInfo)
			{
				case OAuthConnectionInfo oauthConnectionInfo:
					secret = oauthConnectionInfo.ClientSecret;
					clientId = oauthConnectionInfo.ClientId;
					break;
				case JwtConnectionInfo:
					throw new Exception($"The DataCompliance resource cannot be use with a Jwt connection.");
				default:
					throw new Exception($"Unable to determine the connection secret and cient Id. {tokenHandler.ConnectionInfo.GetType()} is an unknown connection type.");
			}

			// Prepare the payload
			var data = new JsonObject
			{
				{ "client_id", clientId },
				{ "user_id", userId },
				{ "account_id", accountId },
				{ "deauthorization_event_received", deauthorizationEventReceived },
				{ "compliance_completed", "true" }
			};

			// Authenticate using clientId+secret and also specify the payload
			request = request
				.WithBasicAuthentication(clientId, secret)
				.WithJsonBody(data);

			// Return the async task
			return request
				.AsMessage();
		}
	}
}
