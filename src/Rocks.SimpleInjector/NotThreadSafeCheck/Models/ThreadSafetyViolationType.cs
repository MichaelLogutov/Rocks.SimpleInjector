using System;

namespace Rocks.SimpleInjector.NotThreadSafeCheck.Models
{
    /// <summary>
    ///     A type of violation of thread safety.
    /// </summary>
    public enum ThreadSafetyViolationType
    {
        /// <summary>
        ///     Member has non singleton registration.
        /// </summary>
        NonSingletonRegistration,

        /// <summary>
        ///     Event found.
        /// </summary>
        EventFound,


        /// <summary>
        ///     Non readonly member.
        /// </summary>
        NonReadonlyMember,


        /// <summary>
        ///     Mutable readonly member.
        /// </summary>
        MutableReadonlyMember
    }


    public static class ThreadSafetyViolationTypeExtensions
    {
        public static string GetDescription (this ThreadSafetyViolationType value)
        {
            switch (value)
            {
                case ThreadSafetyViolationType.NonSingletonRegistration:
                    return "Member has non singleton registration";

                case ThreadSafetyViolationType.EventFound:
                    return "Event found";

                case ThreadSafetyViolationType.NonReadonlyMember:
                    return "Non readonly member";

                case ThreadSafetyViolationType.MutableReadonlyMember:
                    return "Mutable readonly member";

                default:
                    throw new NotSupportedException ("value: " + value);
            }
        }
    }
}