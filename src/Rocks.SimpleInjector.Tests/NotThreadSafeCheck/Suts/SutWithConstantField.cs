using System;
using System.Linq;

namespace Rocks.SimpleInjector.Tests.NotThreadSafeCheck.Suts
{
    internal class SutWithConstantField
    {
        public const int Constant = 1;
    }
}