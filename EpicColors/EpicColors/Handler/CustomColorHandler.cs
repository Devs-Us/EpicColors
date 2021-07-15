using System.Collections.Generic;
using System.Linq;
using HarmonyLib;
using UnhollowerBaseLib;
using System.IO;
using System;

using TC = TranslationController;
using S = StringNames;
using PL = Palette;

namespace EpicColors
{
    public static class CustomColorHandler {
        public static List<string> CustomColorList = new List<string>();
        public static void CustomColor() {
            var ccPath = Path.Combine(Directory.GetCurrentDirectory(), "CustomColor.txt");

            if (File.Exists(ccPath))
                foreach (var datalist in File.ReadLines(ccPath)) {
                    ColorsPlugin.Logger.LogInfo(datalist);
                    CustomColorList.Add(datalist);
                }
        }
    }
}