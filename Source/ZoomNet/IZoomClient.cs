using System;
using System.Collections.Generic;
using ZoomNet.Resources;

namespace ZoomNet
{
	/// <summary>
	/// Interface for the Zoom REST client.
	/// </summary>
	public interface IZoomClient
	{
		/// <summary>
		/// Gets the resource which allows you to manage sub accounts.
		/// </summary>
		IAccounts Accounts { get; }

		/// <summary>
		/// Gets the resource which allows developers with master accounts (also known as "primary accounts") to get information about billing plans of their accounts and subaccounts.
		/// </summary>
		IBilling Billing { get; }

		/// <summary>
		/// Gets the resource which allows you to manage call history.
		/// </summary>
		ICallHistory CallHistory { get; }

		/// <summary>
		/// Gets the resource which allows you to manage call logs.
		/// </summary>
		[Obsolete("Zoom will sunset the CallLogs API endpoints on fully deprecated and sunset on May 30, 2026. Please use the CallHistory instead. For more information, please refer to https://developers.zoom.us/docs/phone/migrate/")]
		ICallLogs CallLogs { get; }

		/// <summary>
		/// Gets the resource which allows you to manage chat channels, messages, etc.
		/// </summary>
		IChat Chat { get; }

		/// <summary>
		/// Gets the resource which allows you to manage chatbot messages.
		/// </summary>
		IChatbot Chatbot { get; }

		/// <summary>
		/// Gets the resource which allows you to manage cloud recordings.
		/// </summary>
		ICloudRecordings CloudRecordings { get; }

		/// <summary>
		/// Gets the resource which allows you to manage contacts.
		/// </summary>
		/// <value>
		/// The contacts resource.
		/// </value>
		IContacts Contacts { get; }

		/// <summary>
		/// Gets the resource which allows you to view metrics.
		/// </summary>
		IDashboards Dashboards { get; }

		/// <summary>
		/// Gets the resource which allows you to manage events.
		/// </summary>
		IEvents Events { get; }

		/// <summary>
		/// Gets the resource which allows you to handle Zoom phone external contacts.
		/// </summary>
		IExternalContacts ExternalContacts { get; }

		/// <summary>
		/// Gets the resource which allows you to manage groups under an account.
		/// </summary>
		IGroups Groups { get; }

		/// <summary>
		/// Gets the resource which allows you to manage meetings.
		/// </summary>
		IMeetings Meetings { get; }

		/// <summary>
		/// Gets the resource which allows you to manage meetings that occured in the past.
		/// </summary>
		IPastMeetings PastMeetings { get; }

		/// <summary>
		/// Gets the resource which allows you to manage webinars that occured in the past.
		/// </summary>
		IPastWebinars PastWebinars { get; }

		/// <summary>
		/// Gets the resource which allows you to access Zoom Phone API endpoints.
		/// </summary>
		IPhone Phone { get; }

		/// <summary>
		/// Gets the resource which allows you to view reports.
		/// </summary>
		IReports Reports { get; }

		/// <summary>
		/// Gets the resource which allows you to manage roles.
		/// </summary>
		IRoles Roles { get; }

		/// <summary>
		/// Gets the resource which allows you to manage rooms.
		/// </summary>
		IRooms Rooms { get; }

		/// <summary>
		/// Gets the resource which allows you to manage SMS messages and sessions.
		/// </summary>
		ISms Sms { get; }

		/// <summary>
		/// Gets the resource which allows you to manage tracking fields.
		/// </summary>
		ITrackingFields TrackingFields { get; }

		/// <summary>
		/// Gets the resource which allows you to manage users.
		/// </summary>
		IUsers Users { get; }

		/// <summary>
		/// Gets the resource which allows you to manage webinars.
		/// </summary>
		IWebinars Webinars { get; }

		/// <summary>
		/// Determines if the specified scopes have been granted.
		/// </summary>
		/// <param name="scopes">The name of the scopes.</param>
		/// <returns>True if all the scopes have been granted, False otherwise.</returns>
		/// <remarks>
		/// The concept of "scopes" only applies to OAuth connections.
		/// Therefore an exception will be thrown if you invoke this method while using
		/// a JWT connection (you shouldn't be using JWT in the first place since this
		/// type of connection has been deprecated in the Zoom API since September 2023).
		/// </remarks>
		bool HasPermissions(IEnumerable<string> scopes);
	}
}
