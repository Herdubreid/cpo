using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.DependencyInjection;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Celin.Services
{
    public class MenuItem : DependencyObject
    {
        public string Name { get; set; }
        public Symbol Symbol { get; set; }
        public string Tooltip { get; set; }
        public Page Page { get; set; }
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
    public class Navigate : DependencyObject
    {
        public ObservableCollection<MenuItem> MenuItems = new ObservableCollection<MenuItem>
        {
            new MenuItem { Name = "Browse", Symbol = Symbol.Home, Tooltip = "Open Orders", Page = new Pages.Browse() }
        };
        public Pages.Settings Settings { get; } = new Pages.Settings();
        #region CurrentPage
        public Page CurrentPage
        {
            get => (Page)GetValue(CurrentPageProperty);
            set => SetValue(CurrentPageProperty, value);
        }
        public static readonly DependencyProperty CurrentPageProperty =
            DependencyProperty.Register(nameof(CurrentPage), typeof(Page), typeof(Navigate), new PropertyMetadata(default(Page)));
        #endregion
        public Navigate()
        {
            var settings = Ioc.Default.GetRequiredService<Doc.Settings>();
            if (settings.IsConfigured)
            {
                foreach (var m in MenuItems) m.IsEnabled = true;
                CurrentPage = MenuItems.First().Page;
            }
            else
            {
                CurrentPage = Settings;
            }
        }
    }
}
