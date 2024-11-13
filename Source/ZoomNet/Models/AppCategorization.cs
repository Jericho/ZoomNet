namespace ZoomNet.Models
{
	/// <summary>
	/// Enumeration to indicate the usage categorization of an app.
	/// </summary>
	public enum AppCategorization
	{
		/// <summary>Unknown.</summary>
		Unknown = 0,

		/// <summary>The app is under admin management.</summary>
		UnderAdminManagement = 1,

		/// <summary>Users have access to the app.</summary>
		Normal = 2,
	}
}
