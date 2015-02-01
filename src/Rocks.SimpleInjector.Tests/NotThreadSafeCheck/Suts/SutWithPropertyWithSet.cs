namespace Rocks.SimpleInjector.Tests.NotThreadSafeCheck.Suts
{
    internal class SutWithPropertyWithSet
    {
        public string String { get; private set; }
    }
}