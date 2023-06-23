using Pathoschild.Http.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Nodes;
using System.Threading;
using System.Threading.Tasks;
using ZoomNet.Models;

namespace ZoomNet.Resources
{
	/// <summary>
	/// Allows you to manage sub accounts under the master account.
	/// </summary>
	/// <seealso cref="ZoomNet.Resources.IAccounts" />
	/// <remarks>
	/// See <a href="https://marketplace.zoom.us/docs/api-reference/zoom-api/accounts/accounts">Zoom documentation</a> for more information.
	/// </remarks>
	public class Accounts : IAccounts
	{
		private readonly Pathoschild.Http.Client.IClient _client;

		/// <summary>
		/// Initializes a new instance of the <see cref="Accounts" /> class.
		/// </summary>
		/// <param name="client">The HTTP client.</param>
		internal Accounts(Pathoschild.Http.Client.IClient client)
		{
			_client = client;
		}

		/// <summary>
		/// Retrieve all the sub accounts under the master account.
		/// </summary>
		/// <param name="recordsPerPage">The number of records returned within a single API call.</param>
		/// <param name="page">The current page number of returned records.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>
		/// An array of <see cref="Account" />.
		/// </returns>
		[Obsolete("Zoom is in the process of deprecating the \"page number\" and \"page count\" fields.")]
		public Task<PaginatedResponse<Account>> GetAllAsync(int recordsPerPage = 30, int page = 1, CancellationToken cancellationToken = default)
		{
			if (recordsPerPage < 1 || recordsPerPage > 300)
			{
				throw new ArgumentOutOfRangeException(nameof(recordsPerPage), "Records per page must be between 1 and 300");
			}

			return _client
				.GetAsync($"accounts")
				.WithArgument("page_size", recordsPerPage)
				.WithArgument("page_number", page)
				.WithCancellationToken(cancellationToken)
				.AsPaginatedResponse<Account>("accounts");
		}

		/// <summary>
		/// Retrieve all the sub accounts under the master account.
		/// </summary>
		/// <param name="recordsPerPage">The number of records returned within a single API call.</param>
		/// <param name="pagingToken">The paging token.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>
		/// An array of <see cref="Account" />.
		/// </returns>
		public Task<PaginatedResponseWithToken<Account>> GetAllAsync(int recordsPerPage = 30, string pagingToken = null, CancellationToken cancellationToken = default)
		{
			if (recordsPerPage < 1 || recordsPerPage > 300)
			{
				throw new ArgumentOutOfRangeException(nameof(recordsPerPage), "Records per page must be between 1 and 300");
			}

			return _client
				.GetAsync($"accounts")
				.WithArgument("page_size", recordsPerPage)
				.WithArgument("next_page_token", pagingToken)
				.WithCancellationToken(cancellationToken)
				.AsPaginatedResponseWithToken<Account>("accounts");
		}

		/// <summary>
		/// Create a sub account under the master account.
		/// </summary>
		/// <param name="firstName">User's first name.</param>
		/// <param name="lastName">User's last name.</param>
		/// <param name="email">User's email address.</param>
		/// <param name="password">User's password.</param>
		/// <param name="useSharedVirtualRoomConnectors">Enable/disable the option for a sub account to use shared Virtual Room Connector(s).</param>
		/// <param name="roomConnectorsIpAddresses">The IP addresses of the Room Connectors that you would like to share with the sub account.</param>
		/// <param name="useSharedMeetingConnectors">Enable/disable the option for a sub account to use shared Meeting Connector(s).</param>
		/// <param name="meetingConnectorsIpAddresses">The IP addresses of the Meeting Connectors that you would like to share with the sub account.</param>
		/// <param name="payMode">Payee.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>
		/// The <see cref="Account" />.
		/// </returns>
		public Task<Account> CreateAsync(string firstName, string lastName, string email, string password, bool useSharedVirtualRoomConnectors = false, IEnumerable<string> roomConnectorsIpAddresses = null, bool useSharedMeetingConnectors = false, IEnumerable<string> meetingConnectorsIpAddresses = null, PayMode payMode = PayMode.Master, CancellationToken cancellationToken = default)
		{
			var data = new JsonObject
			{
				{ "first_name", firstName },
				{ "last_name", lastName },
				{ "email", email },
				{ "password", password },
				{
					"options",
					new JsonObject
					{
						{ "share_rc", useSharedVirtualRoomConnectors },
						{ "room_connectors", string.Join(",", roomConnectorsIpAddresses) },
						{ "share_mc", useSharedMeetingConnectors },
						{ "meeting_connectors", string.Join(",", meetingConnectorsIpAddresses) },
						{ "pay_mode", payMode }
					}
				}
			};

			return _client
				.PostAsync($"accounts")
				.WithJsonBody(data)
				.WithCancellationToken(cancellationToken)
				.AsObject<Account>();
		}

