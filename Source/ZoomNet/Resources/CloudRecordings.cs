using Pathoschild.Http.Client;
using Pathoschild.Http.Client.Extensibility;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text.Json.Nodes;
using System.Threading;
using System.Threading.Tasks;
using ZoomNet.Models;
using ZoomNet.Utilities;

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

		/// <inheritdoc/>
		[Obsolete("Zoom is in the process of deprecating the \"page number\" and \"page count\" fields.")]
		public Task<PaginatedResponseWithTokenAndDateRange<Recording>> GetRecordingsForUserAsync(string userId, bool queryTrash = false, DateTime? from = null, DateTime? to = null, int recordsPerPage = 30, int page = 1, CancellationToken cancellationToken = default)
		{
			if (recordsPerPage < 1 || recordsPerPage > 300)
			{
				throw new ArgumentOutOfRangeException(nameof(recordsPerPage), "Records per page must be between 1 and 300");
			}

			return _client
				.GetAsync($"users/{userId}/recordings")
				.WithArgument("trash", queryTrash.ToString().ToLowerInvariant())
				.WithArgument("from", from?.ToZoomFormat(dateOnly: true))
				.WithArgument("to", to?.ToZoomFormat(dateOnly: true))
				.WithArgument("page_size", recordsPerPage)
				.WithArgument("page_number", page)
				.WithCancellationToken(cancellationToken)
				.AsPaginatedResponseWithTokenAndDateRange<Recording>("meetings");
		}

		/// <inheritdoc/>
		public Task<PaginatedResponseWithTokenAndDateRange<Recording>> GetRecordingsForUserAsync(string userId, bool queryTrash = false, DateTime? from = null, DateTime? to = null, int recordsPerPage = 30, string pagingToken = null, CancellationToken cancellationToken = default)
		{
			if (recordsPerPage < 1 || recordsPerPage > 300)
			{
				throw new ArgumentOutOfRangeException(nameof(recordsPerPage), "Records per page must be between 1 and 300");
			}

			return _client
				.GetAsync($"users/{userId}/recordings")
				.WithArgument("trash", queryTrash.ToString().ToLowerInvariant())
				.WithArgument("from", from?.ToZoomFormat(dateOnly: true))
				.WithArgument("to", to?.ToZoomFormat(dateOnly: true))
				.WithArgument("page_size", recordsPerPage)
				.WithArgument("next_page_token", pagingToken)
				.WithCancellationToken(cancellationToken)
				.AsPaginatedResponseWithTokenAndDateRange<Recording>("meetings");
		}

		/// <inheritdoc/>
		public Task<Recording> GetRecordingInformationAsync(string meetingId, int ttl = 60 * 5, CancellationToken cancellationToken = default)
		{
			return _client
				.GetAsync($"meetings/{meetingId}/recordings")
				.WithArgument("include_fields", "download_access_token")
				.WithArgument("ttl", ttl)
				.WithCancellationToken(cancellationToken)
				.AsObject<Recording>();
		}

		/// <inheritdoc/>
		public Task DeleteRecordingFilesAsync(string meetingId, bool deletePermanently = false, CancellationToken cancellationToken = default)
		{
			return _client
				.DeleteAsync($"meetings/{meetingId}/recordings")
				.WithArgument("action", deletePermanently ? "delete" : "trash")
				.WithCancellationToken(cancellationToken)
				.AsMessage();
		}

		/// <inheritdoc/>
		public Task DeleteRecordingFileAsync(string meetingId, string recordingFileId, bool deletePermanently = false, CancellationToken cancellationToken = default)
		{
			return _client
				.DeleteAsync($"meetings/{meetingId}/recordings/{recordingFileId}")
				.WithArgument("action", deletePermanently ? "delete" : "trash")
				.WithCancellationToken(cancellationToken)
				.AsMessage();
		}

		/// <inheritdoc/>
		public Task RecoverRecordingFilesAsync(string meetingId, CancellationToken cancellationToken = default)
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
		/// <param name="recordingFileId">The recording file id.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>
		/// The async task.
		/// </returns>
		public Task RecoverRecordingFileAsync(string meetingId, string recordingFileId, CancellationToken cancellationToken = default)
		{
			return _client
				.PutAsync($"meetings/{meetingId}/recordings/{recordingFileId}/status")
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
				.AsObject<RecordingSettings>();
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
				.WithArgument("page_number", page)
				.WithCancellationToken(cancellationToken)
				.AsPaginatedResponse<Registrant>("registrants");
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
				.AsPaginatedResponseWithToken<Registrant>("registrants");
		}

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
		public Task<RecordingRegistration> AddRegistrantAsync(long meetingId, string email, string firstName, string lastName, string address, string city, string country, string zip, string state, string phone, string industry, string organization, string jobTitle, string purchasingTimeFrame, string roleInPurchaseProcess, string numberOfEmployees, string comments, CancellationToken cancellationToken = default)
		{
			var data = new JsonObject
			{
				{ "email", email },
				{ "first_name", firstName },
				{ "last_name", lastName },
				{ "address", address },
				{ "city", city },
				{ "country", country },
				{ "zip", zip },
				{ "state", state },
				{ "phone", phone },
				{ "industry", industry },
				{ "org", organization },
				{ "job_title", jobTitle },
				{ "purchasing_time_frame", purchasingTimeFrame },
				{ "role_in_purchasing_process", roleInPurchaseProcess },
				{ "no_of_employees", numberOfEmployees },
				{ "comments", comments }
			};

			return _client
				.PostAsync($"meetings/{meetingId}/recordings/registrants")
				.WithJsonBody(data)
				.WithCancellationToken(cancellationToken)
				.AsObject<RecordingRegistration>();
		}

		/// <summary>
		/// Approve a registration for a meeting.
		/// </summary>
		/// <param name="meetingId">The meeting ID.</param>
		/// <param name="registrantId">The registrant ID.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>
		/// The async task.
		/// </returns>
		public Task ApproveRegistrantAsync(long meetingId, string registrantId, CancellationToken cancellationToken = default)
		{
			return ApproveRegistrantsAsync(meetingId, new[] { registrantId }, cancellationToken);
		}

		/// <summary>
		/// Approve multiple registrations for a meeting.
		/// </summary>
		/// <param name="meetingId">The meeting ID.</param>
		/// <param name="registrantIds">ID for each registrant to be approved.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>
		/// The async task.
		/// </returns>
		public Task ApproveRegistrantsAsync(long meetingId, IEnumerable<string> registrantIds, CancellationToken cancellationToken = default)
		{
			return UpdateRegistrantsStatusAsync(meetingId, registrantIds, "approve", cancellationToken);
		}

		/// <summary>
		/// Reject a registration for a meeting.
		/// </summary>
		/// <param name="meetingId">The meeting ID.</param>
		/// <param name="registrantId">The registrant ID.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>
		/// The async task.
		/// </returns>
		public Task RejectRegistrantAsync(long meetingId, string registrantId, CancellationToken cancellationToken = default)
		{
			return RejectRegistrantsAsync(meetingId, new[] { registrantId }, cancellationToken);
		}

		/// <summary>
		/// Reject multiple registrations for a meeting.
		/// </summary>
		/// <param name="meetingId">The meeting ID.</param>
		/// <param name="registrantIds">ID for each registrant to be rejected.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>
		/// The async task.
		/// </returns>
		public Task RejectRegistrantsAsync(long meetingId, IEnumerable<string> registrantIds, CancellationToken cancellationToken = default)
		{
			return UpdateRegistrantsStatusAsync(meetingId, registrantIds, "deny", cancellationToken);
		}

		/// <inheritdoc/>
		public Task<Stream> DownloadFileAsync(string downloadUrl, string accessToken = null, CancellationToken cancellationToken = default)
		{
			// Prepare the request
			var request = _client
			   .GetAsync(downloadUrl)
			   .WithOptions(completeWhen: HttpCompletionOption.ResponseHeadersRead)
			   .WithCancellationToken(cancellationToken);

			// Use an alternate token if provided. Otherwise, the oAuth token for the current session will be used.
			if (!string.IsNullOrEmpty(accessToken))
			{
				request = request.WithBearerAuthentication(accessToken);
			}

			// Remove our custom error handler because it reads the content of the response to check for error messages returned from the Zoom API.
			// This is problematic because we want the content of the response to be streamed.
			request = request.WithoutFilter<ZoomErrorHandler>();

			// We need to add the default error filter to throw an exception if the request fails.
			// The error handler is safe to use with streaming responses because it does not read the content to determine if an error occured.
			request = request.WithFilter(new DefaultErrorFilter());

			// Dispatch the request
			return request.AsStream();
		}

		private Task UpdateRegistrantsStatusAsync(long meetingId, IEnumerable<string> registrantIds, string status, CancellationToken cancellationToken = default)
		{
			var data = new JsonObject
			{
				{ "action", status },
				{ "registrants", registrantIds.ToArray() }
			};

			return _client
				.PutAsync($"meetings/{meetingId}/recordings/registrants/status")
				.WithJsonBody(data)
				.WithCancellationToken(cancellationToken)
				.AsMessage();
		}
	}
}
