namespace Rocks.SimpleInjector.Tests.NotThreadSafeCheck.Suts
{
    internal class SutWithMutablePropertyWithoutSet
    {
        public object Object { get { return string.Empty; } }
    }
}