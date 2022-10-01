using System;
using System.Diagnostics.CodeAnalysis;

namespace WordListsUI.WordTrainingPages.WritingTestPage.GridManagement;
internal class WriteTestResultGridHelper : WordTrainingPageGridHelper
{
    public WriteTestResultGridHelper(Grid baseGrid, VerticalStackLayout infoGrid) : base(baseGrid, infoGrid) { }

    internal override bool ReSize()
    {
        return base.ReSize();

        //if (ChooseState()) return true;
        //InfoGrid.MaximumWidthRequest = 600;

        //switch (State)
        //{
        //    case GridState.ExtraBig:
        //        break;
        //    case GridState.Big:
        //        break;
        //    case GridState.Normal:
        //        break;
        //    case GridState.Small:
        //        break;

        //}

        //InfoGrid.Margin = new Thickness(25,0,0,0);
        //return false;
    }

  
}
