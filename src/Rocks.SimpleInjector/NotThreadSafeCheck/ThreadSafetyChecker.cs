using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using JetBrains.Annotations;
using Rocks.Helpers;
using Rocks.SimpleInjector.Attributes;
using Rocks.SimpleInjector.NotThreadSafeCheck.Models;
using SimpleInjector;

namespace Rocks.SimpleInjector.NotThreadSafeCheck
{
    /// <summary>
    ///     <para>
    ///         A class that can check <see cref="Type" /> for potential not thread safe members.
    ///         This is not guarantees that the class is safe or not - it's just helps to find
    ///         probable unitentional usage of the type (for example, singleton registration
    ///         in dependency injection container).
    ///     </para>
    ///     <para>
    ///         This class heavily uses reflection so it's very performant and should be used
    ///         mostly in unit tests or uppon application startup.
    ///     </para>
    /// </summary>
    // ReSharper disable once ClassWithVirtualMembersNeverInherited.Global
    public class ThreadSafetyChecker
    {
        #region Private fields

        protected readonly Dictionary<Type, ThreadSafetyCheckResult> cache;
        protected readonly InstanceProducer[] registrations;

        #endregion

        #region Construct

        /// <summary>
        ///     Initializes a new instance of the <see cref="T:System.Object" /> class.
        /// </summary>
        public ThreadSafetyChecker ([NotNull] Container container)
        {
            if (container == null)
                throw new ArgumentNullException ("container");

            this.registrations = container.GetCurrentRegistrations ();
            this.cache = new Dictionary<Type, ThreadSafetyCheckResult> ();

            this.NotMutableTypes = new List<Type>
                                   {
                                       typeof (IEnumerable),
                                       typeof (IEnumerable<>),
                                       typeof (IReadOnlyCollection<>),
                                       typeof (IReadOnlyList<>),
                                       typeof (IReadOnlyDictionary<,>)
                                   };
        }

        #endregion

        #region Public properties

        /// <summary>
        ///     A list of reference types that are considered not mutable.
        ///     By default includes: <see cref="IEnumerable" />, <see cref="IEnumerable{T}" />,
        ///     <see cref="IReadOnlyCollection{T}" />, <see cref="IReadOnlyList{T}" />,
        ///     <see cref="IReadOnlyDictionary{TKey,TValue}" />.
        /// </summary>
        public List<Type> NotMutableTypes { get; set; }

        #endregion

        #region Public methods

        /// <summary>
        ///     Gets a list of potential non thread members of the type.
        /// </summary>
        [NotNull]
        public IReadOnlyList<NotThreadSafeMemberInfo> Check ([NotNull] Type type)
        {
            if (type == null)
                throw new ArgumentNullException ("type");

            var result = this.CheckInternal (type);

            // ReSharper disable once AssignNullToNotNullAttribute
            return result.NotThreadSafeMembers.AsReadOnlyList ();
        }


        /// <summary>
        ///     Clears internal cache of checked types.
        /// </summary>
        public void ClearCache ()
        {
            this.cache.Clear ();
        }

        #endregion

        #region Protected methods

        protected virtual ThreadSafetyCheckResult CheckInternal ([NotNull] Type type)
        {
            if (this.IsNotMutableType (type))
                return ThreadSafetyCheckResult.Safe;

            ThreadSafetyCheckResult result;
            if (!this.cache.TryGetValue (type, out result))
            {
                // mark as "checking started" to prevent infinite recursion
                this.cache[type] = null;

                result = new ThreadSafetyCheckResult ();

                var events = this.GetAllEvents (type);

                foreach (var not_thread_safe_member in this.GetAllFields (type)
                                                           .Where (field => !events.Any (x => x.Name.Equals (field.Name, StringComparison.Ordinal)))
                                                           .Select (this.CheckField)
                                                           .Concat (this.GetAllProperties (type).Select (this.CheckProperty))
                                                           .Concat (events.Select (this.CheckEvent))
                                                           .SkipNull ())
                {
                    if (ReferenceEquals (not_thread_safe_member, NotThreadSafeMemberInfo.PotentiallySafe))
                        result.NotFullyChecked = true;
                    else
                        result.NotThreadSafeMembers.Add (not_thread_safe_member);
                }

                this.cache[type] = result;
            }
            else if (result == null)
            {
                // cyclic reference checking
                return new ThreadSafetyCheckResult { NotFullyChecked = true };
            }

            return result;
        }


