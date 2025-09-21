using System.IO;
using System.Linq;
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

			var paginatedQueues = await client.ContactCenter.GetAllQueuesAsync(null, 30, null, cancellationToken).ConfigureAwait(false);
			await log.WriteLineAsync($"There are {paginatedQueues.TotalRecords} queues in Contact Center").ConfigureAwait(false);

			var paginatedRoles = await client.ContactCenter.GetAllRolesAsync(30, null, cancellationToken).ConfigureAwait(false);
			await log.WriteLineAsync($"There are {paginatedRoles.TotalRecords} roles in Contact Center").ConfigureAwait(false);
			var builtinAgentRole = paginatedRoles.Records.Single(r => r.Name == "Agent");

			// CLEANUP PREVIOUS INTEGRATION TESTS THAT MIGHT HAVE BEEN INTERRUPTED BEFORE THEY HAD TIME TO CLEANUP AFTER THEMSELVES
			var cleanUpTasks = paginatedQueues.Records
				.Where(m => m.Name.StartsWith("ZoomNet Integration Testing:"))
				.Select(async oldQueue =>
				{
					var assignedAgents = await client.ContactCenter.GetAllQueueAgentsAsync(oldQueue.Id, 30, null, cancellationToken).ConfigureAwait(false);
					foreach (var agent in assignedAgents.Records)
					{
						await client.ContactCenter.UnassignAgentAsync(oldQueue.Id, agent.Id, cancellationToken).ConfigureAwait(false);
						await log.WriteLineAsync($"Contact Center user {agent.Id} unassigned from queue {oldQueue.Id}").ConfigureAwait(false);

						await client.ContactCenter.DeleteUserAsync(agent.Id, cancellationToken).ConfigureAwait(false);
						await log.WriteLineAsync($"Contact Center user {agent.Id} deleted").ConfigureAwait(false);
					}

					await client.ContactCenter.DeleteQueueAsync(oldQueue.Id, cancellationToken).ConfigureAwait(false);
					await log.WriteLineAsync($"Queue {oldQueue.Id} deleted").ConfigureAwait(false);
					await Task.Delay(1000, cancellationToken).ConfigureAwait(false);    // Brief pause to ensure Zoom has time to catch up
				});
			await Task.WhenAll(cleanUpTasks).ConfigureAwait(false);

			cleanUpTasks = paginatedRoles.Records
				.Where(m => m.Name.StartsWith("ZoomNet Integration Testing:"))
				.Select(async oldRole =>
				{
					await client.ContactCenter.DeleteRoleAsync(oldRole.Id, builtinAgentRole.Id, cancellationToken).ConfigureAwait(false);
					await log.WriteLineAsync($"Role {oldRole.Id} deleted").ConfigureAwait(false);
					await Task.Delay(1000, cancellationToken).ConfigureAwait(false);    // Brief pause to ensure Zoom has time to catch up
				});
			await Task.WhenAll(cleanUpTasks).ConfigureAwait(false);

			// Make sure there is at least one Zoom user that can be added to Contact Center
			var paginatedContactCenterUsers = await client.ContactCenter.SearchUsersAsync(null, null, null, 10, null, cancellationToken).ConfigureAwait(false);
			await log.WriteLineAsync($"Found {paginatedContactCenterUsers.TotalRecords} user profiles").ConfigureAwait(false);

			var paginatedUsers = await client.Users.GetAllAsync(UserStatus.Active, null, 30, null, cancellationToken).ConfigureAwait(false);
			await log.WriteLineAsync($"There are {paginatedUsers.TotalRecords} active users in the account").ConfigureAwait(false);

			var availableUsers = paginatedUsers.Records.Where(user => !paginatedContactCenterUsers.Records.Any(ccuser => ccuser.EmailAddress.Equals(user.Email, System.StringComparison.OrdinalIgnoreCase))).ToArray();
			if (availableUsers.Length == 0)
			{
				await log.WriteLineAsync("Aborting this integration test because there are no available users to be added to Contact Center").ConfigureAwait(false);
				return;
			}
			else
			{
				await log.WriteLineAsync($"There are {availableUsers.Count()} users available to be added to Contact Center").ConfigureAwait(false);
			}

			// Create a queue
			var newQueue = await client.ContactCenter.CreateQueueAsync("ZoomNet Integration Testing: queue", "This queue is for testing purposes", ContactCenterQueueChannel.Video, cancellationToken).ConfigureAwait(false);
			await log.WriteLineAsync($"Queue {newQueue.Id} created in Contact Center").ConfigureAwait(false);

			// Create a role
			var agentRole = await client.ContactCenter.CreateRoleAsync("ZoomNet Integration Testing: Agent role", "This role is for testing purposes", new[] { "OutboundVoiceCall" }, cancellationToken).ConfigureAwait(false);
			await log.WriteLineAsync($"Role {agentRole.Id} created in Contact Center").ConfigureAwait(false);

			// Create a new user
			var user = await client.ContactCenter.CreateUserAsync(availableUsers[0].Email, agentRole.Id, cancellationToken: cancellationToken).ConfigureAwait(false);
			await log.WriteLineAsync($"User {user.EmailAddress} added to Contact Center").ConfigureAwait(false);

			// Assign the user to the queue
			await client.ContactCenter.AssignAgentAsync(newQueue.Id, user.Id, cancellationToken).ConfigureAwait(false);
			await log.WriteLineAsync($"Contact Center user {user.Id} assigned to queue {newQueue.Id}").ConfigureAwait(false);
		}
	}
}
