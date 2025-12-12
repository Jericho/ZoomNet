using System.Text.Json.Serialization;

namespace ZoomNet.Models.PhoneAccountSettings
{
	/// <summary>
	/// SMS etiquette policy.
	/// </summary>
	public class SmsEtiquettePolicy
	{
		/// <summary>
		/// Gets or sets policy id.
		/// </summary>
		[JsonPropertyName("id")]
		public string Id { get; set; }

		/// <summary>
		/// Gets or sets policy name.
		/// </summary>
		[JsonPropertyName("name")]
		public string Name { get; set; }

		/// <summary>
		/// Gets or sets policy description.
		/// </summary>
		[JsonPropertyName("description")]
		public string Description { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether the policy is active.
		/// </summary>
		[JsonPropertyName("active")]
		public bool Active { get; set; }

		/// <summary>
		/// Gets or sets policy rule kind.
		/// </summary>
		[JsonPropertyName("rule")]
		public SmsEtiquettePolicyRuleKind Rule { get; set; }

		/// <summary>
		/// Gets or sets policy content.
		/// </summary>
		/// <remarks>
		/// For <see cref="SmsEtiquettePolicyRuleKind.Keywords"/> - keywords separated by comma, the following characters are supported A-Z, a-z, 0-9.
		/// For <see cref="SmsEtiquettePolicyRuleKind.RegularExpression"/> - regular expression (back references and zero-width assertions are not supported).
		/// </remarks>
		[JsonPropertyName("content")]
		public string Content { get; set; }

		/// <summary>
		/// Gets or sets the action taken when a policy is triggered.
		/// </summary>
		[JsonPropertyName("action")]
		public SmsEtiquettePolicyAction Action { get; set; }
	}
}
