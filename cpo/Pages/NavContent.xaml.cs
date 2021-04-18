using Celin.Doc;
using Microsoft.Toolkit.Mvvm.DependencyInjection;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Documents;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Celin.Pages
{
    class MenuItem : DependencyObject
    {
        public string Name { get; set; }
        public Symbol Symbol { get; set; }
        public string Tooltip { get; set; }
        #region IsEnabled
        public bool IsEnabled
        {
            get => (bool)GetValue(IsEnabledProperty);
            set => SetValue(IsEnabledProperty, value);
        }
        public static readonly DependencyProperty IsEnabledProperty =
            DependencyProperty.Register("IsEnabled", typeof(bool), typeof(MenuItem), new PropertyMetadata(false));
        #endregion
    }
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class NavContent : Page
    {
        ObservableCollection<MenuItem> menuItems = new ObservableCollection<MenuItem>
        {
            new MenuItem { Name = "Browse", Symbol = Symbol.Home, Tooltip = "Open Orders" }
        };
        private void NavigationView_ItemInvoked(NavigationView sender, NavigationViewItemInvokedEventArgs args)
        {
            if (args.IsSettingsInvoked)
            {
                contentFrame.NavigateToType(typeof(Settings), null, new FrameNavigationOptions
                {
                    IsNavigationStackEnabled = false,
                });
            }
        }
        public NavContent()
        {
            InitializeComponent();
            var settings = Ioc.Default.GetRequiredService<Doc.Settings>();
            if (settings.IsConfigured)
            {
                foreach (var m in menuItems) m.IsEnabled = true;
            }
            else
            {
                contentFrame.NavigateToType(typeof(Settings), null, new FrameNavigationOptions
                {
                    IsNavigationStackEnabled = false
                });
            }
        }
    }
}
