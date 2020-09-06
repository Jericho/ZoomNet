using Pathoschild.Http.Client;
using System;
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
	public class CloudRecordings : ICloudRecordings
	{
		private readonly Pathoschild.Http.Client.IClient _client;

		/// <summary>
		/// Initializes a new instance of the <see cref="CloudRecordings" /> class.
		/// </summary>
		/// <param name="client">The HTTP client.</param>
		internal CloudRecordings(Pathoschild.Http.Client.IClient client)
		{
			_client = client;
		}

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
		public Task<PaginatedResponse<Recording>> GetRecordingsForUserAsync(string userId, bool queryTrash = false, DateTime? from = null, DateTime? to = null, int recordsPerPage = 30, int page = 1, CancellationToken cancellationToken = default)
		{
			if (recordsPerPage < 1 || recordsPerPage > 300)
			{
				throw new ArgumentOutOfRangeException(nameof(recordsPerPage), "Records per page must be between 1 and 300");
			}

			return _client
				.GetAsync($"users/{userId}/recordings")
				.WithArgument("trash", queryTrash.ToString().ToLower())
				.WithArgument("from", from?.ToString("yyyy-mm-dd"))
				.WithArgument("to", to?.ToString("yyyy-mm-dd"))
				.WithArgument("page_size", recordsPerPage)
				.WithArgument("page", page)
				.WithCancellationToken(cancellationToken)
				.AsPaginatedResponse<Recording>("meetings", null);
		}

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
		public Task<PaginatedResponseWithToken<Recording>> GetRecordingsForUserAsync(string userId, bool queryTrash = false, DateTime? from = null, DateTime? to = null, int recordsPerPage = 30, string pagingToken = null, CancellationToken cancellationToken = default)
		{
			if (recordsPerPage < 1 || recordsPerPage > 300)
			{
				throw new ArgumentOutOfRangeException(nameof(recordsPerPage), "Records per page must be between 1 and 300");
			}

			return _client
				.GetAsync($"users/{userId}/recordings")
				.WithArgument("trash", queryTrash.ToString().ToLower())
				.WithArgument("from", from?.ToString("yyyy-mm-dd"))
				.WithArgument("to", to?.ToString("yyyy-mm-dd"))
				.WithArgument("page_size", recordsPerPage)
				.WithArgument("next_page_token", pagingToken)
				.WithCancellationToken(cancellationToken)
				.AsPaginatedResponseWithToken<Recording>("meetings", null);
		}

		/// <summary>
		/// Retrieve all cloud recordings for a meeting.
		/// </summary>
		/// <param name="meetingId">The meeting Id or UUID.</param>
		/// <param name="recordsPerPage">The number of records returned within a single API call.</param>
		/// <param name="page">The current page number of returned records.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>
		/// An array of <see cref="Recording">recordings</see>.
		/// </returns>
		[Obsolete("Zoom is in the process of deprecating the \"page number\" and \"page count\" fields.")]
		public Task<PaginatedResponse<Recording>> GetRecordingsAsync(string meetingId, int recordsPerPage = 30, int page = 1, CancellationToken cancellationToken = default)
		{
			if (recordsPerPage < 1 || recordsPerPage > 300)
			{
				throw new ArgumentOutOfRangeException(nameof(recordsPerPage), "Records per page must be between 1 and 300");
			}

			return _client
				.GetAsync($"meetings/{meetingId}/recordings")
				.WithArgument("page_size", recordsPerPage)
				.WithArgument("page", page)
				.WithCancellationToken(cancellationToken)
				.AsPaginatedResponse<Recording>("meetings", null);
		}

		/// <summary>
		/// Retrieve all cloud recordings for a user.
		/// </summary>
		/// <param name="meetingId">The meeting Id or UUID.</param>
		/// <param name="recordsPerPage">The number of records returned within a single API call.</param>
		/// <param name="pagingToken">The paging token.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>
		/// An array of <see cref="Recording">recordings</see>.
		/// </returns>
		public Task<PaginatedResponseWithToken<Recording>> GetRecordingsAsync(string meetingId, int recordsPerPage = 30, string pagingToken = null, CancellationToken cancellationToken = default)
		{
			if (recordsPerPage < 1 || recordsPerPage > 300)
			{
				throw new ArgumentOutOfRangeException(nameof(recordsPerPage), "Records per page must be between 1 and 300");
			}

			return _client
				.GetAsync($"meetings/{meetingId}/recordings")
				.WithArgument("page_size", recordsPerPage)
				.WithArgument("next_page_token", pagingToken)
				.WithCancellationToken(cancellationToken)
				.AsPaginatedResponseWithToken<Recording>("meetings", null);
		}

		/// <summary>
		/// Move recordings for a meeting to trash.
		/// </summary>
		/// <param name="meetingId">The meeting Id or UUID.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>
		/// The async task.
		/// </returns>
		public Task MoveRecordingsToTrashAsync(string meetingId, CancellationToken cancellationToken = default)
		{
			return _client
				.DeleteAsync($"meetings/{meetingId}/recordings")
				.WithArgument("action", "trash")
				.WithCancellationToken(cancellationToken)
				.AsMessage();
		}

		/// <summary>
		/// Permanently delete recordings for a meeting.
		/// </summary>
		/// <param name="meetingId">The meeting Id or UUID.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>
		/// The async task.
		/// </returns>
		public Task DeleteRecordingsAsync(string meetingId, CancellationToken cancellationToken = default)
		{
			return _client
				.DeleteAsync($"meetings/{meetingId}/recordings")
				.WithArgument("action", "delete")
				.WithCancellationToken(cancellationToken)
				.AsMessage();
		}

		/// <summary>
		/// Move a specific recording file for a meeting to trash.
		/// </summary>
		/// <param name="meetingId">The meeting Id or UUID.</param>
		/// <param name="recordingId">The recording id.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>
		/// The async task.
		/// </returns>
		public Task MoveRecordingToTrashAsync(string meetingId, string recordingId, CancellationToken cancellationToken = default)
		{
			return _client
				.DeleteAsync($"meetings/{meetingId}/recordings/{recordingId}")
				.WithArgument("action", "trash")
				.WithCancellationToken(cancellationToken)
				.AsMessage();
		}

		/// <summary>
		/// Permanently delete a specific recording file for a meeting.
		/// </summary>
		/// <param name="meetingId">The meeting Id or UUID.</param>
		/// <param name="recordingId">The recording id.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>
		/// The async task.
		/// </returns>
		public Task DeleteRecordingAsync(string meetingId, string recordingId, CancellationToken cancellationToken = default)
		{
			return _client
				.DeleteAsync($"meetings/{meetingId}/recordings/{recordingId}")
				.WithArgument("action", "delete")
				.WithCancellationToken(cancellationToken)
				.AsMessage();
		}

		/// <summary>
		/// Recover all deleted recordings of a specific meeting from trash.
		/// </summary>
		/// <param name="meetingId">The meeting Id or UUID.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>
		/// The async task.
		/// </returns>
		/// <remarks>Zoom allows recordings to be recovered from trash for up to 30 days from deletion date.</remarks>
		public Task RecoverRecordingsAsync(string meetingId, CancellationToken cancellationToken = default)
		{
			return _client
				.PutAsync($"meetings/{meetingId}/recordings/status")
				.WithArgument("action", "recover")
				.WithCancellationToken(cancellationToken)
				.AsMessage();
		}

		/// <summary>
		/// Recover a specific recording file of a meeting.
		/// </summary>
		/// <param name="meetingId">The meeting Id or UUID.</param>
		/// <param name="recordingId">The recording id.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>
		/// The async task.
		/// </returns>
		/// <remarks>Zoom allows recordings to be recovered from trash for up to 30 days from deletion date.</remarks>
		public Task RecoverRecordingAsync(string meetingId, string recordingId, CancellationToken cancellationToken = default)
		{
			return _client
				.PutAsync($"meetings/{meetingId}/recordings/{recordingId}/status")
				.WithArgument("action", "recover")
				.WithCancellationToken(cancellationToken)
				.AsMessage();
		}

		/// <summary>
		/// Retrieve settings applied to a meeting's cloud recording.
		/// </summary>
		/// <param name="meetingId">The meeting Id or UUID.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>
		/// The <see cref="RecordingSettings" />.
		/// </returns>
		public Task<RecordingSettings> GetRecordingSettingsAsync(string meetingId, CancellationToken cancellationToken = default)
		{
			return _client
				.GetAsync($"meetings/{meetingId}/recordings/settings")
				.WithCancellationToken(cancellationToken)
				.AsObject<RecordingSettings>(null, null);
		}

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
		public Task<PaginatedResponse<Registrant>> GetRecordingRegistrantsAsync(string meetingId, int recordsPerPage = 30, int page = 1, CancellationToken cancellationToken = default)
		{
			if (recordsPerPage < 1 || recordsPerPage > 300)
			{
				throw new ArgumentOutOfRangeException(nameof(recordsPerPage), "Records per page must be between 1 and 300");
			}

			return _client
				.GetAsync($"meetings/{meetingId}/recordings/registrants")
				.WithArgument("page_size", recordsPerPage)
				.WithArgument("page", page)
				.WithCancellationToken(cancellationToken)
				.AsPaginatedResponse<Registrant>("registrants", null);
		}

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
		public Task<PaginatedResponseWithToken<Registrant>> GetRecordingRegistrantsAsync(string meetingId, int recordsPerPage = 30, string pagingToken = null, CancellationToken cancellationToken = default)
		{
			if (recordsPerPage < 1 || recordsPerPage > 300)
			{
				throw new ArgumentOutOfRangeException(nameof(recordsPerPage), "Records per page must be between 1 and 300");
			}

			return _client
				.GetAsync($"meetings/{meetingId}/recordings/registrants")
				.WithArgument("page_size", recordsPerPage)
				.WithArgument("next_page_token", pagingToken)
				.WithCancellationToken(cancellationToken)
				.AsPaginatedResponseWithToken<Registrant>("registrants", null);
		}
	}
}
