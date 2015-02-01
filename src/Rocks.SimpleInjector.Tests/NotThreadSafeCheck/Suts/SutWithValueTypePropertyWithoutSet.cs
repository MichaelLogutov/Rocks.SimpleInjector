namespace Rocks.SimpleInjector.Tests.NotThreadSafeCheck.Suts
{
    internal class SutWithValueTypePropertyWithoutSet
    {
        public string String { get { return string.Empty; } }
    }
}