#if NETFRAMEWORK || NETSTANDARD
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

#pragma warning disable IDE0130 // Namespace does not match folder structure
namespace System
#pragma warning restore IDE0130 // Namespace does not match folder structure
{
	/// <summary>
	/// Extension methods for <see cref="ArgumentNullException"/>.
	/// </summary>
	internal static class ArgumentNullExceptionExtensions
	{
		// Use the new C# 14 'extension' feature to add static methods to ArgumentNullException
		extension(ArgumentNullException)
		{
			/// <summary>
			/// Throws an exception if the specified argument is null.
			/// </summary>
			/// <param name="argument">The object to validate for null. If this value is null, an <see cref="ArgumentNullException"/> is thrown.</param>
			/// <param name="paramName">The name of the parameter to include in the exception message. If not specified, the caller's argument expression
			/// is used.</param>
			/// <param name="message">An optional custom message to include in the exception. If null, the default exception message is used.</param>
			/// <exception cref="ArgumentNullException">Thrown when <paramref name="argument"/> is null.</exception>
			public static void ThrowIfNull(object argument, [CallerArgumentExpression(nameof(argument))] string paramName = null, string message = null)
			{
				if (argument is null)
					throw new ArgumentNullException(paramName, message);
			}

			/// <summary>
			/// Throws an exception if the specified string argument is null or empty.
			/// </summary>
			/// <remarks>Use this method to enforce that a string parameter is not null or empty before proceeding with
			/// further logic. This is typically used at the start of a method to validate input arguments.</remarks>
			/// <param name="argument">The string value to validate. Cannot be null or empty.</param>
			/// <param name="paramName">The name of the parameter being validated. If not specified, the caller's argument expression is used.</param>
			/// <param name="message">An optional custom error message to include in the exception. If null, a default message is used.</param>
			/// <exception cref="ArgumentNullException">Thrown if <paramref name="argument"/> is null or an empty string.</exception>
			public static void ThrowIfEmpty(string argument, [CallerArgumentExpression(nameof(argument))] string paramName = null, string message = null)
			{
				if (string.IsNullOrEmpty(argument))
					throw new ArgumentNullException(paramName, message);
			}

			/// <summary>
			/// Throws an exception if the specified collection is null or contains no elements.
			/// </summary>
			/// <remarks>Use this method to validate that a collection argument is not null and contains at least one
			/// element before proceeding with further operations.</remarks>
			/// <typeparam name="T">The type of elements in the collection to validate.</typeparam>
			/// <param name="argument">The collection to check for null or emptiness.</param>
			/// <param name="paramName">The name of the parameter to include in the exception message. If not specified, the caller's argument expression
			/// is used.</param>
			/// <param name="message">An optional custom message to include in the exception if thrown.</param>
			/// <exception cref="ArgumentNullException">Thrown if <paramref name="argument"/> is null or contains no elements.</exception>
			public static void ThrowIfEmpty<T>(IEnumerable<T> argument, [CallerArgumentExpression(nameof(argument))] string paramName = null, string message = null)
			{
				if (argument is null || !argument.Any())
					throw new ArgumentNullException(paramName, message);
			}
		}
	}
}
#endif
