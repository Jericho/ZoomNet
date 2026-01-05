using Pathoschild.Http.Client;
using System;
using System.Threading;
using System.Threading.Tasks;
using ZoomNet;
using ZoomNet.Models;
using ZoomNet.Utilities;

namespace ZoomNet.Resources
{
	/// <inheritdoc/>
	public class ExternalContacts : IExternalContacts
	{
		private readonly IClient _client;

		/// <summary>
		/// Initializes a new instance of the <see cref="ExternalContacts" /> class.
		/// </summary>
		/// <param name="client">The HTTP client.</param>
		internal ExternalContacts(IClient client)
		{
			_client = client;
		}

		/// <inheritdoc/>
		public Task<PaginatedResponseWithToken<ExternalContactDetails>> GetAllAsync(int recordsPerPage = 30, string nextPageToken = null, CancellationToken cancellationToken = default)
		{
			Utils.ValidateRecordPerPage(recordsPerPage);

			return _client
				.GetAsync("phone/external_contacts")
				.WithArgument("page_size", recordsPerPage)
				.WithArgument("next_page_token", nextPageToken)
				.WithCancellationToken(cancellationToken)
				.AsPaginatedResponseWithToken<ExternalContactDetails>("external_contacts");
		}

		/// <inheritdoc/>
		public Task<ExternalContactDetails> GetDetailsAsync(string externalContactId, CancellationToken cancellationToken = default)
		{
			ArgumentNullException.ThrowIfNullOrEmpty(externalContactId);

			return _client
				.GetAsync($"phone/external_contacts/{externalContactId}")
				.WithCancellationToken(cancellationToken)
				.AsObject<ExternalContactDetails>();
		}

		/// <inheritdoc/>
		public Task<ExternalContact> AddAsync(ExternalContactDetails externalContact, CancellationToken cancellationToken = default)
		{
			return _client
				.PostAsync("phone/external_contacts")
				.WithJsonBody(externalContact)
				.WithCancellationToken(cancellationToken)
				.AsObject<ExternalContact>();
		}

		/// <inheritdoc/>
		public Task DeleteAsync(string externalContactId, CancellationToken cancellationToken = default)
		{
			ArgumentNullException.ThrowIfNullOrEmpty(externalContactId);

			return _client
				.DeleteAsync($"phone/external_contacts/{externalContactId}")
				.WithCancellationToken(cancellationToken)
				.AsMessage();
		}

		/// <inheritdoc/>
		public Task UpdateAsync(ExternalContactDetails externalContact, CancellationToken cancellationToken = default)
		{
			ArgumentNullException.ThrowIfNull(externalContact);
			ArgumentNullException.ThrowIfNullOrEmpty(externalContact.ExternalContactId, $"{nameof(externalContact)}.{nameof(externalContact.ExternalContactId)}");

			return _client
				.PatchAsync($"phone/external_contacts/{externalContact.ExternalContactId}")
				.WithJsonBody(externalContact)
				.WithCancellationToken(cancellationToken)
				.AsMessage();
		}
	}
}
