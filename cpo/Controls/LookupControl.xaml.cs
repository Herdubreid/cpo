using Celin.Services;
using Microsoft.Toolkit.Mvvm.DependencyInjection;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Celin
{

    public sealed partial class LookupControl : UserControl
    {
        public DataTemplate ItemTemplate { get; set; }
        public string PlaceHolderText { get; set; }
        public string Header { get; set; }
        public Func<(string, CancellationToken), Task<Helpers.ILookupResponse>> DataRequest { get; set; }
        public void SetLabel(string label, InfoBarSeverity severity = InfoBarSeverity.Informational)
        {
            Label = label;
            SetSeverity(severity);
        }
        public void SetSeverity(InfoBarSeverity severity)
            => VisualStateManager.GoToState(this, severity.ToString("g"), false);
        public double LabelWidth { get; set; } = double.NaN;
        public double BoxWidth { get; set; } = double.NaN;
        public async Task<bool> Validate()
        {
            cancel?.Cancel();
            cancel?.Dispose();
            cancel = new CancellationTokenSource();
            try
            {
                var rs = await DataRequest.Invoke((FieldValue, cancel.Token));
                if (rs.GetSummary().records > 0 && rs.GetRows().First().Key.Equals(FieldValue, StringComparison.OrdinalIgnoreCase))
                {
                    SetLabel(rs.GetRows().First().Label);
                    return true;
                }    
                else
                {
                    SetLabel("Field invalid!", InfoBarSeverity.Error);
                    return false;
                }
            }
            catch (OperationCanceledException)
            {
                return false;
            }
            catch (Exception ex)
            {
                SetLabel(ex.Message, InfoBarSeverity.Error);
                return false;
            }
        }
        #region Readonly
        public bool Readonly
        {
            get => (bool)GetValue(ReadonlyProperty);
            set => SetValue(ReadonlyProperty, value);
        }
        public static readonly DependencyProperty ReadonlyProperty =
            DependencyProperty.Register(nameof(Readonly), typeof(bool), typeof(LookupControl), new PropertyMetadata(false));
        #endregion
        #region FieldValue
        public string FieldValue
        {
            get => (string)GetValue(FieldValueProperty);
            set => SetValue(FieldValueProperty, value);
        }
        public static readonly DependencyProperty FieldValueProperty =
            DependencyProperty.Register(nameof(FieldValue), typeof(string), typeof(LookupControl), new PropertyMetadata(default(string)));
        #endregion
        #region Label
        public string Label
        {
            get => (string)GetValue(LabelProperty);
            set => SetValue(LabelProperty, value);
        }
        public static readonly DependencyProperty LabelProperty =
            DependencyProperty.Register(nameof(Label), typeof(string), typeof(LookupControl), new PropertyMetadata(default(string)));
        #endregion
        readonly E1 e1 = Ioc.Default.GetRequiredService<E1>();
        readonly DebounceDispatcher dd = new DebounceDispatcher();
        CancellationTokenSource cancel;
        bool isSelecting;
        private void SuggestionChosen(AutoSuggestBox sender, AutoSuggestBoxSuggestionChosenEventArgs args)
        {
            var row = args.SelectedItem as Helpers.DataRow;
            sender.Text = row.Key;
            SetLabel(row.Label);
            isSelecting = true;
        }
        private void TextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
        {
            if (isSelecting)
            {
                isSelecting = false;
                return;
            }
            dd.Debounce(500, async (ob) =>
            {
                var box = ob as AutoSuggestBox;
                if (!string.IsNullOrWhiteSpace(box.Text))
                {
                    cancel?.Cancel();
                    cancel?.Dispose();
                    cancel = new CancellationTokenSource();
                    try
                    {
                        var rs = await DataRequest.Invoke((box.Text.Trim().ToUpper(), cancel.Token));
                        box.ItemsSource = rs.GetRows();
                        if (rs.GetSummary().records == 0)
                            SetLabel("No matching records!", InfoBarSeverity.Warning);
                    }
                    catch (OperationCanceledException) { }
                    catch (Exception ex)
                    {
                        SetLabel(ex.Message, InfoBarSeverity.Error);
                    }
                }
            }, sender);
        }
        private void OnGotFocus(object sender, RoutedEventArgs e)
        {
            (sender as Control).Width = contentRoot.ActualWidth;
        }
        private void OnLostFocus(object sender, RoutedEventArgs e)
        {
            (sender as Control).Width = BoxWidth;
        }
        public LookupControl()
        {
            InitializeComponent();
        }
    }
}
