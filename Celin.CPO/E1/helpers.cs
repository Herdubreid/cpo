using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Celin.Helpers
{
    public abstract class DataRow : Dictionary<string, JsonElement>
    {
        public string ToString(string key) => this[key].GetString();
        public abstract string Key { get; }
        public abstract string Label { get; }
    }
    public interface ILookupResponse
    {
        AIS.Summary GetSummary();
        IEnumerable<DataRow> GetRows();
    }
}
