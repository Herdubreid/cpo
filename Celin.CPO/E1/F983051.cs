using Celin.AIS;
using Celin.Helpers;
using System.Collections.Generic;

namespace Celin.F983051
{
    public class Row : DataRow
    {
        public string F983051_PID => this[nameof(F983051_PID)].ToString();
        public string F983051_VERS => this[nameof(F983051_VERS)].ToString();
        public string F983051_JD => this[nameof(F983051_JD)].ToString();
        public override string Key => F983051_VERS;
        public override string Label => F983051_JD;
    }
    public class Response : FormResponse, ILookupResponse
    {
        public Form<FormData<Row>> fs_DATABROWSE_F983051 { get; set; }

        public IEnumerable<DataRow> GetRows() => fs_DATABROWSE_F983051.data.gridData.rowset;

        public Summary GetSummary() => fs_DATABROWSE_F983051.data.gridData.summary;
    }
    public class Request : DatabrowserRequest
    {
        public Request(string vers)
        {
            dataServiceType = BROWSE;
            outputType = GRID_DATA;
            targetType = table;
            targetName = "F983051";
            returnControlIDs = "PID|VERS|JD";
            maxPageSize = "20";
            query = new Query
            {
                autoFind = true,
                condition = new []
                {
                    new Condition
                    {
                        controlId = "F983051.PID",
                        @operator = Condition.EQUAL,
                        value = new[]
                        {
                            new Value
                            {
                                content = "P4310",
                                specialValueId = Value.LITERAL
                            }
                        }
                    },
                    new Condition
                    {
                        controlId = "F983051.VERS",
                        @operator = Condition.STR_START_WITH,
                        value = new[]
                        {
                            new Value
                            {
                                content = vers.ToUpper(),
                                specialValueId = Value.LITERAL
                            }
                        }
                    }
                }
            };
        }
    }
}
