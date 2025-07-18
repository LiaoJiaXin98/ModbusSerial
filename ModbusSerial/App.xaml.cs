using FluentModbus;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System.Windows;
using ModbusSerial.Model;
using ModbusSerial.ViewModels;
using ModbusSerial.Views;
using Wpf.Ui;
using Wpf.Ui.DependencyInjection;

namespace ModbusSerial
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public sealed partial class App : Application
    {
        public App()
        {
            Services = ConfigureServices();
            this.InitializeComponent();
        }
        public new static App Current => (App)Application.Current;
        public IServiceProvider Services { get; }
        private static IServiceProvider ConfigureServices()
        {
            var services = new ServiceCollection();

			services.TryAddKeyedTransient<ModbusClient,ModbusTcpClient>(ModbusConnectionType.TCP);
            services.TryAddKeyedTransient<ModbusClient, ModbusRtuClient>(ModbusConnectionType.RTU);

            services.AddSingleton<MainWindowViewModel>();
            services.AddTransient<ModbusDashboardViewModel>();

            services.AddTransient<ModbusDashboardView>();


			services.AddNavigationViewPageProvider();
            services.AddSingleton<INavigationService, NavigationService>();
			App.Current.DispatcherUnhandledException += static (s, e) => {
				var uiMessageBox = new Wpf.Ui.Controls.MessageBox
				{
					Title = "错误",
					Content = e?.Exception?.Message?.ToString(),
				};
				uiMessageBox.ShowDialogAsync();
                return;
			};
			return services.BuildServiceProvider();
        }
    }
}