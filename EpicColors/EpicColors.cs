using System;
using System.Collections.Generic;
using BepInEx;
using BepInEx.IL2CPP;
using BepInEx.Logging;
using EpicColors.Extensions;
using EpicColors.Handler;
using EpicColors.Types.ColorTypes;
using HarmonyLib;
using UnhollowerRuntimeLib;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace EpicColors
{
	[BepInPlugin(Id, "EpicColors", Version)]
	[BepInProcess("Among Us.exe")]
	public class EpicColors : BasePlugin
	{
		static internal ManualLogSource Logger;
		public const String Id = "DevsUs.EpicColors";
		public const String Version = "1.1.0";

		public Harmony Harmony { get; } = new Harmony(Id);

		public override void Load()
		{
			Logger = Log;
			Harmony.PatchAll();
		}
	}

	[HarmonyPatch]
	public static class AmongUsAwake
	{
		static Boolean _patched = false;

		[HarmonyPatch(typeof(AmongUsClient))]
		[HarmonyPatch(nameof(AmongUsClient.Awake))]
		[HarmonyPostfix]
		public static void OnAwake()
		{
			if (_patched) return;
			_patched = true;

			ModManager.Instance.ShowModStamp();
			ColorData.Load();

			if (ColorData.RemoveVanillaColors)
				Helpers.ClearPalette();

			ColorData.AllColors.ForEach(x => x.AddCustomColor());

			try
			{
				ClassInjector.RegisterTypeInIl2Cpp<Types.Animated.AnimatedColors>();
				ClassInjector.RegisterTypeInIl2Cpp<EpicColorsComponent>();
				GameObject epicColors = new("EpicColors");
				UnityEngine.Object.DontDestroyOnLoad(epicColors);
				_ = epicColors.AddComponent<EpicColorsComponent>();
			}

			catch (Exception e)
			{
				EpicColors.Logger.LogError("There's an error while trying to register type, this might causes EpicColors not to load properly.");
				EpicColors.Logger.LogError(e.ToString());
			}
		}
	}

	public class EpicColorsComponent : MonoBehaviour
	{
		public EpicColorsComponent(IntPtr ptr) : base(ptr) { }

		public void Update()
		{
			List<BaseColor> colorList = ColorData.AllColors;
			for (Int32 i = 0; i < colorList.Count; i++)
			{
				if (!colorList[i].IsSpecial) continue;
				_ = colorList.GetType();
				colorList[i].Timer = ((Time.deltaTime / colorList[i].Duration) + colorList[i].Timer) % 1f;
				Palette.PlayerColors[i] = colorList[i].GetBodyColor();
				Palette.ShadowColors[i] = colorList[i].GetShadowColor();
			}
		}
	}
}