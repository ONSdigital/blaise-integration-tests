namespace Blaise.Tests.Helpers.Framework
{
    using System;
    using System.Threading;
    using NUnit.Framework;

    public static class FailFastHelper
    {
        private static int _failureCount;
        private static string _firstFailureMessage;
        private static string _firstFailureScenario;

        public static void Reset()
        {
            Interlocked.Exchange(ref _failureCount, 0);
            _firstFailureMessage = null;
            _firstFailureScenario = null;
        }

        public static void RecordFailure(string scenarioName, string stepText, Exception exception)
        {
            if (exception == null)
            {
                return;
            }

            if (Interlocked.CompareExchange(ref _failureCount, 1, 0) == 0)
            {
                _firstFailureScenario = scenarioName ?? "(unknown scenario)";
                _firstFailureMessage = BuildFailureMessage(scenarioName, stepText, exception);
                Console.WriteLine(_firstFailureMessage);
            }
        }

        public static void ThrowIfPreviousFailed(string scenarioName)
        {
            if (Interlocked.CompareExchange(ref _failureCount, 0, 0) > 0)
            {
                var reason = string.IsNullOrWhiteSpace(_firstFailureMessage)
                    ? "A previous scenario failed."
                    : _firstFailureMessage;

                var scenarioDisplay = string.IsNullOrWhiteSpace(scenarioName)
                    ? "(unknown scenario)"
                    : scenarioName;

                Console.WriteLine($"Fail-fast: skipping scenario '{scenarioDisplay}'. Previous failure: {reason}");
                Assert.Fail($"Fail-fast: skipping scenario '{scenarioDisplay}'. Previous failure: {reason}");
            }
        }

        private static string BuildFailureMessage(string scenarioName, string stepText, Exception exception)
        {
            var scenarioDisplay = string.IsNullOrWhiteSpace(scenarioName)
                ? "(unknown scenario)"
                : scenarioName;

            var stepDisplay = string.IsNullOrWhiteSpace(stepText)
                ? "(unknown step)"
                : stepText;

            return $"Fail-fast armed after scenario '{scenarioDisplay}', step '{stepDisplay}'. " +
                   $"Error: {exception.GetType().Name}: {exception.Message}";
        }
    }
}
