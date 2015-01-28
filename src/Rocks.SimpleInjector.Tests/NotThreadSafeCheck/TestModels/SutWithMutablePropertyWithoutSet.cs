namespace Rocks.SimpleInjector.Tests.NotThreadSafeCheck.TestModels
{
    internal class SutWithMutablePropertyWithoutSet
    {
        public object Object { get { return string.Empty; } }
    }
}