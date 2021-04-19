using Celin.Services;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.DependencyInjection;
using Microsoft.Toolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Celin.Doc
{
    public class Settings : ObservableRecipient
    {
        public readonly Regex RxRequired = new Regex(".+");
        public readonly Regex RxUrl = new Regex(@"^https?:\/\/[^\s|\/]+\S*\/$");
        public readonly IEnumerable<string> RequiredMessages = new[]
        {
            string.Empty,
            string.Empty,
            "Required Field"
        };
        public readonly IEnumerable<string> UrlMessages = new[]
        {
            string.Empty,
            "OK",
            "Url format: http[s]://<server>/<path>/"
        };
        bool isConfigured;
        public bool IsConfigured
        {
            get => isConfigured;
            set
            {
                SetProperty(ref isConfigured, value);
                Store.SetValue(nameof(IsConfigured), value);
            }
        }
        string baseUrl;
        public string BaseUrl
        {
            get => baseUrl;
            set => SetProperty(ref baseUrl, value);
        }
        bool rememberUser;
        public bool RememberUser
        {
            get => rememberUser;
            set
            {
                SetProperty(ref rememberUser, value);
                Store.SetValue(nameof(RememberUser), RememberUser);
                Store.SetValue(nameof(Username), RememberUser ? Username : string.Empty);
                Store.SetValue(nameof(Password), RememberUser ? Password : string.Empty);
            }
        }
        string username;
        public string Username
        {
            get => username;
            set
            {
                SetProperty(ref username, value);
                E1.AuthRequest.username = value;
                Store.SetValue(nameof(Username), RememberUser ? value : string.Empty);
            }
        }
        string password;
        public string Password
        {
            get => password;
            set
            {
                SetProperty(ref password, value);
                E1.AuthRequest.password = value;
                Store.SetValue(nameof(Password), RememberUser ? value : string.Empty);
            }
        }
        string versionP4310;
        public string VersionP4310
        {
            get => versionP4310;
            set
            {
                SetProperty(ref versionP4310, value);
                Store.SetValue(nameof(VersionP4310), value);
            }
        }
        public void Save()
        {
            E1.BaseUrl = BaseUrl;
            E1.AuthRequest.username = Username;
            E1.AuthRequest.password = Password;
            //
            Store.SetValue(nameof(BaseUrl), BaseUrl);
            Store.SetValue(nameof(RememberUser), RememberUser);
            Store.SetValue(nameof(Username), RememberUser ? Username : string.Empty);
            Store.SetValue(nameof(Password), RememberUser ? Password : string.Empty);
        }
        public readonly E1 E1 = Ioc.Default.GetRequiredService<E1>();
        readonly IStore Store = Ioc.Default.GetRequiredService<IStore>();
        public Settings()
        {
            baseUrl = Store.GetValue<string>(nameof(BaseUrl));
            username = Store.GetValue<string>(nameof(Username));
            password = Store.GetValue<string>(nameof(Password));
            rememberUser = Store.GetValue<bool>(nameof(RememberUser));
            versionP4310 = Store.GetValue<string>(nameof(VersionP4310));
            isConfigured = Store.GetValue<bool>(nameof(IsConfigured));
        }
    }
}
