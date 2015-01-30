using System.Reflection;

namespace Rocks.SimpleInjector.NotThreadSafeCheck.Models
{
    public class NotThreadSafeMemberInfo
    {
        #region Private fields

        private readonly MemberInfo member;
        private readonly ThreadSafetyViolationType violationType;

        #endregion

        #region Construct

        /// <summary>
        ///     Initializes a new instance of the <see cref="T:System.Object" /> class.
        /// </summary>
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