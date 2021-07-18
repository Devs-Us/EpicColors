using System;
using UnityEngine;
using System.Text.RegularExpressions;
using System.Linq;
using System.IO;
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

        // Get main and shadow value from string (format)
        public static Tuple<Color32, Color32, bool> ToColorMainShadow(this string data) {
            var empty = new Color32();
            var main = empty;
            var shadow = empty;

            if (!data.Contains("main;"))
                return Tuple.Create(empty, empty, false);

            foreach (var colordata in data.Split(" ")) {
                var s = "shadow;";

                if (colordata.StartsWith("main;"))
                    main = colordata.StringToColor32();

                if (colordata.StartsWith(s))
                    shadow = colordata.StringToColor32();
                else if (!data.Contains(s))
                    shadow = Color32.Lerp(main, Color.black, .4f);

                ColorsPlugin.Logger.LogDebug(main.ToString() + shadow.ToString());
            }

            return 
                Tuple.Create(main, shadow, true);
        }

        // Get color's name from string
        public static string RealColorName(this string data) {
            return data.Replace("_", " ");
        }

        // This will convert the name to capitals during scan
        public static Tuple<string, bool> ToColorName(this string data) {
            var name = "";
            if (!data.Contains("name;"))
                    return Tuple.Create("", false);

            foreach (var colordata in data.Split(" ")) {
                name = colordata.StartsWith("name;") 
                ? colordata.ToUpper().Replace("NAME;","").Replace("_", "") : name;
            }
            return Tuple.Create(name, true);
        }

        // Convert from string (with format) to Color32
        public static Color32 StringToColor32(this string color) {

            // Input need to be "byte,byte,byte"
            string[] excludedword = {"shadow;","main;","name;"};
            var finalstring = color;

            foreach (string excluded in excludedword)
                if (color.Contains(excluded))
                    finalstring = finalstring.Replace(excluded, "");

            // Check if the string only contains 0 - 255 (byte)
            if (!finalstring.Split(',').IsByteOnly()) {
                finalstring = "1,1,1";
                ColorsPlugin.Logger.LogError($"There's an error while loading the color from strings. STRINGS_NOT_BYTE");
            }
            
            // Change to byte
            var rgb = Array.ConvertAll(finalstring
                .Split(',')
                .Select(c => c)
                .ToArray(), 
                byte.Parse);

            return
                new Color32(rgb[0], rgb[1], rgb[2], 255);
        }

        // Check if the string is convertable to byte or not
        public static bool IsByteOnly(this string[] value) {
            foreach (string val in value)
                try {
                    byte.Parse(val); 
                    return true;
                }
                catch (Exception e) {
                    ColorsPlugin.Logger.LogError($"Unable to convert to byte: {e}");
                }

            return false;
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