using HarmonyLib;
using System;
using System.Collections.Generic;
using UnhollowerRuntimeLib;
using UnityEngine;

using static EpicColors.Logger;
using static EpicColors.CustomColorHandler;

namespace EpicColors
{
    internal class AnimatedColors
    {
        public static List<(int id, float timer, float duration, Func<float, Color> bodyColor)> ColorsList = new()
		{
            ((OldPaletteCount+EpicColors.BuiltInColor.Length) -2, 0f, 120f, RainbowColor),
            ((OldPaletteCount+EpicColors.BuiltInColor.Length) -1, 0f, 130f, SeasonalColor),
        };

        public class Coroutines : MonoBehaviour
        {
            public Coroutines(IntPtr ptr) : base(ptr) {}

            public void FixedUpdate()
            {
                try {
                    for (int i = 0; i < ColorsList.Count; i++)
                    {
                        float newTimer = (ColorsList[i].timer + (1f / ColorsList[i].duration)) % 1f;
                        ColorsList[i] = (ColorsList[i].id, newTimer, ColorsList[i].duration, ColorsList[i].bodyColor);
                        Palette.PlayerColors[ColorsList[i].id] = ColorsList[i].bodyColor(ColorsList[i].timer);
                        Palette.ShadowColors[ColorsList[i].id] = Color32.Lerp(ColorsList[i].bodyColor(ColorsList[i].timer), Color.black, .4f);
                    }
                } catch {}
            }
        }

        public class AnimatedColor : MonoBehaviour
        {
            public int ColorId;
            public Renderer PlayerRender;

            public AnimatedColor(IntPtr ptr) : base(ptr)
            {
            }

            public void Initialize(int colorId, Renderer playerRender)
            {
                PlayerRender = playerRender;
                ColorId = colorId;
            }

            public void Update()
            {
                try {
                    if (!PlayerRender) return;
                    PlayerRender.material.SetColor("_BodyColor", Palette.PlayerColors[ColorId]);
                    PlayerRender.material.SetColor("_BackColor", Palette.ShadowColors[ColorId]);
                } catch{}
            }
        }

        public static Color RainbowColor(float clock)
        {
            return Color.HSVToRGB(clock, 1f, 1f);
        }

        public static Color SeasonalColor(float clock)
        {
            var selectPoints = new float[] { 1f / 6f, 1f / 3f, 0.5f, 2f / 3f, 5f / 6f };
            var huePoints = new float[] { 1f / 12f, 1f / 6f, 1f / 3f, 23f / 36f, 5f / 6f };
            var hue = 0f;
            for (int i = 0; i < selectPoints.Length; i++)
            {
                if (clock > selectPoints[i]) hue = huePoints[i];
            }
            return Color.HSVToRGB(hue, 1f, 1f);
        }

        [HarmonyPatch(typeof(AmongUsClient), nameof(AmongUsClient.Awake))]
        public static class AmongUsClientPatch
        {
            public static void Postfix()
            {
                try {
                    var gameObject = GameObject.Find("EpicColor");
                    if (gameObject != null) return;
                    ClassInjector.RegisterTypeInIl2Cpp<Coroutines>();
                    ClassInjector.RegisterTypeInIl2Cpp<AnimatedColor>();
                    var epicColor = new GameObject("EpicColor");
				    GameObject.DontDestroyOnLoad(epicColor);
				    _ = epicColor.AddComponent<Coroutines>();
                } catch (Exception e) {
                    ErrorLogger("registering type", e);
                }
            }
        }

        [HarmonyPatch(typeof(PlayerControl), nameof(PlayerControl.SetPlayerMaterialColors), typeof(int), typeof(Renderer))]
        public class PlayerControlPatch
        {
            public static bool Prefix([HarmonyArgument(0)] int colorId, [HarmonyArgument(1)] Renderer rend)
            {
                var color = rend.gameObject.GetComponent<AnimatedColor>();
                if (color != null && color.ColorId != colorId) GameObject.Destroy(color);
                else if (color != null) return true;
                for (int i = 0; i < ColorsList.Count; i++)
                {
                    if (ColorsList[i].id != colorId) continue;
                    color = rend.gameObject.AddComponent<AnimatedColor>();
                    color.Initialize(colorId, rend);
                    return false;
                }
                return true;
            }
        }
    }
}
