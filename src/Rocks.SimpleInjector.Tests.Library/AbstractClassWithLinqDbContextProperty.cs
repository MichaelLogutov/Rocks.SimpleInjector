#if NET461 || NET471
    using System.Data.Entity;
#endif
#if NETSTANDARD2_0
    using Microsoft.EntityFrameworkCore;
#endif

namespace Rocks.SimpleInjector.Tests.Library
{
    public abstract class AbstractClassWithLinqDbContextProperty
    {
        public DbContext DbContext => null;
    }
}