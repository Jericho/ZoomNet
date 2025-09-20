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

			var paginatedUsers = await client.ContactCenter.SearchUsersAsync("*", null, null, 10, null, cancellationToken).ConfigureAwait(false);
			await log.WriteLineAsync($"Found {paginatedUsers.TotalRecords} user profiles").ConfigureAwait(false);

			var paginatedRoles = await client.ContactCenter.GetAllRolesAsync(30, null, cancellationToken).ConfigureAwait(false);
			await log.WriteLineAsync($"There are {paginatedRoles.TotalRecords} roles in Contact Center").ConfigureAwait(false);
		}
	}
}
