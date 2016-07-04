using System.Reflection;

namespace Rocks.SimpleInjector.NotThreadSafeCheck.Models
{
    public sealed class NotThreadSafeMemberInfo
    {
        /// <summary>
        ///     Represents a result of checking the type when all it's members are thread safe but
        ///     there are some that lead to cyclic reference checking.
        /// </summary>
        public static readonly NotThreadSafeMemberInfo PotentiallySafe = new NotThreadSafeMemberInfo();


        private NotThreadSafeMemberInfo()
        {
        }


        public NotThreadSafeMemberInfo(MemberInfo member, ThreadSafetyViolationType violationType)
        {
            this.Member = member;
            this.ViolationType = violationType;
        }


        /// <summary>
        ///     A member that is potentially not thread safe.
        /// </summary>
        public MemberInfo Member { get; }

        /// <summary>
        ///     Member thread safety violation type.
        /// </summary>
        public ThreadSafetyViolationType ViolationType { get; }


        /// <summary>
        ///     Returns a string that represents the current object.
        /// </summary>
        /// <returns>
        ///     A string that represents the current object.
        /// </returns>
        public override string ToString() => $"{this.ViolationType.GetDescription()}: {this.Member}";
    }
}