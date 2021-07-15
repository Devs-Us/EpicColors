using HarmonyLib;
using System;
using System.Collections.Generic;
using UnhollowerRuntimeLib;
using UnityEngine;

namespace EpicColors
{
    internal class AnimatedColours
    {
        public static List<(int id, float timer, float duration, Func<float, Color> bodyColour)> ColoursList = new List<(int id, float timer, float duration, Func<float, Color> bodyColour)>
        {
            ((EpicColors.OldPaletteCount+EpicColors.builtInColor.Length) -2, 0f, 120f, RainbowColour),
            ((EpicColors.OldPaletteCount+EpicColors.builtInColor.Length) -1, 0f, 130f, SeasonalColour),
        };

        public class Coroutines : MonoBehaviour
        {
            public Coroutines(IntPtr ptr) : base(ptr) {}

            public void FixedUpdate()
            {
                try {
                    for (int i = 0; i < ColoursList.Count; i++)
                    {
                        float newTimer = (ColoursList[i].timer + (1f / ColoursList[i].duration)) % 1f;
                        ColoursList[i] = (ColoursList[i].id, newTimer, ColoursList[i].duration, ColoursList[i].bodyColour);
                        Palette.PlayerColors[ColoursList[i].id] = ColoursList[i].bodyColour(ColoursList[i].timer);
                        Palette.ShadowColors[ColoursList[i].id] = Color32.Lerp(ColoursList[i].bodyColour(ColoursList[i].timer), Color.black, .4f);
                    }
                } catch {}
            }
        }

        public class AnimatedColour : MonoBehaviour
        {
            public int ColourId;
            public Renderer PlayerRender;

            public AnimatedColour(IntPtr ptr) : base(ptr)
            {
            }

            public void Initialize(int colourId, Renderer playerRender)
            {
                PlayerRender = playerRender;
                ColourId = colourId;
            }

            public void Update()
            {
                if (!PlayerRender) return;
                PlayerRender.material.SetColor("_BodyColor", Palette.PlayerColors[ColourId]);
                PlayerRender.material.SetColor("_BackColor", Palette.ShadowColors[ColourId]);
            }
        }

        public static Color RainbowColour(float clock)
        {
            return Color.HSVToRGB(clock, 1f, 1f);
        }

        public static Color SeasonalColour(float clock)
        {
            float[] selectPoints = new float[] { 1f / 6f, 1f / 3f, 0.5f, 2f / 3f, 5f / 6f };
            float[] huePoints = new float[] { 1f / 12f, 1f / 6f, 1f / 3f, 23f / 36f, 5f / 6f };
            float hue = 0f;
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
                var gameObject = GameObject.Find("BetterColours");
                if (gameObject != null) return;
                ClassInjector.RegisterTypeInIl2Cpp<Coroutines>();
                ClassInjector.RegisterTypeInIl2Cpp<AnimatedColour>();
                var betterColours = new GameObject("BetterColours");
				GameObject.DontDestroyOnLoad(betterColours);
				_ = betterColours.AddComponent<Coroutines>();
            }
        }

        [HarmonyPatch(typeof(PlayerControl), nameof(PlayerControl.SetPlayerMaterialColors), typeof(int), typeof(Renderer))]
        public class PlayerControlPatch
        {
            public static bool Prefix([HarmonyArgument(0)] int colorId, [HarmonyArgument(1)] Renderer rend)
            {
                var colour = rend.gameObject.GetComponent<AnimatedColour>();
                if (colour != null && colour.ColourId != colorId) GameObject.Destroy(colour);
                else if (colour != null) return true;
                for (int i = 0; i < ColoursList.Count; i++)
                {
                    if (ColoursList[i].id != colorId) continue;
                    colour = rend.gameObject.AddComponent<AnimatedColour>();
                    colour.Initialize(colorId, rend);
                    return false;
                }
                return true;
            }
        }
    }
}
