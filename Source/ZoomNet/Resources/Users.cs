using Pathoschild.Http.Client;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text.Json.Nodes;
using System.Threading;
using System.Threading.Tasks;
using ZoomNet.Models;

namespace ZoomNet.Resources
{
	/// <summary>
	/// Allows you to manage users.
	/// </summary>
	/// <seealso cref="ZoomNet.Resources.IUsers" />
	/// <remarks>
	/// See <a href="https://marketplace.zoom.us/docs/api-reference/zoom-api/users/">Zoom documentation</a> for more information.
	/// </remarks>
	public class Users : IUsers
	{
		private readonly Pathoschild.Http.Client.IClient _client;

		/// <summary>
		/// Initializes a new instance of the <see cref="Users" /> class.
		/// </summary>
		/// <param name="client">The HTTP client.</param>
		internal Users(Pathoschild.Http.Client.IClient client)
		{
			_client = client;
		}

		/// <summary>
		/// Retrieve all users on your account.
		/// </summary>
		/// <param name="status">The user status. Allowed values: Active, Inactive, pending.</param>
		/// <param name="roleId">Unique identifier for the role. Provide this parameter if you would like to filter the response by a specific role.</param>
		/// <param name="recordsPerPage">The number of records returned within a single API call.</param>
		/// <param name="page">The current page number of returned records.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>
		/// An array of <see cref="User">users</see>.
		/// </returns>
		[Obsolete("Zoom is in the process of deprecating the \"page number\" and \"page count\" fields.")]
		public Task<PaginatedResponse<User>> GetAllAsync(UserStatus status = UserStatus.Active, string roleId = null, int recordsPerPage = 30, int page = 1, CancellationToken cancellationToken = default)
		{
			if (recordsPerPage < 1 || recordsPerPage > 300)
			{
				throw new ArgumentOutOfRangeException(nameof(recordsPerPage), "Records per page must be between 1 and 300");
			}

			return _client
				.GetAsync($"users")
				.WithArgument("status", status.ToEnumString())
				.WithArgument("role_id", roleId)
				.WithArgument("page_size", recordsPerPage)
				.WithArgument("page_number", page)
				.WithCancellationToken(cancellationToken)
				.AsPaginatedResponse<User>("users");
		}

		/// <summary>
		/// Retrieve all users on your account.
		/// </summary>
		/// <param name="status">The user status. Allowed values: Active, Inactive, pending.</param>
		/// <param name="roleId">Unique identifier for the role. Provide this parameter if you would like to filter the response by a specific role.</param>
		/// <param name="recordsPerPage">The number of records returned within a single API call.</param>
		/// <param name="pagingToken">The paging token.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>
		/// An array of <see cref="User">users</see>.
		/// </returns>
		public Task<PaginatedResponseWithToken<User>> GetAllAsync(UserStatus status = UserStatus.Active, string roleId = null, int recordsPerPage = 30, string pagingToken = null, CancellationToken cancellationToken = default)
		{
			if (recordsPerPage < 1 || recordsPerPage > 300)
			{
				throw new ArgumentOutOfRangeException(nameof(recordsPerPage), "Records per page must be between 1 and 300");
			}

			return _client
				.GetAsync($"users")
				.WithArgument("status", status.ToEnumString())
				.WithArgument("role_id", roleId)
				.WithArgument("page_size", recordsPerPage)
				.WithArgument("next_page_token", pagingToken)
				.WithCancellationToken(cancellationToken)
				.AsPaginatedResponseWithToken<User>("users");
		}

		/// <summary>
		/// Creates a user.
		/// </summary>
		/// <param name="email">The email address.</param>
		/// <param name="firstName">First name.</param>
		/// <param name="lastName">Last name.</param>
		/// <param name="password">User password. Only used when createType is <see cref="UserCreateType.Auto"/>.</param>
		/// <param name="type">The type of user.</param>
		/// <param name="createType">Specify how to create the user.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>
		/// The new user.
		/// </returns>
		public Task<User> CreateAsync(string email, string firstName = null, string lastName = null, string password = null, UserType type = UserType.Basic, UserCreateType createType = UserCreateType.Normal, CancellationToken cancellationToken = default)
		{
			var data = new JsonObject
			{
				{ "action", createType.ToEnumString() },
				{
					"user_info", new JsonObject
					{
						{ "email", email },
						{ "type", type },
						{ "first_name", firstName },
						{ "last_name", lastName },
						{ "password", password }
					}
				}
			};

			return _client
				.PostAsync("users")
				.WithJsonBody(data)
				.WithCancellationToken(cancellationToken)
				.AsObject<User>();
		}

