
using Microsoft.Extensions.DependencyInjection;
using Wpf.Ui;
using Wpf.Ui.Abstractions;
using WPFSerialDebug.ViewModels;
using WPFSerialDebug.Views;

namespace WPFSerialDebug
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        public MainWindow()
        {
			InitializeComponent();
            this.DataContext = App.Current.Services.GetService<MainWindowViewModel>();
            var pageProvider = App.Current.Services.GetRequiredService<INavigationViewPageProvider>();
            NavigationControl.SetPageProviderService(pageProvider);
            Loaded += (_, _) => NavigationControl.Navigate(typeof(ModbusDashboardView));
        }

        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);
           
        }
    }
}