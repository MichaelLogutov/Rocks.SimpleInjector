using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using Rocks.SimpleInjector.Attributes;
using Rocks.SimpleInjector.NotThreadSafeCheck.Models;
using SimpleInjector;

namespace Rocks.SimpleInjector.NotThreadSafeCheck
{
    public static class Extensions
    {
        /// <summary>
        ///     Gets information about all current registrations and potential thread safety problems.
        /// </summary>
        [UsedImplicitly]
        public static IList<SimpleInjectorRegistrationInfo> GetRegistrationsInfo ([NotNull] this Container container,
                                                                                  Func<InstanceProducer, bool> registrationsPredicate = null)
        {
            if (container == null)
                throw new ArgumentNullException ("container");

            var thread_safety_checker = new ThreadSafetyChecker (container);

            var result = container
                .GetCurrentRegistrations ()
                .Where (registrationsPredicate ?? (x => true))
                .Select (x =>
                         {
                             var info = new SimpleInjectorRegistrationInfo ();

                             info.ImplementationType = x.GetInstance ().GetType ();
                             info.ServiceType = x.ServiceType;
                             info.Lifestyle = x.Lifestyle;
                             info.NotThreadSafeMembers = thread_safety_checker.Check (info.ImplementationType);
                             info.HasSingletonAttribute = SingletonAttribute.ExsitsOn (info.ImplementationType);
                             info.HasNotSingletonAttribute = NotSingletonAttribute.ExsitsOn (info.ImplementationType);

                             return info;
                         })
                .ToList ();

            return result;
        }
    }
}