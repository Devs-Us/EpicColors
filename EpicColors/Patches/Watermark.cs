using HarmonyLib;
using UnityEngine;
using static EpicColors.CustomColorHandler;

namespace EpicColors
{
    [HarmonyPatch]
    [HarmonyPriority(Priority.Last)]
    public static class Watermark {
        public static string GetColorName(this int colorId) {
            colorId -= RemoveVanillaColors(out var oldColor) ? 0 : oldColor;
            var name = AllColors[colorId].Name;
            var color = Palette.PlayerColors[PlayerControl.LocalPlayer.Data.ColorId].ToHexString();

            return $"You are using <color=#{color}>{name}</color>\n";
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
                    try {
                        t.text += id.GetColorName();
                        t.text += !RemoveVanillaColors(out _) ? (id >= OldMain.Count ? Author : "") : Author;
                    } catch {}
                }
            }
        }
    }
}