		/// <inheritdoc/>
		public Task UpdateAsync(string userId, string firstName = null, string lastName = null, string company = null, string department = null, string groupId = null, string hostKey = null, string jobTitle = null, string language = null, string location = null, string manager = null, IEnumerable<PhoneNumber> phoneNumbers = null, string pmi = null, string pronouns = null, PronounDisplayType? pronounsDisplay = null, TimeZones? timezone = null, UserType? type = null, bool? usePmi = null, string personalMeetingRoomName = null, CancellationToken cancellationToken = default)
		{
			var data = new JsonObject
			{
				{ "company", company },
				{ "dept", department },
				{ "first_name", firstName },
				{ "group_id", groupId },
				{ "host_key", hostKey },
				{ "job_title", jobTitle },
				{ "language", language },
				{ "last_name", lastName },
				{ "location", location },
				{ "manager", manager },
				{ "phone_numbers", phoneNumbers },
				{ "pmi", pmi },
				{ "pronouns", pronouns },
				{ "pronouns_option", pronounsDisplay?.ToEnumString() },
				{ "timezone", timezone?.ToEnumString() },
				{ "type", type?.ToEnumString() },
				{ "use_pmi", usePmi },
				{ "vanity_name", personalMeetingRoomName },
			};

			return _client
				.PatchAsync($"users/{userId}")
				.WithJsonBody(data)
				.WithCancellationToken(cancellationToken)
				.AsMessage();
		}

		/// <summary>
		/// Retrieve the information of a specific user on a Zoom account.
		/// </summary>
		/// <param name="userId">The user Id or email address.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>
		/// The <see cref="User" />.
		/// </returns>
		public Task<User> GetAsync(string userId, CancellationToken cancellationToken = default)
		{
			return _client
				.GetAsync($"users/{userId}")
				.WithCancellationToken(cancellationToken)
				.AsObject<User>();
		}

		/// <summary>
		/// Delete a user.
		/// </summary>
		/// <param name="userId">The user Id or email address.</param>
		/// <param name="transferEmail">Transfer email.</param>
		/// <param name="transferMeetings">Transfer meetings.</param>
		/// <param name="transferWebinars">Transfer webinars.</param>
		/// <param name="transferRecordings">Transfer recordings.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>
		/// The async task.
		/// </returns>
		public Task DeleteAsync(string userId, string transferEmail, bool transferMeetings, bool transferWebinars, bool transferRecordings, CancellationToken cancellationToken = default)
		{
			return _client
				.DeleteAsync($"users/{userId}")
				.WithArgument("action", "delete")
				.WithArgument("transfer_email", transferEmail)
				.WithArgument("transfer_meetings", transferMeetings.ToString().ToLower())
				.WithArgument("transfer_webinars", transferWebinars.ToString().ToLower())
				.WithArgument("transfer_recordings", transferRecordings.ToString().ToLower())
				.WithCancellationToken(cancellationToken)
				.AsMessage();
		}

		/// <summary>
		/// Disassociate a user.
		/// </summary>
		/// <param name="userId">The user Id or email address.</param>
		/// <param name="transferEmail">Transfer email.</param>
		/// <param name="transferMeetings">Transfer meetings.</param>
		/// <param name="transferWebinars">Transfer webinars.</param>
		/// <param name="transferRecordings">Transfer recordings.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>
		/// The async task.
		/// </returns>
		public Task DisassociateAsync(string userId, string transferEmail, bool transferMeetings, bool transferWebinars, bool transferRecordings, CancellationToken cancellationToken = default)
		{
			return _client
				.DeleteAsync($"users/{userId}")
				.WithArgument("action", "disassociate")
				.WithArgument("transfer_email", transferEmail)
				.WithArgument("transfer_meetings", transferMeetings.ToString().ToLower())
				.WithArgument("transfer_webinars", transferWebinars.ToString().ToLower())
				.WithArgument("transfer_recordings", transferRecordings.ToString().ToLower())
				.WithCancellationToken(cancellationToken)
				.AsMessage();
		}

