using System.IO;
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

			// GET MY USER
			var myUser = await client.Users.GetCurrentAsync(cancellationToken).ConfigureAwait(false);
			await log.WriteLineAsync($"My user retrieved. My email address is {myUser.Email}").ConfigureAwait(false);
			await Task.Delay(500, cancellationToken).ConfigureAwait(false);

			// UPDATE MY USER
			await client.Users.UpdateAsync(myUser.Id,
				firstName: "Hello",
				lastName: "World",
				company: "Microsoft",
				department: "Accounting",
				jobTitle: "CFO",
				location: "3rd floor",
				manager: "Bob",
				phoneNumbers: new[]
				{
					new UserPhoneNumber { Country = Country.Canada, CountryCode = "+1", Number = "555-555-1234", Type = UserPhoneType.Office  }
				},
				cancellationToken: cancellationToken).ConfigureAwait(false);
			await log.WriteLineAsync("My user was updated").ConfigureAwait(false);
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
			var myPermissions = await client.Users.GetPermissionsAsync("me", cancellationToken).ConfigureAwait(false);
			await log.WriteLineAsync($"My permissions retrieved: I have been granted {myPermissions.Length} permissions").ConfigureAwait(false);
			await Task.Delay(500, cancellationToken).ConfigureAwait(false);
		}
	}
}
