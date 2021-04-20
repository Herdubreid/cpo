using Microsoft.Toolkit.Mvvm.DependencyInjection;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media.Animation;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.ObjectModel;

namespace Celin.Services
{
    public class MenuItem : DependencyObject
    {
        public string Name { get; set; }
        public Symbol Symbol { get; set; }
        public string Tooltip { get; set; }
        public Type Page { get; set; }
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
        public Frame ContentFrame { get; set; }
        public ObservableCollection<MenuItem> MenuItems = new ObservableCollection<MenuItem>
        {
            new MenuItem { Name = "Browse", Symbol = Symbol.Home, Tooltip = "Open Orders", Page = typeof(Pages.Browse) }
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
        public void ToPage(Type page, object para = null)
        {
            ContentFrame.Navigate(page, para, new EntranceNavigationTransitionInfo());
        }
        public void OnNavigate(NavigationView nav, NavigationViewItemInvokedEventArgs args)
        {
            if (args.IsSettingsInvoked)
            {
                ContentFrame.NavigateToType(typeof(Pages.Settings), null, new FrameNavigationOptions
                {
                    TransitionInfoOverride = new EntranceNavigationTransitionInfo(),
                    IsNavigationStackEnabled = false
                });
            }
            else
            {
                var m = nav.SelectedItem as MenuItem;
                ToPage(m.Page);
            }
        }
    }
}
