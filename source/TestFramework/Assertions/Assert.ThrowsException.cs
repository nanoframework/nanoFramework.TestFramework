// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using TestFrameworkShared;

#nullable enable

namespace nanoFramework.TestFramework
{
    /// <summary>
    /// A collection of helper classes to test various conditions within unit tests. If the condition being tested is not met, an exception is thrown.
    /// </summary>
    public sealed partial class Assert
    {
        /// <summary>
        /// Asserts that the delegate <paramref name="action"/> throws an exception of type <typeparamref name="TException"/>
        /// (or derived type) and throws <c>AssertFailedException</c> if code does not throw exception or throws
        /// exception of type other than <typeparamref name="TException"/>.
        /// </summary>
        /// <param name="action">
        /// Delegate to code to be tested and which is expected to throw exception.
        /// </param>
        /// <param name="messageBuilder">
        /// A func that takes the thrown Exception (or null if the action didn't throw any exception) to construct the message to include in the exception when <paramref name="action"/> does not throw exception of type <typeparamref name="TException"/>.
        /// </param>
        /// <param name="actionExpression">
        /// The syntactic expression of action as given by the compiler via caller argument expression.
        /// Users shouldn't pass a value for this parameter.
        /// </param>
        /// <typeparam name="TException">
        /// The type of exception expected to be thrown.
        /// </typeparam>
        /// <exception cref="AssertFailedException">
        /// Thrown if <paramref name="action"/> does not throw exception of type <typeparamref name="TException"/>.
        /// </exception>
        /// <returns>
        /// The exception that was thrown.
        /// </returns>
        public static TException Throws<TException>(Action action, Func<Exception?, string> messageBuilder, [CallerArgumentExpression(nameof(action))] string actionExpression = "")
            where TException : Exception
            => ThrowsException<TException>(action, isStrictType: false, messageBuilder, actionExpression);

        /// <inheritdoc cref="Throws{TException}(Action, Func{Exception?, string}, string)"/>
        public static TException Throws<TException>(Func<object?> action, Func<Exception?, string> messageBuilder, [CallerArgumentExpression(nameof(action))] string actionExpression = "")
            where TException : Exception
            => ThrowsException<TException>(() => _ = action(), isStrictType: false, messageBuilder, actionExpression);

        /// <summary>
        /// Asserts that the delegate <paramref name="action"/> throws an exception of type <typeparamref name="TException"/>
        /// (and not of derived type) and throws <c>AssertFailedException</c> if code does not throw exception or throws
        /// exception of type other than <typeparamref name="TException"/>.
        /// </summary>
        /// <param name="action">
        /// Delegate to code to be tested and which is expected to throw exception.
        /// </param>
        /// <param name="message">
        /// The message to include in the exception when <paramref name="action"/> does not throw exception of type <typeparamref name="TException"/>.
        /// </param>
        /// <param name="actionExpression">
        /// The syntactic expression of action as given by the compiler via caller argument expression.
        /// Users shouldn't pass a value for this parameter.
        /// </param>
        /// <typeparam name="TException">
        /// The type of exception expected to be thrown.
        /// </typeparam>
        /// <exception cref="AssertFailedException">
        /// Thrown if <paramref name="action"/> does not throw exception of type <typeparamref name="TException"/>.
        /// </exception>
        /// <returns>
        /// The exception that was thrown.
        /// </returns>
        public static TException ThrowsExactly<TException>(Action action, string? message = "", [CallerArgumentExpression(nameof(action))] string actionExpression = "")
            where TException : Exception
            => ThrowsException<TException>(action, isStrictType: true, message, actionExpression);

        /// <inheritdoc cref="ThrowsExactly{TException}(Action, string, string)" />
        public static TException ThrowsExactly<TException>(Func<object?> action, string? message = "", [CallerArgumentExpression(nameof(action))] string actionExpression = "")
            where TException : Exception
            => ThrowsException<TException>(() => _ = action(), isStrictType: true, message, actionExpression);

