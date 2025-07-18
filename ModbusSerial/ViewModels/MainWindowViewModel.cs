using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;
using ModbusSerial.Views;
using Wpf.Ui.Controls;

namespace ModbusSerial.ViewModels;

public partial class MainWindowViewModel : ObservableObject
{
	[ObservableProperty]
	private ObservableCollection<object> _navigationItems = [];

	public MainWindowViewModel()
	{
		NavigationItems =
		[
			new NavigationViewItem()
			{
				Content = "Modbus Serial Dashboard",
				TargetPageType = typeof(ModbusDashboardView)
			},
		];
	}
}