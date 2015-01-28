using System.Collections.Generic;

namespace Rocks.SimpleInjector.Tests.NonThreadSafeCheck.TestModels
{
    internal class SutWithReadonlyMutableField
    {
#pragma warning disable 169
        private readonly List<string> member;
#pragma warning restore 169
    }
}