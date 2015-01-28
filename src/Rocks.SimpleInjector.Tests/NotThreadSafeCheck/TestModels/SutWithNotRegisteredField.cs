namespace Rocks.SimpleInjector.Tests.NotThreadSafeCheck.TestModels
{
    internal class SutWithNotRegisteredField
    {
#pragma warning disable 169
        private readonly NotRegisteredService notRegisteredService;
#pragma warning restore 169
    }
}