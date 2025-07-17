using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;
using Wpf.Ui.Controls;
using WPFSerialDebug.Views;

namespace WPFSerialDebug.ViewModels;

public partial class MainWindowViewModel : ObservableObject
{
	[ObservableProperty]
	private ObservableCollection<object> _navigationItems = [];

	public MainWindowViewModel()
	{
		NavigationItems =
		[
			//new NavigationViewItem()
			//{
			//	Content = "���ڼ��",
			//	Icon = new SymbolIcon { Symbol = SymbolRegular.Home24 },
			//	TargetPageType = typeof(DashboardView)
			//},
			new NavigationViewItem()
			{
				Content = "Modbus Serial Dashboard",
				TargetPageType = typeof(ModbusDashboardView)
			},
		];
	}
}