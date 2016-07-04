using System.Collections.Generic;
using JetBrains.Annotations;

namespace Rocks.SimpleInjector.NotThreadSafeCheck.Models
{
    /// <summary>
    ///     The result of type thread safety checking.
    /// </summary>
    public class ThreadSafetyCheckResult
    {
        /// <summary>
        ///     Represents a constant value for potentially thread safe objects.
        /// </summary>
        public static readonly ThreadSafetyCheckResult Safe = new ThreadSafetyCheckResult();


        /// <summary>
        ///     A list of potentially not thread safe members.
        /// </summary>
        [NotNull]
        public IList<NotThreadSafeMemberInfo> NotThreadSafeMembers { get; set; }

        /// <summary>
        ///     If true then type was not fully checked and contains members
        ///     with types leading to cyclic references when checking for thread safety.
        ///     All other members were checked.
        /// </summary>
        public bool NotFullyChecked { get; set; }


        public ThreadSafetyCheckResult()
        {
            this.NotThreadSafeMembers = new List<NotThreadSafeMemberInfo>();
        }
    }
}