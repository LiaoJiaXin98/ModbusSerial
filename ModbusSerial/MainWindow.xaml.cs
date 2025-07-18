using Microsoft.Extensions.DependencyInjection;
using Wpf.Ui;
using Wpf.Ui.Abstractions;
using System.Windows.Input;
using ModbusSerial.ViewModels;
using ModbusSerial.Views;

namespace ModbusSerial
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
            this.MouseLeftButtonDown += MainWindow_MouseLeftButtonDown;
        }

        private void MainWindow_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            // 只有在点击空白区域时才允许拖拽
            if (e.Source == this || e.Source == NavigationControl)
            {
                try
                {
                    this.DragMove();
                }
                catch (InvalidOperationException)
                {
                    // 忽略拖拽时可能出现的异常
                }
            }
        }

        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);
           
        }
    }
}