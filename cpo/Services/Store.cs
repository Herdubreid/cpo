using Windows.Foundation.Collections;
using Windows.Storage;

namespace Celin.Services
{
    public sealed class Store : IStore
    {
        readonly IPropertySet Storage = ApplicationData.Current.LocalSettings.Values;
        public T GetValue<T>(string key)
        {
            if (Storage.TryGetValue(key, out object value))
            {
                return (T)value;
            }

            return default;
        }

        public void SetValue<T>(string key, T value)
        {
            if (Storage.ContainsKey(key))
            {
                Storage[key] = value ?? default(T);
            }
            else
            {
                Storage.Add(key, value as object);
            }
        }
    }
}
