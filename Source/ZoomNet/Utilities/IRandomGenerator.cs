namespace ZoomNet.Utilities
{
	/// <summary>
	/// Defines methods for generating random values, including integers, strings, and byte arrays.
	/// </summary>
	public interface IRandomGenerator
	{
		/// <summary>
		/// Generates a random integer between two values.
		/// </summary>
		/// <param name="minValueInclusive">The minimum value.</param>
		/// <param name="maxValueExclusive">The maximum value.</param>
		/// <returns>A random value.</returns>
		int GetInt32(int minValueInclusive, int maxValueExclusive);

		/// <summary>
		/// Generates a random string of the specified length using the provided set of allowable characters.
		/// </summary>
		/// <param name="length">The number of characters to include in the generated string. Must be a positive integer; if zero or negative, an
		/// empty string is returned.</param>
		/// <param name="allowableCharacters">A string containing the characters that can be used in the generated string. If not specified, lowercase letters
		/// and digits are used by default.</param>
		/// <returns>A randomly generated string composed of characters from the specified allowable set.</returns>
		string GenerateString(int length, string allowableCharacters = "abcdefghijklmnopqrstuvwxyz0123456789");

		/// <summary>
		/// Generates a cryptographically secure random salt of the specified length.
		/// </summary>
		/// <remarks>This method is useful for creating salts for password hashing or other cryptographic operations.
		/// Ensure that the length is appropriate for the intended security requirements.</remarks>
		/// <param name="length">The length of the salt to generate, specified in bytes. Must be a positive integer.</param>
		/// <returns>A byte array containing the generated salt. The length of the array is equal to the specified 'length' parameter.</returns>
		byte[] GenerateSalt(int length);

		/// <summary>
		/// Generates a cryptographically secure random salt string of the specified length.
		/// </summary>
		/// <remarks>The generated salt enhances security by ensuring that identical inputs produce different hashes.
		/// It is recommended to use a unique salt for each password or secret.</remarks>
		/// <param name="length">The length, in characters, of the salt string to generate. Must be a positive integer.</param>
		/// <returns>A string containing the generated salt, suitable for use in cryptographic operations such as password hashing.</returns>
		string GenerateSaltString(int length);
	}
}
