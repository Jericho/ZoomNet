using Pathoschild.Http.Client;
using System.Threading;
using System.Threading.Tasks;
using ZoomNet.Models;
using ZoomNet.Utilities;

namespace ZoomNet.Resources
{
	/// <inheritdoc />
	public class Contacts : IContacts
	{
		private readonly IClient _client;

		/// <summary>
		/// Initializes a new instance of the <see cref="Contacts" /> class.
		/// </summary>
		/// <param name="client">The HTTP client.</param>
		internal Contacts(IClient client)
		{
			_client = client;
		}

		/// <inheritdoc />
		public Task<PaginatedResponseWithToken<Contact>> GetAllAsync(ContactType type = ContactType.Internal, int recordsPerPage = 30, string pagingToken = null, CancellationToken cancellationToken = default)
		{
			Utils.ValidateRecordPerPage(recordsPerPage, max: 50);

			return _client
				.GetAsync("chat/users/me/contacts")
				.WithArgument("type", type.ToEnumString())
				.WithArgument("page_size", recordsPerPage)
				.WithArgument("next_page_token", pagingToken)
				.WithCancellationToken(cancellationToken)
				.AsPaginatedResponseWithToken<Contact>("contacts");
		}

		/// <inheritdoc />
		public Task<PaginatedResponseWithToken<Contact>> SearchAsync(string keyword, bool queryPresenceStatus = true, int recordsPerPage = 1, string pagingToken = null, CancellationToken cancellationToken = default)
		{
			Utils.ValidateRecordPerPage(recordsPerPage, max: 25);

			return _client
				.GetAsync("contacts")
				.WithArgument("search_key", keyword)
				.WithArgument("query_presence_status", queryPresenceStatus)
				.WithArgument("page_size", recordsPerPage)
				.WithArgument("next_page_token", pagingToken)
				.WithCancellationToken(cancellationToken)
				.AsPaginatedResponseWithToken<Contact>("contacts");
		}

		/// <inheritdoc />
		public Task<Contact> GetAsync(string contactId, bool queryPresenceStatus = true, CancellationToken cancellationToken = default)
		{
			return _client
				.GetAsync($"chat/users/me/contacts/{contactId}")
				.WithArgument("contactid", contactId)
				.WithArgument("query_presence_status", queryPresenceStatus)
				.WithCancellationToken(cancellationToken)
				.AsObject<Contact>();
		}
	}
}
