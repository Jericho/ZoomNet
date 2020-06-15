using System.Threading;
using System.Threading.Tasks;
using ZoomNet.Models;
using ZoomNet.Resources;

namespace ZoomNet
{
	/// <summary>
	/// Public extension methods.
	/// </summary>
	public static class Public
	{
		/// <summary>
		/// Returns information about the current user.
		/// </summary>
		/// <param name="usersResource">The user resource.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>The current user.</returns>
		public static Task<User> GetCurrentAsync(this IUsers usersResource, CancellationToken cancellationToken)
		{
			return usersResource.GetAsync("me", cancellationToken);
		}

		/// <summary>
		/// Add an assistant to a user.
		/// </summary>
		/// <param name="usersResource">The user resource.</param>
		/// <param name="userId">The user Id.</param>
		/// <param name="assistantId">The id of the assistant to associate with this user.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>
		/// The async task.
		/// </returns>
		public static Task AddAssistantByIdAsync(this IUsers usersResource, string userId, string assistantId, CancellationToken cancellationToken = default)
		{
			return usersResource.AddAssistantsByIdAsync(userId, new[] { assistantId }, cancellationToken);
		}

		/// <summary>
		/// Add an assistant to a user.
		/// </summary>
		/// <param name="usersResource">The user resource.</param>
		/// <param name="userId">The user Id.</param>
		/// <param name="assistantEmail">The email address of the assistant to associate with this user.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>
		/// The async task.
		/// </returns>
		public static Task AddAssistantByEmailAsync(this IUsers usersResource, string userId, string assistantEmail, CancellationToken cancellationToken = default)
		{
			return usersResource.AddAssistantsByIdAsync(userId, new[] { assistantEmail }, cancellationToken);
		}
	}
}
