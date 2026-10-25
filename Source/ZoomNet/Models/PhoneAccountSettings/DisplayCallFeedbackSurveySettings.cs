using System.Text.Json.Serialization;

namespace ZoomNet.Models.PhoneAccountSettings
{
	/// <summary>Settings that allow to display call feedback at the end of the calls.</summary>
	public record DisplayCallFeedbackSurveySettings : SettingsGroupBase
	{
		/// <summary>Gets or sets feedback duration setting.</summary>
		[JsonPropertyName("feedback_duration")]
		public FeedbackDurationSetting FeedbackDuration { get; init; }

		/// <summary>Gets or sets feedback MOS setting.</summary>
		[JsonPropertyName("feedback_mos")]
		public FeedbackMeanOpinionScoreSetting FeedbackMeanOpinionScore { get; init; }

		/// <summary>Gets or sets feedback survey type.</summary>
		[JsonPropertyName("feedback_type")]
		public CallFeedbackType? FeedbackType { get; init; }
	}
}
