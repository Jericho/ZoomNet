using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using ZoomNet.Models;

namespace ZoomNet.Resources
{
	/// <summary>
	/// Allows you to manage cloud recordings.
	/// </summary>
	/// <remarks>
	/// See <a href="https://marketplace.zoom.us/docs/api-reference/zoom-api/cloud-recording/">Zoom documentation</a> for more information.
	/// </remarks>
	public interface ICloudRecordings
	{
		/// <summary>
		/// Retrieve all cloud recordings for a user.
		/// </summary>
		/// <param name="userId">The user Id or email address.</param>
		/// <param name="queryTrash">Indicates if you want to list recordings from trash.</param>
		/// <param name="from">The start date.</param>
		/// <param name="to">The end date.</param>
		/// <param name="recordsPerPage">The number of records returned within a single API call.</param>
		/// <param name="page">The current page number of returned records.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>
		/// An array of <see cref="Recording">recordings</see>.
		/// </returns>
		[Obsolete("Zoom is in the process of deprecating the \"page number\" and \"page count\" fields.")]
		Task<PaginatedResponseWithTokenAndDateRange<Recording>> GetRecordingsForUserAsync(string userId, bool queryTrash = false, DateTime? from = null, DateTime? to = null, int recordsPerPage = 30, int page = 1, CancellationToken cancellationToken = default);

		/// <summary>
		/// Retrieve all cloud recordings for a user.
		/// </summary>
		/// <param name="userId">The user Id or email address.</param>
		/// <param name="queryTrash">Indicates if you want to list recordings from trash.</param>
		/// <param name="from">The start date.</param>
		/// <param name="to">The end date.</param>
		/// <param name="recordsPerPage">The number of records returned within a single API call.</param>
		/// <param name="pagingToken">The paging token.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>
		/// An array of <see cref="Recording">recordings</see>.
		/// </returns>
		Task<PaginatedResponseWithTokenAndDateRange<Recording>> GetRecordingsForUserAsync(string userId, bool queryTrash = false, DateTime? from = null, DateTime? to = null, int recordsPerPage = 30, string pagingToken = null, CancellationToken cancellationToken = default);

		/// <summary>
		/// Retrieve all cloud recordings for a meeting or webinar.
		/// </summary>
		/// <param name="meetingId">The meeting Id or UUID.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>Details of recordings made for a particular meeding or webinar.</returns>
		Task<Recording> GetRecordingsAsync(string meetingId, CancellationToken cancellationToken = default);

		/// <summary>
		/// Move recording files for a meeting to trash.
		/// </summary>
		/// <param name="meetingId">The meeting Id or UUID.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>
		/// The async task.
		/// </returns>
		Task MoveRecordingsToTrashAsync(string meetingId, CancellationToken cancellationToken = default);

		/// <summary>
		/// Permanently delete recording files for a meeting.
		/// </summary>
		/// <param name="meetingId">The meeting Id or UUID.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>
		/// The async task.
		/// </returns>
		Task DeleteRecordingsAsync(string meetingId, CancellationToken cancellationToken = default);

		/// <summary>
		/// Move a specific recording file for a meeting to trash.
		/// </summary>
		/// <param name="meetingId">The meeting Id or UUID.</param>
		/// <param name="recordingId">The recording id.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>
		/// The async task.
		/// </returns>
		Task MoveRecordingToTrashAsync(string meetingId, string recordingId, CancellationToken cancellationToken = default);

		/// <summary>
		/// Permanently delete a specific recording file for a meeting.
		/// </summary>
		/// <param name="meetingId">The meeting Id or UUID.</param>
		/// <param name="recordingId">The recording id.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>
		/// The async task.
		/// </returns>
		Task DeleteRecordingAsync(string meetingId, string recordingId, CancellationToken cancellationToken = default);

		/// <summary>
		/// Recover all deleted recordings of a meeting from trash.
		/// </summary>
		/// <param name="meetingId">The meeting Id or UUID.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>
		/// The async task.
		/// </returns>
		/// <remarks>Zoom allows recordings to be recovered from trash for up to 30 days from deletion date.</remarks>
		Task RecoverRecordingsAsync(string meetingId, CancellationToken cancellationToken = default);

		/// <summary>
		/// Recover a specific recording file of a meeting.
		/// </summary>
		/// <param name="meetingId">The meeting Id or UUID.</param>
		/// <param name="recordingId">The recording id.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>
		/// The async task.
		/// </returns>
		Task RecoverRecordingAsync(string meetingId, string recordingId, CancellationToken cancellationToken = default);

		/// <summary>
		/// Retrieve settings applied to a meeting's cloud recording.
		/// </summary>
		/// <param name="meetingId">The meeting Id or UUID.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>
		/// The <see cref="RecordingSettings" />.
		/// </returns>
		Task<RecordingSettings> GetRecordingSettingsAsync(string meetingId, CancellationToken cancellationToken = default);

