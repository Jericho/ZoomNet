using Pathoschild.Http.Client;
using System;
using System.Linq;
using System.Text.Json.Nodes;
using System.Threading;
using System.Threading.Tasks;
using ZoomNet.Models.Webhooks;
using ZoomNet.Utilities;

namespace ZoomNet.Resources
{
	/// <inheritdoc/>
	[Obsolete("The Data Compliance API is deprecated")]
	public class DataCompliance : IDataCompliance
	{
		private readonly IClient _client;

		/// <summary>
		/// Initializes a new instance of the <see cref="DataCompliance" /> class.
		/// </summary>
		/// <param name="client">The HTTP client.</param>
		internal DataCompliance(IClient client)
		{
			_client = client;
		}

		/// <inheritdoc/>
		[Obsolete("The Data Compliance API is deprecated")]
		public Task NotifyAsync(string userId, long accountId, AppDeauthorizedEvent deauthorizationEventReceived, CancellationToken cancellationToken = default)
		{
			// Prepare the request (but do not dispatch it yet)
			var request = _client
				.PostAsync("oauth/data/compliance")
				.WithCancellationToken(cancellationToken);

			// Figure out the client id and secret for authentication purposes
			var tokenHandler = (ITokenHandler)request.Filters.Single(f => f.GetType().IsAssignableFrom(typeof(ITokenHandler)));
			var secret = string.Empty;
			var clientId = string.Empty;
			switch (tokenHandler.ConnectionInfo)
			{
				case OAuthConnectionInfo oauthConnectionInfo:
					secret = oauthConnectionInfo.ClientSecret;
					clientId = oauthConnectionInfo.ClientId;
					break;
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

			// This endpoint relies on clientId+secret for authentication. It does not need tokens.
			request.Filters.Remove<OAuthTokenHandler>();

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
