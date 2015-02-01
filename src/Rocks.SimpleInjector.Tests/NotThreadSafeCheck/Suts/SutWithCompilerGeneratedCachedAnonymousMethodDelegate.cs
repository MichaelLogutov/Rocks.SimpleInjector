using System;
using System.Collections.Generic;
using System.Linq;

namespace Rocks.SimpleInjector.Tests.NotThreadSafeCheck.Suts
{
    internal class SutWithCompilerGeneratedCachedAnonymousMethodDelegate
    {
        public void Method (IEnumerable<string> items)
        {
            if (items.Count (x => x == "test") > 2)
                throw new InvalidOperationException ();
        }
    }
}