using System;
using System.Linq;
using Rocks.SimpleInjector.Tests.NotThreadSafeCheck.Services;

namespace Rocks.SimpleInjector.Tests.NotThreadSafeCheck.Suts
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