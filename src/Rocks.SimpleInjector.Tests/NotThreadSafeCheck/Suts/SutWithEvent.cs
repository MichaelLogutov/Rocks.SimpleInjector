using System;

namespace Rocks.SimpleInjector.Tests.NotThreadSafeCheck.Suts
{
    internal class SutWithEvent
    {
#pragma warning disable 67
        public event EventHandler Event;
#pragma warning restore 67
    }
}