using HarmonyLib;
using UnityEngine;
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
                    Debug.logger.Log(Author);
                    t.text += "\nEpicColors by Devs-Us <size=80%>v1.0.0</size>\n";
                    t.text += $"You are using {id.GetColorName()}\n";
                    t.text += Author;
                }
            }
        }
    }
}