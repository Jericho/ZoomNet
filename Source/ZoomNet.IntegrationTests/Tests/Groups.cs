using System.IO;
using System.Threading;
using System.Threading.Tasks;
using ZoomNet.Models;

namespace ZoomNet.IntegrationTests.Tests
{
	public class Groups : IIntegrationTest
	{
		public async Task RunAsync(User myUser, string[] myPermissions, IZoomClient client, TextWriter log, CancellationToken cancellationToken)
		{
			if (cancellationToken.IsCancellationRequested) return;

			await log.WriteLineAsync("\n***** GROUPS *****\n").ConfigureAwait(false);

			// GET ALL THE GROUPS
			var groups = await client.Groups.GetAllAsync(cancellationToken).ConfigureAwait(false);
			await log.WriteLineAsync($"There are {groups.Length} groups").ConfigureAwait(false);

		}
	}
}
