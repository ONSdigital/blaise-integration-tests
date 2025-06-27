using System;

namespace Blaise.Tests.Helpers.Framework.Extensions
{
    public static class ParameterValidationExtensions
    {
        public static void ThrowExceptionIfNullOrEmpty(this string parameter, string parameterName)
        {
            if (parameter == null)
            {
                throw new ArgumentNullException(parameterName);
            }

            if (string.IsNullOrWhiteSpace(parameter))
            {
                throw new ArgumentException($"A value for the argument '{parameterName}' must be supplied");
            }
        }

        public static void ThrowExceptionIfNotInt(this string parameter, string parameterName)
        {
            if (!int.TryParse(parameter, out _))
            {
                throw new ArgumentException($"A int value for the argument '{parameterName}' must be supplied");
            }
        }
    }
}
