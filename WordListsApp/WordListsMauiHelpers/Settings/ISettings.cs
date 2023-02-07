namespace WordListsMauiHelpers.Settings;
public interface ISettings
{
    string? DefaultParserName { get; set; }
    bool? ShuffleTrainingWords { get; set; }
    bool? ShowLearnedWords { get; set; }
    bool? ShowWeakWords { get; set; }
    bool? ShowUnheardWords { get; set; }
    bool? RemoveUserDataWhenExporting { get; set; }
    string? DefaultWordCollectionLanguage { get; set; }

    event SettingsChangedEventHandler SettingChanged;
}
