using System;
using System.Collections.Generic;
using Rocks.SimpleInjector.Attributes;

namespace Rocks.SimpleInjector.Tests.NotThreadSafeCheck.Suts
{
    internal class SutWithMutableMembersWithThreadSafeAttribute
    {
#pragma warning disable 169
        [ThreadSafe]
        private List<string> member;
#pragma warning restore 169

        [ThreadSafe]
        public List<string> Member { get; set; }

#pragma warning disable 67
        [ThreadSafe]
        public event EventHandler Event;
#pragma warning restore 67
    }
}