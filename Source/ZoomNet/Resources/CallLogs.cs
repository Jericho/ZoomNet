using Pathoschild.Http.Client;
using System;
using System.Threading;
using System.Threading.Tasks;
using ZoomNet.Models;

namespace ZoomNet.Resources
{
	/// <summary>
	/// Allows you to manage call logs.
	/// </summary>
	/// <seealso cref="ZoomNet.Resources.ICallLogs" />
	/// <remarks>
	/// See <a href="https://marketplace.zoom.us/docs/api-reference/phone/methods/#tag/Call-Logs">Zoom documentation</a> for more information.
	/// </remarks>
	public class CallLogs : ICallLogs
	{
		private readonly Pathoschild.Http.Client.IClient _client;

		/// <summary>
		/// Initializes a new instance of the <see cref="CallLogs" /> class.
		/// </summary>
		/// <param name="client">The HTTP client.</param>
		internal CallLogs(Pathoschild.Http.Client.IClient client)
		{
			_client = client;
		}

		/// <summary>
		/// Get call logs for specified user.
		/// </summary>
		/// <param name="userId">The user Id or email address.</param>
		/// <param name="from">The start date.</param>
		/// <param name="to">The end date.</param>
		/// <param name="type">Type of call log.</param>
		/// <param name="phoneNumber">Phone number for filtering call log.</param>
		/// <param name="recordsPerPage">The number of records to return.</param>
		/// <param name="pagingToken">The paging token.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>
		/// An array of <see cref="UserCallLog" />.
		/// </returns>
		public Task<PaginatedResponseWithToken<UserCallLog>> GetAsync(string userId, DateTime? from = null, DateTime? to = null, CallLogType type = CallLogType.All, string phoneNumber = null, int recordsPerPage = 30, string pagingToken = null, CancellationToken cancellationToken = default)
		{
			if (recordsPerPage < 1 || recordsPerPage > 300)
			{
				throw new ArgumentOutOfRangeException(nameof(recordsPerPage), "Records per page must be between 1 and 300");
			}

			return _client
				.GetAsync($"phone/users/{userId}/call_logs")
				.WithArgument("from", from.ToZoomFormat(dateOnly: true))
				.WithArgument("to", to.ToZoomFormat(dateOnly: true))
				.WithArgument("type", type.ToEnumString())
				.WithArgument("phone_number", phoneNumber)
				.WithArgument("page_size", recordsPerPage)
				.WithArgument("next_page_token", pagingToken)
				.WithCancellationToken(cancellationToken)
				.AsPaginatedResponseWithToken<UserCallLog>("call_logs");
		}
	}
}
