namespace Rocks.SimpleInjector.Tests.NotThreadSafeCheck.TestModels
{
    internal class SutWithTransientProperty
    {
        // ReSharper disable once UnusedMember.Local
        private TransientService TransientService
        {
            get { return null; }
        }
    }
}