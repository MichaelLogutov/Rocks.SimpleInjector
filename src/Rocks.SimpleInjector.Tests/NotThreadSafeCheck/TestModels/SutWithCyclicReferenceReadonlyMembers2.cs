namespace Rocks.SimpleInjector.Tests.NotThreadSafeCheck.TestModels
{
    internal class SutWithCyclicReferenceReadonlyMembers2
    {
#pragma warning disable 169
        private readonly SutWithCyclicReferenceReadonlyMembers member;
#pragma warning restore 169

        // ReSharper disable once UnusedMember.Global
        public SutWithCyclicReferenceReadonlyMembers Member { get; set; }
    }
}