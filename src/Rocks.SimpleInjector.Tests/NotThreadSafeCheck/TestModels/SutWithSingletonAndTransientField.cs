namespace Rocks.SimpleInjector.Tests.NotThreadSafeCheck.TestModels
{
    internal class SutWithSingletonAndTransientField
    {
#pragma warning disable 169
        private readonly TransientService transientService;
        private readonly SingletonService singletonService;
#pragma warning restore 169
    }
}