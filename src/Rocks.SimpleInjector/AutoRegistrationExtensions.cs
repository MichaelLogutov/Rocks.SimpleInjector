using System;
using System.Reflection;
using JetBrains.Annotations;
using Rocks.Helpers;
using Rocks.SimpleInjector.Attributes;
using SimpleInjector;

namespace Rocks.SimpleInjector
{
    [UsedImplicitly]
    public static class AutoRegistrationExtensions
    {
        /// <summary>
        ///     Performs search of all non abstract classes that implementing interface having the same name
        ///     as the class prefixed with "I" (for example, TestService class implementing ITestService interface).
        ///     All such types registered within <paramref name="container" />
        ///     with lifestyle calculated by <see cref="GetLifestyle" /> method
        ///     (implementation class type will be passed).
        /// </summary>
        /// <param name="container">Container for registrations.</param>
        /// <param name="assembly">Assembly where to search types.</param>
        /// <param name="defaultLifestyle">Default lifestyle (see <see cref="GetLifestyle" /> method).</param>
        /// <param name="customLifestyleSelector">Custom lifestyle selector function (see <see cref="GetLifestyle" /> method).</param>
        /// <param name="instancePredicate">Predicate to filter out instances (classes) from the process.</param>
        /// <param name="interfacePredicate">Predicate to filter out interfaces from the process.</param>
        /// <param name="onlyPublicInterfaces">If true then only public interfaces will be considered.</param>
        [UsedImplicitly]
        public static void AutoRegisterSelfImplementingTypes([NotNull] this Container container,
                                                             [NotNull] Assembly assembly,
                                                             [NotNull] Lifestyle defaultLifestyle,
                                                             Func<Type, Lifestyle> customLifestyleSelector = null,
                                                             Func<Type, bool> instancePredicate = null,
                                                             Func<Type, bool> interfacePredicate = null,
                                                             bool onlyPublicInterfaces = true)
        {
            container.RequiredNotNull("container");
            assembly.RequiredNotNull("assembly");
            defaultLifestyle.RequiredNotNull("defaultLifestyle");

            var types = assembly.GetSelfImplementingTypes(instancePredicate: instancePredicate,
                                                          interfacePredicate: interfacePredicate,
                                                          onlyPublicInterfaces: onlyPublicInterfaces);


            foreach (var t in types)
            {
                if (NoAutoRegistrationAttribute.ExsitsOn(t.InstanceType) || NoAutoRegistrationAttribute.ExsitsOn(t.InterfaceType))
                    continue;

                var lifestyle = GetLifestyle(t.InstanceType,
                                             defaultLifestyle,
                                             customLifestyleSelector);

                container.Register(t.InterfaceType, t.InstanceType, lifestyle);
            }
        }


        /// <summary>
        ///     Gets lifestyle for <paramref name="type" />.
        ///     If <paramref name="customLifestyleSelector" /> specified and returns not null then
        ///     it's result is returned. Otherwise if <paramref name="type" /> has <see cref="SingletonAttribute" />
        ///     then <see cref="Lifestyle.Singleton" /> is returned.
        ///     Otherwise <paramref name="defaultLifestyle"/> is returned.
        /// </summary>
        public static Lifestyle GetLifestyle([NotNull] Type type,
                                             [NotNull] Lifestyle defaultLifestyle,
                                             [CanBeNull] Func<Type, Lifestyle> customLifestyleSelector = null)
        {
            Lifestyle result;

            if (customLifestyleSelector != null)
            {
                result = customLifestyleSelector(type);
                if (result != null)
                    return result;
            }

            result = SingletonAttribute.ExsitsOn(type)
                         ? Lifestyle.Singleton
                         : defaultLifestyle;

            return result;
        }
    }
}