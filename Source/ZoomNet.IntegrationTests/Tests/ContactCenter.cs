using System;
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

			var paginatedSkillCategories = await client.ContactCenter.GetAllSkillCategoriesAsync(30, null, cancellationToken).ConfigureAwait(false);
			await log.WriteLineAsync($"There are {paginatedSkillCategories.TotalRecords} skill categories in Contact Center").ConfigureAwait(false);

			var paginatedSkills = await client.ContactCenter.GetAllSkillsAsync(30, null, cancellationToken).ConfigureAwait(false);
			await log.WriteLineAsync($"There are {paginatedSkills.TotalRecords} skills in Contact Center").ConfigureAwait(false);

			var paginatedAgentStatuses = await client.ContactCenter.GetAllAgentStatusesAsync(30, null, cancellationToken).ConfigureAwait(false);
			await log.WriteLineAsync($"There are {paginatedAgentStatuses.TotalRecords} agent statuses in Contact Center").ConfigureAwait(false);

			var paginatedAddressBookUnits = await client.ContactCenter.GetAllAddressBookUnitsAsync(30, null, cancellationToken).ConfigureAwait(false);
			await log.WriteLineAsync($"There are {paginatedAddressBookUnits.TotalRecords} address book units in Contact Center").ConfigureAwait(false);

			var paginatedCustomFields = await client.ContactCenter.GetAllAddressBookCustomFieldsAsync(30, null, cancellationToken).ConfigureAwait(false);
			await log.WriteLineAsync($"There are {paginatedCustomFields.TotalRecords} address book custom fields in Contact Center").ConfigureAwait(false);

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

			cleanUpTasks = paginatedSkills.Records
				.Where(m => m.Name.StartsWith("ZoomNet Integration Testing:"))
				.Select(async oldSkill =>
				{
					await client.ContactCenter.DeleteSkillAsync(oldSkill.Id, cancellationToken).ConfigureAwait(false);
					await log.WriteLineAsync($"Skill {oldSkill.Id} deleted").ConfigureAwait(false);
					await Task.Delay(1000, cancellationToken).ConfigureAwait(false);    // Brief pause to ensure Zoom has time to catch up
				});
			await Task.WhenAll(cleanUpTasks).ConfigureAwait(false);

			cleanUpTasks = paginatedSkillCategories.Records
				.Where(m => m.Name.StartsWith("ZoomNet Integration Testing:"))
				.Select(async oldSkillCategory =>
				{
					await client.ContactCenter.DeleteSkillCategoryAsync(oldSkillCategory.Id, cancellationToken).ConfigureAwait(false);
					await log.WriteLineAsync($"Skill category {oldSkillCategory.Id} deleted").ConfigureAwait(false);
					await Task.Delay(1000, cancellationToken).ConfigureAwait(false);    // Brief pause to ensure Zoom has time to catch up
				});
			await Task.WhenAll(cleanUpTasks).ConfigureAwait(false);

			cleanUpTasks = paginatedAgentStatuses.Records
				.Where(s => s.Name.StartsWith("ZoomNet"))
				.Select(async oldStatus =>
				{
					await client.ContactCenter.DeleteAgentStatusAsync(oldStatus.Id, cancellationToken).ConfigureAwait(false);
					await log.WriteLineAsync($"Agent status {oldStatus.Id} deleted").ConfigureAwait(false);
					await Task.Delay(1000, cancellationToken).ConfigureAwait(false);    // Brief pause to ensure Zoom has time to catch up
				});
			await Task.WhenAll(cleanUpTasks).ConfigureAwait(false);

			cleanUpTasks = paginatedCustomFields.Records
				.Where(f => f.Name.StartsWith("ZoomNet") || f.Name.Equals("field1", StringComparison.OrdinalIgnoreCase) || f.Name.Equals("field2", StringComparison.OrdinalIgnoreCase))
				.Select(async oldCustomField =>
				{
					await client.ContactCenter.DeleteAddressBookCustomFieldAsync(oldCustomField.Id, cancellationToken).ConfigureAwait(false);
					await log.WriteLineAsync($"Address book custom field {oldCustomField.Id} deleted").ConfigureAwait(false);
				});
			await Task.WhenAll(cleanUpTasks).ConfigureAwait(false);

			cleanUpTasks = paginatedAddressBookUnits.Records
				.Where(u => u.Name.StartsWith("ZoomNet"))
				.Select(async oldUnit =>
				{
					var paginatedAddressBooks = await client.ContactCenter.GetAllAddressBooksAsync(oldUnit.Id, 30, null, cancellationToken).ConfigureAwait(false);
					foreach (var addressBook in paginatedAddressBooks.Records.Where(b => b.Name.StartsWith("ZoomNet")))
					{
						await client.ContactCenter.DeleteAddressBookAsync(addressBook.Id, cancellationToken).ConfigureAwait(false);
						await log.WriteLineAsync($"Address book {addressBook.Id} deleted").ConfigureAwait(false);
					}

					await client.ContactCenter.DeleteAddressBookUnitAsync(oldUnit.Id, cancellationToken).ConfigureAwait(false);
					await log.WriteLineAsync($"Address book unit {oldUnit.Id} deleted").ConfigureAwait(false);
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
				await log.WriteLineAsync($"There are {availableUsers.Length} users available to be added to Contact Center").ConfigureAwait(false);
			}

			var newNotReadyReason = await client.ContactCenter.CreateAgentNotReadyReasonAsync("ZoomNet Integration Testing: Not Ready reason", null, true, cancellationToken).ConfigureAwait(false);
			await log.WriteLineAsync($"Not ready reason created: {newNotReadyReason.Id}").ConfigureAwait(false);

			// Create a skill category
			var newSkillCategory = await client.ContactCenter.CreateSkillCategoryAsync("ZoomNet Integration Testing: skill category", "This skill category is for testing purposes", ContactCenterSkillType.Proficiency, 4, cancellationToken).ConfigureAwait(false);
			await log.WriteLineAsync($"Skill category {newSkillCategory.Id} created in Contact Center").ConfigureAwait(false);

			await client.ContactCenter.UpdateSkillCategoryAsync(newSkillCategory.Id, description: "Updated description", cancellationToken: cancellationToken).ConfigureAwait(false);
			await log.WriteLineAsync($"Skill category {newSkillCategory.Id} updated in Contact Center").ConfigureAwait(false);

			// Create a skill
			var newSkill = await client.ContactCenter.CreateSkillAsync("ZoomNet Integration Testing: skill", newSkillCategory.Id, cancellationToken).ConfigureAwait(false);
			await log.WriteLineAsync($"Skill {newSkill.Id} created in Contact Center").ConfigureAwait(false);

			await client.ContactCenter.UpdateSkillAsync(newSkill.Id, "ZoomNet Integration Testing: UPDATED name", cancellationToken).ConfigureAwait(false);
			await log.WriteLineAsync($"Skill {newSkill.Id} updated in Contact Center").ConfigureAwait(false);

			// Create a queue
			var newQueue = await client.ContactCenter.CreateQueueAsync("ZoomNet Integration Testing: queue", "This queue is for testing purposes", ContactCenterQueueChannel.Video, cancellationToken).ConfigureAwait(false);
			await log.WriteLineAsync($"Queue {newQueue.Id} created in Contact Center").ConfigureAwait(false);

			// Create a role
			var newRole = await client.ContactCenter.CreateRoleAsync("ZoomNet Integration Testing: Agent role", "This role is for testing purposes", new[] { "OutboundVoiceCall" }, cancellationToken).ConfigureAwait(false);
			await log.WriteLineAsync($"Role {newRole.Id} created in Contact Center").ConfigureAwait(false);

			// Create a new user
			var newUser = await client.ContactCenter.CreateUserAsync(availableUsers[0].Email, newRole.Id, cancellationToken: cancellationToken).ConfigureAwait(false);
			await log.WriteLineAsync($"User {newUser.EmailAddress} added to Contact Center").ConfigureAwait(false);

			await client.ContactCenter.UpdateUserStatusAsync(newUser.Id, ContactCenterUserStatus.NotReady, ContactCenterUserSubStatus.Training, cancellationToken).ConfigureAwait(false);
			await log.WriteLineAsync($"Contact Center user {newUser.Id} set to 'Not Ready', because the user is in training").ConfigureAwait(false);

			// Assign the user to the queue
			await client.ContactCenter.AssignAgentAsync(newQueue.Id, newUser.Id, cancellationToken).ConfigureAwait(false);
			await log.WriteLineAsync($"Contact Center user {newUser.Id} assigned to queue {newQueue.Id}").ConfigureAwait(false);

			// Asign the skill to the user
			await client.ContactCenter.AssignSkillsAsync(newUser.Id, [(newSkill.Id, 2)], cancellationToken).ConfigureAwait(false);
			await log.WriteLineAsync($"Contact Center skill {newSkill.Id} assigned to user {newUser.Id}").ConfigureAwait(false);

			// Manage an address book
			var newAddressBookUnit = await client.ContactCenter.CreateAddressBookUnitAsync("ZoomNet Integration Testing: address book unit", "This address book unit was created during integration testing", cancellationToken).ConfigureAwait(false);
			await log.WriteLineAsync($"Address book unit {newAddressBookUnit.Id} created").ConfigureAwait(false);

			var newAddressBook = await client.ContactCenter.CreateAddressBookAsync("ZoomNet Integration Testing: address book", "This address book was created during integration testing", newAddressBookUnit.Id, cancellationToken).ConfigureAwait(false);
			await log.WriteLineAsync($"Address book {newAddressBook.Id} created").ConfigureAwait(false);

			var newCustomField1 = await client.ContactCenter.CreateAddressBookCustomFieldAsync("ZoomNet field1", ContactCenterAddressBookCustomFieldDataType.String, "This is a custom field", null, null, [newAddressBook.Id], cancellationToken: cancellationToken).ConfigureAwait(false);
			await log.WriteLineAsync($"Custom field {newCustomField1.Id} created").ConfigureAwait(false);

			var newCustomField2 = await client.ContactCenter.CreateAddressBookCustomFieldAsync("ZoomNet field2", ContactCenterAddressBookCustomFieldDataType.Number, "This is another custom field", null, null, [newAddressBook.Id], cancellationToken: cancellationToken).ConfigureAwait(false);
			await log.WriteLineAsync($"Custom field {newCustomField2.Id} created").ConfigureAwait(false);

			var addressBookContactCustomFields = new (string, string)[]
			{
				(newCustomField1.Id, "aaabbbccc"),
				(newCustomField2.Id, "456")
			};
			var newAddressBookContact = await client.ContactCenter.CreateAddressBookContactAsync(newAddressBook.Id, "John Doe", "John", "Doe", [("1234567890", ContactCenterAddressBookPhoneNumberType.Work)], ["john@example.com"], null, null, null, "Big Corp Inc.", "Purchaser", null, null, addressBookContactCustomFields, cancellationToken).ConfigureAwait(false);
			await log.WriteLineAsync($"Contact {newAddressBookContact.DisplayName} created in address book {newAddressBook.Id}").ConfigureAwait(false);

			var contactCustomFields = await client.ContactCenter.GetAllContactCustomFieldsAsync(newAddressBookContact.Id, 30, null, cancellationToken).ConfigureAwait(false);
			await log.WriteLineAsync($"Contact {newAddressBookContact.DisplayName} has {contactCustomFields.TotalRecords} custom fields").ConfigureAwait(false);

			// Miscellaneous queries
			var paginatedUserQueues = await client.ContactCenter.GetUserQueuesAsync(newUser.Id, null, ContactCenterQueueAssignmentType.Any, 30, null, cancellationToken).ConfigureAwait(false);
			await log.WriteLineAsync($"Contact Center user {newUser.Id} is assigned to {paginatedUserQueues.TotalRecords} queues").ConfigureAwait(false);

			var paginatedUserSkills = await client.ContactCenter.GetUserSkillsAsync(newUser.Id, null, null, 30, null, cancellationToken).ConfigureAwait(false);
			await log.WriteLineAsync($"Contact Center user {newUser.Id} has {paginatedUserSkills.TotalRecords} skills").ConfigureAwait(false);

			var paginatedSkillUsers = await client.ContactCenter.GetSkillUsersAsync(newSkill.Id, 30, null, cancellationToken).ConfigureAwait(false);
			await log.WriteLineAsync($"Contact Center skill {newSkill.Id} is assigned to {paginatedSkillUsers.TotalRecords} users").ConfigureAwait(false);

			// Clean up
			await client.ContactCenter.UnassignAgentAsync(newQueue.Id, newUser.Id, cancellationToken).ConfigureAwait(false);
			await log.WriteLineAsync($"Contact Center user {newUser.Id} unassigned from queue {newQueue.Id}").ConfigureAwait(false);

			await client.ContactCenter.DeleteUserAsync(newUser.Id, cancellationToken).ConfigureAwait(false);
			await log.WriteLineAsync($"Contact Center user {newUser.Id} deleted").ConfigureAwait(false);

			await client.ContactCenter.DeleteQueueAsync(newQueue.Id, cancellationToken).ConfigureAwait(false);
			await log.WriteLineAsync($"Queue {newQueue.Id} deleted").ConfigureAwait(false);

			await client.ContactCenter.DeleteSkillAsync(newSkill.Id, cancellationToken).ConfigureAwait(false);
			await log.WriteLineAsync($"Skill {newSkill.Id} deleted").ConfigureAwait(false);

			await client.ContactCenter.DeleteSkillCategoryAsync(newSkillCategory.Id, cancellationToken).ConfigureAwait(false);
			await log.WriteLineAsync($"Skill category {newSkillCategory.Id} deleted").ConfigureAwait(false);

			await client.ContactCenter.DeleteAgentStatusAsync(newNotReadyReason.Id, cancellationToken).ConfigureAwait(false);
			await log.WriteLineAsync($"'Not Ready' reason {newNotReadyReason.Id} deleted").ConfigureAwait(false);

			await client.ContactCenter.DeleteAddressBookCustomFieldAsync(newCustomField1.Id, cancellationToken).ConfigureAwait(false);
			await log.WriteLineAsync($"Address book custom field {newCustomField1.Id} deleted").ConfigureAwait(false);

			await client.ContactCenter.DeleteAddressBookCustomFieldAsync(newCustomField2.Id, cancellationToken).ConfigureAwait(false);
			await log.WriteLineAsync($"Address book custom field {newCustomField2.Id} deleted").ConfigureAwait(false);

			await client.ContactCenter.DeleteAddressBookAsync(newAddressBook.Id, cancellationToken).ConfigureAwait(false);
			await log.WriteLineAsync($"Address book {newAddressBook.Id} deleted").ConfigureAwait(false);

			await client.ContactCenter.DeleteAddressBookUnitAsync(newAddressBookUnit.Id, cancellationToken).ConfigureAwait(false);
			await log.WriteLineAsync($"Address book unit {newAddressBookUnit.Id} deleted").ConfigureAwait(false);
		}
	}
}
