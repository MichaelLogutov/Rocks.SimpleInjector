using System;
using System.Linq;

namespace Rocks.SimpleInjector.Tests.NotThreadSafeCheck.Suts
{
    internal class BaseGenericThreadSafeClass<T>
    {
        public const string Const = "";

#pragma warning disable 169
        private readonly int privateReadonly;
#pragma warning restore 169

        public T Property
        {
            get { return default (T); }
        }
    }
}