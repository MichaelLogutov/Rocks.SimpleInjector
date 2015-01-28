using System;
using System.Linq;

namespace Rocks.SimpleInjector.Tests.NotThreadSafeCheck.TestModels
{
    internal class SutWithThreadSafeReadonlyReferenceMembers
    {
#pragma warning disable 169
        private readonly ThreadSafeService member;
#pragma warning restore 169

        // ReSharper disable once UnusedMember.Global
        public ThreadSafeService Member
        {
            get { return null; }
        }
    }
}