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
	/// <inheritdoc/>
	public class Accounts : IAccounts
	{
		private readonly IClient _client;

		/// <summary>
		/// Initializes a new instance of the <see cref="Accounts" /> class.
		/// </summary>
		/// <param name="client">The HTTP client.</param>
		internal Accounts(IClient client)
		{
			_client = client;
		}

		/// <inheritdoc/>
		[Obsolete("Zoom is in the process of deprecating the \"page number\" and \"page count\" fields.")]
		public Task<PaginatedResponse<Account>> GetAllAsync(int recordsPerPage = 30, int page = 1, CancellationToken cancellationToken = default)
		{
			if (recordsPerPage < 1 || recordsPerPage > 300)
			{
				throw new ArgumentOutOfRangeException(nameof(recordsPerPage), "Records per page must be between 1 and 300");
			}

			return _client
				.GetAsync("accounts")
				.WithArgument("page_size", recordsPerPage)
				.WithArgument("page_number", page)
				.WithCancellationToken(cancellationToken)
				.AsPaginatedResponse<Account>("accounts");
		}

		/// <inheritdoc/>
		public Task<PaginatedResponseWithToken<Account>> GetAllAsync(int recordsPerPage = 30, string pagingToken = null, CancellationToken cancellationToken = default)
		{
			if (recordsPerPage < 1 || recordsPerPage > 300)
			{
				throw new ArgumentOutOfRangeException(nameof(recordsPerPage), "Records per page must be between 1 and 300");
			}

			return _client
				.GetAsync("accounts")
				.WithArgument("page_size", recordsPerPage)
				.WithArgument("next_page_token", pagingToken)
				.WithCancellationToken(cancellationToken)
				.AsPaginatedResponseWithToken<Account>("accounts");
		}

		/// <inheritdoc/>
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
				.PostAsync("accounts")
				.WithJsonBody(data)
				.WithCancellationToken(cancellationToken)
				.AsObject<Account>();
		}

		/// <inheritdoc/>
		public Task<Account> GetAsync(long accountId, CancellationToken cancellationToken = default)
		{
			// The information returned from this API call is vastly different than what is returned by GetAllAsync
			return _client
				.GetAsync($"accounts/{accountId}")
				.WithCancellationToken(cancellationToken)
				.AsObject<Account>();
		}

		/// <inheritdoc/>
		public Task DisassociateAsync(long accountId, CancellationToken cancellationToken = default)
		{
			return _client
				.DeleteAsync($"accounts/{accountId}")
				.WithCancellationToken(cancellationToken)
				.AsMessage();
		}

		/// <inheritdoc/>
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

		/// <inheritdoc/>
		public async Task<AuthenticationSettings> GetMeetingAuthenticationSettingsAsync(long accountId, CancellationToken cancellationToken = default)
		{
			var response = await _client
				.GetAsync($"accounts/{accountId}/settings")
				.WithArgument("option", "meeting_authentication")
				.WithCancellationToken(cancellationToken)
				.AsJson()
				.ConfigureAwait(false);

			var settings = new AuthenticationSettings()
			{
				RequireAuthentication = response.GetPropertyValue("meeting_authentication", false),
				AuthenticationOptions = response.GetProperty("authentication_options", false)?.ToObject<AuthenticationOptions[]>() ?? Array.Empty<AuthenticationOptions>()
			};

			return settings;
		}

		/// <inheritdoc/>
		public async Task<AuthenticationSettings> GetRecordingAuthenticationSettingsAsync(long accountId, CancellationToken cancellationToken = default)
		{
			var response = await _client
				.GetAsync($"accounts/{accountId}/settings")
				.WithArgument("option", "recording_authentication")
				.WithCancellationToken(cancellationToken)
				.AsJson()
				.ConfigureAwait(false);

			var settings = new AuthenticationSettings()
			{
				RequireAuthentication = response.GetPropertyValue("recording_authentication", false),
				AuthenticationOptions = response.GetProperty("authentication_options", false)?.ToObject<AuthenticationOptions[]>() ?? Array.Empty<AuthenticationOptions>()
			};

			return settings;
		}

		/// <inheritdoc/>
		public async Task<(string Domain, string Status)[]> GetManagedDomainsAsync(long accountId, CancellationToken cancellationToken = default)
		{
			var response = await _client
				.GetAsync($"accounts/{accountId}/managed_domains")
				.WithCancellationToken(cancellationToken)
				.AsJson("domains")
				.ConfigureAwait(false);

			var managedDomains = response
				.EnumerateArray()
				.Select(jsonElement =>
				{
					var key = jsonElement.GetPropertyValue("domain", string.Empty);
					var value = jsonElement.GetPropertyValue("status", string.Empty);
					return (key, value);
				}).ToArray();

			return managedDomains;
		}

		/// <inheritdoc/>
		public Task<string[]> GetTrustedDomainsAsync(long accountId, CancellationToken cancellationToken = default)
		{
			return _client
				.GetAsync($"accounts/{accountId}/trusted_domains")
				.WithCancellationToken(cancellationToken)
				.AsObject<string[]>("trusted_domains");
		}

		/// <inheritdoc/>
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
