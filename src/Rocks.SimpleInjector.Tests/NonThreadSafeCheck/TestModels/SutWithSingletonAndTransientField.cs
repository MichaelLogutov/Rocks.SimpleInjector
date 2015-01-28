namespace Rocks.SimpleInjector.Tests.NonThreadSafeCheck.TestModels
{
    internal class SutWithSingletonAndTransientField
    {
#pragma warning disable 169
        private readonly TransientService transientService;
        private readonly SingletonService singletonService;
#pragma warning restore 169
    }
}