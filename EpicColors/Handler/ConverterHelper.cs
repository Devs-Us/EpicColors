using System;
using System.Linq;
using UnityEngine;
using static Palette;

namespace EpicColors
{
    public static class ConverterHelper
    {
        public static void ClearPalette()
        {
            PlayerColors = Array.Empty<Color32>();
            ShadowColors = Array.Empty<Color32>();
            ColorNames = Array.Empty<StringNames>();
        }

        // Apply custom colors to the game
        public static void AddCustomColor(Color32 main, Color32 shadow, StringNames name)
        {
            PlayerColors = PlayerColors.Concat(new Color32[] { main }).ToArray();
            ShadowColors = ShadowColors.Concat(new Color32[] { shadow }).ToArray();
            ColorNames = ColorNames.Concat(new StringNames[] { name }).ToArray();
        }

        // Get color's name from string
        public static string RealColorName(this string data) =>
            data.Replace("_", " ");

        public static string ToHexString(this Color32 c) =>
            string.Format("{0:X2}{1:X2}{2:X2}", c.r, c.g, c.b);

        public static int ToHex(this Color32 c) =>
            (c.r << 16) | (c.g << 8) | (c.b);
    }
}