using Microsoft.Extensions.Logging;
using System.Runtime.CompilerServices;
using static Microsoft.Maui.Storage.Preferences;

namespace WordListsMauiHelpers.Settings;
public class Settings : ISettings
{
    public Settings(ILogger logger)
    {
        Logger = logger;
    }

    public string? DefaultParserName { get => Get<string?>(); set => Set(value ?? string.Empty); }
    public string? DefaultWordCollectionLanguage { get => Get<string?>(); set => Set(value ?? string.Empty); }
    public bool? ShuffleTrainingWords { get => Get<bool?>(); set => Set(value ?? false); }
    public bool? ShowLearnedWords { get => Get<bool?>();  set => Set(value ?? false); }
    public bool? ShowUnheardWords { get => Get<bool?>();  set => Set(value ?? false); }
    public bool? ShowWeakWords { get => Get<bool?>();  set => Set(value ?? false); }
    public bool? RemoveUserDataWhenExporting { get => Get<bool?>();  set => Set(value ?? false); }
    ILogger Logger { get; }

    public event SettingsChangedEventHandler? SettingChanged;


    /// <summary>
    /// Get T? value from Preferences.Default. 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="key"></param>
    /// <returns>Saved value if defined, if value is not defined, returns null or default(T)</returns>
    private T? Get<T>([CallerMemberName]string? key = null)
    {
        if (string.IsNullOrWhiteSpace(key))
        {
            Logger.LogError("Cannot get setting value, because key is empty.");
            throw new ArgumentNullException("Cannot get value from preferences, because key is empty", nameof(key));
        }
#if ANDROID
        if (typeof(T) == typeof(bool?))
        {
            if (Default.ContainsKey(key) is false)
            {
                return default;
            }
            // have to box this, because java in android device crashes whilst trying convert bool to string
            // Because of boxing, I can always return the bool coming from get
            return (T?)(object)Default.Get(key, false);
        }
#endif
#pragma warning disable CS8604 // Possible null reference argument.
        var result = Default.Get<T>(key, default);
#pragma warning restore CS8604 // Possible null reference argument.
        return result;
    }


    /// <summary>
    /// Save given value to given key in Preferences.Default.
    /// KEY MUST HAVE MATCHING PROPERTY name in this object. 
    /// By default gets the key from caller property.
    /// Also CALLS ONSETTINGCHANGED if risesChangedEvent is set false.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="value"></param>
    /// <param name="key"></param>
    /// <param name="risesChangedEvent"></param>
    /// <exception cref="ArgumentNullException">If key is null or empty</exception>
    private void Set<T>(T value, [CallerMemberName] string? key = null, bool risesChangedEvent = true) where T : notnull
    {
        if (string.IsNullOrWhiteSpace(key))
        {
            Logger.LogError("Cannot set setting value, if key is null or empty");
            throw new ArgumentNullException("Cannot set setting value, if key is null or empty", nameof(key));
        }
        if (value is null)
        {
            return;
        }
        // throws with nullable types
        Default.Set(key, value);
        if (risesChangedEvent)
        {
            OnSettingChanged(value, key);
        }
    }

    /// <summary>
    /// Invokes SettingChanged event. 
    /// Automatically gets calling property name and value if they are not passed as params.
    /// </summary>
    /// <param name="value"></param>
    /// <param name="propertyName"></param>
    /// <exception cref="ArgumentNullException">if key is null or whitespace</exception>
    private void OnSettingChanged(object? value = null, [CallerMemberName]string? propertyName = null) 
    {
        if (string.IsNullOrWhiteSpace(propertyName))
        {
            Logger.LogError("Cannot invoke settings changed, because property name is empty.");
            throw new ArgumentNullException(nameof(propertyName));
        }
        
        value ??= typeof(Settings).GetProperty(propertyName)?.GetValue(this);

        SettingChanged?.Invoke(this, new SettingsChangedEventArgs
        {
             SettingName = propertyName,
             Value = value
        });
    }
}
