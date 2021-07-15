using System;
using UnityEngine;
using System.Text.RegularExpressions;
using System.Linq;
using System.IO;

using PL = Palette;

namespace EpicColors
{
    public static class ConverterHelper {

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

        private static int SortColors(Color32 a, Color32 b)
        {
            if (a.r < b.r)
                return 1;
            else if (a.r > b.r)
                return -1;
            else 
            {
                if (a.g < b.g)
                    return 1;
                else if (a.g > b.g)
                    return -1;
                else 
                {
                    if (a.b < b.b)
                        return 1;
                    else if (a.b > b.b)
                        return -1;
                }
            }
            return 0;
        }

        public static string ToMainHex(this PlayerControl player) {
            return Palette.PlayerColors[player.Data.ColorId].ToHexString();
        }
        public static string RealColorName(this string data) {
            var name = "";
            if (!data.Contains("name;"))
                    return "";

            foreach (var colordata in data.Split(" ")) {
                name = colordata.StartsWith("name;") 
                ? colordata.Replace("name;","").Replace("_", " ") : name;
            }
            return name;
        }

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
            
            var rgb = Array.ConvertAll(finalstring
                .Split(',')
                .Select(c => c)
                .ToArray(), 
                byte.Parse);

            return
                new Color32(rgb[0], rgb[1], rgb[2], 255);
        }

        public static bool includeBuiltinColor() {
            var ccPath = Path.Combine(Directory.GetCurrentDirectory(), "CustomColor.txt");
            if (!File.Exists(ccPath)) return true;

            var custom = string.Join(" ", File.ReadLines(ccPath));
            return
                custom.Contains("removeBuiltIn;") ? false : true;
        }

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