        [CanBeNull]
        protected virtual NotThreadSafeMemberInfo CheckField (FieldInfo field)
        {
            if (field.Name.EndsWith ("__BackingField", StringComparison.OrdinalIgnoreCase) ||
                NotMutableAttribute.ExsitsOn (field))
                return null;

            if (!field.IsInitOnly)
                return new NotThreadSafeMemberInfo (field, ThreadSafetyViolationType.NonReadonlyMember);

            var result = this.CheckMember (field, field.FieldType);

            return result;
        }


        [CanBeNull]
        protected virtual NotThreadSafeMemberInfo CheckProperty (PropertyInfo property)
        {
            if (NotMutableAttribute.ExsitsOn (property))
                return null;

            if (property.CanWrite)
                return new NotThreadSafeMemberInfo (property, ThreadSafetyViolationType.NonReadonlyMember);

            var result = this.CheckMember (property, property.PropertyType);

            return result;
        }


        [CanBeNull]
        protected virtual NotThreadSafeMemberInfo CheckEvent (EventInfo e)
        {
            if (NotMutableAttribute.ExsitsOn (e))
                return null;

            var result = new NotThreadSafeMemberInfo (e, ThreadSafetyViolationType.EventFound);

            return result;
        }


        [CanBeNull]
        protected virtual NotThreadSafeMemberInfo CheckMember (MemberInfo member, Type memberType)
        {
            if (this.IsNotMutableType (memberType) || this.HasSingletonRegistration (memberType))
                return null;

            if (this.HasNotSingletonRegistration (memberType))
                return new NotThreadSafeMemberInfo (member, ThreadSafetyViolationType.NonSingletonRegistration);

            if (memberType == typeof (object))
                return new NotThreadSafeMemberInfo (member, ThreadSafetyViolationType.MutableReadonlyMember);

            var check_result = this.CheckInternal (memberType);

            if (!check_result.NotThreadSafeMembers.IsNullOrEmpty ())
                return new NotThreadSafeMemberInfo (member, ThreadSafetyViolationType.MutableReadonlyMember);

            if (check_result.NotFullyChecked)
                return NotThreadSafeMemberInfo.PotentiallySafe;

            return null;
        }


        protected virtual bool IsNotMutableType (Type type)
        {
            if (type.IsValueType || type == typeof (string))
                return true;

            if (this.NotMutableTypes.Any (t => t == type ||
                                               (t.IsGenericTypeDefinition && type.IsGenericType && type.GetGenericTypeDefinition () == t)))
                return true;

            return false;
        }


        protected virtual bool HasNotSingletonRegistration (Type type)
        {
            var result = this.registrations.Any (x => x.ServiceType == type && x.Lifestyle != Lifestyle.Singleton);

            return result;
        }


        protected virtual bool HasSingletonRegistration (Type type)
        {
            var result = this.registrations.Any (x => x.ServiceType == type && x.Lifestyle == Lifestyle.Singleton);

            return result;
        }


        protected virtual List<FieldInfo> GetAllFields (Type type)
        {
            var result = type.GetFields (BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly)
                             .ToList ();

            if (type.BaseType != null)
                result.AddRange (this.GetAllFields (type.BaseType));

            return result;
        }


        protected virtual List<EventInfo> GetAllEvents (Type type)
        {
            var result = type.GetEvents (BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly)
                             .ToList ();

            if (type.BaseType != null)
                result.AddRange (this.GetAllEvents (type.BaseType));

            return result;
        }


        protected virtual List<PropertyInfo> GetAllProperties (Type type)
        {
            var result = type.GetProperties (BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly)
                             .ToList ();

            if (type.BaseType != null)
                result.AddRange (this.GetAllProperties (type.BaseType));

            return result;
        }

        #endregion
    }
}