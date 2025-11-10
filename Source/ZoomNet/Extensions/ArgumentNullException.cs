using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

namespace ZoomNet
{
	internal static class ArgumentNullExceptionExtensions
	{
		// Use the new C# 14 'extension' feature to add static methods to ArgumentNullException
		extension(ArgumentNullException)
		{
			public static void ThrowIfNull(object argument, [CallerArgumentExpression(nameof(argument))] string paramName = null, string message = null)
			{
				if (argument is null)
					throw new ArgumentNullException(paramName, message);
			}

			public static void ThrowIfEmpty(string argument, [CallerArgumentExpression(nameof(argument))] string paramName = null, string message = null)
			{
				if (string.IsNullOrEmpty(argument))
					throw new ArgumentNullException(paramName, message);
			}

			public static void ThrowIfEmpty<T>(IEnumerable<T> argument, [CallerArgumentExpression(nameof(argument))] string paramName = null, string message = null)
			{
				if (argument is null || !argument.Any())
					throw new ArgumentNullException(paramName, message);
			}
		}
	}
}
