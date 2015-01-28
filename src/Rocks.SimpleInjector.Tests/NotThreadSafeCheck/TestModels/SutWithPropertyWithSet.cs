namespace Rocks.SimpleInjector.Tests.NotThreadSafeCheck.TestModels
{
    internal class SutWithPropertyWithSet
    {
        public string String { get; private set; }
    }
}