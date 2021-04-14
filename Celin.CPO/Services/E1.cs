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
        public E1()
            : base(Ioc.Default.GetService<IStore>().GetValue<string>(nameof(Settings.BaseUrl)),
                   Ioc.Default.GetService<ILogger<E1>>())
        {
            var set = Ioc.Default.GetService<IStore>();
            AuthRequest.username = set.GetValue<string>(nameof(Settings.Username));
            AuthRequest.password = set.GetValue<string>(nameof(Settings.Password));
        }
    }
}
