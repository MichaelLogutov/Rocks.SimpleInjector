using System;
using System.Reflection;
using JetBrains.Annotations;

namespace Rocks.SimpleInjector.Attributes
{
    /// <summary>
    ///     Marks class safe to be registered as singleton in dependency injection container.
    /// </summary>
    [AttributeUsage (AttributeTargets.Class, Inherited = false)]
    public sealed class SingletonAttribute : Attribute
    {
        public static bool ExsitsOn ([NotNull] Type type)
        {
            var attr = type.GetCustomAttribute (typeof (SingletonAttribute), false);
            return attr != null;
        }
    }
}