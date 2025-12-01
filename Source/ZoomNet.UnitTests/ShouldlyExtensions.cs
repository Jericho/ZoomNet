using Shouldly;
using System;
using System.Threading.Tasks;

namespace ZoomNet.UnitTests
{
	// Inspired by : https://github.com/GitTools/GitVersion/blob/main/src/GitVersion.Core.Tests/Extensions/ShouldlyExtensions.cs
	internal static class ShouldlyExtensions
	{
		/// <summary>
		/// Asserts that the specified action throws an exception of the given type and returns the thrown exception instance.
		/// </summary>
		/// <remarks>Use this method in unit tests to verify that a specific exception type is thrown by an action. If
		/// the action does not throw the expected exception, the assertion will fail.</remarks>
		/// <typeparam name="TException">The type of exception expected to be thrown by the action.</typeparam>
		/// <param name="action">The action to execute and test for the expected exception. Cannot be null.</param>
		/// <returns>The exception instance of type TException that was thrown by the action.</returns>
		public static Exception ShouldThrow<TException>(this Action action) where TException : Exception
		{
			return Should.Throw<TException>(action);
		}

		/// <summary>
		/// Verifies that the specified action throws an exception of type TException with the expected message.
		/// </summary>
		/// <remarks>This method asserts that the action throws an exception of the specified type and that the
		/// exception's message matches the expected value. If the action does not throw the expected exception or the message
		/// does not match, an assertion failure will occur.</remarks>
		/// <typeparam name="TException">The type of exception that is expected to be thrown by the action.</typeparam>
		/// <param name="action">The action to execute and verify for the expected exception and message.</param>
		/// <param name="expectedMessage">The exact exception message that is expected to be thrown by the action.</param>
		/// <returns>The exception of type TException that was thrown by the action.</returns>
		public static Exception ShouldThrowWithMessage<TException>(this Action action, string expectedMessage) where TException : Exception
		{
			var e = Should.Throw<TException>(action);
			e.Message.ShouldBe(expectedMessage);
			return e;
		}

		/// <summary>
		/// Asynchronously verifies that the specified task throws an exception of the given type when awaited.
		/// </summary>
		/// <remarks>Use this method in unit tests to assert that an asynchronous operation throws a specific
		/// exception type. If the task does not throw the expected exception, the assertion will fail.</remarks>
		/// <typeparam name="TException">The type of exception expected to be thrown by the task. Must derive from <see cref="Exception"/>.</typeparam>
		/// <param name="task">The task to execute and monitor for the expected exception. The task is awaited and its exception is inspected.</param>
		/// <returns>A task that represents the asynchronous operation. The result contains the exception of type <typeparamref
		/// name="TException"/> that was thrown by the awaited task.</returns>
		public static async Task<TException> ShouldThrowAsync<TException>(Task task) where TException : Exception
		{
			return await Should.ThrowAsync<TException>(() => task);
		}

		/// <summary>
		/// Asynchronously asserts that the specified task throws an exception of type TException with the expected message.
		/// </summary>
		/// <remarks>Throws an assertion exception if the task does not throw an exception of type TException or if
		/// the exception message does not match the expected value.</remarks>
		/// <typeparam name="TException">The type of exception expected to be thrown by the task.</typeparam>
		/// <param name="task">The task to execute and check for the expected exception.</param>
		/// <param name="expectedMessage">The exact exception message expected to be thrown by the task.</param>
		/// <returns>A task that represents the asynchronous operation. The result contains the thrown exception of type TException if
		/// the assertion passes.</returns>
		public static async Task<TException> ShouldThrowWithMessageAsync<TException>(Task task, string expectedMessage) where TException : Exception
		{
			var e = await Should.ThrowAsync<TException>(() => task);
			e.Message.ShouldBe(expectedMessage);
			return e;
		}
	}
}
