using Microsoft.Maui.Controls.Platform;

namespace WordListsUI.WordDataPages.ListGeneratorPage;

#if WINDOWS
internal class WordAddPage : ContentPage
{
    string? Value { get; set; }
    private WordAddPage(
        string title,
        string message,
        string accept,
        string cancel,
        string placeholder,
        uint maxLength,
        Keyboard? keyboard,
        string? initialValue)
    {
        Title = title;
        Value = initialValue;
        HeightRequest = 200;
        WidthRequest = 400;
        Background = Colors.White;
        Content = new VerticalStackLayout
        {
        Children = {
                new Label
                {
                    HorizontalOptions = LayoutOptions.Start,
                    VerticalOptions = LayoutOptions.Start,
                    Margin = 20,
                    Text = message
                },
                new Entry
                {
                    WidthRequest = 300,
                    VerticalOptions = LayoutOptions.Center,
                    Placeholder = placeholder,
                    Keyboard = keyboard ?? Keyboard.Default,
                    MaxLength = maxLength > int.MaxValue ? int.MaxValue : (int)maxLength,
                },
                new HorizontalStackLayout
                {
                    HorizontalOptions = LayoutOptions.End,
                    Margin = 20,
                    Children =
                    {
                        new Button
                        {
                            Text = cancel,
                        },
                        new Button
                        {
                            Text = accept,
                        }
                    }
                }
            }
        };
    }


    public static async Task<string?> Show(
        string title,
        string message,
        string accept = "OK",
        string cancel = "Cancel",
        string placeholder = "",
        uint maxLength = int.MaxValue,
        Keyboard? keyboard = null,
        string? initialValue = null)
    {
        WordAddPage page = new(title, message, accept, cancel, placeholder, maxLength, keyboard, initialValue);
        await Shell.Current.Navigation.PushModalAsync(page);
        return page.Value;
    }

    async Task Close() 
    { 
        await Shell.Current.Navigation.PopModalAsync();

    }



}
#endif