using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ZoomNet.IntegrationTests.Tests
{
	public class Devices : IIntegrationTest
	{
		public async Task RunAsync(string userId, IZoomClient client, TextWriter log, CancellationToken cancellationToken)
		{
			if (cancellationToken.IsCancellationRequested) return;

			await log.WriteLineAsync("\n***** DEVICES *****\n").ConfigureAwait(false);

			// GET ALL THE DEVICES
			var paginatedDevices = await client.Devices.GetAllAsync(100, null, cancellationToken).ConfigureAwait(false);
			await log.WriteLineAsync($"There are {paginatedDevices.Records.Length} devices").ConfigureAwait(false);

			// CLEANUP PREVIOUS INTEGRATION TESTS THAT MIGHT HAVE BEEN INTERRUPTED BEFORE THEY HAD TIME TO CLEANUP AFTER THEMSELVES
			var cleanUpTasks = paginatedDevices.Records
				.Where(m => m.Name.StartsWith("ZoomNet Integration Testing:"))
				.Select(async oldDevice =>
				{
					//await client.Devices.DeleteAsync(oldDevice.Id, cancellationToken).ConfigureAwait(false);
					await log.WriteLineAsync($"Device {oldDevice.Id} deleted").ConfigureAwait(false);
					await Task.Delay(250, cancellationToken).ConfigureAwait(false);    // Brief pause to ensure Zoom has time to catch up
				});
			await Task.WhenAll(cleanUpTasks).ConfigureAwait(false);

			//// GET MY USER
			//var myUser = await client.Users.GetCurrentAsync(cancellationToken).ConfigureAwait(false);
			//await log.WriteLineAsync($"My user retrieved. My email address is {myUser.Email}").ConfigureAwait(false);

			//// GET MY ASSISTANTS
			//var myAssistants = await client.Users.GetAssistantsAsync(myUser.Id, cancellationToken).ConfigureAwait(false);
			//await log.WriteLineAsync($"My user has {myAssistants.Length} assistants").ConfigureAwait(false);

			//// GET MY SCHEDULERS
			//var mySchedulers = await client.Users.GetSchedulersAsync(myUser.Id, cancellationToken).ConfigureAwait(false);
			//await log.WriteLineAsync($"My user has {mySchedulers.Length} schedulers").ConfigureAwait(false);

			//// GET MY SETTINGS
			//var mySettings = await client.Users.GetSettingsAsync(myUser.Id, cancellationToken).ConfigureAwait(false);
			//var myMeetingAuthSettings = await client.Users.GetMeetingAuthenticationSettingsAsync(myUser.Id, cancellationToken).ConfigureAwait(false);
			//var myRecordingAuthSettings = await client.Users.GetRecordingAuthenticationSettingsAsync(myUser.Id, cancellationToken).ConfigureAwait(false);
			//await log.WriteLineAsync("My settings retrieved").ConfigureAwait(false);

			//// GET MY PERMISSIONS
			//var myPermissions = await client.Users.GetPermissionsAsync(myUser.Id, cancellationToken).ConfigureAwait(false);
			//await log.WriteLineAsync($"My permissions retrieved: I have been granted {myPermissions.Length} permissions").ConfigureAwait(false);
		}
	}
}
