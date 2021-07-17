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

        public static List<(Color32 main, Color32 shadow, StringNames name)> 
        OldCCList = new List<(Color32 main, Color32 shadow, StringNames name)>();

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

        internal class AnimatedColor
        {
            public static List<(Color32 main, StringNames name, string real)> ColorList = 
            new List<(Color32 main, StringNames name, string real)>
            {
                
            };
        }
    }
}