using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using UnityEngine;

using PL = Palette;

namespace EpicColors
{
    [HarmonyPriority(Priority.Last)]
    [HarmonyPatch]
    public static class Watermark {

        public static Tuple<bool,string> ToAuthor(this int colorId) {
            var ccPath = Path.Combine(Directory.GetCurrentDirectory(), "CustomColor.txt");
            if (!File.Exists(ccPath)) 
                return Tuple.Create(false, "");

            foreach (var author in File.ReadLines(ccPath)) {
                if (author.StartsWith("author;")) {
                    var finalauthor = author.Replace("author;","").Replace("_", " ");
                    return colorId > (ConverterHelper.includeBuiltinColor() ? EpicColors.builtInColor.Count() - 1 : -1) ? 
                        Tuple.Create(true, finalauthor) : 
                        Tuple.Create(false, "");
                }
            }
            return Tuple.Create(false, "");
        }

        public static string GetColorName(this int colorId) {
            var cclist = CustomColorHandler.CustomColorList;
            cclist.RemoveAll(x => !x.Contains("name;"));

            var name = cclist[colorId].RealColorName();
            var color = Palette.PlayerColors[colorId + EpicColors.OldPaletteCount].ToHexString();

            return $"<color=#{color}>{name}</color>";
        }

        [HarmonyPatch(typeof(PingTracker), nameof(PingTracker.Update))]
        private static class PingTrackerPatch
        {
            static void Postfix(PingTracker __instance) {
               
                var t = __instance.text;
                var pos = __instance.transform.localPosition;
                var id = PlayerControl.LocalPlayer ? PlayerControl.LocalPlayer.Data.ColorId - EpicColors.OldPaletteCount : -1;
                var (exists, name) = id.ToAuthor();
                var builtInlist = EpicColors.AllCustomColorList;

                if (!t.text.Contains("\n")) {
                    __instance.transform.localPosition = new Vector3(pos.x, 2.8f, pos.z);
                    t.alignment = TMPro.TextAlignmentOptions.TopGeoAligned;
                }

                t.text += "\nEpicColors by Devs-Us\n";

                if (ConverterHelper.includeBuiltinColor()) {
                    t.text += PlayerControl.LocalPlayer.Data.ColorId > 17 
                    ? $"You are using <color=#{Palette.PlayerColors[id + EpicColors.OldPaletteCount].ToHexString()}>{builtInlist[id].RealColorName()}</color>\n" : "";
                } else {
                   t.text += PlayerControl.LocalPlayer.Data.ColorId > 17 ? $"You are using {id.GetColorName()}\n" : "";
                }

                t.text += exists ? name : "";
            }
        }
    }
}