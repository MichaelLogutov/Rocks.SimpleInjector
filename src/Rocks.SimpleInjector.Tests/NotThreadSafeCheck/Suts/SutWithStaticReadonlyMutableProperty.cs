using System;
using System.Collections.Generic;
using System.Linq;

namespace Rocks.SimpleInjector.Tests.NotThreadSafeCheck.Suts
{
    internal class SutWithStaticReadonlyMutableProperty
    {
        // ReSharper disable once UnusedMember.Global
        public static IList<string> List
        {
            get { return null; }
        }
    }
}