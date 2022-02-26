using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using ZoomNet.Models;

namespace ZoomNet.Resources
{
	/// <summary>
	/// Allows you to manage users.
	/// </summary>
	/// <remarks>
	/// See <a href="https://marketplace.zoom.us/docs/api-reference/zoom-api/users/">Zoom documentation</a> for more information.
	/// </remarks>
	public interface IUsers
	{
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
		Task<PaginatedResponse<User>> GetAllAsync(UserStatus status = UserStatus.Active, string roleId = null, int recordsPerPage = 30, int page = 1, CancellationToken cancellationToken = default);

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
		Task<PaginatedResponseWithToken<User>> GetAllAsync(UserStatus status = UserStatus.Active, string roleId = null, int recordsPerPage = 30, string pagingToken = null, CancellationToken cancellationToken = default);

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
		Task<User> CreateAsync(string email, string firstName = null, string lastName = null, string password = null, UserType type = UserType.Basic, UserCreateType createType = UserCreateType.Normal, CancellationToken cancellationToken = default);

		/// <summary>
		/// Retrieve the information of a specific user on a Zoom account.
		/// </summary>
		/// <param name="userId">The user Id or email address.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>
		/// The <see cref="User" />.
		/// </returns>
		Task<User> GetAsync(string userId, CancellationToken cancellationToken = default);

		/// <summary>
		/// Permanently delete a user.
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
		/// <remarks>
		/// To delete a pending user in the account, use <see cref="DisassociateAsync"/>.
		/// </remarks>
		Task DeleteAsync(string userId, string transferEmail, bool transferMeetings, bool transferWebinars, bool transferRecordings, CancellationToken cancellationToken = default);

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
		Task DisassociateAsync(string userId, string transferEmail, bool transferMeetings, bool transferWebinars, bool transferRecordings, CancellationToken cancellationToken = default);

		/// <summary>
		/// Retrieve a user's assistants.
		/// </summary>
		/// <param name="userId">The user Id.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>
		/// An array of <see cref="Assistant">assistants</see>.
		/// </returns>
		Task<Assistant[]> GetAssistantsAsync(string userId, CancellationToken cancellationToken = default);

		/// <summary>
		/// Add assistants to a user.
		/// </summary>
		/// <param name="userId">The user Id.</param>
		/// <param name="assistantIds">The id of the assistants to associate with this user.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>
		/// The async task.
		/// </returns>
		Task AddAssistantsByIdAsync(string userId, IEnumerable<string> assistantIds, CancellationToken cancellationToken = default);

		/// <summary>
		/// Add assistants to a user.
		/// </summary>
		/// <param name="userId">The user Id.</param>
		/// <param name="assistantEmails">The email address of the assistants to associate with this user.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>
		/// The async task.
		/// </returns>
		Task AddAssistantsByEmailAsync(string userId, IEnumerable<string> assistantEmails, CancellationToken cancellationToken = default);

		/// <summary>
		/// Delete a specific assistant of a user.
		/// </summary>
		/// <param name="userId">The user Id.</param>
		/// <param name="assistantId">The id of the assistant to disassociate from this user.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>
		/// The async task.
		/// </returns>
		Task DeleteAssistantAsync(string userId, string assistantId, CancellationToken cancellationToken = default);

		/// <summary>
		/// Delete all assistants of a user.
		/// </summary>
		/// <param name="userId">The user Id.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>
		/// The async task.
		/// </returns>
		Task DeleteAllAssistantsAsync(string userId, CancellationToken cancellationToken = default);

		/// <summary>
		/// Retrieve a user's schedulers.
		/// </summary>
		/// <param name="userId">The user Id.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>
		/// An array of <see cref="Assistant">schedulers</see>.
		/// </returns>
		Task<Assistant[]> GetSchedulersAsync(string userId, CancellationToken cancellationToken = default);

		/// <summary>
		/// Delete a specific scheduler of a user.
		/// </summary>
		/// <param name="userId">The user Id.</param>
		/// <param name="assistantId">The id of the scheduler to disassociate from this user.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>
		/// The async task.
		/// </returns>
		Task DeleteSchedulerAsync(string userId, string assistantId, CancellationToken cancellationToken = default);

		/// <summary>
		/// Delete all schedulers of a user.
		/// </summary>
		/// <param name="userId">The user Id.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>
		/// The async task.
		/// </returns>
		Task DeleteAllSchedulersAsync(string userId, CancellationToken cancellationToken = default);

