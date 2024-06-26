using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

// This file is intended to be used by components that don't have access to ArgumentNullException.ThrowIfNull.
#pragma warning disable CS0436 // Type conflicts with imported type

namespace ZoomNet.Utilities.Log
{
	internal class ThrowHelper
	{
		/// <summary>Throws an <see cref="ArgumentNullException"/> if <paramref name="argument"/> is null.</summary>
		/// <param name="argument">The reference type argument to validate as non-null.</param>
		/// <param name="paramName">The name of the parameter with which <paramref name="argument"/> corresponds.</param>
		/// <remarks>From the Microsoft.Extensions.Logging.Abstractions project.</remarks>
		public static void ThrowIfNull(
#if NET
			[NotNull]
#endif
			object argument,
			[CallerArgumentExpression(nameof(argument))] string paramName = null)
		{
			if (argument is null)
			{
				Throw(paramName);
			}
		}

#if NET
		[DoesNotReturn]
#endif
		public static void Throw(string paramName) => throw new ArgumentNullException(paramName);

		/// <summary>
		/// Throws either an <see cref="System.ArgumentNullException"/> or an <see cref="System.ArgumentException"/>
		/// if the specified string is <see langword="null"/> or whitespace respectively.
		/// </summary>
		/// <param name="argument">String to be checked for <see langword="null"/> or whitespace.</param>
		/// <param name="paramName">The name of the parameter being checked.</param>
		/// <returns>The original value of <paramref name="argument"/>.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#if NET
		[return: NotNull]
#endif
		public static string IfNullOrWhitespace(
#if NET
			[NotNull]
#endif
			string argument,
			[CallerArgumentExpression(nameof(argument))] string paramName = "")
		{
#if !NET
			if (argument == null)
			{
				throw new ArgumentNullException(paramName);
			}
#endif

			if (string.IsNullOrWhiteSpace(argument))
			{
				if (argument == null)
				{
					throw new ArgumentNullException(paramName);
				}
				else
				{
					throw new ArgumentException(paramName, "Argument is whitespace");
				}
			}

			return argument;
		}
	}
}
