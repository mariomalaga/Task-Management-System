using System.Linq;

namespace TMS.Storage.API.Unit.Tests
{
    public interface IdbContext
    {
        IQueryable<T> Set<T>() where T : class;
    }
}
