using System.Diagnostics.CodeAnalysis;

namespace WordListsMauiHelpers.Settings;
public class SettingsChangedEventArgs : EventArgs
{
    public SettingsChangedEventArgs() { }

    public SettingsChangedEventArgs(string? settingName, object? value)
	{
        SettingName = settingName;
        Value = value;
    }

    public string? SettingName { get; init; }
    public object? Value { get; init; }
}
