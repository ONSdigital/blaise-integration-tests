using System;

namespace BlaiseNisraCaseProcessor.Tests.Behaviour.Helpers
{
    public class EnumHelper
    {
        public static T Parse<T>(string enumValue)
        {
            return (T) Enum.Parse(typeof(T), enumValue, true);
        }
    }
}
