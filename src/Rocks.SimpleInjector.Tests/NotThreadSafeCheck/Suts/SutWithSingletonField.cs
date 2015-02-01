using Rocks.SimpleInjector.Tests.NotThreadSafeCheck.Services;

namespace Rocks.SimpleInjector.Tests.NotThreadSafeCheck.Suts
{
    internal class SutWithSingletonField
    {
#pragma warning disable 169
        private readonly SingletonService member;
#pragma warning restore 169
    }
}