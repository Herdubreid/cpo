using Celin.Services;
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
        #region VersionP4310
        bool isSelecting;
        private void VersionP4310_SuggestionChosen(AutoSuggestBox sender, AutoSuggestBoxSuggestionChosenEventArgs args)
        {
            var row = args.SelectedItem as Helpers.DataRow;
            sender.Text = row.ToString("F983051_VERS");
            sender.Description = row.ToString("F983051_JD");
            isSelecting = true;
        }
        private void VersionP4310_TextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
        {
            if (isSelecting)
            {
                isSelecting = false;
                return;
            }
            dd.Debounce(500, async (o) =>
            {
                var asb = o as AutoSuggestBox;
                if (!string.IsNullOrEmpty(asb.Text.Trim()))
                {
                    cancel?.Cancel();
                    cancel = new CancellationTokenSource();
                    try
                    {
                        var rs = await ViewModel.E1.RequestAsync<F983051.Response>(new F983051.Request(asb.Text.Trim()), cancel.Token);
                        asb.ItemsSource = rs.fs_DATABROWSE_F983051.data.gridData.rowset;
                        asb.Description = rs.fs_DATABROWSE_F983051.data.gridData.summary.records == 0 ? "No matching records" : string.Empty;
                    }
                    catch (Exception ex)
                    {
                        asb.Description = ex.Message;
                    }
                }
            }, sender);
        }
        #endregion
        private async void Next_Click(object sender, RoutedEventArgs e)
        {
            if (Busy)
            {
                cancel?.Cancel();
                return;
            }
            ViewModel.Save();
            Busy = true;
            switch (PageNo)
            {
                case 0:
                    if (baseUrl.Validate())
                    {
                        try
                        {
                            cancel = new CancellationTokenSource();
                            await ViewModel.E1.DefaultConfigurationAsync(cancel.Token);
                            PageNo++;
                        }
                        catch (Exception ex)
                        {
                            baseUrl.SetError(ex.Message);
                        }
                    }
                    break;
                case 1:
                    if (username.Validate())
                    {
                        try
                        {
                            cancel = new CancellationTokenSource();
                            await ViewModel.E1.AuthenticateAsync(cancel.Token);
                            PageNo++;
                        }
                        catch (Exception ex)
                        {
                            username.SetError(ex.Message);
                        }
                    }
                    break;
                case 2:
                    break;
                default:
                    PageNo++;
                    break;
            }
            Busy = false;
        }
        CancellationTokenSource cancel { get; set; }
        readonly DebounceDispatcher dd = new DebounceDispatcher();
        readonly Doc.Settings ViewModel = Ioc.Default.GetRequiredService<Doc.Settings>();
        public Settings()
        {
            PageNo = 2;
            InitializeComponent();
            VersionLookup.DataRequest = async args
                => await ViewModel.E1.RequestAsync<F983051.Response>(new F983051.Request(args.Item1), args.Item2);
        }
    }
}
