using System.Threading.Tasks;

namespace Fluxor.Persist.Storage
{
    public interface IStoreHandler
    {
        Task<object> GetState(IFeature feature);
        Task SetState(IFeature feature);
    }
}
