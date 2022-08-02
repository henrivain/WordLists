namespace WordListsUI.Helpers;
internal static class UIInteractionHelper
{
    internal static void FocusITextInputText(ITextInput input, BindableObject dispatcher)
    {
        dispatcher.Dispatcher.Dispatch(async () =>
        {
            // these ui changes will not appear if no delay is added, bug maybe?
            await Task.Delay(50);
            input.CursorPosition = 0;
            input.SelectionLength = (input.Text is null) ? 0 : input.Text.Length;
        });
    }

}
