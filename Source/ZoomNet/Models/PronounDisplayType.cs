namespace ZoomNet.Models
{
	/// <summary>
	/// Enumeration to indicate the user's display pronouns setting.
	/// </summary>
	public enum PronounDisplayType
	{
		/// <summary>Ask the user every time they join meetings and webinars.</summary>
		Ask = 1,

		/// <summary>Always display pronouns in meetings and webinars.</summary>
		Display = 2,

		/// <summary>Do not display pronouns in meetings and webinars.</summary>
		DoNotDisplay = 3,
	}
}
