#if NET46 || NET461 || NET462 || NET47
using System.Data.Linq;
#endif
#if NETSTANDARD2_0
using System.Linq;
#endif

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