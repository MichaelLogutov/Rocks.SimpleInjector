using System.Collections.Generic;

namespace Rocks.SimpleInjector.Tests.NotThreadSafeCheck.Suts
{
    internal class SutWithNotMutableReferenceReadonlyFields
    {
#pragma warning disable 169
        private readonly IEnumerable<string> enumerable;
        private readonly IReadOnlyCollection<string> readOnlyCollection;
        private readonly IReadOnlyList<string> readOnlyList;
        private readonly IReadOnlyDictionary<string, string> readOnlyDictionary;
#pragma warning restore 169
    }
}