using Pathoschild.Http.Client;
using System;
using System.Threading;
using System.Threading.Tasks;
using ZoomNet.Models;
using ZoomNet.Utilities;

namespace ZoomNet.Resources
{
	/// <inheritdoc/>
	[Obsolete("The CallLogs API endpoints will be deprecated in April 2026. Please use the CallHistory instead. For more information, please refer to https://developers.zoom.us/docs/phone/migrate/")]
	public class CallLogs : ICallLogs
	{
		private readonly IClient _client;

		/// <summary>
		/// Initializes a new instance of the <see cref="CallLogs" /> class.
		/// </summary>
		/// <param name="client">The HTTP client.</param>
		internal CallLogs(IClient client)
		{
			_client = client;
		}

		/// <inheritdoc/>
		public Task<PaginatedResponseWithToken<UserCallLog>> GetAsync(string userId, DateOnly? from = null, DateOnly? to = null, CallLogType type = CallLogType.All, string phoneNumber = null, int recordsPerPage = 30, string pagingToken = null, CancellationToken cancellationToken = default)
		{
			Utils.ValidateRecordPerPage(recordsPerPage);

			return _client
				.GetAsync($"phone/users/{userId}/call_logs")
				.WithArgument("from", from.ToZoomFormat())
				.WithArgument("to", to.ToZoomFormat())
				.WithArgument("type", type.ToEnumString())
				.WithArgument("phone_number", phoneNumber)
				.WithArgument("page_size", recordsPerPage)
				.WithArgument("next_page_token", pagingToken)
				.WithCancellationToken(cancellationToken)
				.AsPaginatedResponseWithToken<UserCallLog>("call_logs");
		}

		/// <inheritdoc/>
		public Task<PaginatedResponseWithTokenAndDateRange<AccountCallLog>> GetAsync(DateOnly? from = null, DateOnly? to = null, CallLogType type = CallLogType.All, CallLogPathType? pathType = null, CallLogTimeType? timeType = CallLogTimeType.StartTime, string siteId = null, bool chargedCallLogs = false, int recordsPerPage = 30, string pagingToken = null, CancellationToken cancellationToken = default)
		{
			Utils.ValidateRecordPerPage(recordsPerPage);

			return _client
				.GetAsync("phone/call_logs")
				.WithArgument("from", from.ToZoomFormat())
				.WithArgument("to", to.ToZoomFormat())
				.WithArgument("type", type.ToEnumString())
				.WithArgument("path", pathType?.ToEnumString())
				.WithArgument("timeType", timeType?.ToEnumString())
				.WithArgument("site_id", siteId)
				.WithArgument("charged_call_logs", chargedCallLogs)
				.WithArgument("page_size", recordsPerPage)
				.WithArgument("next_page_token", pagingToken)
				.WithCancellationToken(cancellationToken)
				.AsPaginatedResponseWithTokenAndDateRange<AccountCallLog>("call_logs");
		}
	}
}
