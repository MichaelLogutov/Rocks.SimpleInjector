using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using JetBrains.Annotations;
using Rocks.SimpleInjector.Attributes;
using SimpleInjector;

namespace Rocks.SimpleInjector.NotThreadSafeCheck
{
    public static class Extensions
    {
        #region Public static fields

        /// <summary>
        ///     A list of types that are considered as not mutable.
        /// </summary>
        // ReSharper disable once MemberCanBePrivate.Global
        // ReSharper disable once InconsistentNaming
        // ReSharper disable once FieldCanBeMadeReadOnly.Global
        public static List<Type> NotMutableTypes = new List<Type>
                                                   {
                                                       typeof (IEnumerable),
                                                       typeof (IEnumerable<>),
                                                       typeof (IReadOnlyCollection<>),
                                                       typeof (IReadOnlyList<>),
                                                       typeof (IReadOnlyDictionary<,>)
                                                   };

        #endregion

        #region Static methods

        /// <summary>
        ///     Gets information about all current registrations and potential thread safety problems.
        /// </summary>
        [UsedImplicitly]
        public static IList<SimpleInjectorRegistrationInfo> GetRegistrationsInfo ([NotNull] this Container container,
                                                                                  Func<InstanceProducer, bool> registrationsPredicate = null)
        {
            if (container == null)
                throw new ArgumentNullException ("container");

            var result = container
                .GetCurrentRegistrations ()
                .Where (registrationsPredicate ?? (x => true))
                .Select (x =>
                         {
                             var info = new SimpleInjectorRegistrationInfo ();

                             info.ImplementationType = x.GetInstance ().GetType ();
                             info.ServiceType = x.ServiceType;
                             info.Lifestyle = x.Lifestyle;
                             info.NotThreadSafeMembers = container.GetNotThreadSafeMembers (info.ImplementationType);
                             info.HasSingletonAttribute = SingletonAttribute.ExsitsOn (info.ImplementationType);
                             info.HasNotSingletonAttribute = NotSingletonAttribute.ExsitsOn (info.ImplementationType);

                             return info;
                         })
                .ToList ();

            return result;
        }


        /// <summary>
        ///     Gets a list of potential non thread members of the type.
        /// </summary>
        [NotNull]
        public static List<NotThreadMemberInfo> GetNotThreadSafeMembers ([NotNull] this Container container, [NotNull] Type type)
        {
            if (container == null)
                throw new ArgumentNullException ("container");

            if (type == null)
                throw new ArgumentNullException ("type");

            var registrations = container.GetCurrentRegistrations ();

            var result = new List<NotThreadMemberInfo> ();

            if (IsNotMutableType (type))
                return result;

            foreach (var field in GetAllFields (type))
            {
                if (field.Name.EndsWith ("__BackingField", StringComparison.OrdinalIgnoreCase))
                    continue;

                if (!field.IsInitOnly)
                {
                    result.Add (new NotThreadMemberInfo { Member = field, ViolationType = ThreadSafetyViolationType.NonReadonlyMember });
                    continue;
                }

                CheckMember (field, field.FieldType, registrations, result);
            }

            foreach (var property in GetAllProperties (type))
            {
                if (property.CanWrite)
                {
                    result.Add (new NotThreadMemberInfo { Member = property, ViolationType = ThreadSafetyViolationType.NonReadonlyMember });
                    continue;
                }

                CheckMember (property, property.PropertyType, registrations, result);
            }

            result.AddRange (GetAllEvents (type).Select (x => new NotThreadMemberInfo
                                                              {
                                                                  Member = x,
                                                                  ViolationType = ThreadSafetyViolationType.EventFound
                                                              }));

            return result;
        }

        #endregion

        #region Private methods

        private static void CheckMember (MemberInfo member,
                                         Type memberType,
                                         IList<InstanceProducer> registrations,
                                         ICollection<NotThreadMemberInfo> result)
        {
            if (IsNotMutableType (memberType) || HasSingletonRegistration (registrations, memberType))
                return;

            if (HasNotSingletonRegistration (registrations, memberType))
                result.Add (new NotThreadMemberInfo { Member = member, ViolationType = ThreadSafetyViolationType.NonSingletonRegistration });
            else
                result.Add (new NotThreadMemberInfo { Member = member, ViolationType = ThreadSafetyViolationType.MutableReadonlyMember });
        }


        private static bool IsNotMutableType (Type type)
        {
            if (type.IsValueType || type == typeof (string))
                return true;

            if (NotMutableTypes.Any (t => t == type ||
                                          (t.IsGenericTypeDefinition && type.IsGenericType && type.GetGenericTypeDefinition () == t)))
                return true;

            return false;
        }


        private static bool HasNotSingletonRegistration (IEnumerable<InstanceProducer> registrations,
                                                         Type type)
        {
            var result = registrations.Any (x => x.ServiceType == type && x.Lifestyle != Lifestyle.Singleton);

            return result;
        }


        private static bool HasSingletonRegistration (IEnumerable<InstanceProducer> registrations,
                                                      Type type)
        {
            var result = registrations.Any (x => x.ServiceType == type && x.Lifestyle == Lifestyle.Singleton);

            return result;
        }


        private static List<FieldInfo> GetAllFields (Type type)
        {
            var result = type.GetFields (BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly)
                             .ToList ();

            if (type.BaseType != null)
                result.AddRange (GetAllFields (type.BaseType));

            return result;
        }


        private static List<EventInfo> GetAllEvents (Type type)
        {
            var result = type.GetEvents (BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly)
                             .ToList ();

            if (type.BaseType != null)
                result.AddRange (GetAllEvents (type.BaseType));

            return result;
        }


        private static List<PropertyInfo> GetAllProperties (Type type)
        {
            var result = type.GetProperties (BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly)
                             .ToList ();

            if (type.BaseType != null)
                result.AddRange (GetAllProperties (type.BaseType));

            return result;
        }

        #endregion
    }
}