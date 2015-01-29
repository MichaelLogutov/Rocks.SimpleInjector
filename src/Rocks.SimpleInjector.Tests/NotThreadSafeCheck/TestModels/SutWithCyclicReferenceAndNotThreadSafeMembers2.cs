namespace Rocks.SimpleInjector.Tests.NotThreadSafeCheck.TestModels
{
    internal class SutWithCyclicReferenceAndNotThreadSafeMembers2
    {
#pragma warning disable 169
        private readonly SutWithCyclicReferenceAndNotThreadSafeMembers member;
#pragma warning restore 169

        // ReSharper disable once UnusedMember.Global
        public SutWithCyclicReferenceAndNotThreadSafeMembers Member { get; set; }
    }
}