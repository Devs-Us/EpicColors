using System;
using System.Linq;
using EpicColors.Extensions;
using EpicColors.Handler;
using HarmonyLib;
using UnityEngine;

namespace EpicColors.Patches
{
	[HarmonyPatch]
	[HarmonyPriority(Priority.Last)]
	public static class Watermark
	{
		public static String GetColorName(this Int32 colorId)
		{
			Int32 IdList = colorId - ColorData.OldMainCount;

			String name = ColorData.AllColors.ElementAtOrDefault(IdList) != null ?
			ColorData.AllColors[IdList].Name : "";
			String color = Palette.PlayerColors[colorId].ToHexString();

			return name != "" ? $"You are using <color=#{color}>{name}</color>" : "";
		}

		[HarmonyPatch(typeof(PingTracker), nameof(PingTracker.Update))]
		private static class PingTrackerPatch
		{
			[HarmonyPostfix]
			static void WatermarkPatch(PingTracker __instance)
			{
				TMPro.TextMeshPro t = __instance.text;
				Vector3 pos = __instance.transform.localPosition;
				Int32 id = PlayerControl.LocalPlayer?.Data.ColorId ?? -1;

				if (!t.text.Contains("\n"))
				{
					__instance.transform.localPosition = new Vector3(pos.x, 2.8f, pos.z);
					t.alignment = TMPro.TextAlignmentOptions.TopGeoAligned;
				}

				if (AmongUsClient.Instance.GameState != InnerNet.InnerNetClient.GameStates.Started)
				{
					t.text += "\nEpicColors by Devs-Us <size=80%>v1.1</size>\n";
					t.text += id != -1 ? id.GetColorName() : String.Empty;
					t.text += !ColorData.RemoveVanillaColors ? 
						(id >= ColorData.OldMain.Count ? ColorData.Author : "") : ColorData.Author;
				}
			}
		}
	}
}