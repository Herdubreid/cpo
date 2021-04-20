using Microsoft.Toolkit.Mvvm.DependencyInjection;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Celin.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Login : Page
    {

        #region Busy
        public bool Busy
        {
            get { return (bool)GetValue(BusyProperty); }
            set { SetValue(BusyProperty, value); }
        }
        public static readonly DependencyProperty BusyProperty =
            DependencyProperty.Register(nameof(Busy), typeof(bool), typeof(Login), new PropertyMetadata(default(bool)));
        #endregion
        Doc.Settings Settings => Ioc.Default.GetRequiredService<Doc.Settings>();
        Services.Navigate Nav => Ioc.Default.GetRequiredService<Services.Navigate>();
        public Login()
        {
            InitializeComponent();
        }
    }
}
