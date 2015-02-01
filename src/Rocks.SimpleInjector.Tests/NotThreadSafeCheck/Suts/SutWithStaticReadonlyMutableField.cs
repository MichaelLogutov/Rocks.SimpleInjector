using System;
using System.Collections.Generic;
using System.Linq;

namespace Rocks.SimpleInjector.Tests.NotThreadSafeCheck.Suts
{
    internal class SutWithStaticReadonlyMutableField
    {
        // ReSharper disable once UnusedMember.Global
        public static readonly IList<string> List = new List<string> ();
    }
}