﻿namespace WordListsUI.WordTrainingPage.FlipCardControl;
internal class SlideAnimation
{
    internal SlideAnimation(VisualElement element)
    {
        if (element is null) throw new ArgumentNullException(nameof(element));
        Element = element;
    }
    internal SlideAnimation(VisualElement element, double maxOffsetFromMiddle) : this(element)
    {
        MaxOffsetFromMiddle = maxOffsetFromMiddle;
    }

    private VisualElement Element { get; set; }

    public double MaxOffsetFromMiddle = 50;
    public uint Length { get; set; } = 125;


    public async void ToMaxLeft()
    {
        await Element.TranslateTo(-MaxOffsetFromMiddle, 0, Length, Easing.Linear);
    }
    
    public async void ToMaxRight()
    {
        await Element.TranslateTo(MaxOffsetFromMiddle, 0, Length, Easing.Linear);
    }

    public async void FromMaxRightToMiddle()
    {
        Element.TranslationX = MaxOffsetFromMiddle;
        await Element.TranslateTo(0, 0, Length, Easing.Linear);
    }

    public async void FromMaxLeftToMiddle()
    {
        Element.TranslationX = -MaxOffsetFromMiddle;
        await Element.TranslateTo(0, 0, Length, Easing.Linear);
    }
}
