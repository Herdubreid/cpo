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
using System.Threading;
using Windows.Foundation;
using Windows.Foundation.Collections;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Celin
{
    public sealed partial class CancelButtonControl : UserControl
    {
        #region Busy
        public bool Busy
        {
            get => (bool)GetValue(BusyProperty);
            set
            {
                SetValue(BusyProperty, value);
                VisualStateManager.GoToState(this, value ? nameof(BusyStyle) : nameof(NormalStyle), true);
            }
        }
        public static readonly DependencyProperty BusyProperty =
            DependencyProperty.Register(nameof(Busy), typeof(bool), typeof(CancelButtonControl), new PropertyMetadata(false));
        #endregion
        #region Text
        public string Text
        {
            get => (string)GetValue(TextProperty);
            set => SetValue(TextProperty, value);
        }
        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register("Text", typeof(string), typeof(CancelButtonControl), new PropertyMetadata(default));
        #endregion
        public CancellationTokenSource Cancel { get; set; }
        public CancelButtonControl()
        {
            InitializeComponent();
        }
    }
}
