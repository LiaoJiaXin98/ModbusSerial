using Microsoft.Extensions.DependencyInjection;
using ModbusSerial.ViewModels;

namespace ModbusSerial.Views
{
    /// <summary>
    /// ModbusDashboardViewModel.xaml 的交互逻辑
    /// </summary>
    public partial class ModbusDashboardView 
    {
        public ModbusDashboardView()
        {
            InitializeComponent();
            DataContext = App.Current.Services.GetService<ModbusDashboardViewModel>();
        }
    }
}
