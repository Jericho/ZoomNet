using System;
using System.Threading;
using System.Threading.Tasks;
using ZoomNet.Models;

namespace ZoomNet.Resources
{
	/// <summary>
	/// Allows you to access Zoom Phone API endpoints.
	/// </summary>
	/// <remarks>
	/// See <a href="https://developers.zoom.us/docs/api/rest/reference/phone/methods/#overview">
	/// Zoom API documentation</a> for more information.
	/// </remarks>
	public interface IPhone
	{
		#region Recordings endpoints

		/// <summary>
		/// Get recording of a specific phone call by its call ID or call log ID.
		/// </summary>
		/// <param name="callId">The call ID or call log ID.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>
		/// The requested <see cref="PhoneCallRecording" />, if any.
		/// </returns>
		/// <remarks>
		/// See <a href="https://developers.zoom.us/docs/api/rest/reference/phone/methods/#operation/getPhoneRecordingsByCallIdOrCallLogId">
		/// Zoom endpoint documentation</a> for more information.
		/// </remarks>
		Task<PhoneCallRecording> GetRecordingAsync(
			string callId, CancellationToken cancellationToken = default);

		/// <summary>
		/// Get transcript of a specific phone call recording by its recording ID.
		/// </summary>
		/// <param name="recordingId">The call recording ID.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>
		/// The requested <see cref="PhoneCallRecordingTranscript"/>, if any.
		/// </returns>
		/// <remarks>
		/// See <a href="https://developers.zoom.us/docs/api/rest/reference/phone/methods/#operation/phoneDownloadRecordingTranscript">
		/// Zoom endpoint documentation</a> for more information.
		/// </remarks>
		Task<PhoneCallRecordingTranscript> GetRecordingTranscriptAsync(
			string recordingId, CancellationToken cancellationToken = default);

		#endregion

		#region Users Endpoints

		/// <summary>
		/// Retrieves the phone call user profile for the specified user ID asynchronously.
		/// </summary>
		/// <param name="userId">The ID of the user for which to retrieve the phone call user profile.</param>
		/// <param name="cancellationToken">A cancellation token that can be used to cancel the asynchronous operation.</param>
		/// <returns>A task representing the asynchronous operation. The task result contains the phone call user profile.</returns>
		Task<PhoneCallUserProfile> GetPhoneCallUserProfileAsync(
			string userId, CancellationToken cancellationToken = default);

		#endregion

		#region SMS endpoints

		/// <summary>
		/// Get details about an SMS session.
		/// </summary>
		/// <param name="sessionId">SMS session ID.</param>
		/// <param name="from">The start time and date. The date range defined by the <paramref name="to"/> and <paramref name="from"/> parameters should be a month as the response only includes one month's worth of data at once. If unspecified, returns data from the past 30 days</param>
		/// <param name="to">Required only when the from parameter is specified.</param>
		/// <param name="orderAscending">Order of SMS to return based on creation time. True - ascending, false - descending, null - doesn't sort.</param>
		/// <param name="recordsPerPage">The number of records returned within a single API call. Max value 100.</param>
		/// <param name="pagingToken">The page identifier of returned records.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>An array of <see cref="SmsHistory">sms histories</see>.</returns>
		/// <remarks>
		/// See <a href="https://developers.zoom.us/docs/api/rest/reference/phone/methods/#operation/smsSessionDetails">
		/// Zoom endpoint documentation</a> for more information.
		/// </remarks>
		Task<PaginatedResponseWithToken<SmsHistory>> GetSmsSessionDetailsAsync(
			string sessionId, DateTime? from, DateTime? to, bool? orderAscending = true, int recordsPerPage = 30, string pagingToken = null, CancellationToken cancellationToken = default);

		#endregion
	}
}
