using System.Data.Linq;

namespace Rocks.SimpleInjector.Tests.Library
{
    public abstract class AbstractClassWithLinqDataContextProperty
    {
        public DataContext DataContext
        {
            get { return null; }
        }
    }
}