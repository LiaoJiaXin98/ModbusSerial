using Microsoft.Extensions.DependencyInjection;
using WPFSerialDebug.ViewModels;

namespace WPFSerialDebug.Views;

public partial class DashboardView

{
    public DashboardView()
    {
        InitializeComponent();
        DataContext = App.Current.Services.GetService<DashboardViewModel>();
    }
}