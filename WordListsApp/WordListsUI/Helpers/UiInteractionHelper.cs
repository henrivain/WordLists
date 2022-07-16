using Microsoft.Maui.Dispatching;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WordListsUI.Helpers;
internal static class UiInteractionHelper
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