		/// <summary>
		/// Retrieve a user's assistants.
		/// </summary>
		/// <param name="userId">The user Id.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>
		/// An array of <see cref="Assistant">assistants</see>.
		/// </returns>
		public Task<Assistant[]> GetAssistantsAsync(string userId, CancellationToken cancellationToken = default)
		{
			return _client
				.GetAsync($"users/{userId}/assistants")
				.WithCancellationToken(cancellationToken)
				.AsObject<Assistant[]>("assistants");
		}

		/// <summary>
		/// Add assistants to a user.
		/// </summary>
		/// <param name="userId">The user Id.</param>
		/// <param name="assistantIds">The id of the assistants to associate with this user.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>
		/// The async task.
		/// </returns>
		public Task AddAssistantsByIdAsync(string userId, IEnumerable<string> assistantIds, CancellationToken cancellationToken = default)
		{
			if (assistantIds == null || !assistantIds.Any()) throw new ArgumentNullException(nameof(assistantIds), "You must provide at least one assistant Id.");

			var data = new JsonObject
			{
				{ "assistants", assistantIds.Select(id => new JsonObject { { "id", id } }).ToArray() }
			};

			return _client
				.PostAsync($"users/{userId}/assistants")
				.WithJsonBody(data)
				.WithCancellationToken(cancellationToken)
				.AsMessage();
		}

		/// <summary>
		/// Add assistants to a user.
		/// </summary>
		/// <param name="userId">The user Id.</param>
		/// <param name="assistantEmails">The email address of the assistants to associate with this user.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>
		/// The async task.
		/// </returns>
		public Task AddAssistantsByEmailAsync(string userId, IEnumerable<string> assistantEmails, CancellationToken cancellationToken = default)
		{
			if (assistantEmails == null || !assistantEmails.Any()) throw new ArgumentNullException(nameof(assistantEmails), "You must provide at least one assistant email address.");

			var data = new JsonObject
			{
				{ "assistants", assistantEmails.Select(id => new JsonObject { { "email", id } }).ToArray() }
			};

			return _client
				.PostAsync($"users/{userId}/assistants")
				.WithJsonBody(data)
				.WithCancellationToken(cancellationToken)
				.AsMessage();
		}

		/// <summary>
		/// Delete a specific assistant of a user.
		/// </summary>
		/// <param name="userId">The user Id.</param>
		/// <param name="assistantId">The id of the assistant to disassociate from this user.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>
		/// The async task.
		/// </returns>
		public Task DeleteAssistantAsync(string userId, string assistantId, CancellationToken cancellationToken = default)
		{
			return _client
				.DeleteAsync($"users/{userId}/assistants/{assistantId}")
				.WithCancellationToken(cancellationToken)
				.AsMessage();
		}

		/// <summary>
		/// Delete all assistants of a user.
		/// </summary>
		/// <param name="userId">The user Id.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>
		/// The async task.
		/// </returns>
		public Task DeleteAllAssistantsAsync(string userId, CancellationToken cancellationToken = default)
		{
			return _client
				.DeleteAsync($"users/{userId}/assistants")
				.WithCancellationToken(cancellationToken)
				.AsMessage();
		}

		/// <summary>
		/// Retrieve a user's schedulers.
		/// </summary>
		/// <param name="userId">The user Id.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>
		/// An array of <see cref="Assistant">schedulers</see>.
		/// </returns>
		public Task<Assistant[]> GetSchedulersAsync(string userId, CancellationToken cancellationToken = default)
		{
			return _client
				.GetAsync($"users/{userId}/schedulers")
				.WithCancellationToken(cancellationToken)
				.AsObject<Assistant[]>("schedulers");
		}

