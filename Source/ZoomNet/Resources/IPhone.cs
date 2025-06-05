using System.Threading;
using System.Threading.Tasks;
using ZoomNet.Models;
using ZoomNet.Models.CallHandlingSettings;

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

		/// <summary>
		/// Retrieves a list of all of an account's users who are assigned a Zoom Phone license.
		/// </summary>
		/// <param name="recordsPerPage">The number of records returned from a single API call. Default is 30.</param>
		/// <param name="nextPageToken">
		/// The next page token paginates through a large set of results.
		/// A next page token is returned whenever the set of available results exceeds the current page size.
		/// The expiration period for this token is 15 minutes.
		/// </param>
		/// <param name="siteId">The unique identifier of the site.</param>
		/// <param name="callingType">The type of calling plan.</param>
		/// <param name="status">The status of the Zoom Phone user.</param>
		/// <param name="department">The department where the user belongs.</param>
		/// <param name="costCenter">The cost center where the user belongs.</param>
		/// <param name="keyword">The partial string of user's name, extension number, or phone number.</param>
		/// <param name="cancellationToken">A cancellation token that can be used to cancel the asynchronous operation.</param>
		/// <returns>
		/// A task representing the asynchronous operation. The task result contains an array of Zoom Phone users in type of <see cref="PhoneUser"/>.
		/// </returns>
		Task<PaginatedResponseWithToken<PhoneUser>> ListPhoneUsersAsync(
			int recordsPerPage = 30,
			string nextPageToken = null,
			string siteId = null,
			int? callingType = null,
			PhoneCallUserStatus? status = null,
			string department = null,
			string costCenter = null,
			string keyword = null,
			CancellationToken cancellationToken = default);

		/// <summary>
		/// Retrieves a list of all of an account's phone numbers.
		/// </summary>
		/// <param name="recordsPerPage">The number of records returned from a single API call. Default is 30.</param>
		/// <param name="nextPageToken">
		/// The next page token paginates through a large set of results.
		/// A next page token is returned whenever the set of available results exceeds the current page size.
		/// The expiration period for this token is 15 minutes.
		/// </param>
		/// <param name="siteId">The unique identifier of the site.</param>
		/// <param name="assignmentType">The assignment status of the phone number.</param>
		/// <param name="extensionType">The type of extension.</param>
		/// <param name="numberType">The type of phone number.</param>
		/// <param name="pendingNumbers">Indicates whether to include pending numbers.</param>
		/// <param name="keyword">The partial string of phone number.</param>
		/// <param name="cancellationToken">A cancellation token that can be used to cancel the asynchronous operation.</param>
		/// <returns>
		/// A task representing the asynchronous operation. The task result contains an array of phone numbers in type of <see cref="PhoneDetail"/>.
		/// </returns>
		Task<PaginatedResponseWithToken<PhoneDetail>> ListPhonesAsync(
			int recordsPerPage = 30,
			string nextPageToken = null,
			string siteId = null,
			PhoneDetailAssignmentType? assignmentType = null,
			PhoneDetailExtensionType? extensionType = null,
			PhoneDetailNumberType? numberType = null,
			bool? pendingNumbers = null,
			string keyword = null,
			CancellationToken cancellationToken = default);

		/// <summary>
		/// Updates a Zoom Phone's call handling setting.
		/// Call handling settings allow you to control how your system routes calls during business, closed, or holiday hours.
		/// </summary>
		/// <param name="extensionId">Extension id.</param>
		/// <param name="settingType">Call handling setting type.</param>
		/// <param name="subsettings">Call handling subsettings. Allowed subsettings:
		/// <list type="bullet">
		/// <item><see cref="CallHandlingSubsettings"/></item>
		/// <item><see cref="CallForwardingSubsettings"/></item>
		/// <item><see cref="HolidaySubsettings"/></item>
		/// <item><see cref="CustomHoursSubsettings"/></item>
		/// </list></param>
		/// <param name="cancellationToken">A cancellation token that can be used to cancel the asynchronous operation.</param>
		/// <returns>A Task representing the asynchronous operation.</returns>
		Task UpdateCallHandlingSettingsAsync(
			string extensionId,
			CallHandlingSettingType settingType,
			CallHandlingSubsettingsBase subsettings,
			CancellationToken cancellationToken);

		#endregion
	}
}
