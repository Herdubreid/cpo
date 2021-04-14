using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Celin
{
    public sealed partial class TextControl : UserControl
    {
        public IEnumerable<string> MessageList { get; set; } = Enumerable.Empty<string>();
        public Regex ValueTest { get; set; } = new Regex(".*");
        public string PlaceHolderText { get; set; }
        public string Header { get; set; }
        public void SetError(string msg)
        {
            Message = msg;
            Severity = InfoBarSeverity.Error;
        }
        public bool Validate()
        {
            Severity = ValueTest.IsMatch(FieldValue)
                ? InfoBarSeverity.Success
                : InfoBarSeverity.Warning;
            Message = MessageList.ElementAtOrDefault((int)Severity) ?? string.Empty;
            return Severity == InfoBarSeverity.Success;
        }
        #region Readonly
        public bool Readonly
        {
            get => (bool)GetValue(ReadonlyProperty);
            set => SetValue(ReadonlyProperty, value);
        }
        public static readonly DependencyProperty ReadonlyProperty =
            DependencyProperty.Register(nameof(Readonly), typeof(bool), typeof(TextControl), new PropertyMetadata(0));
        #endregion
        #region FieldValue
        public string FieldValue
        {
            get => (string)GetValue(FieldValueProperty);
            set
            {
                SetValue(FieldValueProperty, value);
                Validate();
            }
        }
        public static readonly DependencyProperty FieldValueProperty =
            DependencyProperty.Register(nameof(FieldValue), typeof(string), typeof(TextControl), new PropertyMetadata(default(string)));
        #endregion
        #region Message
        public string Message
        {
            get => (string)GetValue(MessageProperty);
            set
            {
                IsOpen = !string.IsNullOrEmpty(value);
                SetValue(MessageProperty, value);
            }
        }
        public static readonly DependencyProperty MessageProperty =
            DependencyProperty.Register(nameof(Severity), typeof(int), typeof(TextControl), new PropertyMetadata(0));
        #endregion
        #region Severity
        public InfoBarSeverity Severity
        {
            get => (InfoBarSeverity)GetValue(SeverityProperty);
            set { SetValue(SeverityProperty, value); }
        }
        public static readonly DependencyProperty SeverityProperty =
            DependencyProperty.Register(nameof(Severity), typeof(InfoBarSeverity), typeof(TextControl), new PropertyMetadata(default(InfoBarSeverity)));
        #endregion
        #region IsOpen
        public bool IsOpen
        {
            get { return (bool)GetValue(IsOpenProperty); }
            set { SetValue(IsOpenProperty, value); }
        }
        public static readonly DependencyProperty IsOpenProperty =
            DependencyProperty.Register(nameof(IsOpen), typeof(bool), typeof(TextControl), new PropertyMetadata(default(bool)));
        #endregion
        public TextControl()
        {
            InitializeComponent();
        }
    }
}
