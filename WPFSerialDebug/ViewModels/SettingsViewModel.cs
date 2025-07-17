using CommunityToolkit.Mvvm.ComponentModel;

namespace WPFSerialDebug.ViewModels;

public class SettingsViewModel : ObservableObject
{
    public string UrlPathSegment => "settings";
    public SettingsViewModel()
    {
    }
}
