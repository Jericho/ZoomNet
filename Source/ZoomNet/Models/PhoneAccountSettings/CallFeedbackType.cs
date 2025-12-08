namespace ZoomNet.Models.PhoneAccountSettings
{
	/// <summary>
	/// Call feedback type.
	/// </summary>
	public enum CallFeedbackType
	{
		/// <summary>
		/// Display call feedback survey for every call.
		/// </summary>
		EveryCall = 1,

		/// <summary>
		/// Display call feedback survey when quality issues are detected.
		/// </summary>
		CallsWithQualityIssues = 2,
	}
}