		/// <summary>
		/// Upload a user’s profile picture.
		/// </summary>
		/// <param name="userId">The user Id.</param>
		/// <param name="fileName">The file name.</param>
		/// <param name="pictureData">The binary data.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>
		/// The async task.
		/// </returns>
		Task UploadProfilePicture(string userId, string fileName, Stream pictureData, CancellationToken cancellationToken = default);

		/// <summary>
		/// Retrieve a user's settings.
		/// </summary>
		/// <param name="userId">The user Id.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>
		/// The <see cref="UserSettings">settings</see>.
		/// </returns>
		Task<UserSettings> GetSettingsAsync(string userId, CancellationToken cancellationToken = default);

		/// <summary>
		/// Retrieve a user's meeting authentication settings.
		/// </summary>
		/// <param name="userId">The user Id.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>
		/// The <see cref="AuthenticationSettings">settings</see>.
		/// </returns>
		Task<AuthenticationSettings> GetMeetingAuthenticationSettingsAsync(string userId, CancellationToken cancellationToken = default);

		/// <summary>
		/// Retrieve a user's recording authentication settings.
		/// </summary>
		/// <param name="userId">The user Id.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>
		/// The <see cref="AuthenticationSettings">settings</see>.
		/// </returns>
		Task<AuthenticationSettings> GetRecordingAuthenticationSettingsAsync(string userId, CancellationToken cancellationToken = default);

		/// <summary>
		/// Retrieve a user's security settings.
		/// </summary>
		/// <param name="userId">The user Id.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>
		/// The <see cref="SecuritySettings">settings</see>.
		/// </returns>
		Task<SecuritySettings> GetSecuritySettingsAsync(string userId, CancellationToken cancellationToken = default);

		/// <summary>
		/// Deactivate a specific user on a Zoom account.
		/// </summary>
		/// <param name="userId">The user Id or email address.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>
		/// The async task.
		/// </returns>
		Task DeactivateAsync(string userId, CancellationToken cancellationToken = default);

		/// <summary>
		/// Reactivate a specific user on a Zoom account.
		/// </summary>
		/// <param name="userId">The user Id or email address.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>
		/// The async task.
		/// </returns>
		Task ReactivateAsync(string userId, CancellationToken cancellationToken = default);

		/// <summary>
		/// Change the password of a user.
		/// </summary>
		/// <param name="userId">The user Id or email address.</param>
		/// <param name="password">The password.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>
		/// The async task.
		/// </returns>
		Task ChangePasswordAsync(string userId, string password, CancellationToken cancellationToken = default);

		/// <summary>
		/// Retrieve the permissions that have been granted to a user.
		/// </summary>
		/// <param name="userId">The user Id or email address.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>
		/// An array of strings.
		/// </returns>
		Task<string[]> GetPermissionsAsync(string userId, CancellationToken cancellationToken = default);

		/// <summary>
		/// Revoke a user's SSO token.
		/// </summary>
		/// <param name="userId">The user Id or email address.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>
		/// The async task.
		/// </returns>
		Task RevokeSsoTokenAsync(string userId, CancellationToken cancellationToken = default);

		/// <summary>
		/// Verify if a user's email is registered with Zoom.
		/// </summary>
		/// <param name="email">The email address.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>
		/// true if the email is registered to a user within your account.
		/// </returns>
		Task<bool> CheckEmailInUseAsync(string email, CancellationToken cancellationToken = default);

		/// <summary>
		/// Change a user's email address on a Zoom account that has managed domain set up.
		/// </summary>
		/// <param name="userId">The user Id or email address.</param>
		/// <param name="email">The email address.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>
		/// The async task.
		/// </returns>
		Task ChangeEmailAsync(string userId, string email, CancellationToken cancellationToken = default);

		/// <summary>
		/// Verify if a personal meeting room with the given name exists or not.
		/// </summary>
		/// <param name="name">The room name.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>
		/// true if a room with the given name exists.
		/// </returns>
		Task<bool> CheckPersonalMeetingRoomNameInUseAsync(string name, CancellationToken cancellationToken = default);

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
		Task SwitchAccountAsync(string userId, string currentAccountId, string newAccountId, CancellationToken cancellationToken = default);
	}
}
