using System;
using System.Reflection;
using JetBrains.Annotations;

namespace Rocks.SimpleInjector.Attributes
{
    /// <summary>
    ///     Marks class not safe to be registered as singleton in dependency injection container.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public sealed class NotSingletonAttribute : Attribute
    {
        public static bool ExsitsOn([NotNull] Type type)
        {
            var attr = type.GetCustomAttribute(typeof(NotSingletonAttribute), false);
            return attr != null;
        }
    }
}