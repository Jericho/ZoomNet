using System.Text.Json.Serialization;

namespace ZoomNet.Models
{
	/// <summary>Password requirements.</summary>
	public record PasswordRequirements
	{
		/// <summary>Gets or sets a value indicating whether password must contain at least one alphabetical letter.</summary>
		[JsonPropertyName("have_letter")]
		public bool AtLeastOneLetter { get; init; }

		/// <summary>Gets or sets a value indicating whether password must contain at least one number.</summary>
		[JsonPropertyName("have_number")]
		public bool AtLeastOneNumber { get; init; }

		/// <summary>Gets or sets a value indicating whether password must contain at least one special character.</summary>
		[JsonPropertyName("have_special_character")]
		public bool AtLeastOneSpecialCharacter { get; init; }

		/// <summary>Gets or sets the minimum required length.</summary>
		[JsonPropertyName("length")]
		public int MinimumLength { get; init; }

		/// <summary>Gets or sets a value indicating whether password must contain only numeric characters.</summary>
		[JsonPropertyName("only_allow_numeric")]
		public bool MustOnlyContainNumbers { get; init; }
	}
}
