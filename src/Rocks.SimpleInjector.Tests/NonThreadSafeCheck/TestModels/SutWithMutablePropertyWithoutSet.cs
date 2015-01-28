namespace Rocks.SimpleInjector.Tests.NonThreadSafeCheck.TestModels
{
    internal class SutWithMutablePropertyWithoutSet
    {
        public object Object { get { return string.Empty; } }
    }
}