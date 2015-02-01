namespace Rocks.SimpleInjector.Tests.NotThreadSafeCheck.Suts
{
    internal class SutWithCyclicReferenceReadonlyMembers
    {
#pragma warning disable 169
        private readonly SutWithCyclicReferenceReadonlyMembers2 member;
#pragma warning restore 169

        // ReSharper disable once UnusedMember.Global
        public SutWithCyclicReferenceReadonlyMembers2 Member
        {
            get { return null; }
        }
    }
}