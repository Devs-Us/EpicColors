using EpicColors.Patches.ColorTypes;
using HarmonyLib;
using System;
using System.Collections.Generic;
using UnhollowerRuntimeLib;
using UnityEngine;
using static EpicColors.CustomColorHandler;

namespace EpicColors.Handler
{
    [HarmonyPatch(typeof(AmongUsClient), nameof(AmongUsClient.Awake))]
    public static class AmongUsClientPatch
    {
        public static void Postfix()
        {
            try
            {
                var gameObject = GameObject.Find("EpicColor");
                if (gameObject != null) return;
                ClassInjector.RegisterTypeInIl2Cpp<Patches.Animated.AnimatedColors>();
                ClassInjector.RegisterTypeInIl2Cpp<EpicColors>();
                var epicColor = new GameObject("EpicColor");
                GameObject.DontDestroyOnLoad(epicColor);
                _ = epicColor.AddComponent<EpicColors>();
            }
            catch (Exception e)
            {
                Logger.ErrorLogger("registering type", e);
            }
        }
    }

    public class EpicColors : MonoBehaviour
    {
        public EpicColors(IntPtr ptr) : base(ptr) { }

        public void Update()
        {
            List<BaseColor> colorList = AllColors;
            for (int i = 0; i < colorList.Count; i++)
            {
                if (!colorList[i].IsSpecial) continue;
                _ = colorList.GetType();
                colorList[i].Timer = ((Time.deltaTime / colorList[i].Duration) + colorList[i].Timer) % 1f;
                Palette.PlayerColors[i + OldMainCount] = colorList[i].GetBodyColor();
                Palette.ShadowColors[i + OldMainCount] = colorList[i].GetShadowColor();
            }
        }
    }
}