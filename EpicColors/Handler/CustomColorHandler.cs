using System.Collections.Generic;
using System.Linq;
using System.IO;
using UnityEngine;

namespace EpicColors
{
    public static class CustomColorHandler {
        public static readonly int OldPaletteCount = Palette.PlayerColors.Length;
        public static List<string> AllCCList = new List<string>();
        public static List<string> TxtContentList = new List<string>();
        public static List<string> CustomColorList = new List<string>();
        public static List<string> SpecialColorList = new List<string>();

        public static void CustomColor() {
            var ccPath = Path.Combine(Directory.GetCurrentDirectory(), "CustomColors.txt");

            // Read CustomColors.txt contents and add to list
            if (File.Exists(ccPath))
                foreach (var datalist in File.ReadLines(ccPath))
                    TxtContentList.Add(datalist);
            
            // Filter TxtContentList with "name;" to prevent wrong calculation when using "Count()"
            var allcc = TxtContentList.ToList();
            allcc.RemoveAll(x => !x.Contains("name;"));
            CustomColorList = allcc;

            // Append all color list into one
            AllCCList = EpicColors.BuiltInColor.ToList();
            foreach (string s in CustomColorList)
                AllCCList.Add(s + " custom;");
        }

        // Option for color creator to turn off built in color
        public static bool IncludeBuiltinColor() {
            var custom = string.Join(" ", TxtContentList);
            return
                custom.Contains("removeBuiltIn;") ? false : true;
        }

        // I want this to declare whether the player is using a custom color or not
        // and are they using built in one or they own.
        public static bool IsUsingCustomColor(int id, out bool CustomColor) {
            CustomColor = id >= OldPaletteCount + EpicColors.BuiltInColor.Count();
            return id > OldPaletteCount;
        }

        // Check if the string is convertable to byte or not
        public static bool IsByteOnly(this string[] value) {
            foreach (string val in value)
                try {
                    byte.Parse(val); 
                    return true;
                }
                catch (System.Exception e) {
                    Logger.ErrorLogger("converting string to byte", e);
                }

            return false;
        }

    }
}