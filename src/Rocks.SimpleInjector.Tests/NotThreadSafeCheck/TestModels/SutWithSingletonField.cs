namespace Rocks.SimpleInjector.Tests.NotThreadSafeCheck.TestModels
{
    internal class SutWithSingletonField
    {
#pragma warning disable 169
        private readonly SingletonService member;
#pragma warning restore 169
    }
}