		/// <summary>
		/// Delete a specific scheduler of a user.
		/// </summary>
		/// <param name="userId">The user Id.</param>
		/// <param name="assistantId">The id of the scheduler to disassociate from this user.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>
		/// The async task.
		/// </returns>
		public Task DeleteSchedulerAsync(string userId, string assistantId, CancellationToken cancellationToken = default)
		{
			return _client
				.DeleteAsync($"users/{userId}/schedulers/{assistantId}")
				.WithCancellationToken(cancellationToken)
				.AsMessage();
		}

		/// <summary>
		/// Delete all schedulers of a user.
		/// </summary>
		/// <param name="userId">The user Id.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>
		/// The async task.
		/// </returns>
		public Task DeleteAllSchedulersAsync(string userId, CancellationToken cancellationToken = default)
		{
			return _client
				.DeleteAsync($"users/{userId}/schedulers")
				.WithCancellationToken(cancellationToken)
				.AsMessage();
		}

		/// <summary>
		/// Upload a user's profile picture.
		/// </summary>
		/// <param name="userId">The user Id.</param>
		/// <param name="fileName">The file name.</param>
		/// <param name="pictureData">The binary data.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>
		/// The async task.
		/// </returns>
		/// <remarks>
		/// File size cannot exceed 2M.
		/// Only jpg/jpeg, gif or png image file can be uploaded.
		/// </remarks>
		public Task UploadProfilePictureAsync(string userId, string fileName, Stream pictureData, CancellationToken cancellationToken = default)
		{
			return _client
				.PostAsync($"users/{userId}/picture")
				.WithBody(bodyBuilder =>
				{
					var content = new MultipartFormDataContent();

					// Zoom requires the 'name' to be 'pic_file'. Also, you
					// must specify the 'fileName' otherwise Zoom will return
					// a very confusing HTTP 400 error with the following body:
					// {"code":120,"message":""}
					content.Add(new StreamContent(pictureData), "pic_file", fileName);

					return content;
				})
				.WithCancellationToken(cancellationToken)
				.AsMessage();
		}

		/// <inheritdoc/>
		public Task DeleteProfilePictureAsync(string userId, CancellationToken cancellationToken = default)
		{
			return _client
				.DeleteAsync($"users/{userId}/picture")
				.WithCancellationToken(cancellationToken)
				.AsMessage();
		}

		/// <summary>
		/// Retrieve a user's settings.
		/// </summary>
		/// <param name="userId">The user Id.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>
		/// The <see cref="UserSettings">settings</see>.
		/// </returns>
		public Task<UserSettings> GetSettingsAsync(string userId, CancellationToken cancellationToken = default)
		{
			return _client
				.GetAsync($"users/{userId}/settings")
				.WithCancellationToken(cancellationToken)
				.AsObject<UserSettings>();
		}

		/// <summary>
		/// Retrieve a user's meeting authentication settings.
		/// </summary>
		/// <param name="userId">The user Id.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>
		/// The <see cref="AuthenticationSettings">settings</see>.
		/// </returns>
		public async Task<AuthenticationSettings> GetMeetingAuthenticationSettingsAsync(string userId, CancellationToken cancellationToken = default)
		{
			var response = await _client
				.GetAsync($"users/{userId}/settings")
				.WithArgument("option", "meeting_authentication")
				.WithCancellationToken(cancellationToken)
				.AsRawJsonDocument()
				.ConfigureAwait(false);

			var settings = new AuthenticationSettings()
			{
				RequireAuthentication = response.RootElement.GetPropertyValue("meeting_authentication", false),
				AuthenticationOptions = response.RootElement.GetProperty("authentication_options", false)?.ToObject<AuthenticationOptions[]>() ?? Array.Empty<AuthenticationOptions>()
			};

			return settings;
		}

