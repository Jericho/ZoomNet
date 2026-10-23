using Pathoschild.Http.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Nodes;
using System.Threading;
using System.Threading.Tasks;
using ZoomNet.Models;
using ZoomNet.Utilities;

namespace ZoomNet.Resources
{
	/// <inheritdoc/>
	public class CallHistory : ICallHistory
	{
		private readonly IClient _client;

		/// <summary>
		/// Initializes a new instance of the <see cref="CallHistory" /> class.
		/// </summary>
		/// <param name="client">The HTTP client.</param>
		internal CallHistory(IClient client)
		{
			_client = client;
		}

		/// <inheritdoc/>
		public Task AddClientCodeToCallHistory(string callLogId, string clientCode, CancellationToken cancellationToken = default)
		{
			var data = new JsonObject
			{
				{ "client_code", clientCode },
			};

			return _client
				.PatchAsync($"phone/call_history/{callLogId}/client_code")
				.WithJsonBody(data)
				.WithCancellationToken(cancellationToken)
				.AsMessage();
		}

		/// <inheritdoc/>
		public Task DeleteUserCallHistory(string userId, string callLogId, CancellationToken cancellationToken = default)
		{
			return _client
				.DeleteAsync($"phone/users/{userId}/phone/call_history/{callLogId}")
				.WithCancellationToken(cancellationToken)
				.AsMessage();
		}

		/// <inheritdoc/>
		public Task<PaginatedResponseWithTokenAndDateRange<CallElement>> GetAccountCallHistoryAsync(
			DateOnly? from = null,
			DateOnly? to = null,
			string keyword = null,
			IEnumerable<CallElementDirection> directions = null,
			IEnumerable<CallElementConnectType> connectionTypes = null,
			IEnumerable<CallElementNumberType> numberTypes = null,
			IEnumerable<CallElementCallType> callTypes = null,
			IEnumerable<CallElementExtensionType> extensionTypes = null,
			IEnumerable<CallElementResult> callResults = null,
			IEnumerable<string> groupIds = null,
			IEnumerable<string> siteIds = null,
			string department = null,
			string costCenter = null,
			CallLogTimeType? timeType = null,
			bool? isRecorded = null,
			bool? withVoicemail = null,
			int recordsPerPage = 30,
			string pagingToken = null,
			CancellationToken cancellationToken = default)
		{
			Utils.ValidateRecordPerPage(recordsPerPage);

			return _client
				.GetAsync("phone/call_history")
				.WithArgument("from", from.ToZoomFormat())
				.WithArgument("to", to.ToZoomFormat())
				.WithArgument("keyword", keyword)
				.WithArguments(directions?.Select(item => new KeyValuePair<string, string>("directions", item.ToEnumString())))
				.WithArguments(connectionTypes?.Select(item => new KeyValuePair<string, string>("connection_types", item.ToEnumString())))
				.WithArguments(numberTypes?.Select(item => new KeyValuePair<string, string>("number_types", item.ToEnumString())))
				.WithArguments(callTypes?.Select(item => new KeyValuePair<string, string>("call_types", item.ToEnumString())))
				.WithArguments(extensionTypes?.Select(item => new KeyValuePair<string, string>("extension_types", item.ToEnumString())))
				.WithArguments(callResults?.Select(item => new KeyValuePair<string, string>("call_results", item.ToEnumString())))
				.WithArguments(groupIds?.Select(item => new KeyValuePair<string, string>("group_ids", item)))
				.WithArguments(siteIds?.Select(item => new KeyValuePair<string, string>("site_ids", item)))
				.WithArgument("department", department)
				.WithArgument("cost_center", costCenter)
				.WithArgument("time_type", timeType?.ToEnumString())
				.WithArgument("recording_status", isRecorded.HasValue ? (isRecorded.Value ? "recorded" : "non-recorded") : null)
				.WithArgument("with_voicemail", withVoicemail)
				.WithArgument("page_size", recordsPerPage)
				.WithArgument("next_page_token", pagingToken)
				.WithCancellationToken(cancellationToken)
				.AsPaginatedResponseWithTokenAndDateRange<CallElement>("call_history");
		}

		/// <inheritdoc/>
		public Task<CallElement> Get​CallElementAsync(string callElementId, CancellationToken cancellationToken = default)
		{
			return _client
				.GetAsync($"phone/call_element/{callElementId}")
				.WithCancellationToken(cancellationToken)
				.AsObject<CallElement>();
		}

