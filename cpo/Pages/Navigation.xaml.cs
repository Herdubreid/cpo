using Microsoft.Toolkit.Mvvm.DependencyInjection;
using Microsoft.UI.Xaml.Controls;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Celin.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Navigation : Page
    {
        readonly Services.Navigate Nav = Ioc.Default.GetRequiredService<Services.Navigate>();
        public Navigation()
        {
            InitializeComponent(); 
            Nav.ContentFrame = ContentFrame;
        }
    }
}
