namespace Rocks.SimpleInjector.Tests.NotThreadSafeCheck.Suts
{
    internal class SutWithCyclicReferenceReadonlyMembers2
    {
#pragma warning disable 169
        private readonly SutWithCyclicReferenceReadonlyMembers member;
#pragma warning restore 169

        // ReSharper disable once UnusedMember.Global
        public SutWithCyclicReferenceReadonlyMembers Member
        {
            get { return null; }
        }
    }
}