		/// <inheritdoc/>
		public Task<CallHistory> GetCallHistoryAsync(string callHistoryUuid, CancellationToken cancellationToken = default)
		{
			return _client
				.GetAsync($"phone/call_history/{callHistoryUuid}")
				.WithCancellationToken(cancellationToken)
				.AsObject<CallHistory>();
		}

		/// <inheritdoc/>
		public Task<AiCallSummaryDetail> GetUserAICallSummaryDetail​Async(string userId, string aiCallSummaryId, CancellationToken cancellationToken = default)
		{
			return _client
				.GetAsync($"phone/user/{userId}/ai_call_summary/{aiCallSummaryId}")
				.WithCancellationToken(cancellationToken)
				.AsObject<AiCallSummaryDetail>();
		}

		/// <inheritdoc/>
		public Task<PaginatedResponseWithTokenAndDateRange<CallElement>> GetUserCallHistoryAsync(
			string userId,
			DateOnly? from = null,
			DateOnly? to = null,
			string keyword = null,
			IEnumerable<CallElementDirection> directions = null,
			IEnumerable<CallElementConnectType> connectionTypes = null,
			IEnumerable<CallElementNumberType> numberTypes = null,
			IEnumerable<CallElementCallType> callTypes = null,
			IEnumerable<CallElementExtensionType> extensionTypes = null,
			IEnumerable<CallElementResult> callResults = null,
			IEnumerable<string> groupIds = null,
			IEnumerable<string> siteIds = null,
			string department = null,
			string costCenter = null,
			CallLogTimeType? timeType = null,
			bool? isRecorded = null,
			bool? withVoicemail = null,
			int recordsPerPage = 30,
			string pagingToken = null,
			CancellationToken cancellationToken = default)
		{
			Utils.ValidateRecordPerPage(recordsPerPage);

			return _client
				.GetAsync($"phone/users/{userId}/call_history")
				.WithArgument("from", from.ToZoomFormat())
				.WithArgument("to", to.ToZoomFormat())
				.WithArgument("keyword", keyword)
				.WithArguments(directions?.Select(item => new KeyValuePair<string, string>("directions", item.ToEnumString())))
				.WithArguments(connectionTypes?.Select(item => new KeyValuePair<string, string>("connection_types", item.ToEnumString())))
				.WithArguments(numberTypes?.Select(item => new KeyValuePair<string, string>("number_types", item.ToEnumString())))
				.WithArguments(callTypes?.Select(item => new KeyValuePair<string, string>("call_types", item.ToEnumString())))
				.WithArguments(extensionTypes?.Select(item => new KeyValuePair<string, string>("extension_types", item.ToEnumString())))
				.WithArguments(callResults?.Select(item => new KeyValuePair<string, string>("call_results", item.ToEnumString())))
				.WithArguments(groupIds?.Select(item => new KeyValuePair<string, string>("group_ids", item)))
				.WithArguments(siteIds?.Select(item => new KeyValuePair<string, string>("site_ids", item)))
				.WithArgument("department", department)
				.WithArgument("cost_center", costCenter)
				.WithArgument("time_type", timeType?.ToEnumString())
				.WithArgument("recording_status", isRecorded.HasValue ? (isRecorded.Value ? "recorded" : "non-recorded") : null)
				.WithArgument("with_voicemail", withVoicemail)
				.WithArgument("page_size", recordsPerPage)
				.WithArgument("next_page_token", pagingToken)
				.WithCancellationToken(cancellationToken)
				.AsPaginatedResponseWithTokenAndDateRange<CallElement>("call_logs");
		}

		/// <inheritdoc/>
		public async Task<(CallElement[] CallElements, CallLog[] CallLogs)> SynchronizeUserCallHistoryAsync(string userId, SynchronizationType synchronizationType, int recordsPerPage, string pagingToken, CancellationToken cancellationToken = default)
		{
			Utils.ValidateRecordPerPage(recordsPerPage);

			var response = await _client
				.GetAsync($"phone/users/{userId}/call_history/sync")
				.WithArgument("synchronization_type", synchronizationType.ToEnumString())
				.WithArgument("count", recordsPerPage) // Normally this parameter is called "page_size" but the sync endpoint uses "count"
				.WithArgument("sync_token", pagingToken) // Normally this parameter is called "next_page_token" but the sync endpoint uses "sync_token"
				.WithCancellationToken(cancellationToken)
				.AsJson()
				.ConfigureAwait(false);

			var callElements = response.GetPropertyValue("call_elements", Array.Empty<CallElement>());
			var callLogs = response.GetPropertyValue("call_logs", Array.Empty<CallLog>());

			return (callElements, callLogs);
		}
	}
}
