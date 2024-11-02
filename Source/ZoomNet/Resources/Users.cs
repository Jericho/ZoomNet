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
	/// <inheritdoc/>
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

		/// <inheritdoc/>
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

		/// <inheritdoc/>
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

		/// <inheritdoc/>
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
		public Task UpdateAsync(string userId, string firstName = null, string lastName = null, string company = null, string department = null, string groupId = null, string hostKey = null, string jobTitle = null, string language = null, string location = null, string manager = null, IEnumerable<PhoneNumber> phoneNumbers = null, string pmi = null, string pronouns = null, PronounDisplayType? pronounsDisplay = null, TimeZones? timezone = null, UserType? type = null, bool? usePmi = null, string personalMeetingRoomName = null, IEnumerable<CustomAttribute> customAttributes = null, CancellationToken cancellationToken = default)
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
				{ "phone_numbers", phoneNumbers?.ToArray() },
				{ "pmi", pmi },
				{ "pronouns", pronouns },
				{ "pronouns_option", pronounsDisplay?.ToEnumString() },
				{ "timezone", timezone?.ToEnumString() },
				{ "type", type },
				{ "use_pmi", usePmi },
				{ "vanity_name", personalMeetingRoomName },
				{ "custom_attributes", customAttributes?.ToArray() }
			};

			return _client
				.PatchAsync($"users/{userId}")
				.WithJsonBody(data)
				.WithCancellationToken(cancellationToken)
				.AsMessage();
		}

		/// <inheritdoc/>
		public Task<User> GetAsync(string userId, CancellationToken cancellationToken = default)
		{
			return _client
				.GetAsync($"users/{userId}")
				.WithCancellationToken(cancellationToken)
				.AsObject<User>();
		}

		/// <inheritdoc/>
		public Task DeleteAsync(string userId, string transferEmail, bool transferMeetings, bool transferWebinars, bool transferRecordings, CancellationToken cancellationToken = default)
		{
			return _client
				.DeleteAsync($"users/{userId}")
				.WithArgument("action", "delete")
				.WithArgument("transfer_email", transferEmail)
				.WithArgument("transfer_meetings", transferMeetings.ToString().ToLowerInvariant())
				.WithArgument("transfer_webinars", transferWebinars.ToString().ToLowerInvariant())
				.WithArgument("transfer_recordings", transferRecordings.ToString().ToLowerInvariant())
				.WithCancellationToken(cancellationToken)
				.AsMessage();
		}

		/// <inheritdoc/>
		public Task DisassociateAsync(string userId, string transferEmail, bool transferMeetings, bool transferWebinars, bool transferRecordings, CancellationToken cancellationToken = default)
		{
			return _client
				.DeleteAsync($"users/{userId}")
				.WithArgument("action", "disassociate")
				.WithArgument("transfer_email", transferEmail)
				.WithArgument("transfer_meetings", transferMeetings.ToString().ToLowerInvariant())
				.WithArgument("transfer_webinars", transferWebinars.ToString().ToLowerInvariant())
				.WithArgument("transfer_recordings", transferRecordings.ToString().ToLowerInvariant())
				.WithCancellationToken(cancellationToken)
				.AsMessage();
		}

		/// <inheritdoc/>
		public Task<Assistant[]> GetAssistantsAsync(string userId, CancellationToken cancellationToken = default)
		{
			return _client
				.GetAsync($"users/{userId}/assistants")
				.WithCancellationToken(cancellationToken)
				.AsObject<Assistant[]>("assistants");
		}

		/// <inheritdoc/>
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

		/// <inheritdoc/>
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

		/// <inheritdoc/>
		public Task DeleteAssistantAsync(string userId, string assistantId, CancellationToken cancellationToken = default)
		{
			return _client
				.DeleteAsync($"users/{userId}/assistants/{assistantId}")
				.WithCancellationToken(cancellationToken)
				.AsMessage();
		}

		/// <inheritdoc/>
		public Task DeleteAllAssistantsAsync(string userId, CancellationToken cancellationToken = default)
		{
			return _client
				.DeleteAsync($"users/{userId}/assistants")
				.WithCancellationToken(cancellationToken)
				.AsMessage();
		}

		/// <inheritdoc/>
		public Task<Assistant[]> GetSchedulersAsync(string userId, CancellationToken cancellationToken = default)
		{
			return _client
				.GetAsync($"users/{userId}/schedulers")
				.WithCancellationToken(cancellationToken)
				.AsObject<Assistant[]>("schedulers");
		}

		/// <inheritdoc/>
		public Task DeleteSchedulerAsync(string userId, string assistantId, CancellationToken cancellationToken = default)
		{
			return _client
				.DeleteAsync($"users/{userId}/schedulers/{assistantId}")
				.WithCancellationToken(cancellationToken)
				.AsMessage();
		}

		/// <inheritdoc/>
		public Task DeleteAllSchedulersAsync(string userId, CancellationToken cancellationToken = default)
		{
			return _client
				.DeleteAsync($"users/{userId}/schedulers")
				.WithCancellationToken(cancellationToken)
				.AsMessage();
		}

		/// <inheritdoc/>
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

		/// <inheritdoc/>
		public Task<UserSettings> GetSettingsAsync(string userId, CancellationToken cancellationToken = default)
		{
			return _client
				.GetAsync($"users/{userId}/settings")
				.WithCancellationToken(cancellationToken)
				.AsObject<UserSettings>();
		}

		/// <inheritdoc/>
		public async Task<AuthenticationSettings> GetMeetingAuthenticationSettingsAsync(string userId, CancellationToken cancellationToken = default)
		{
			var response = await _client
				.GetAsync($"users/{userId}/settings")
				.WithArgument("option", "meeting_authentication")
				.WithCancellationToken(cancellationToken)
				.AsJson()
				.ConfigureAwait(false);

			var settings = new AuthenticationSettings()
			{
				RequireAuthentication = response.GetPropertyValue("meeting_authentication", false),
				AuthenticationOptions = response.GetProperty("authentication_options", false)?.ToObject<AuthenticationOptions[]>() ?? Array.Empty<AuthenticationOptions>()
			};

			return settings;
		}

		/// <inheritdoc/>
		public async Task<AuthenticationSettings> GetRecordingAuthenticationSettingsAsync(string userId, CancellationToken cancellationToken = default)
		{
			var response = await _client
				.GetAsync($"users/{userId}/settings")
				.WithArgument("option", "recording_authentication")
				.WithCancellationToken(cancellationToken)
				.AsJson()
				.ConfigureAwait(false);

			var settings = new AuthenticationSettings()
			{
				RequireAuthentication = response.GetPropertyValue("recording_authentication", false),
				AuthenticationOptions = response.GetProperty("authentication_options", false)?.ToObject<AuthenticationOptions[]>() ?? Array.Empty<AuthenticationOptions>()
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

		/// <inheritdoc/>
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

		/// <inheritdoc/>
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

		/// <inheritdoc/>
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

		/// <inheritdoc/>
		public Task<string[]> GetPermissionsAsync(string userId, CancellationToken cancellationToken = default)
		{
			return _client
				.GetAsync($"users/{userId}/permissions")
				.WithCancellationToken(cancellationToken)
				.AsObject<string[]>("permissions");
		}

		/// <inheritdoc/>
		public Task RevokeSsoTokenAsync(string userId, CancellationToken cancellationToken = default)
		{
			return _client
				.DeleteAsync($"users/{userId}/token")
				.WithCancellationToken(cancellationToken)
				.AsMessage();
		}

		/// <inheritdoc/>
		public Task<bool> CheckEmailInUseAsync(string email, CancellationToken cancellationToken = default)
		{
			return _client
				.GetAsync($"users/email")
				.WithArgument("email", email)
				.WithCancellationToken(cancellationToken)
				.AsObject<bool>("existed_email");
		}

		/// <inheritdoc/>
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

		/// <inheritdoc/>
		public Task<bool> CheckPersonalMeetingRoomNameInUseAsync(string name, CancellationToken cancellationToken = default)
		{
			return _client
				.GetAsync($"users/vanity_name")
				.WithArgument("vanity_name", name)
				.WithCancellationToken(cancellationToken)
				.AsObject<bool>("existed");
		}

		/// <inheritdoc/>
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
			if (status == PresenceStatus.Unknown) throw new ArgumentOutOfRangeException(nameof(status), "You can not change a user's status to Unknown.");

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

		/// <inheritdoc/>
		public Task<PresenceStatusResponse> GetPresenceStatusAsync(string userId, CancellationToken cancellationToken = default)
		{
			return _client
				.GetAsync($"users/{userId}/presence_status")
				.WithCancellationToken(cancellationToken)
				.AsObject<PresenceStatusResponse>();
		}
	}
}
