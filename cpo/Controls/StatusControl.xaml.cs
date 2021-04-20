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

namespace Celin
{
    public sealed partial class StatusControl : UserControl
    {
        #region Text
        static readonly string defaultText = "Signed-out";
        public string Text
        {
            get => (string)GetValue(TextProperty);
            set => SetValue(TextProperty, value);
        }
        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register(nameof(Text), typeof(string), typeof(StatusControl), new PropertyMetadata(defaultText));
        #endregion
        private void SettingsChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(Settings.Authenticated):
                    VisualStateManager.GoToState(this,
                        Settings.Authenticated
                        ? InfoBarSeverity.Success.ToString("g")
                        : InfoBarSeverity.Informational.ToString("g"),
                        false);
                    break;
                case nameof(Settings.AuthResponse):
                    if (Settings.AuthResponse is null)
                    {
                        Text = defaultText;
                    }
                    else
                    {
                        Text = $"Welcome, {Settings.AuthResponse.username}";
                        VisualStateManager.GoToState(this, InfoBarSeverity.Success.ToString("g"), false);
                    }
                    break;
            }
        }
        Doc.Settings Settings => Ioc.Default.GetRequiredService<Doc.Settings>();
        public StatusControl()
        {
            InitializeComponent();

            Settings.PropertyChanged += SettingsChanged;
        }
    }
}