		/// <summary>
		/// Retrieve a user's recording authentication settings.
		/// </summary>
		/// <param name="userId">The user Id.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>
		/// The <see cref="AuthenticationSettings">settings</see>.
		/// </returns>
		public async Task<AuthenticationSettings> GetRecordingAuthenticationSettingsAsync(string userId, CancellationToken cancellationToken = default)
		{
			var response = await _client
				.GetAsync($"users/{userId}/settings")
				.WithArgument("option", "recording_authentication")
				.WithCancellationToken(cancellationToken)
				.AsRawJsonDocument()
				.ConfigureAwait(false);

			var settings = new AuthenticationSettings()
			{
				RequireAuthentication = response.RootElement.GetPropertyValue("recording_authentication", false),
				AuthenticationOptions = response.RootElement.GetProperty("authentication_options", false)?.ToObject<AuthenticationOptions[]>() ?? Array.Empty<AuthenticationOptions>()
			};

			return settings;
		}

		/// <inheritdoc/>
		public Task<SecuritySettings> GetSecuritySettingsAsync(string userId, CancellationToken cancellationToken = default)
		{
			return _client
				.GetAsync($"users/{userId}/settings")
				.WithArgument("option", "meeting_security")
				.WithCancellationToken(cancellationToken)
				.AsObject<SecuritySettings>("meeting_security");
		}

		/// <summary>
		/// Deactivate a specific user on a Zoom account.
		/// </summary>
		/// <param name="userId">The user Id or email address.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>
		/// The async task.
		/// </returns>
		public Task DeactivateAsync(string userId, CancellationToken cancellationToken = default)
		{
			var data = new JsonObject
			{
				{ "action", "deactivate" }
			};

			return _client
				.PutAsync($"users/{userId}/status")
				.WithJsonBody(data)
				.WithCancellationToken(cancellationToken)
				.AsMessage();
		}

		/// <summary>
		/// Reactivate a specific user on a Zoom account.
		/// </summary>
		/// <param name="userId">The user Id or email address.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>
		/// The async task.
		/// </returns>
		public Task ReactivateAsync(string userId, CancellationToken cancellationToken = default)
		{
			var data = new JsonObject
			{
				{ "action", "activate" }
			};

			return _client
				.PutAsync($"users/{userId}/status")
				.WithJsonBody(data)
				.WithCancellationToken(cancellationToken)
				.AsMessage();
		}

		/// <summary>
		/// Change the password of a user.
		/// </summary>
		/// <param name="userId">The user Id or email address.</param>
		/// <param name="password">The password.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>
		/// The async task.
		/// </returns>
		public Task ChangePasswordAsync(string userId, string password, CancellationToken cancellationToken = default)
		{
			var data = new JsonObject
			{
				{ "password", password }
			};

			return _client
				.PutAsync($"users/{userId}/password")
				.WithJsonBody(data)
				.WithCancellationToken(cancellationToken)
				.AsMessage();
		}

		/// <summary>
		/// Retrieve the permissions that have been granted to a user.
		/// </summary>
		/// <param name="userId">The user Id or email address.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>
		/// An array of strings.
		/// </returns>
		public Task<string[]> GetPermissionsAsync(string userId, CancellationToken cancellationToken = default)
		{
			return _client
				.GetAsync($"users/{userId}/permissions")
				.WithCancellationToken(cancellationToken)
				.AsObject<string[]>("permissions");
		}

		/// <summary>
		/// Revoke a user's SSO token.
		/// </summary>
		/// <param name="userId">The user Id or email address.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>
		/// The async task.
		/// </returns>
		public Task RevokeSsoTokenAsync(string userId, CancellationToken cancellationToken = default)
		{
			return _client
				.DeleteAsync($"users/{userId}/token")
				.WithCancellationToken(cancellationToken)
				.AsMessage();
		}

		/// <summary>
		/// Verify if a user's email is registered with Zoom.
		/// </summary>
		/// <param name="email">The email address.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>
		/// true if the email is registered to a user within your account.
		/// </returns>
		public Task<bool> CheckEmailInUseAsync(string email, CancellationToken cancellationToken = default)
		{
			return _client
				.GetAsync($"users/email")
				.WithArgument("email", email)
				.WithCancellationToken(cancellationToken)
				.AsObject<bool>("existed_email");
		}

