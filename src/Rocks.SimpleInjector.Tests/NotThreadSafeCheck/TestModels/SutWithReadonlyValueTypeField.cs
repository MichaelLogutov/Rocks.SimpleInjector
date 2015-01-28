namespace Rocks.SimpleInjector.Tests.NotThreadSafeCheck.TestModels
{
    internal class SutWithReadonlyValueTypeField
    {
#pragma warning disable 169
        private readonly string member;
#pragma warning restore 169
    }
}