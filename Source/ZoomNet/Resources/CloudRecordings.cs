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
	/// <inheritdoc/>
	public class CloudRecordings : ICloudRecordings
	{
		private readonly IClient _client;

		/// <summary>
		/// Initializes a new instance of the <see cref="CloudRecordings" /> class.
		/// </summary>
		/// <param name="client">The HTTP client.</param>
		internal CloudRecordings(IClient client)
		{
			_client = client;
		}

		/// <inheritdoc/>
		[Obsolete("Zoom is in the process of deprecating the \"page number\" and \"page count\" fields.")]
		public Task<PaginatedResponseWithTokenAndDateRange<Recording>> GetRecordingsForUserAsync(string userId, bool queryTrash = false, DateOnly? from = null, DateOnly? to = null, int recordsPerPage = 30, int page = 1, CancellationToken cancellationToken = default)
		{
			Utils.ValidateRecordPerPage(recordsPerPage);

			return _client
				.GetAsync($"users/{userId}/recordings")
				.WithArgument("trash", queryTrash.ToString().ToLowerInvariant())
				.WithArgument("from", from.ToZoomFormat())
				.WithArgument("to", to.ToZoomFormat())
				.WithArgument("page_size", recordsPerPage)
				.WithArgument("page_number", page)
				.WithCancellationToken(cancellationToken)
				.AsPaginatedResponseWithTokenAndDateRange<Recording>("meetings");
		}

		/// <inheritdoc/>
		public Task<PaginatedResponseWithTokenAndDateRange<Recording>> GetRecordingsForUserAsync(string userId, bool queryTrash = false, DateOnly? from = null, DateOnly? to = null, int recordsPerPage = 30, string pagingToken = null, CancellationToken cancellationToken = default)
		{
			Utils.ValidateRecordPerPage(recordsPerPage);

			return _client
				.GetAsync($"users/{userId}/recordings")
				.WithArgument("trash", queryTrash.ToString().ToLowerInvariant())
				.WithArgument("from", from.ToZoomFormat())
				.WithArgument("to", to.ToZoomFormat())
				.WithArgument("page_size", recordsPerPage)
				.WithArgument("next_page_token", pagingToken)
				.WithCancellationToken(cancellationToken)
				.AsPaginatedResponseWithTokenAndDateRange<Recording>("meetings");
		}

		/// <inheritdoc/>
		public Task<Recording> GetRecordingInformationAsync(string meetingId, int ttl = 60 * 5, CancellationToken cancellationToken = default)
		{
			return _client
				.GetAsync($"meetings/{Utils.EncodeUUID(meetingId)}/recordings")
				.WithArgument("include_fields", "download_access_token")
				.WithArgument("ttl", ttl)
				.WithCancellationToken(cancellationToken)
				.AsObject<Recording>();
		}

		/// <inheritdoc/>
		public Task DeleteRecordingFilesAsync(string meetingId, bool deletePermanently = false, CancellationToken cancellationToken = default)
		{
			return _client
				.DeleteAsync($"meetings/{Utils.EncodeUUID(meetingId)}/recordings")
				.WithArgument("action", deletePermanently ? "delete" : "trash")
				.WithCancellationToken(cancellationToken)
				.AsMessage();
		}

		/// <inheritdoc/>
		public Task DeleteRecordingFileAsync(string meetingId, string recordingFileId, bool deletePermanently = false, CancellationToken cancellationToken = default)
		{
			return _client
				.DeleteAsync($"meetings/{Utils.EncodeUUID(meetingId)}/recordings/{recordingFileId}")
				.WithArgument("action", deletePermanently ? "delete" : "trash")
				.WithCancellationToken(cancellationToken)
				.AsMessage();
		}

		/// <inheritdoc/>
		public Task RecoverRecordingFilesAsync(string meetingId, CancellationToken cancellationToken = default)
		{
			return _client
				.PutAsync($"meetings/{Utils.EncodeUUID(meetingId)}/recordings/status")
				.WithArgument("action", "recover")
				.WithCancellationToken(cancellationToken)
				.AsMessage();
		}

		/// <inheritdoc/>
		public Task RecoverRecordingFileAsync(string meetingId, string recordingFileId, CancellationToken cancellationToken = default)
		{
			return _client
				.PutAsync($"meetings/{Utils.EncodeUUID(meetingId)}/recordings/{recordingFileId}/status")
				.WithArgument("action", "recover")
				.WithCancellationToken(cancellationToken)
				.AsMessage();
		}

		/// <inheritdoc/>
		public Task<RecordingSettings> GetRecordingSettingsAsync(string meetingId, CancellationToken cancellationToken = default)
		{
			return _client
				.GetAsync($"meetings/{Utils.EncodeUUID(meetingId)}/recordings/settings")
				.WithCancellationToken(cancellationToken)
				.AsObject<RecordingSettings>();
		}

		/// <inheritdoc/>
		[Obsolete("Zoom is in the process of deprecating the \"page number\" and \"page count\" fields.")]
		public Task<PaginatedResponse<Registrant>> GetRecordingRegistrantsAsync(string meetingId, int recordsPerPage = 30, int page = 1, CancellationToken cancellationToken = default)
		{
			Utils.ValidateRecordPerPage(recordsPerPage);

			return _client
				.GetAsync($"meetings/{Utils.EncodeUUID(meetingId)}/recordings/registrants")
				.WithArgument("page_size", recordsPerPage)
				.WithArgument("page_number", page)
				.WithCancellationToken(cancellationToken)
				.AsPaginatedResponse<Registrant>("registrants");
		}

		/// <inheritdoc/>
		public Task<PaginatedResponseWithToken<Registrant>> GetRecordingRegistrantsAsync(string meetingId, int recordsPerPage = 30, string pagingToken = null, CancellationToken cancellationToken = default)
		{
			Utils.ValidateRecordPerPage(recordsPerPage);

			return _client
				.GetAsync($"meetings/{Utils.EncodeUUID(meetingId)}/recordings/registrants")
				.WithArgument("page_size", recordsPerPage)
				.WithArgument("next_page_token", pagingToken)
				.WithCancellationToken(cancellationToken)
				.AsPaginatedResponseWithToken<Registrant>("registrants");
		}

		/// <inheritdoc/>
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

		/// <inheritdoc/>
		public Task ApproveRegistrantAsync(long meetingId, string registrantId, CancellationToken cancellationToken = default)
		{
			return ApproveRegistrantsAsync(meetingId, new[] { registrantId }, cancellationToken);
		}

		/// <inheritdoc/>
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

		/// <inheritdoc/>
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
