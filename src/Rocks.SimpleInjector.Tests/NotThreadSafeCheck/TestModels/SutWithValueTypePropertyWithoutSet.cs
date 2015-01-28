namespace Rocks.SimpleInjector.Tests.NotThreadSafeCheck.TestModels
{
    internal class SutWithValueTypePropertyWithoutSet
    {
        public string String { get { return string.Empty; } }
    }
}