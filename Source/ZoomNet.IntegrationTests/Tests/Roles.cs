using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ZoomNet.Models;

namespace ZoomNet.IntegrationTests.Tests
{
	public class Roles : IIntegrationTest
	{
		public async Task RunAsync(User myUser, string[] myPermissions, IZoomClient client, TextWriter log, CancellationToken cancellationToken)
		{
			if (cancellationToken.IsCancellationRequested) return;

			await log.WriteLineAsync("\n***** ROLES *****\n").ConfigureAwait(false);

			// GET ALL THE ROLES
			var paginatedRoles = await client.Roles.GetAllAsync(300, null, cancellationToken).ConfigureAwait(false);
			await log.WriteLineAsync($"There are {paginatedRoles.Records.Length} roles").ConfigureAwait(false);

			// CLEANUP PREVIOUS INTEGRATION TESTS THAT MIGHT HAVE BEEN INTERRUPTED BEFORE THEY HAD TIME TO CLEANUP AFTER THEMSELVES
			var cleanUpTasks = paginatedRoles.Records
				.Where(m => m.Name.StartsWith("ZoomNet Integration Testing", StringComparison.OrdinalIgnoreCase))
				.Select(async oldRole =>
				{
					//await client.Roles.DeleteAsync(oldRole.Id, cancellationToken).ConfigureAwait(false);
					await log.WriteLineAsync($"Role {oldRole.Id} deleted").ConfigureAwait(false);
					await Task.Delay(250, cancellationToken).ConfigureAwait(false);    // Brief pause to ensure Zoom has time to catch up
				});
			await Task.WhenAll(cleanUpTasks).ConfigureAwait(false);
		}
	}
}
