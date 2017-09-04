using Microsoft.EntityFrameworkCore;

namespace Rocks.SimpleInjector.Tests.Library
{
    public abstract class AbstractClassWithLinqDbContextProperty
    {
        public DbContext DbContext
        {
            get { return null; }
        }
    }
}