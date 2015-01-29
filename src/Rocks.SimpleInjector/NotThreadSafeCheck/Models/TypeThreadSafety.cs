namespace Rocks.SimpleInjector.NotThreadSafeCheck.Models
{
    public enum TypeThreadSafety
    {
        /// <summary>
        ///     The type is thread safe.
        /// </summary>
        Safe,

        /// <summary>
        ///     The type is not thread safe.
        /// </summary>
        NotSafe,

        /// <summary>
        ///     The type is potentially safe - does not conains not thread safe members,
        ///     but contains recursive not resolved references.
        /// </summary>
        PotentiallySafe
    }
}
