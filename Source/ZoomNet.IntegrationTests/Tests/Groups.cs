using System;
using System.IO;
using System.Linq;
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

			// CLEANUP PREVIOUS INTEGRATION TESTS THAT MIGHT HAVE BEEN INTERRUPTED BEFORE THEY HAD TIME TO CLEANUP AFTER THEMSELVES
			var cleanUpTasks = groups
				.Where(g => g.Name.StartsWith("ZoomNet Integration Testing:"))
				.Select(async group =>
				{
					await client.Groups.DeleteAsync(group.Id, cancellationToken).ConfigureAwait(false);
					await log.WriteLineAsync($"Group {group.Id} deleted").ConfigureAwait(false);
					await Task.Delay(250, cancellationToken).ConfigureAwait(false);    // Brief pause to ensure Zoom has time to catch up
				});
			await Task.WhenAll(cleanUpTasks).ConfigureAwait(false);

			// CREATE A NEW GROUP
			var newGroup = await client.Groups.CreateAsync("ZoomNet Integration Testing: this is a test", cancellationToken).ConfigureAwait(false);
			await log.WriteLineAsync($"Group {newGroup.Id} created").ConfigureAwait(false);

			await client.Groups.UpdateAsync(newGroup.Id, "ZoomNet Integration Testing: updated name", cancellationToken).ConfigureAwait(false);
			await log.WriteLineAsync($"The name of group {newGroup.Id} was updated").ConfigureAwait(false);

			var group = await client.Groups.GetAsync(newGroup.Id, cancellationToken).ConfigureAwait(false);
			await log.WriteLineAsync($"Group {group.Id} retrieved").ConfigureAwait(false);

			// UPLOAD BAKGROUND IMAGES
			// Check that this computer has a folder containing sample images
			var samplePicturesFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyPictures), "Samples");
			if (Directory.Exists(samplePicturesFolder))
			{
				var rnd = new Random();
				var samplePictures = Directory.EnumerateFiles(samplePicturesFolder, "*.jpg");
				if (samplePictures.Any())
				{
					// UPLOAD A FILE
					var samplePicture = samplePictures.ElementAt(rnd.Next(0, samplePictures.Count()));
					using var fileToUploadStream = File.OpenRead(samplePicture);
					var uploadedFileId = await client.Groups.UploadVirtualBackgroundAsync(group.Id, Path.GetFileName(samplePicture), fileToUploadStream, cancellationToken).ConfigureAwait(false);
					await log.WriteLineAsync($"File {uploadedFileId} uploaded").ConfigureAwait(false);
				}
			}

			// ADD AND REMOVE MEMBERS/ADMINISTRATORS
			var memberId = await client.Groups.AddMemberByIdAsync(group.Id, myUser.Id, cancellationToken).ConfigureAwait(false);
			if (!string.IsNullOrEmpty(memberId)) await log.WriteLineAsync($"{myUser.Id} added as a member to group {group.Id}").ConfigureAwait(false);

			var adminId = await client.Groups.AddAdministratorByIdAsync(group.Id, myUser.Id, cancellationToken).ConfigureAwait(false);
			if (!string.IsNullOrEmpty(adminId)) await log.WriteLineAsync($"{myUser.Id} added as an administrator to group {group.Id}").ConfigureAwait(false);

			var members = await client.Groups.GetMembersAsync(group.Id, cancellationToken: cancellationToken).ConfigureAwait(false);
			await log.WriteLineAsync($"Group {group.Id} has {members.TotalRecords} members").ConfigureAwait(false);

			var admins = await client.Groups.GetAdministratorsAsync(newGroup.Id, cancellationToken: cancellationToken).ConfigureAwait(false);
			await log.WriteLineAsync($"Group {group.Id} has {admins.TotalRecords} administrators").ConfigureAwait(false);

			if (!string.IsNullOrEmpty(memberId))
			{
				await client.Groups.RemoveMemberAsync(newGroup.Id, memberId, cancellationToken).ConfigureAwait(false);
				await log.WriteLineAsync($"{myUser.Id} removed as a member from group {newGroup.Id}").ConfigureAwait(false);
			}

			if (!string.IsNullOrEmpty(adminId))
			{
				await client.Groups.RemoveAdministratorAsync(newGroup.Id, adminId, cancellationToken).ConfigureAwait(false);
				await log.WriteLineAsync($"{myUser.Id} removed as an administrator from group {newGroup.Id}").ConfigureAwait(false);
			}

			// DELETE THE GROUP
			await client.Groups.DeleteAsync(group.Id, cancellationToken).ConfigureAwait(false);
			await log.WriteLineAsync($"Group {group.Id} deleted").ConfigureAwait(false);
		}
	}
}
