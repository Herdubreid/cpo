using Celin.Doc;
using Microsoft.Extensions.Logging;
using Microsoft.Toolkit.Mvvm.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Celin.Services
{
    public class E1 : AIS.Server
    {
        public bool IsValid { get; set; }
        readonly IStore store;
        public E1()
            : base(Ioc.Default.GetService<IStore>().GetValue<string>(nameof(Settings.BaseUrl)),
                   Ioc.Default.GetService<ILogger<E1>>())
        {
            store = Ioc.Default.GetService<IStore>();
            IsValid = store.GetValue<bool>(nameof(IsValid));
            AuthRequest.username = store.GetValue<string>(nameof(Settings.Username));
            AuthRequest.password = store.GetValue<string>(nameof(Settings.Password));
        }
    }
}
