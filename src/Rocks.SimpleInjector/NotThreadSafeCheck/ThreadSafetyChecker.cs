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
    public class ThreadSafetyChecker
    {
        #region Private fields

        protected readonly Dictionary<Type, TypeThreadSafety> typeThreadSafetyCache;
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
            this.typeThreadSafetyCache = new Dictionary<Type, TypeThreadSafety> ();

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
        public List<NotThreadSafeMemberInfo> Check ([NotNull] Type type)
        {
            if (type == null)
                throw new ArgumentNullException ("type");

            if (this.IsNotMutableType (type))
                return new List<NotThreadSafeMemberInfo> ();

            var result = this.CheckInternal (type, new HashSet<Type> ());

            var members = this.GetAllMembers (type);

            return result;
        }


        /// <summary>
        ///     Clears internal cache of checked types.
        /// </summary>
        public void ClearCache ()
        {
            this.typeThreadSafetyCache.Clear ();
        }

        #endregion

        #region Protected methods

        //protected virtual TypeThreadSafety GetTypeThreadSafety ([NotNull] Type type,
        //                                                        [NotNull] HashSet<Type> callStack,
        //                                                        [NotNull] IList<MemberInfo> members)
        //{
        //    if (callStack.Contains (type))
        //        return TypeThreadSafety.PotentiallySafe;

        //    var has_potentially_safe_members = false;

        //    foreach (var member in members)
        //    {
        //        this.IsNotMutableType (member)
        //    }
        //}


        protected virtual List<NotThreadSafeMemberInfo> CheckInternal ([NotNull] Type type, HashSet<Type> callStack)
        {
            var result = new List<NotThreadSafeMemberInfo> ();

            var events = this.GetAllEvents (type);

            result.AddRange (this.GetAllFields (type)
                                 .Select (x => this.CheckField (x, events))
                                 .SkipNull ());

            result.AddRange (this.GetAllProperties (type)
                                 .Select (this.CheckProperty)
                                 .SkipNull ());

            result.AddRange (this.GetAllEvents (type)
                                 .Select (this.CheckEvent)
                                 .SkipNull ());

            return result;
        }


        [CanBeNull]
        protected virtual NotThreadSafeMemberInfo CheckField (FieldInfo field, IEnumerable<EventInfo> events)
        {
            if (field.Name.EndsWith ("__BackingField", StringComparison.OrdinalIgnoreCase) ||
                NotMutableAttribute.ExsitsOn (field) ||
                events.Any (x => x.Name.Equals (field.Name, StringComparison.Ordinal)))
                return null;

            if (!field.IsInitOnly)
                return new NotThreadSafeMemberInfo { Member = field, ViolationType = ThreadSafetyViolationType.NonReadonlyMember };

            var result = this.CheckMember (field, field.FieldType);

            return result;
        }


        [CanBeNull]
        protected virtual NotThreadSafeMemberInfo CheckProperty (PropertyInfo property)
        {
            if (NotMutableAttribute.ExsitsOn (property))
                return null;

            if (property.CanWrite)
                return new NotThreadSafeMemberInfo { Member = property, ViolationType = ThreadSafetyViolationType.NonReadonlyMember };

            var result = this.CheckMember (property, property.PropertyType);

            return result;
        }


        [CanBeNull]
        protected virtual NotThreadSafeMemberInfo CheckEvent (EventInfo e)
        {
            if (NotMutableAttribute.ExsitsOn (e))
                return null;

            var result = new NotThreadSafeMemberInfo
                         {
                             Member = e,
                             ViolationType = ThreadSafetyViolationType.EventFound
                         };

            return result;
        }


        [CanBeNull]
        protected virtual NotThreadSafeMemberInfo CheckMember (MemberInfo member, Type memberType)
        {
            if (this.IsNotMutableType (memberType) || this.HasSingletonRegistration (this.registrations, memberType))
                return null;

            if (this.HasNotSingletonRegistration (this.registrations, memberType))
                return new NotThreadSafeMemberInfo { Member = member, ViolationType = ThreadSafetyViolationType.NonSingletonRegistration };

            return new NotThreadSafeMemberInfo { Member = member, ViolationType = ThreadSafetyViolationType.MutableReadonlyMember };
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


        protected virtual bool HasNotSingletonRegistration (IEnumerable<InstanceProducer> registrations,
                                                            Type type)
        {
            var result = registrations.Any (x => x.ServiceType == type && x.Lifestyle != Lifestyle.Singleton);

            return result;
        }


        protected virtual bool HasSingletonRegistration (IEnumerable<InstanceProducer> registrations,
                                                         Type type)
        {
            var result = registrations.Any (x => x.ServiceType == type && x.Lifestyle == Lifestyle.Singleton);

            return result;
        }


        protected virtual List<MemberInfo> GetAllMembers (Type type)
        {
            var result = type.GetMembers (BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly)
                             .ToList ();

            if (type.BaseType != null)
                result.AddRange (this.GetAllMembers (type.BaseType));

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