		/// <summary>
		/// Change a user's email address on a Zoom account that has managed domain set up.
		/// </summary>
		/// <param name="userId">The user Id or email address.</param>
		/// <param name="email">The email address.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>
		/// The async task.
		/// </returns>
		public Task ChangeEmailAsync(string userId, string email, CancellationToken cancellationToken = default)
		{
			var data = new JsonObject
			{
				{ "email", email }
			};

			return _client
				.PutAsync($"users/{userId}/email")
				.WithJsonBody(data)
				.WithCancellationToken(cancellationToken)
				.AsMessage();
		}

		/// <summary>
		/// Verify if a personal meeting room with the given name exists or not.
		/// </summary>
		/// <param name="name">The room name.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>
		/// true if a room with the given name exists.
		/// </returns>
		public Task<bool> CheckPersonalMeetingRoomNameInUseAsync(string name, CancellationToken cancellationToken = default)
		{
			return _client
				.GetAsync($"users/vanity_name")
				.WithArgument("vanity_name", name)
				.WithCancellationToken(cancellationToken)
				.AsObject<bool>("existed");
		}

		/// <summary>
		/// Disassociate a user from one account and move the user to another account under the same master account.
		/// </summary>
		/// <param name="userId">The user Id or email address.</param>
		/// <param name="currentAccountId">The current account id.</param>
		/// <param name="newAccountId">The new account id.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>
		/// The async task.
		/// </returns>
		public Task SwitchAccountAsync(string userId, string currentAccountId, string newAccountId, CancellationToken cancellationToken = default)
		{
			var data = new JsonObject
			{
				{ "account_id", newAccountId }
			};

			return _client
				.PutAsync($"accounts/{currentAccountId}/users/{userId}/account")
				.WithJsonBody(data)
				.WithCancellationToken(cancellationToken)
				.AsMessage();
		}

		/// <inheritdoc/>
		public Task<string> GetAccessTokenAsync(string userId, int? ttl = null, CancellationToken cancellationToken = default)
		{
			return _client
				.GetAsync($"users/{userId}/token")
				.WithArgument("type", "zak")
				.WithArgument("ttl", ttl)
				.WithCancellationToken(cancellationToken)
				.AsObject<string>("token");
		}

		/// <inheritdoc/>
		public Task<VirtualBackgroundFile> UploadVirtualBackgroundAsync(string userId, string fileName, Stream pictureData, CancellationToken cancellationToken = default)
		{
			return _client
				.PostAsync($"users/{userId}/settings/virtual_backgrounds")
				.WithBody(bodyBuilder =>
				{
					var content = new MultipartFormDataContent();

					// Zoom requires the 'name' to be 'file'. Also, you
					// must specify the 'fileName' otherwise Zoom will return
					// a very confusing HTTP 400 error with the following body:
					// {"code":-1,"message":"Required request part 'file' is not present"}
					content.Add(new StreamContent(pictureData), "file", fileName);

					return content;
				})
				.WithCancellationToken(cancellationToken)
				.AsObject<VirtualBackgroundFile>();
		}

		/// <inheritdoc/>
		public Task DeleteVirtualBackgroundAsync(string userId, string fileId, CancellationToken cancellationToken = default)
		{
			return _client
				.DeleteAsync($"users/{userId}/settings/virtual_backgrounds")
				.WithArgument("file_ids", fileId)
				.WithCancellationToken(cancellationToken)
				.AsMessage();
		}

		/// <inheritdoc/>
		public Task UpdatePresenceStatusAsync(string userId, PresenceStatus status, int? duration = null, CancellationToken cancellationToken = default)
		{
			if (status == PresenceStatus.Unknown) throw new ArgumentOutOfRangeException("You can not change a user's status to Unknown.", nameof(status));

			var data = new JsonObject
			{
				{ "status", status.ToEnumString() }
			};
			if (status == PresenceStatus.DoNotDisturb) data.Add("duration", duration);

			return _client
				.PutAsync($"users/{userId}/presence_status")
				.WithJsonBody(data)
				.WithCancellationToken(cancellationToken)
				.AsMessage();
		}
	}
}
