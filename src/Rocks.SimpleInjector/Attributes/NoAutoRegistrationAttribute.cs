using System;
using System.Reflection;
using JetBrains.Annotations;

namespace Rocks.SimpleInjector.Attributes
{
    /// <summary>
    ///     Marks class as not to be auto registered with <see cref="AutoRegistrationExtensions"/> methods.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface, Inherited = false)]
    public sealed class NoAutoRegistrationAttribute : Attribute
    {
        public static bool ExsitsOn([NotNull] Type type)
        {
            var attr = type.GetCustomAttribute(typeof(NoAutoRegistrationAttribute), false);
            return attr != null;
        }
    }
}