namespace Rocks.SimpleInjector.Tests.NotThreadSafeCheck.Suts
{
    public class SutWithSelfReferenceReadonlyMembers
    {
#pragma warning disable 169
        private readonly SutWithSelfReferenceReadonlyMembers member;
#pragma warning restore 169

        // ReSharper disable once UnusedMember.Global
        public SutWithSelfReferenceReadonlyMembers Member { get { return null; } }
    }
}
