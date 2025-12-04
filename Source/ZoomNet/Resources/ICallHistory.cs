using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ZoomNet.Models;

namespace ZoomNet.Resources
{
	/// <summary>
	/// Allows you to manage call logs.
	/// </summary>
	/// <remarks>
	/// See <a href="https://developers.zoom.us/docs/api/phone/#tag/call-logs">Zoom documentation</a> for more information.
	/// </remarks>
	public interface ICallHistory
	{
		/// <summary>
		/// Adds the specified client code to the call history associated with the given identifier.
		/// </summary>
		/// <param name="callLogId">The unique identifier of the call element to which the client code will be added. Cannot be null or empty.</param>
		/// <param name="clientCode">The client code (3 to 16 digit number) to mark the call log.</param>
		/// <param name="cancellationToken">A cancellation token that can be used to cancel the asynchronous operation.</param>
		/// <returns>A task that represents the asynchronous operation of adding the client code to the call history.</returns>
		Task AddClientCodeToCallHistory(string callLogId, string clientCode, CancellationToken cancellationToken = default);

		/// <summary>
		/// Deletes the specified call history record for a user asynchronously.
		/// </summary>
		/// <param name="userId">The unique identifier of the user whose call history record will be deleted. Cannot be null or empty.</param>
		/// <param name="callLogId">The unique identifier of the call log to delete. Cannot be null or empty.</param>
		/// <param name="cancellationToken">A cancellation token that can be used to cancel the delete operation.</param>
		/// <returns>A task that represents the asynchronous delete operation.</returns>
		Task DeleteUserCallHistory(string userId, string callLogId, CancellationToken cancellationToken = default);

		/// <summary>
		/// Retrieves a paginated list of account call history records that match the specified filter criteria.
		/// </summary>
		/// <remarks>Filtering parameters are optional; omitting them will return all available call history records
		/// for the account. The method supports server-side paging. To retrieve additional pages, use the paging token
		/// returned in the response.</remarks>
		/// <param name="from">The start date and time, in UTC, for filtering call records. Only calls occurring on or after this date are
		/// included. If null, no lower date bound is applied.</param>
		/// <param name="to">The end date and time, in UTC, for filtering call records. Only calls occurring on or before this date are
		/// included. If null, no upper date bound is applied.</param>
		/// <param name="keyword">A keyword to search within call records. Matches may include caller or callee names, numbers, or other relevant
		/// fields. If null or empty, no keyword filtering is applied.</param>
		/// <param name="directions">A collection of call directions to filter by, such as inbound or outbound. If null or empty, all directions are
		/// included.</param>
		/// <param name="connectionTypes">A collection of connection types to filter by, such as internal or external calls. If null or empty, all
		/// connection types are included.</param>
		/// <param name="numberTypes">A collection of number types to filter by, such as extension or direct numbers. If null or empty, all number types
		/// are included.</param>
		/// <param name="callTypes">A collection of call types to filter by, such as voice or video calls. If null or empty, all call types are
		/// included.</param>
		/// <param name="extensionTypes">A collection of extension types to filter by. If null or empty, all extension types are included.</param>
		/// <param name="callResults">A collection of call results to filter by, such as answered or missed calls. If null or empty, all results are
		/// included.</param>
		/// <param name="groupIds">A collection of group identifiers to filter call records by group membership. If null or empty, calls from all
		/// groups are included.</param>
		/// <param name="siteIds">A collection of site identifiers to filter call records by site. If null or empty, calls from all sites are
		/// included.</param>
		/// <param name="department">The department name to filter call records. If null or empty, calls from all departments are included.</param>
		/// <param name="costCenter">The cost center name to filter call records. If null or empty, calls from all cost centers are included.</param>
		/// <param name="timeType">The type of time to use for filtering, such as start time or end time. If null, the default time type is used.</param>
		/// <param name="isRecorded">Indicates whether to filter for calls that were recorded. If <see langword="true"/>, only recorded calls are
		/// included; if <see langword="false"/>, only unrecorded calls are included; if null, both are included.</param>
		/// <param name="withVoicemail">Indicates whether to filter for calls with voicemail. If <see langword="true"/>, only calls with voicemail are
		/// included; if <see langword="false"/>, only calls without voicemail are included; if null, both are included.</param>
		/// <param name="recordsPerPage">The maximum number of call records to return per page. Must be a positive integer. The default is 30.</param>
		/// <param name="pagingToken">A token indicating the position in the paginated result set. Use the token from a previous response to retrieve
		/// the next page. If null, the first page is returned.</param>
		/// <param name="cancellationToken">A cancellation token that can be used to cancel the asynchronous operation.</param>
		/// <returns>A task that represents the asynchronous operation. The task result contains a paginated response with a collection
		/// of user call log records, a paging token for retrieving subsequent pages, and the date range covered by the
		/// results.</returns>
		Task<PaginatedResponseWithTokenAndDateRange<CallElement>> GetAccountCallHistoryAsync(
			DateTime? from = null,
			DateTime? to = null,
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
			CancellationToken cancellationToken = default);

		/// <summary>
		/// Asynchronously retrieves the call element associated with the specified identifier.
		/// </summary>
		/// <param name="callElementId">The unique identifier of the call element to retrieve. Cannot be null or empty.</param>
		/// <param name="cancellationToken">A cancellation token that can be used to cancel the asynchronous operation.</param>
		/// <returns>A task that represents the asynchronous operation. The task result contains the <see cref="CallElement"/>
		/// corresponding to the specified identifier, or <c>null</c> if no matching element is found.</returns>
		Task<CallElement> Get​CallElementAsync(string callElementId, CancellationToken cancellationToken = default);

