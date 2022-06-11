using System.IO;
using System.Threading;
using System.Threading.Tasks;
using ZoomNet.Models;

namespace ZoomNet.IntegrationTests.Tests
{
	public class ContactCenter : IIntegrationTest
	{
		public async Task RunAsync(User myUser, string[] myPermissions, IZoomClient client, TextWriter log, CancellationToken cancellationToken)
		{
			if (cancellationToken.IsCancellationRequested) return;

			await log.WriteLineAsync("\n***** CONTACT CENTER *****\n").ConfigureAwait(false);

			var paginatedUserProfiles = await client.ContactCenter.SearchUserProfilesAsync("zzz", 10, null, cancellationToken).ConfigureAwait(false);
			await log.WriteLineAsync($"Found {paginatedUserProfiles.TotalRecords} user profiles").ConfigureAwait(false);

			var user = await client.ContactCenter.CreateUserAsync("zzz@example.com", cancellationToken).ConfigureAwait(false);
			await log.WriteLineAsync($"User {user.Id} created").ConfigureAwait(false);

			paginatedUserProfiles = await client.ContactCenter.SearchUserProfilesAsync("zzz", 10, null, cancellationToken).ConfigureAwait(false);
			await log.WriteLineAsync($"Found {paginatedUserProfiles.TotalRecords} user profiles").ConfigureAwait(false);
		}
	}
}
