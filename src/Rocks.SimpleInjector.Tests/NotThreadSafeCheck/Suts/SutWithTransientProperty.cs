using Rocks.SimpleInjector.Tests.NotThreadSafeCheck.Services;

namespace Rocks.SimpleInjector.Tests.NotThreadSafeCheck.Suts
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