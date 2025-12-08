namespace ZoomNet.Models.PhoneAccountSettings
{
	/// <summary>
	/// SMS etiquette policy rule kinds.
	/// </summary>
	public enum SmsEtiquettePolicyRuleKind
	{
		/// <summary>
		/// SMS etiquette policy rule is based on specific keywords.
		/// </summary>
		Keywords = 1,

		/// <summary>
		/// SMS etiquette policy rule is based on regular expression.
		/// </summary>
		RegularExpression = 2,
	}
}
