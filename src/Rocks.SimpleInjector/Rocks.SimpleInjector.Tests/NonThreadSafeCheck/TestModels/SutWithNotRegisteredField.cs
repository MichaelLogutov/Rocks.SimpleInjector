namespace Rocks.SimpleInjector.Tests.NonThreadSafeCheck.TestModels
{
    internal class SutWithNotRegisteredField
    {
#pragma warning disable 169
        private readonly NotRegisteredService notRegisteredService;
#pragma warning restore 169
    }
}