		/// <summary>
		/// Retrieve all registrants for a recording.
		/// </summary>
		/// <param name="meetingId">The meeting Id or UUID.</param>
		/// <param name="recordsPerPage">The number of records returned within a single API call.</param>
		/// <param name="page">The current page number of returned records.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>
		/// An array of <see cref="Registrant">registrants</see>.
		/// </returns>
		[Obsolete("Zoom is in the process of deprecating the \"page number\" and \"page count\" fields.")]
		Task<PaginatedResponse<Registrant>> GetRecordingRegistrantsAsync(string meetingId, int recordsPerPage = 30, int page = 1, CancellationToken cancellationToken = default);

		/// <summary>
		/// Retrieve all registrants for a recording.
		/// </summary>
		/// <param name="meetingId">The meeting Id or UUID.</param>
		/// <param name="recordsPerPage">The number of records returned within a single API call.</param>
		/// <param name="pagingToken">The paging token.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>
		/// An array of <see cref="Registrant">registrants</see>.
		/// </returns>
		Task<PaginatedResponseWithToken<Registrant>> GetRecordingRegistrantsAsync(string meetingId, int recordsPerPage = 30, string pagingToken = null, CancellationToken cancellationToken = default);

		/// <summary>
		/// Add a registrant to an on-demand recording.
		/// </summary>
		/// <param name="meetingId">The meeting ID.</param>
		/// <param name="email">Registrant's email address.</param>
		/// <param name="firstName">Registrant's first name.</param>
		/// <param name="lastName">Registrant's last name.</param>
		/// <param name="address">Registrant's address.</param>
		/// <param name="city">Registrant's city.</param>
		/// <param name="country">Registrant's country.</param>
		/// <param name="zip">Registrant's zip/postal code.</param>
		/// <param name="state">Registrant's state/province.</param>
		/// <param name="phone">Registrant's phone number.</param>
		/// <param name="industry">Registrant's industry.</param>
		/// <param name="organization">Registrant's organization.</param>
		/// <param name="jobTitle">Registrant's job title.</param>
		/// <param name="purchasingTimeFrame">This field can be included to gauge interest of attendees towards buying your product or service.</param>
		/// <param name="roleInPurchaseProcess">Registrant's role in purchase decision.</param>
		/// <param name="numberOfEmployees">Number of employees.</param>
		/// <param name="comments">A field that allows registrants to provide any questions or comments that they might have.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>
		/// A <see cref="RecordingRegistration" />.
		/// </returns>
		Task<RecordingRegistration> AddRegistrantAsync(long meetingId, string email, string firstName, string lastName, string address, string city, string country, string zip, string state, string phone, string industry, string organization, string jobTitle, string purchasingTimeFrame, string roleInPurchaseProcess, string numberOfEmployees, string comments, CancellationToken cancellationToken = default);

		/// <summary>
		/// Approve a registration for a meeting.
		/// </summary>
		/// <param name="meetingId">The meeting ID.</param>
		/// <param name="registrantId">The registrant ID.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>
		/// The async task.
		/// </returns>
		Task ApproveRegistrantAsync(long meetingId, string registrantId, CancellationToken cancellationToken = default);

		/// <summary>
		/// Approve multiple registrations for a meeting.
		/// </summary>
		/// <param name="meetingId">The meeting ID.</param>
		/// <param name="registrantIds">ID for each registrant to be approved.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>
		/// The async task.
		/// </returns>
		Task ApproveRegistrantsAsync(long meetingId, IEnumerable<string> registrantIds, CancellationToken cancellationToken = default);

		/// <summary>
		/// Reject a registration for a meeting.
		/// </summary>
		/// <param name="meetingId">The meeting ID.</param>
		/// <param name="registrantId">The registrant ID.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>
		/// The async task.
		/// </returns>
		Task RejectRegistrantAsync(long meetingId, string registrantId, CancellationToken cancellationToken = default);

		/// <summary>
		/// Reject multiple registrations for a meeting.
		/// </summary>
		/// <param name="meetingId">The meeting ID.</param>
		/// <param name="registrantIds">ID for each registrant to be rejected.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>
		/// The async task.
		/// </returns>
		Task RejectRegistrantsAsync(long meetingId, IEnumerable<string> registrantIds, CancellationToken cancellationToken = default);

		/// <summary>
		/// Download the recording file.
		/// </summary>
		/// <param name="downloadUrl">The URL of the recording file to download.</param>
		/// <param name="cancellationToken">Cancellation token.</param>
		/// <returns>
		/// The <see cref="Stream"/> containing the file.
		/// </returns>
		Task<Stream> DownloadFileAsync(string downloadUrl, CancellationToken cancellationToken = default);
	}
}