		/// <summary>
		/// Retrieve the details of a sub account.
		/// </summary>
		/// <param name="accountId">The account Id.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>
		/// The <see cref="Account" />.
		/// </returns>
		public Task<Account> GetAsync(long accountId, CancellationToken cancellationToken = default)
		{
			// The information returned from this API call is vastly different than what is returned by GetAllAsync
			return _client
				.GetAsync($"accounts/{accountId}")
				.WithCancellationToken(cancellationToken)
				.AsObject<Account>();
		}

		/// <summary>
		/// Disassociate a Sub Account from the Master Account.
		/// </summary>
		/// <param name="accountId">The account Id that must be disassociated from its master account.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>
		/// The async task.
		/// </returns>
		/// <remarks>
		/// This will leave the Sub Account intact but it will no longer be associated with the master account.
		/// </remarks>
		public Task DisassociateAsync(long accountId, CancellationToken cancellationToken = default)
		{
			return _client
				.DeleteAsync($"accounts/{accountId}")
				.WithCancellationToken(cancellationToken)
				.AsMessage();
		}

		/// <summary>
		/// Update a Sub Account's options under the Master Account.
		/// </summary>
		/// <param name="accountId">The account Id.</param>
		/// <param name="useSharedVirtualRoomConnectors">Enable/disable the option for a sub account to use shared Virtual Room Connector(s).</param>
		/// <param name="roomConnectorsIpAddresses">The IP addresses of the Room Connectors that you would like to share with the sub account.</param>
		/// <param name="useSharedMeetingConnectors">Enable/disable the option for a sub account to use shared Meeting Connector(s).</param>
		/// <param name="meetingConnectorsIpAddresses">The IP addresses of the Meeting Connectors that you would like to share with the sub account.</param>
		/// <param name="payMode">Payee.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>
		/// The async task.
		/// </returns>
		public Task UpdateOptionsAsync(long accountId, bool? useSharedVirtualRoomConnectors = null, IEnumerable<string> roomConnectorsIpAddresses = null, bool? useSharedMeetingConnectors = null, IEnumerable<string> meetingConnectorsIpAddresses = null, PayMode? payMode = null, CancellationToken cancellationToken = default)
		{
			var data = new JsonObject
			{
				{ "share_rc", useSharedVirtualRoomConnectors },
				{ "room_connectors", string.Join(",", roomConnectorsIpAddresses) },
				{ "share_mc", useSharedMeetingConnectors },
				{ "meeting_connectors", string.Join(",", meetingConnectorsIpAddresses) }
			};
			if (payMode.HasValue) data.Add("pay_mode", payMode.Value);

			return _client
				.PatchAsync($"accounts/{accountId}/options")
				.WithJsonBody(data)
				.WithCancellationToken(cancellationToken)
				.AsMessage();
		}