		/// <summary>
		/// Retrieves the call log associated with the specified call history identifier.
		/// </summary>
		/// <param name="callHistoryUuid">The unique identifier of the call history for which to retrieve the call log. Cannot be null or empty.</param>
		/// <param name="cancellationToken">A token that can be used to cancel the asynchronous operation.</param>
		/// <returns>A task that represents the asynchronous operation to retrieve the call log.</returns>
		Task<> GetCallHistoryAsync(string callHistoryUuid, CancellationToken cancellationToken = default);

		/// <summary>
		/// Asynchronously retrieves detailed information about a specific AI call summary for a given user.
		/// </summary>
		/// <param name="userId">The unique identifier of the user whose AI call summary detail is to be retrieved. Cannot be null or empty.</param>
		/// <param name="aiCallSummaryId">The unique identifier of the AI call summary to retrieve details for. Cannot be null or empty.</param>
		/// <param name="cancellationToken">A cancellation token that can be used to cancel the asynchronous operation.</param>
		/// <returns>A task that represents the asynchronous operation.</returns>
		Task<> GetUserAICallSummaryDetail​Async(string userId, string aiCallSummaryId, CancellationToken cancellationToken = default);

		/// <summary>
		/// Retrieves a paginated list of call history records for the specified user, filtered by date range, call
		/// attributes, and organizational criteria.
		/// </summary>
		/// <remarks>If no filters are specified, all call history records for the user are returned, subject to
		/// pagination. Use the paging token from the response to retrieve additional pages of results. The method supports
		/// cancellation via the provided cancellation token.</remarks>
		/// <param name="userId">The unique identifier of the user whose call history is to be retrieved. Cannot be null or empty.</param>
		/// <param name="from">The start date and time for filtering call records. Only calls occurring on or after this date are included. If
		/// null, no lower date bound is applied.</param>
		/// <param name="to">The end date and time for filtering call records. Only calls occurring on or before this date are included. If
		/// null, no upper date bound is applied.</param>
		/// <param name="keyword">A keyword to search within call records, such as caller name or number. If null, no keyword filtering is applied.</param>
		/// <param name="directions">A collection of call directions to filter by, such as inbound or outbound. If null, calls of all directions are
		/// included.</param>
		/// <param name="connectionTypes">A collection of connection types to filter the call records, such as internal or external. If null, all connection
		/// types are included.</param>
		/// <param name="numberTypes">A collection of number types to filter the call records, such as direct or extension numbers. If null, all number
		/// types are included.</param>
		/// <param name="callTypes">A collection of call types to filter the call records, such as voice or video calls. If null, all call types are
		/// included.</param>
		/// <param name="extensionTypes">A collection of extension types to filter the call records. If null, all extension types are included.</param>
		/// <param name="callResults">A collection of call results to filter by, such as answered or missed calls. If null, all call results are
		/// included.</param>
		/// <param name="groupIds">A collection of group identifiers to filter the call records by group membership. If null, calls from all groups
		/// are included.</param>
		/// <param name="siteIds">A collection of site identifiers to filter the call records by site. If null, calls from all sites are included.</param>
		/// <param name="department">The department name to filter the call records. If null, calls from all departments are included.</param>
		/// <param name="costCenter">The cost center to filter the call records. If null, calls from all cost centers are included.</param>
		/// <param name="timeType">The type of time to use for filtering, such as call start or end time. If null, the default time type is used.</param>
		/// <param name="isRecorded">A value indicating whether to include only recorded calls (<see langword="true"/>), only unrecorded calls (<see
		/// langword="false"/>), or all calls (null).</param>
		/// <param name="withVoicemail">A value indicating whether to include only calls with voicemail (<see langword="true"/>), only calls without
		/// voicemail (<see langword="false"/>), or all calls (null).</param>
		/// <param name="recordsPerPage">The maximum number of call records to return in a single page. Must be a positive integer.</param>
		/// <param name="pagingToken">A token indicating the position in the paginated result set from which to continue retrieving records. If null,
		/// retrieval starts from the beginning.</param>
		/// <param name="cancellationToken">A token to monitor for cancellation requests. The operation is canceled if the token is triggered.</param>
		/// <returns>A task that represents the asynchronous operation. The task result contains a paginated response with call history
		/// records matching the specified filters, along with a token for retrieving subsequent pages and the date range
		/// covered.</returns>
		Task<PaginatedResponseWithTokenAndDateRange<CallElement>> GetUserCallHistoryAsync(
			string userId,
			DateTime? from = null,
			DateTime? to = null,
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
			CancellationToken cancellationToken = default);

		/// <summary>
		/// Synchronizes the call history for the specified user using the given synchronization type.
		/// </summary>
		/// <param name="userId">The unique identifier of the user whose call history is to be synchronized. Cannot be null or empty.</param>
		/// <param name="synchronizationType">The type of synchronization to perform. Determines whether a full or incremental synchronization is executed.</param>
		/// <param name="recordsPerPage">The maximum number of call history records to retrieve per page. Must be a positive integer.</param>
		/// <param name="pagingToken">A token indicating the position in the paged call history from which to continue synchronization. Pass null or
		/// empty to start from the beginning.</param>
		/// <param name="cancellationToken">A token to monitor for cancellation requests. The operation is canceled if this token is triggered.</param>
		/// <returns>A task that represents the asynchronous synchronization operation.</returns>
		Task SynchronizeUserCallHistoryAsync(string userId, SynchronizationType synchronizationType, int recordsPerPage, string pagingToken, CancellationToken cancellationToken = default);
	}
}
