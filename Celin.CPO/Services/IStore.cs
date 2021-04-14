using System.Diagnostics.Contracts;

namespace Celin.Services
{
    public interface IStore
    {
        void SetValue<T>(string key, T value);
        [Pure]
        T GetValue<T>(string key);
    }
}
