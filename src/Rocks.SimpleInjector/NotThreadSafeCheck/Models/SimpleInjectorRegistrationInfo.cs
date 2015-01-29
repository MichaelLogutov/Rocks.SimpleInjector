using System;
using System.Collections.Generic;
using Rocks.Helpers;
using Rocks.SimpleInjector.Attributes;
using SimpleInjector;

namespace Rocks.SimpleInjector.NotThreadSafeCheck.Models
{
    public class SimpleInjectorRegistrationInfo
    {
        /// <summary>
        ///     A type of the service (usually interface).
        /// </summary>
        public Type ServiceType { get; set; }

        /// <summary>
        ///     A type of the service actual implementation (resolved from container).
        /// </summary>
        public Type ImplementationType { get; set; }

        /// <summary>
        ///     Lifestyle for the service registration.
        /// </summary>
        public Lifestyle Lifestyle { get; set; }

        /// <summary>
        ///     A list of potential non thread members.
        /// </summary>
        public IList<NotThreadSafeMemberInfo> NotThreadSafeMembers { get; set; }

        /// <summary>
        ///     True if <see cref="ImplementationType" /> has <see cref="SingletonAttribute" />.
        /// </summary>
        public bool HasSingletonAttribute { get; set; }

        /// <summary>
        ///     True if <see cref="ImplementationType" /> has <see cref="NotSingletonAttribute" />.
        /// </summary>
        public bool HasNotSingletonAttribute { get; set; }

        /// <summary>
        ///     True if <see cref="NotThreadSafeMembers" /> is not null and contains items.
        /// </summary>
        public bool HasNotThreadSafeMembers
        {
            get { return !this.NotThreadSafeMembers.IsNullOrEmpty (); }
        }
    }
}