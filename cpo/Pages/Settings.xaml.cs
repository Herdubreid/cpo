﻿using Celin.Services;
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
using System.Threading;
using Windows.Foundation;
using Windows.Foundation.Collections;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Celin.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Settings : Page
    {
        #region NextCaption
        public string NextCaption
        {
            get => (string)GetValue(NextCaptionProperty);
            set => SetValue(NextCaptionProperty, value);
        }
        public static readonly DependencyProperty NextCaptionProperty =
            DependencyProperty.Register(nameof(NextCaption), typeof(string), typeof(Settings), new PropertyMetadata("Next"));
        #endregion
        #region Busy
        public bool Busy
        {
            get => (bool)GetValue(BusyProperty);
            set
            {
                NextCaption = value ? "Cancel" : "Next";
                SetValue(BusyProperty, value);
            }
        }
        public static readonly DependencyProperty BusyProperty =
            DependencyProperty.Register(nameof(Busy), typeof(bool), typeof(Settings), new PropertyMetadata(default(bool)));
        #endregion
        #region PageNo
        public int PageNo
        {
            get => (int)GetValue(PageNoProperty);
            set
            {
                IsFirstPage = value == 0;
                SetValue(PageNoProperty, value);
            }
        }
        public static readonly DependencyProperty PageNoProperty =
            DependencyProperty.Register(nameof(PageNo), typeof(int), typeof(Settings), new PropertyMetadata(default(int)));
        #endregion
        #region IsFirstPage
        public bool IsFirstPage
        {
            get => (bool)GetValue(IsFirstPageProperty);
            set => SetValue(IsFirstPageProperty, value);
        }
        public static readonly DependencyProperty IsFirstPageProperty =
            DependencyProperty.Register(nameof(IsFirstPage), typeof(bool), typeof(Settings), new PropertyMetadata(true));
        #endregion
        private void Previous_Click(object sender, RoutedEventArgs e)
        {
            PageNo--;
        }
        CancellationTokenSource Cancel { get; set; }
        private async void Next_Click(object sender, RoutedEventArgs e)
        {
            if (Busy)
            {
                Cancel?.Cancel();
                return;
            }
            ViewModel.Save();
            Busy = true;
            switch (PageNo)
            {
                case 0:
                    if (BaseUrl.Validate())
                    {
                        try
                        {
                            Cancel = new CancellationTokenSource();
                            await ViewModel.E1.DefaultConfigurationAsync(Cancel.Token);
                            PageNo++;
                        }
                        catch (Exception ex)
                        {
                            BaseUrl.SetError(ex.Message);
                        }
                    }
                    break;
                case 1:
                    if (Username.Validate())
                    {
                        try
                        {
                            Cancel = new CancellationTokenSource();
                            await ViewModel.E1.AuthenticateAsync(Cancel.Token);
                        }
                        catch (Exception ex)
                        {
                            Username.SetError(ex.Message);
                        }
                    }
                    break;
                default:
                    PageNo++;
                    break;
            }
            Busy = false;
        }
        public Settings()
        {
            this.InitializeComponent();
        }
    }
}
