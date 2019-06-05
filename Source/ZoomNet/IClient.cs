namespace ZoomNet
{
	/// <summary>
	/// Interface for the Zoom REST client.
	/// </summary>
	public interface IClient
	{
		/// <summary>
		/// Gets the resource which allows you to manage sub accounts.
		/// </summary>
		/// <value>
		/// The accounts resource.
		/// </value>
		//IAccounts Accounts { get; }

		/// <summary>
		/// Gets the resource which allows you to manage billing information.
		/// </summary>
		/// <value>
		/// The billing resource.
		/// </value>
		//IBillingInformation BillingInformation { get; }

		/// <summary>
		/// Gets the resource which allows you to manage users.
		/// </summary>
		/// <value>
		/// The users resource.
		/// </value>
		//IUsers Users { get; }

		/// <summary>
		/// Gets the resource wich alloes you to manage roles.
		/// </summary>
		/// <value>
		/// The roles resource.
		/// </value>
		//IRoles Roles { get; }

		/// <summary>
		/// Gets the resource which allows you to manage meetings.
		/// </summary>
		/// <value>
		/// The meetings resource.
		/// </value>
		//IMeetings Meetings { get; }

		/// <summary>
		/// Gets the resource which allows you to manage webinars.
		/// </summary>
		/// <value>
		/// The webinars resource.
		/// </value>
		//IWebinars Webinars { get; }
	}
}
