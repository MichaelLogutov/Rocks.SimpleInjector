using System.Reflection;

namespace Rocks.SimpleInjector.NotThreadSafeCheck
{
    public class NotThreadSafeMemberInfo
    {
        /// <summary>
        ///     A member that is potentially not thread safe.
        /// </summary>
        public MemberInfo Member { get; set; }


        /// <summary>
        ///     Member thread safety violation type.
        /// </summary>
        public ThreadSafetyViolationType ViolationType { get; set; }


        /// <summary>
        ///     Returns a string that represents the current object.
        /// </summary>
        /// <returns>
        ///     A string that represents the current object.
        /// </returns>
        public override string ToString ()
        {
            return this.ViolationType.GetDescription () + ": " + this.Member;
        }
    }
}