		/// <summary>
		/// Retrieve an account's meeting authentication settings.
		/// </summary>
		/// <param name="accountId">The account Id.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>
		/// The <see cref="AuthenticationSettings">settings</see>.
		/// </returns>
		public async Task<AuthenticationSettings> GetMeetingAuthenticationSettingsAsync(long accountId, CancellationToken cancellationToken = default)
		{
			var response = await _client
				.GetAsync($"accounts/{accountId}/settings")
				.WithArgument("option", "meeting_authentication")
				.WithCancellationToken(cancellationToken)
				.AsRawJsonDocument()
				.ConfigureAwait(false);

			var settings = new AuthenticationSettings()
			{
				RequireAuthentication = response.RootElement.GetPropertyValue("meeting_authentication", false),
				AuthenticationOptions = response.RootElement.GetProperty("authentication_options", false)?.ToObject<AuthenticationOptions[]>() ?? Array.Empty<AuthenticationOptions>()
			};

			return settings;
		}

		/// <summary>
		/// Retrieve an account's recording authentication settings.
		/// </summary>
		/// <param name="accountId">The account Id.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>
		/// The <see cref="AuthenticationSettings">settings</see>.
		/// </returns>
		public async Task<AuthenticationSettings> GetRecordingAuthenticationSettingsAsync(long accountId, CancellationToken cancellationToken = default)
		{
			var response = await _client
				.GetAsync($"accounts/{accountId}/settings")
				.WithArgument("option", "recording_authentication")
				.WithCancellationToken(cancellationToken)
				.AsRawJsonDocument()
				.ConfigureAwait(false);

			var settings = new AuthenticationSettings()
			{
				RequireAuthentication = response.RootElement.GetPropertyValue("recording_authentication", false),
				AuthenticationOptions = response.RootElement.GetProperty("authentication_options", false)?.ToObject<AuthenticationOptions[]>() ?? Array.Empty<AuthenticationOptions>()
			};

			return settings;
		}

		/// <summary>
		/// Retrieve a sub account's managed domains.
		/// </summary>
		/// <param name="accountId">The account Id.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>
		/// An array of managed domains and their status.
		/// </returns>
		public async Task<(string Domain, string Status)[]> GetManagedDomainsAsync(long accountId, CancellationToken cancellationToken = default)
		{
			var response = await _client
				.GetAsync($"accounts/{accountId}/managed_domains")
				.WithCancellationToken(cancellationToken)
				.AsRawJsonDocument("domains")
				.ConfigureAwait(false);

			var managedDomains = response.RootElement
				.EnumerateArray()
				.Select(jsonElement =>
				{
					var key = jsonElement.GetPropertyValue("domain", string.Empty);
					var value = jsonElement.GetPropertyValue("status", string.Empty);
					return (key, value);
				}).ToArray();

			return managedDomains;
		}

		/// <summary>
		/// Retrieve a sub account's trusted domains.
		/// </summary>
		/// <param name="accountId">The account Id.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>
		/// An array of trusted domains.
		/// </returns>
		public Task<string[]> GetTrustedDomainsAsync(long accountId, CancellationToken cancellationToken = default)
		{
			return _client
				.GetAsync($"accounts/{accountId}/trusted_domains")
				.WithCancellationToken(cancellationToken)
				.AsObject<string[]>("trusted_domains");
		}

		/// <summary>
		/// Change the owner of a Sub Account to another user on the same account.
		/// </summary>
		/// <param name="accountId">The account Id.</param>
		/// <param name="newOwnerEmail">The new owner's email address.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>
		/// The async task.
		/// </returns>
		public Task UpdateOwnerAsync(long accountId, string newOwnerEmail, CancellationToken cancellationToken = default)
		{
			var data = new JsonObject
			{
				{ "email", newOwnerEmail }
			};

			return _client
				.PatchAsync($"accounts/{accountId}/owner")
				.WithJsonBody(data)
				.WithCancellationToken(cancellationToken)
				.AsMessage();
		}
	}
}
