using System.IO;
using System.Threading;
using System.Threading.Tasks;
using ZoomNet.Models;

namespace ZoomNet.IntegrationTests.Tests
{
	public class Contacts : IIntegrationTest
	{
		public async Task RunAsync(User myUser, string[] myPermissions, IZoomClient client, TextWriter log, CancellationToken cancellationToken)
		{
			await log.WriteLineAsync("\n***** CONTACTS *****\n").ConfigureAwait(false);

			var keyword = "zzz";
			var paginatedSearchedContacts = await client.Contacts.SearchAsync(keyword, true, 1, null, cancellationToken).ConfigureAwait(false);
			await log.WriteLineAsync($"Found {paginatedSearchedContacts.TotalRecords} contacts containing the keyword: {keyword}").ConfigureAwait(false);

			/*
			 * ===============================================================================
			 * The `GetAsync` and `GetAllAsync` methods cannot be used by a S2S OAuth app.
			 * ===============================================================================

			var contact = await client.Contacts.GetAsync(myUser.Id, true, cancellationToken).ConfigureAwait(false);
			await log.WriteLineAsync($"{contact.EmailAddress} is {contact.PresenceStatus}").ConfigureAwait(false);

			var paginatedInternalContacts = await client.Contacts.GetAllAsync(ContactType.Internal, 50, null, cancellationToken).ConfigureAwait(false);
			await log.WriteLineAsync($"There are {paginatedInternalContacts.TotalRecords} internal contacts for user {myUser.Id}").ConfigureAwait(false);

			var paginatedExternalContacts = await client.Contacts.GetAllAsync(ContactType.External, 50, null, cancellationToken).ConfigureAwait(false);
			await log.WriteLineAsync($"There are {paginatedExternalContacts.TotalRecords} external contacts for user {myUser.Id}").ConfigureAwait(false);
			*/
		}
	}
}
