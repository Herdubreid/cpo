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
        public Func<(string, CancellationToken), Task<Helpers.ILookupResponse>> DataRequest { get; set; }
        public void SetLabel(string label, InfoBarSeverity severity = InfoBarSeverity.Informational)
        {
            Label = label;
            Severity = severity;
        }
        public double LabelWidth { get; set; } = double.NaN;
        public double BoxWidth { get; set; } = double.NaN;
        public async Task<bool> Validate()
        {
            if (Severity == InfoBarSeverity.Success)
                return true;
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
        #region Severity
        public InfoBarSeverity Severity
        {
            get => (InfoBarSeverity)GetValue(SeverityProperty);
            set
            {
                SetValue(SeverityProperty, value);
                VisualStateManager.GoToState(this, value.ToString("g"), false);
            }
        }
        public static readonly DependencyProperty SeverityProperty =
            DependencyProperty.Register(nameof(Severity), typeof(InfoBarSeverity), typeof(LookupControl), new PropertyMetadata(default(InfoBarSeverity)));
        #endregion
        #region IsReadonly
        public bool IsReadonly
        {
            get => (bool)GetValue(IsReadonlyProperty);
            set => SetValue(IsReadonlyProperty, value);
        }
        public static readonly DependencyProperty IsReadonlyProperty =
            DependencyProperty.Register(nameof(IsReadonly), typeof(bool), typeof(LookupControl), new PropertyMetadata(false));
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
                        switch (rs.GetSummary().records)
                        {
                            case 0:
                                SetLabel("No matching records!", InfoBarSeverity.Warning);
                                break;
                            case 1:
                                SetLabel(rs.GetRows().First().Label, InfoBarSeverity.Success);
                                FieldValue = rs.GetRows().First().Key;
                                break;
                        }
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
