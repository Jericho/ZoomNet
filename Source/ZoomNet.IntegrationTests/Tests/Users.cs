using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ZoomNet.Models;

namespace ZoomNet.IntegrationTests.Tests
{
	public class Users : IIntegrationTest
	{
		public async Task RunAsync(string userId, IZoomClient client, TextWriter log, CancellationToken cancellationToken)
		{
			if (cancellationToken.IsCancellationRequested) return;

			await log.WriteLineAsync("\n***** USERS *****\n").ConfigureAwait(false);

			// GET ALL THE USERS
			var paginatedUsers = await client.Users.GetAllAsync(UserStatus.Active, null, 100, (string)null, cancellationToken).ConfigureAwait(false);
			await log.WriteLineAsync($"There are {paginatedUsers.Records.Length} users").ConfigureAwait(false);

			// CLEANUP PREVIOUS INTEGRATION TESTS THAT MIGHT HAVE BEEN INTERRUPTED BEFORE THEY HAD TIME TO CLEANUP AFTER THEMSELVES
			var cleanUpTasks = paginatedUsers.Records
				.Where(m => m.FirstName == "ZoomNet" && m.LastName == "Integration Testing")
				.Select(async oldUser =>
				{
					await client.Users.DeleteAsync(oldUser.Id, null, false, false, false, cancellationToken).ConfigureAwait(false);
					await log.WriteLineAsync($"User {oldUser.Id} deleted").ConfigureAwait(false);
					await Task.Delay(250, cancellationToken).ConfigureAwait(false);    // Brief pause to ensure Zoom has time to catch up
				});
			await Task.WhenAll(cleanUpTasks).ConfigureAwait(false);

			// GET MY USER
			var myUser = await client.Users.GetCurrentAsync(cancellationToken).ConfigureAwait(false);
			await log.WriteLineAsync($"My user retrieved. My email address is {myUser.Email}").ConfigureAwait(false);
			await Task.Delay(500, cancellationToken).ConfigureAwait(false);

			// GET MY ASSISTANTS
			var myAssistants = await client.Users.GetAssistantsAsync(myUser.Id, cancellationToken).ConfigureAwait(false);
			await log.WriteLineAsync($"My user has {myAssistants.Length} assistants").ConfigureAwait(false);
			await Task.Delay(500, cancellationToken).ConfigureAwait(false);

			// GET MY SCHEDULERS
			var mySchedulers = await client.Users.GetSchedulersAsync(myUser.Id, cancellationToken).ConfigureAwait(false);
			await log.WriteLineAsync($"My user has {mySchedulers.Length} schedulers").ConfigureAwait(false);
			await Task.Delay(500, cancellationToken).ConfigureAwait(false);

			// GET MY SETTINGS
			var mySettings = await client.Users.GetSettingsAsync(myUser.Id, cancellationToken).ConfigureAwait(false);
			await log.WriteLineAsync("My settings retrieved").ConfigureAwait(false);
			await Task.Delay(500, cancellationToken).ConfigureAwait(false);

			var myMeetingAuthSettings = await client.Users.GetMeetingAuthenticationSettingsAsync(myUser.Id, cancellationToken).ConfigureAwait(false);
			await log.WriteLineAsync("My meeting settings retrieved").ConfigureAwait(false);
			await Task.Delay(500, cancellationToken).ConfigureAwait(false);

			var myRecordingAuthSettings = await client.Users.GetRecordingAuthenticationSettingsAsync(myUser.Id, cancellationToken).ConfigureAwait(false);
			await log.WriteLineAsync("My recording authentication settings retrieved").ConfigureAwait(false);
			await Task.Delay(500, cancellationToken).ConfigureAwait(false);

			// GET MY PERMISSIONS
			var myPermissions = await client.Users.GetPermissionsAsync(myUser.Id, cancellationToken).ConfigureAwait(false);
			await log.WriteLineAsync($"My permissions retrieved: I have been granted {myPermissions.Length} permissions").ConfigureAwait(false);
			//await Task.Delay(500, cancellationToken).ConfigureAwait(false);

			// CREATE NEW USER (commenting out this integration test because I currently do not have permission to create users)
			//var newUser = await client.Users.CreateAsync("integrationtesting@example.com", "ZoomNet", "Integration Testing", null, UserType.Basic, UserCreateType.Normal, cancellationToken).ConfigureAwait(false);
			//await log.WriteLineAsync($"New user created: {newUser.Id}").ConfigureAwait(false);
			//await Task.Delay(500, cancellationToken).ConfigureAwait(false);

			// DELETE USER
			//await client.Users.DeleteAsync(newUser.Id, null, false, false, false, cancellationToken).ConfigureAwait(false);
			//await log.WriteLineAsync($"User {newUser.Id} deleted").ConfigureAwait(false);
		}
	}
}
