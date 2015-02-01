using Rocks.SimpleInjector.Tests.NotThreadSafeCheck.Services;

namespace Rocks.SimpleInjector.Tests.NotThreadSafeCheck.Suts
{
    internal class SutWithNotRegisteredField
    {
#pragma warning disable 169
        private readonly NotRegisteredService notRegisteredService;
#pragma warning restore 169
    }
}