        /// <summary>
        /// Asserts that the delegate <paramref name="action"/> throws an exception of type <typeparamref name="TException"/>
        /// (and not of derived type) and throws <c>AssertFailedException</c> if code does not throw exception or throws
        /// exception of type other than <typeparamref name="TException"/>.
        /// </summary>
        /// <param name="action">
        /// Delegate to code to be tested and which is expected to throw exception.
        /// </param>
        /// <param name="messageBuilder">
        /// A func that takes the thrown Exception (or null if the action didn't throw any exception) to construct the message to include in the exception when <paramref name="action"/> does not throw exception of type <typeparamref name="TException"/>.
        /// </param>
        /// <param name="actionExpression">
        /// The syntactic expression of action as given by the compiler via caller argument expression.
        /// Users shouldn't pass a value for this parameter.
        /// </param>
        /// <typeparam name="TException">
        /// The type of exception expected to be thrown.
        /// </typeparam>
        /// <exception cref="AssertFailedException">
        /// Thrown if <paramref name="action"/> does not throw exception of type <typeparamref name="TException"/>.
        /// </exception>
        /// <returns>
        /// The exception that was thrown.
        /// </returns>
        public static TException ThrowsExactly<TException>(Action action, Func<Exception?, string> messageBuilder, [CallerArgumentExpression(nameof(action))] string actionExpression = "")
            where TException : Exception
            => ThrowsException<TException>(action, isStrictType: true, messageBuilder, actionExpression);

        /// <inheritdoc cref="ThrowsExactly{TException}(Action, Func{Exception?, string}, string)" />
        public static TException ThrowsExactly<TException>(Func<object?> action, Func<Exception?, string> messageBuilder, [CallerArgumentExpression(nameof(action))] string actionExpression = "")
        where TException : Exception
            => ThrowsException<TException>(() => _ = action(), isStrictType: true, messageBuilder, actionExpression);

        private static TException ThrowsException<TException>(Action action, bool isStrictType, string? message, string actionExpression, [CallerMemberName] string assertMethodName = "")
            where TException : Exception
        {
            Guard.NotNull(action);
            Guard.NotNull(message);

            ThrowsExceptionState state = IsThrowsFailing<TException>(action, isStrictType, assertMethodName);
            if (state.FailAction is not null)
            {
                state.FailAction(BuildUserMessageForActionExpression(message, actionExpression));
            }
            else
            {
                return (TException)state.ExceptionThrown!;
            }

            // This will not hit, but need it for compiler.
            return null!;
        }

        private static TException ThrowsException<TException>(
            Action action,
            bool isStrictType,
            Func<Exception?, string> messageBuilder,
            string actionExpression,
            [CallerMemberName] string assertMethodName = "")
            where TException : Exception
        {
            Guard.NotNull(action);
            Guard.NotNull(messageBuilder);

            ThrowsExceptionState state = IsThrowsFailing<TException>(action, isStrictType, assertMethodName);
            if (state.FailAction is not null)
            {
                state.FailAction(BuildUserMessageForActionExpression(messageBuilder(state.ExceptionThrown), actionExpression));
            }
            else
            {
                return (TException)state.ExceptionThrown!;
            }

            // This will not hit, but need it for compiler.
            return null!;
        }

        private static ThrowsExceptionState IsThrowsFailing<TException>(Action action, bool isStrictType, string assertMethodName)
            where TException : Exception
        {
            try
            {
                action();
            }
            catch (Exception ex)
            {
                bool isExceptionOfType = isStrictType
                    ? typeof(TException) == ex.GetType()
                    : ex is TException;

                return isExceptionOfType
                    ? ThrowsExceptionState.CreateNotFailingState(ex)
                    : ThrowsExceptionState.CreateFailingState(
                        userMessage =>
                        {
                            string finalMessage = string.Format(
                                "Expected exception type:&lt;{1}&gt;. Actual exception type:&lt;{2}&gt;. {0}",
                                userMessage,
                                typeof(TException),
                                ex.GetType());
                            ThrowAssertFailed("Assert." + assertMethodName, finalMessage, actual: ex);
                        }, ex);
            }

            return ThrowsExceptionState.CreateFailingState(
                failAction: userMessage =>
                {
                    string finalMessage = string.Format(
                        "Expected exception type:&lt;{1}&gt; but no exception was thrown. {0}",
                        userMessage,
                        typeof(TException));
                    ThrowAssertFailed("Assert." + assertMethodName, finalMessage);
                }, null);
        }

        private readonly struct ThrowsExceptionState
        {
            public Exception? ExceptionThrown { get; }

            public Action<string>? FailAction { get; }

            private ThrowsExceptionState(Exception? exceptionThrown, Action<string>? failAction)
            {
                // If the assert is failing, failAction should be non-null, and exceptionWhenNotFailing may or may not be null.
                // If the assert is not failing, exceptionWhenNotFailing should be non-null, and failAction should be null.
                ExceptionThrown = exceptionThrown;
                FailAction = failAction;
            }

            public static ThrowsExceptionState CreateFailingState(Action<string> failAction, Exception? exceptionThrown)
                => new(exceptionThrown, failAction);

            public static ThrowsExceptionState CreateNotFailingState(Exception exception)
                => new(exception, failAction: null);
        }
    }
}
