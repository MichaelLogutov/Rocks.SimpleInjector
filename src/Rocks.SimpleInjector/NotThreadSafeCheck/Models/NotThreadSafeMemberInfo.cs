using System.Reflection;

namespace Rocks.SimpleInjector.NotThreadSafeCheck.Models
{
    public sealed class NotThreadSafeMemberInfo
    {
        #region Constants

        /// <summary>
        ///     Represents a result of checking the type when all it's members are thread safe but
        ///     there are some that lead to cyclic reference checking.
        /// </summary>
        public static readonly NotThreadSafeMemberInfo PotentiallySafe = new NotThreadSafeMemberInfo ();

        #endregion

        #region Private fields

        private readonly MemberInfo member;
        private readonly ThreadSafetyViolationType violationType;

        #endregion

        #region Construct

        private NotThreadSafeMemberInfo ()
        {
        }


        public NotThreadSafeMemberInfo (MemberInfo member, ThreadSafetyViolationType violationType)
        {
            this.member = member;
            this.violationType = violationType;
        }

        #endregion

        #region Public properties

        /// <summary>
        ///     A member that is potentially not thread safe.
        /// </summary>
        public MemberInfo Member
        {
            get { return this.member; }
        }

        /// <summary>
        ///     Member thread safety violation type.
        /// </summary>
        public ThreadSafetyViolationType ViolationType
        {
            get { return this.violationType; }
        }

        #endregion

        #region Public methods

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

        #endregion
    }
}