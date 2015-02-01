using Rocks.SimpleInjector.Tests.NotThreadSafeCheck.Services;

namespace Rocks.SimpleInjector.Tests.NotThreadSafeCheck.Suts
{
    internal class SutWithTransientField
    {
#pragma warning disable 169
        private readonly TransientService member;
#pragma warning restore 169
    }
}