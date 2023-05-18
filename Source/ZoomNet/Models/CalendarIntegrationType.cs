namespace ZoomNet.Models
{
	/// <summary>
	/// Type of calendar integration used to schedule the meeting.
	/// </summary>
	public enum CalendarIntegrationType
	{
		/// <summary>
		/// <a href="https://support.zoom.us/hc/en-us/articles/360031592971-Getting-started-with-Outlook-plugin-and-add-in">Zoom Outlook add-in</a>.
		/// </summary>
		Outlook = 1,

		/// <summary>
		/// <a href="https://support.zoom.us/hc/en-us/articles/360020187492-Using-the-Zoom-for-Google-Workspace-add-on">Zoom for Google Workspace add-on</a>.
		/// </summary>
		GoogleWorkspace = 2,
	}
}
