using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WordListsMauiHelpers;
public class ColorHelper
{


    public static Color GetResourceColor(ResourceColor color)
    {
        Color result = color switch
        {
            ResourceColor.NeverHeardWord => Get("NeverHeardWord"),
            ResourceColor.MightKnowWord => Get("MightKnowWord"),
            ResourceColor.LearnedWord => Get("LearnedWord"),
            ResourceColor.DefaultFlipCard => Get("FlipCardColor"),
            _ => throw new NotImplementedException($"Given color is not implemented; was given: {color}")
        };

        if (result is null)
        {
            throw new NotImplementedException($"Given color is was not found from resources; was given: {color}");
        }
        return result;
    }

    private static Color Get(string key)
    {
        if (Application.Current.Resources.TryGetValue(key, out var result))
        {
            return (Color)result;
        }
        return null;
    }
}
