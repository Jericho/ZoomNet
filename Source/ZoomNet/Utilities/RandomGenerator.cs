using System;
using System.Security.Cryptography;

namespace ZoomNet.Utilities
{
	/// <summary>
	/// Random generator.
	/// </summary>
	public static class RandomGenerator
	{
		#region PUBLIC METHODS

		/// <summary>
		/// This method simulates a roll of the dice. The input parameter is the
		/// number of sides of the dice.
		/// </summary>
		/// <param name="numberSides">Number of sides of the dice.</param>
		/// <returns>A random value.</returns>
		/// <remarks>
		/// From RNGCryptoServiceProvider <a href="https://msdn.microsoft.com/en-us/library/system.security.cryptography.rngcryptoserviceprovider.aspx">documentation</a>.
		/// </remarks>
		public static byte RollDice(byte numberSides)
		{
			if (numberSides <= 0)
				throw new ArgumentOutOfRangeException(nameof(numberSides));

			// Create a byte array to hold the random value.
			byte[] randomNumber = new byte[1];
			using (var random = RandomNumberGenerator.Create())
			{
				do
				{
					// Fill the array with a random value.
					random.GetBytes(randomNumber);
				}
				while (!IsFairRoll(randomNumber[0], numberSides));
			}

			// Return the random number mod the number of sides.
			// The possible values are zero-based, so we add one.
			return (byte)((randomNumber[0] % numberSides) + 1);
		}

		#endregion

		#region PRIVATE METHODS

		private static bool IsFairRoll(byte roll, byte numSides)
		{
			// There are MaxValue / numSides full sets of numbers that can come up
			// in a single byte. For instance, if we have a 6 sided die, there are
			// 42 full sets of 1-6 that come up. The 43rd set is incomplete.
			int fullSetsOfValues = byte.MaxValue / numSides;

			// If the roll is within this range of fair values, then we let it continue.
			// In the 6 sided die case, a roll between 0 and 251 is allowed. (We use
			// < rather than <= since the = portion allows through an extra 0 value).
			// 252 through 255 would provide an extra 0, 1, 2, 3 so they are not fair
			// to use.
			return roll < numSides * fullSetsOfValues;
		}

		#endregion
	}
}
