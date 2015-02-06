using System;
using System.Collections.Generic;
using System.Linq;

namespace Rocks.SimpleInjector.Tests.NotThreadSafeCheck.Suts
{
    internal class SutWithKnownNotMutableGenericTypesAndComplexThreadSafeArguments
    {
#pragma warning disable 169
        private readonly IReadOnlyList<IReadOnlyDictionary<int, IReadOnlyCollection<IReadOnlyList<int>>>> member;
#pragma warning restore 169
    }
}