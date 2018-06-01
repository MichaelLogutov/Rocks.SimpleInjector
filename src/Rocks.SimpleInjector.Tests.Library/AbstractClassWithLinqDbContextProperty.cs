#if NET471
    using System.Data.Entity;
#else
    using Microsoft.EntityFrameworkCore;
#endif

namespace Rocks.SimpleInjector.Tests.Library
{
    public abstract class AbstractClassWithLinqDbContextProperty
    {
        public DbContext DbContext => null;
    }
}