using System.Threading;
using System.Threading.Tasks;
using ZoomNet.Models.Webhooks;

namespace ZoomNet.Resources
{
	/// <summary>
	/// Allows you to notify Zoom that you comply with the policy which requires you to handle
	/// user's data in accordance to the user's preference after the user uninstalls your app.
	/// </summary>
	/// <remarks>
	/// This resource can only be used when you connect to Zoom using OAuth. It cannot be used with a Jwt connection.
	/// See <a href="https://marketplace.zoom.us/docs/api-reference/data-compliance/">Zoom documentation</a> for more information.
	/// </remarks>
	public interface IDataCompliance
	{
		/// <summary>
		/// Notify Zoom that you comply with the policy which requires you to handle user's
		/// data in accordance to the user's preference after the user uninstalls your app.
		/// </summary>
		/// <param name="userId">The Zoom user's id who uninstalled your app.</param>
		/// <param name="accountId">The account Id.</param>
		/// <param name="deauthorizationEventReceived">This object represents the payload of the webhook event sent by Zoom in your Deauthorization Endpoint Url after a user uninstalls your app. The values of the parameters in this object should be the same as that of the deauthorization event that you receive.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>
		/// The async task.
		/// </returns>
		Task NotifyAsync(string userId, long accountId, AppDeauthorizedEvent deauthorizationEventReceived, CancellationToken cancellationToken = default);
	}
}
