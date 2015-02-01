using System.Collections.Generic;

namespace Rocks.SimpleInjector.Tests.NotThreadSafeCheck.Suts
{
    internal class SutWithReadonlyMutableField
    {
#pragma warning disable 169
        private readonly List<string> member;
#pragma warning restore 169
    }
}