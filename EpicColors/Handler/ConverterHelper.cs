using System;
using UnityEngine;
using System.Linq;
using static EpicColors.CustomColorHandler;

using PL = Palette;
using System.Collections.Generic;

namespace EpicColors
{
    public static class ConverterHelper {
        
        public static void ClearPalette()
		{
            int colorCount = AllColors.Count;
            PL.PlayerColors = Array.Empty<Color32>();
            PL.ShadowColors = Array.Empty<Color32>();
            PL.ColorNames = Array.Empty<StringNames>();
        }

        // Apply custom colors to the game
        public static void AddCustomColor(Color32 main, Color32 shadow, StringNames name) {
            var maincolor = PL.PlayerColors.ToList();
            var shadowcolor = PL.ShadowColors.ToList();
            var namecolor = PL.ColorNames.ToList();

            maincolor.Add(main);
            shadowcolor.Add(shadow);
            namecolor.Add(name);

            PL.PlayerColors = maincolor.ToArray();
            PL.ShadowColors = shadowcolor.ToArray();
            PL.ColorNames = namecolor.ToArray();
        }

        // Get color's name from string
        public static string RealColorName(this string data) {
            return data.Replace("_", " ");
        }

        public static string ToHexString(this Color32 c) {
		return string.Format ("{0:X2}{1:X2}{2:X2}", c.r, c.g, c.b);
	    }
	
	    public static int ToHex(this Color32 c) {
		    return (c.r<<16)|(c.g<<8)|(c.b);
	    }

        public static string ToHexString(this Color color){
		    Color32 c = color;
		    return c.ToHexString();
	    }
    }
}