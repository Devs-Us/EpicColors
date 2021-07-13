using HarmonyLib;
using System;
using System.Collections.Generic;
using UnhollowerRuntimeLib;
using UnityEngine;

namespace BetterColours.Source
{
    internal class AnimatedColours
    {
        public static List<(int id, float timer, float duration, Func<float, Color> bodyColour, Func<float, Color> backColour)> ColoursList = new List<(int id, float timer, float duration, Func<float, Color> bodyColour, Func<float, Color> backColour)>
        {
            (36, 0f, 120f, RainbowBodyColour, RainbowBackColour),
            (37, 0f, 130f, SeasonalBodyColour, SeasonalBackColour),
        };

        private static Color RainbowBodyColour(float clock)
        {
            return Color.HSVToRGB(clock, 1f, 1f);
        }

        private static Color RainbowBackColour(float clock)
        {
            return Color.HSVToRGB(clock, 1f, 0.6f);
        }

        private static Color SeasonalBodyColour(float clock)
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

        private static Color SeasonalBackColour(float clock)
        {
            float[] selectPoints = new float[] { 1f / 6f, 1f / 3f, 0.5f, 2f / 3f, 5f / 6f };
            float[] huePoints = new float[] { 1f / 12f, 1f / 6f, 1f / 3f, 23f / 36f, 5f / 6f };
            float hue = 0f;
            for (int i = 0; i < selectPoints.Length; i++)
            {
                if (clock > selectPoints[i]) hue = huePoints[i];
            }
            return Color.HSVToRGB(hue, 1f, 0.6f);
        }

        public class Coroutines : MonoBehaviour
        {
            public Coroutines(IntPtr ptr) : base(ptr)
            {

            }

            public void FixedUpdate()
            {
                for (int i = 0; i < ColoursList.Count; i++)
                {
                    float newTimer = (ColoursList[i].timer + (1f / ColoursList[i].duration)) % 1f;
                    ColoursList[i] = (ColoursList[i].id, newTimer, ColoursList[i].duration, ColoursList[i].bodyColour, ColoursList[i].backColour);
                    Palette.PlayerColors[ColoursList[i].id] = ColoursList[i].bodyColour(ColoursList[i].timer);
                    Palette.ShadowColors[ColoursList[i].id] = ColoursList[i].backColour(ColoursList[i].timer);
                }
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

        [HarmonyPatch(typeof(AmongUsClient), nameof(AmongUsClient.Awake))]
        public static class AmongUsClientPatch
        {
            public static void Postfix()
            {
                GameObject gameObject = GameObject.Find("BetterColours");
                if (gameObject != null) return;
                ClassInjector.RegisterTypeInIl2Cpp<Coroutines>();
                ClassInjector.RegisterTypeInIl2Cpp<AnimatedColour>();
                GameObject betterColours = new GameObject("BetterColours");
				UnityEngine.Object.DontDestroyOnLoad(betterColours);
				_ = betterColours.AddComponent<Coroutines>();
            }
        }

        [HarmonyPatch(typeof(PlayerControl), nameof(PlayerControl.SetPlayerMaterialColors), typeof(int), typeof(Renderer))]
        public class PlayerControlPatch
        {
            public static bool Prefix([HarmonyArgument(0)] int colorId, [HarmonyArgument(1)] Renderer rend)
            {
                AnimatedColour colour = rend.gameObject.GetComponent<AnimatedColour>();
                if (colour != null && colour.ColourId != colorId) UnityEngine.Object.Destroy(colour);
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

        [HarmonyPatch(typeof(PlayerTab), nameof(PlayerTab.Update))]
        public class PlayerTabPatch
        {
            public static void Postfix(PlayerTab __instance)
            {
                for (int i = 0; i < ColoursList.Count; i++) __instance.ColorChips[ColoursList[i].id].gameObject.GetComponent<SpriteRenderer>().color = Palette.PlayerColors[ColoursList[i].id];
            }
        }
    }
}
