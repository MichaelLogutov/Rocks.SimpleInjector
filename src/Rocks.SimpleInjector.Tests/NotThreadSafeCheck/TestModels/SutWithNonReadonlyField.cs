namespace Rocks.SimpleInjector.Tests.NotThreadSafeCheck.TestModels
{
    internal class SutWithNonReadonlyField
    {
#pragma warning disable 169
        private string member;
#pragma warning restore 169
    }
}