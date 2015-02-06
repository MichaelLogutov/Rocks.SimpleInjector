using System;
using System.Linq;

namespace Rocks.SimpleInjector.Tests.NotThreadSafeCheck.Suts
{
    internal class SutWithBaseGenericClassAndMutableReadonlyProperty : BaseGenericThreadSafeClass<object>
    {
    }
}