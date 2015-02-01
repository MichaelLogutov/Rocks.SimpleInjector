using System;
using System.Collections.Generic;
using Rocks.SimpleInjector.Attributes;

namespace Rocks.SimpleInjector.Tests.NotThreadSafeCheck.Suts
{
    internal class SutWithMutableMembersWithNotMutableAttribute
    {
#pragma warning disable 169
        [NotMutable]
        private List<string> member;
#pragma warning restore 169

        [NotMutable]
        public List<string> Member { get; set; }

#pragma warning disable 67
        [NotMutable]
        public event EventHandler Event;
#pragma warning restore 67
    }
}