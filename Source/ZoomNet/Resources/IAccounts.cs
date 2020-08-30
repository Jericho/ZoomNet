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
	/// <remarks>
	/// See <a href="https://marketplace.zoom.us/docs/api-reference/zoom-api/accounts/accounts">Zoom documentation</a> for more information.
	/// </remarks>
	public interface IAccounts
	{
		/// <summary>
		/// Retrieve all sub accounts under the master account.
		/// </summary>
		/// <param name="recordsPerPage">The number of records returned within a single API call.</param>
		/// <param name="page">The current page number of returned records.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>
		/// An array of <see cref="Account">accounts</see>.
		/// </returns>
		[Obsolete("Zoom is in the process of deprecating the \"page number\" and \"page count\" fields.")]
		Task<PaginatedResponse<Account>> GetAllAsync(int recordsPerPage = 30, int page = 1, CancellationToken cancellationToken = default);

		/// <summary>
		/// Retrieve all sub accounts under the master account.
		/// </summary>
		/// <param name="recordsPerPage">The number of records returned within a single API call.</param>
		/// <param name="pagingToken">The paging token.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>
		/// An array of <see cref="Account">accounts</see>.
		/// </returns>
		Task<PaginatedResponseWithToken<Account>> GetAllAsync(int recordsPerPage = 30, string pagingToken = null, CancellationToken cancellationToken = default);

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
		Task<Account> CreateAsync(string firstName, string lastName, string email, string password, bool useSharedVirtualRoomConnectors = false, IEnumerable<string> roomConnectorsIpAddresses = null, bool useSharedMeetingConnectors = false, IEnumerable<string> meetingConnectorsIpAddresses = null, PayMode payMode = PayMode.Master, CancellationToken cancellationToken = default);

		/// <summary>
		/// Retrieve the details of a sub account.
		/// </summary>
		/// <param name="accountId">The account Id.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>
		/// The <see cref="Account" />.
		/// </returns>
		Task<Account> GetAsync(string accountId, CancellationToken cancellationToken = default);

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
		Task DisassociateAsync(string accountId, CancellationToken cancellationToken = default);
	}
}
