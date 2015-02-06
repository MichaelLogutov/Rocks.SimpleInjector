using System;
using System.Collections.Generic;
using System.Linq;

namespace Rocks.SimpleInjector.Tests.NotThreadSafeCheck.Suts
{
    internal class SutWithKnownNotMutableGenericTypeButWithMutableArgument
    {
#pragma warning disable 169
        private readonly IReadOnlyList<IReadOnlyDictionary<string, object>> member;
#pragma warning restore 169
    }
}