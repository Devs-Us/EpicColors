using System.Collections.Generic;
using System.Linq;
using System.IO;
using UnityEngine;
using static Palette;

using EpicColors.Patches.ColorTypes;

namespace EpicColors
{
    public static class CustomColorHandler {
        public static List<Color32> OldMain = PlayerColors.ToList();
        public static List<Color32> OldShadow = ShadowColors.ToList();
        public static List<StringNames> OldNames = ColorNames.ToList();
        public static List<BaseColor> AllColors = new();
        private static List<string> txtContentList = new();
        public static string Author = "";

        public static void CustomColor() {
            var ccPath = Path.Combine(Directory.GetCurrentDirectory(), "CustomColors.txt");

            // Read CustomColors.txt contents and add to list
            if (File.Exists(ccPath)) 
                foreach (var datalist in File.ReadLines(ccPath)) 
                    txtContentList.Add(datalist);
            else
			{
				string defaultLines = ConfigBuilder.BuildDefaultConfig();
                txtContentList = defaultLines.Split('\n').ToList();
            }

            foreach (string content in txtContentList)
            {
                int index = content.IndexOf("author;");
                if (index == -1) continue;
                Author = content[(index + 7)..];
            }

            int idTracker = 0;
            foreach (string colorLine in txtContentList)
                if (colorLine.Contains("name;")) 
                    AllColors.Add(StringToObject(colorLine, idTracker++));
        }

        private static BaseColor StringToObject(string colorLine, int id)
        {
            string[] colorData = colorLine.Split(' ');
            foreach (string colorFieldString in colorData)
            {
                string[] colorField = colorFieldString.Split(';');
                if (colorField.Length != 2 || colorField[0] != "special") continue;
                switch (colorField[1])
                {
                    case "hue":
                    {
                        Hue colorAnim = new() { Id = id };
                        colorAnim.Initialize(colorLine);
                        return colorAnim;
                    }
                    case "refresh":
                    {
                        Refresh colorAnim = new() { Id = id };
                        colorAnim.Initialize(colorLine);
                        return colorAnim;
                    }
                }
            }
            Static color = new();
            color.Initialize(colorLine);
            return color;
        }

        public static bool RemoveVanillaColors(out int oldColor) {
            oldColor = OldMain.Count;
            return txtContentList.Contains("RemoveVanillaColors;") 
                ? true : false;
        }
    }
}