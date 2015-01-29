using System.Collections.Generic;

namespace Rocks.SimpleInjector.Tests.NotThreadSafeCheck.TestModels
{
    internal class SutWithCyclicReferenceAndNotThreadSafeMembers
    {
#pragma warning disable 169
        private readonly SutWithCyclicReferenceAndNotThreadSafeMembers2 member;
#pragma warning restore 169

        // ReSharper disable once UnusedMember.Global
        public SutWithCyclicReferenceAndNotThreadSafeMembers2 Member { get; set; }

        // ReSharper disable once UnusedMember.Global
        public List<string> List { get { return null; } }
    }
}