using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Celin.Services
{
    public class Store : IStore
    {
        Dictionary<string, object> Storage { get; } = new Dictionary<string, object>();
        public T GetValue<T>(string key)
        {
            if (Storage.TryGetValue(key, out var value))
            {
                return (T)value;
            }
            return default;
        }
        public void SetValue<T>(string key, T value)
        {
            if (Storage.ContainsKey(key))
            {
                Storage[key] = value;
            }
            else
            {
                Storage.Add(key, value);
            }
        }
    }
}
