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
			if (cancellationToken.IsCancellationRequested) return;

			await log.WriteLineAsync("\n***** CONTACTS *****\n").ConfigureAwait(false);

			// GET THE CONTACTS FOR THIS USER
			var paginatedInternalContacts = await client.Contacts.GetAllAsync(ContactType.Internal, 50, null, cancellationToken).ConfigureAwait(false);
			await log.WriteLineAsync($"There are {paginatedInternalContacts.TotalRecords} internal contacts for user {myUser.Id}").ConfigureAwait(false);

			var paginatedExternalContacts = await client.Contacts.GetAllAsync(ContactType.External, 50, null, cancellationToken).ConfigureAwait(false);
			await log.WriteLineAsync($"There are {paginatedExternalContacts.TotalRecords} external contacts for user {myUser.Id}").ConfigureAwait(false);

			var paginatedSearchedContacts = await client.Contacts.SearchAsync("zzz", true, 1, null, cancellationToken).ConfigureAwait(false);
			await log.WriteLineAsync($"Found {paginatedSearchedContacts.TotalRecords} contacts").ConfigureAwait(false);

			var contact = await client.Contacts.GetAsync(myUser.Id, true, cancellationToken).ConfigureAwait(false);
			await log.WriteLineAsync($"{contact.EmailAddress} is {contact.PresenceStatus}").ConfigureAwait(false);
		}
	}
}
