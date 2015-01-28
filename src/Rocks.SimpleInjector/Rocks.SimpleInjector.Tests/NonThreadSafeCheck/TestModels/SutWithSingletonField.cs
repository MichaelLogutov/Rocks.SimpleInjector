namespace Rocks.SimpleInjector.Tests.NonThreadSafeCheck.TestModels
{
    internal class SutWithSingletonField
    {
#pragma warning disable 169
        private readonly SingletonService member;
#pragma warning restore 169
    }
}