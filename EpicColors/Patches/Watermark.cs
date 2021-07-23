using HarmonyLib;
using UnityEngine;
using System.Linq;
using static EpicColors.CustomColorHandler;

namespace EpicColors
{
    [HarmonyPatch]
    [HarmonyPriority(Priority.Last)]
    public static class Watermark {
        public static string GetColorName(this int colorId) {
            var IdList = colorId - OldMainCount;

            var name = AllColors.ElementAtOrDefault(IdList) != null? 
            AllColors[IdList].Name : "";
            var color = Palette.PlayerColors[colorId].ToHexString();

            return name != ""? $"You are using <color=#{color}>{name}</color>": "";
        }

        [HarmonyPatch(typeof(PingTracker), nameof(PingTracker.Update))]
        private static class PingTrackerPatch
        {
            [HarmonyPostfix]
            static void WatermarkPatch(PingTracker __instance) {
                var t = __instance.text;
                var pos = __instance.transform.localPosition;
                var id = PlayerControl.LocalPlayer?.Data.ColorId;

                if (!t.text.Contains("\n")) {
                    __instance.transform.localPosition = new Vector3(pos.x, 2.8f, pos.z);
                    t.alignment = TMPro.TextAlignmentOptions.TopGeoAligned;
                }

                if (AmongUsClient.Instance.GameState != InnerNet.InnerNetClient.GameStates.Started) {
                    t.text += "\nEpicColors by Devs-Us <size=80%>v1.1</size>\n";
                    t.text += id?.GetColorName();
                    t.text += !RemoveVanillaColors ? (id >= OldMain.Count ? Author : "") : Author;
                }
            }
        }
    }
}