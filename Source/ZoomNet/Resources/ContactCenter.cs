using Pathoschild.Http.Client;
using System;
using System.Text.Json.Nodes;
using System.Threading;
using System.Threading.Tasks;
using ZoomNet.Models;

namespace ZoomNet.Resources
{
	/// <summary>
	/// Allows you to manage data on the Zoom Contact Center.
	/// </summary>
	/// <seealso cref="ZoomNet.Resources.IContactCenter" />
	/// <remarks>
	/// See <a href="https://marketplace.zoom.us/docs/api-reference/contact-center/methods/">Zoom documentation</a> for more information.
	/// </remarks>
	public class ContactCenter : IContactCenter
	{
		private readonly Pathoschild.Http.Client.IClient _client;

		/// <summary>
		/// Initializes a new instance of the <see cref="ContactCenter" /> class.
		/// </summary>
		/// <param name="client">The HTTP client.</param>
		internal ContactCenter(Pathoschild.Http.Client.IClient client)
		{
			_client = client;
		}

		/// <inheritdoc/>
		public Task<PaginatedResponseWithToken<Contact>> SearchUserProfilesAsync(string keyword, int recordsPerPage = 30, string pagingToken = null, CancellationToken cancellationToken = default)
		{
			if (recordsPerPage < 1 || recordsPerPage > 300)
			{
				throw new ArgumentOutOfRangeException(nameof(recordsPerPage), "Records per page must be between 1 and 300");
			}

			return _client
				.GetAsync($"contacts")
				.WithArgument("search_key", keyword)
				.WithArgument("page_size", recordsPerPage)
				.WithArgument("next_page_token", pagingToken)
				.WithCancellationToken(cancellationToken)
				.AsPaginatedResponseWithToken<Contact>("users");
		}

		/// <inheritdoc/>
		public Task<User> CreateUserAsync(string email, CancellationToken cancellationToken = default)
		{
			var data = new JsonObject
			{
				{ "user_email", email },
			};

			return _client
				.PostAsync("contact_center/users")
				.WithJsonBody(data)
				.WithCancellationToken(cancellationToken)
				.AsObject<User>();
		}
	}
}
