using Microsoft.Extensions.DependencyInjection;
using WPFSerialDebug.ViewModels;

namespace WPFSerialDebug.Views
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
