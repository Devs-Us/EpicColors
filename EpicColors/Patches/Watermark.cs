using HarmonyLib;
using System;
using System.Linq;
using System.IO;
using UnityEngine;
using static EpicColors.ConverterHelper;
using static EpicColors.CustomColorHandler;

namespace EpicColors
{
    [HarmonyPatch]
    [HarmonyPriority(Priority.Last)]
    public static class Watermark {
        public static string GetColorName(this int colorId) {
            var name = AllColors[colorId].Name;
            var color = Palette.PlayerColors[colorId].ToHexString();

            return $"<color=#{color}>{name}</color>";
        }

        [HarmonyPatch(typeof(PingTracker), nameof(PingTracker.Update))]
        private static class PingTrackerPatch
        {
            static void Postfix(PingTracker __instance) {
               
                var t = __instance.text;
                var pos = __instance.transform.localPosition;
                var id = PlayerControl.LocalPlayer ? PlayerControl.LocalPlayer.Data.ColorId : -1;

                if (!t.text.Contains("\n")) {
                    __instance.transform.localPosition = new Vector3(pos.x, 2.8f, pos.z);
                    t.alignment = TMPro.TextAlignmentOptions.TopGeoAligned;
                }

                if (AmongUsClient.Instance.GameState != InnerNet.InnerNetClient.GameStates.Started) {
                    t.text += "\nEpicColors by Devs-Us\n";
                    t.text += $"You are using {id.GetColorName()}\n";
                    t.text += Author;
                }
            }
        }

        public static string ToAuthor(this int colorId) {
            var name = "";
            foreach (var author in TxtContentList)
                if (author.StartsWith("author;") && 
                IsUsingCustomColor(colorId, out bool customColor) && customColor) {
                    var finalAuthor = author.Replace("author;","");
                    name += finalAuthor + "\n";
                }
            return name;
        }

        public static string GetColorName(this int colorId) {
            var name = IncludeBuiltinColor() ? AllCCList[colorId-OldPaletteCount].RealColorName() 
            : CustomColorList[colorId-OldPaletteCount].RealColorName();
            var color = Palette.PlayerColors[colorId].ToHexString();

            return IsUsingCustomColor(colorId+1, out _) 
            ? $"You are using <color=#{color}>{name}</color>\n" : "";
        }
    }
}