namespace Blaise.Tests.Helpers.Framework
{
    using System;
    using System.Threading;
    using OpenQA.Selenium;

    public static class RetryHelper
    {
        public static void RetryOnStale(Action action, int maxAttempts = 3, int delayMs = 250)
        {
            for (var attempt = 0; attempt < maxAttempts; attempt++)
            {
                try
                {
                    action();
                    return;
                }
                catch (StaleElementReferenceException)
                {
                    if (attempt >= maxAttempts - 1)
                    {
                        throw;
                    }

                    Thread.Sleep(delayMs);
                }
            }
        }

        public static T RetryOnStale<T>(Func<T> func, int maxAttempts = 3, int delayMs = 250)
        {
            for (var attempt = 0; attempt < maxAttempts; attempt++)
            {
                try
                {
                    return func();
                }
                catch (StaleElementReferenceException)
                {
                    if (attempt >= maxAttempts - 1)
                    {
                        throw;
                    }

                    Thread.Sleep(delayMs);
                }
            }

            throw new InvalidOperationException("Unreachable");
        }
    }
}
