using Newtonsoft.Json.Linq;
using Pathoschild.Http.Client;
using System;
using System.Collections.Generic;
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
				.WithArgument("page", page)
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
		/// An array of <see cref="Account">accounts</see>.
		/// </returns>
		public Task<Account> CreateAsync(string firstName, string lastName, string email, string password, bool useSharedVirtualRoomConnectors = false, IEnumerable<string> roomConnectorsIpAddresses = null, bool useSharedMeetingConnectors = false, IEnumerable<string> meetingConnectorsIpAddresses = null, PayMode payMode = PayMode.Master, CancellationToken cancellationToken = default)
		{
			var data = new JObject()
			{
				{ "first_name", firstName },
				{ "last_name", lastName },
				{ "email", email },
				{ "password", password }
			};
			data.AddPropertyIfValue("options/share_rc", useSharedVirtualRoomConnectors);
			data.AddPropertyIfValue("options/room_connectors", string.Join(",", roomConnectorsIpAddresses ?? Array.Empty<string>()));
			data.AddPropertyIfValue("options/share_mc", useSharedMeetingConnectors);
			data.AddPropertyIfValue("options/meeting_connectors", string.Join(",", meetingConnectorsIpAddresses ?? Array.Empty<string>()));
			data.AddPropertyIfValue("options/pay_mode", payMode);

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
		public Task<Account> GetAsync(string accountId, CancellationToken cancellationToken = default)
		{
			//The information returned from this API call is vastly different than what is returned by GetAllAsync
			//so they can't both return 'Account'
			return _client
				.GetAsync($"accounts/{accountId}")
				.WithCancellationToken(cancellationToken)
				.AsObject<Account>();
		}
	}
}
