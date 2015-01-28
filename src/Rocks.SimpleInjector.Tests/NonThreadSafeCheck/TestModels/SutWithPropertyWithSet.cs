namespace Rocks.SimpleInjector.Tests.NonThreadSafeCheck.TestModels
{
    internal class SutWithPropertyWithSet
    {
        public string String { get; private set; }
    }
}