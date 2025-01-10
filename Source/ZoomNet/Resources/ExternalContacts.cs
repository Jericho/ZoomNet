using Pathoschild.Http.Client;
using System;
using System.Threading;
using System.Threading.Tasks;
using ZoomNet;
using ZoomNet.Models;

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
		public Task<PaginatedResponseWithToken<ExternalContactDetails>> ListExternalContactsAsync(int pageSize = 30, string nextPageToken = null, CancellationToken cancellationToken = default)
		{
			if (pageSize < 1 || pageSize > 300)
			{
				throw new ArgumentOutOfRangeException(nameof(pageSize), "Page size must be between 1 and 300");
			}

			return _client
				.GetAsync("phone/external_contacts")
				.WithArgument("page_size", pageSize)
				.WithArgument("next_page_token", nextPageToken)
				.WithCancellationToken(cancellationToken)
				.AsPaginatedResponseWithToken<ExternalContactDetails>("external_contacts");
		}

		/// <inheritdoc/>
		public Task<ExternalContactDetails> GetExternalContactDetailsAsync(string externalContactId, CancellationToken cancellationToken = default)
		{
			if (string.IsNullOrEmpty(externalContactId))
			{
				throw new ArgumentException(nameof(externalContactId), "External contact id is not set.");
			}

			return _client
				.GetAsync($"phone/external_contacts/{externalContactId}")
				.WithCancellationToken(cancellationToken)
				.AsObject<ExternalContactDetails>();
		}

		/// <inheritdoc/>
		public Task<ExternalContact> AddExternalContactAsync(ExternalContactDetails externalContact, CancellationToken cancellationToken = default)
		{
			return _client
				.PostAsync("phone/external_contacts")
				.WithJsonBody(externalContact)
				.WithCancellationToken(cancellationToken)
				.AsObject<ExternalContact>();
		}

		/// <inheritdoc/>
		public Task DeleteExternalContactAsync(string externalContactId, CancellationToken cancellationToken = default)
		{
			if (string.IsNullOrEmpty(externalContactId))
			{
				throw new ArgumentException(nameof(externalContactId), "External contact id is not set.");
			}

			return _client
				.DeleteAsync($"phone/external_contacts/{externalContactId}")
				.WithCancellationToken(cancellationToken)
				.AsMessage();
		}

		/// <inheritdoc/>
		public Task UpdateExternalContactAsync(
			ExternalContactDetails externalContact, CancellationToken cancellationToken = default)
		{
			if (string.IsNullOrEmpty(externalContact.ExternalContactId))
			{
				throw new ArgumentException(nameof(externalContact.ExternalContactId), "External contact id is not set.");
			}

			return _client
				.PatchAsync($"phone/external_contacts/{externalContact.ExternalContactId}")
				.WithJsonBody(externalContact)
				.WithCancellationToken(cancellationToken)
				.AsMessage();
		}
	}
}
