using BepInEx;
using BepInEx.IL2CPP;
using HarmonyLib;
using System;
using UnhollowerBaseLib;
using UnityEngine;
using TMPro;

using P = PlayerTab;
using PL = Palette;
using S = StringNames;
using TC = TranslationController;

namespace BetterColours
{
    [BepInPlugin(Id, "BetterColours", Version)]
    [BepInProcess("Among Us.exe")]
    public class BetterColoursPlugin : BasePlugin
    {
        public const string Id = "com.devsus.bettercolors";
        public const string Version = "0.1.0";

        public Harmony Harmony { get; } = new Harmony(Id);

        public override void Load()
        {
            ACN();
            APC();
            ACS();

            Harmony.PatchAll();
        }

        // This will add the color's name
        public static void ACN() => PL.ColorNames = new[]
           {
                S.ColorRed,
                S.ColorBlue,
                S.ColorGreen,
                S.ColorPink,
                S.ColorOrange,
                S.ColorYellow,
                S.ColorBlack,
                S.ColorWhite,
                S.ColorPurple,
                S.ColorBrown,
                S.ColorCyan,
                S.ColorLime,
                S.ColorMaroon,
                S.ColorRose,
                S.ColorBanana,
                S.ColorGray,
                S.ColorTan,
                S.ColorCoral,

                // New colours goes here
                (S)999900, // "AcidGreen"
                (S)999901, // "AquaBlue"
                (S)999902, // "BloodRed"
                (S)999903, // "Chocolate"
                (S)999904, // "Flame"
                (S)999905, // "Crimson"
                (S)999906, // "Gold"
                (S)999907, // "Mint"
                (S)999908, // "Lavender"
                (S)999909, // "NightBlue"
                (S)999910, // "JungleGreen"
                (S)999911, // "LightPink"
                (S)999912, // "BlackWhite"
                (S)999913, // "Mustard"
                (S)999914, // "Blurple"
                (S)999915, // "NavyBlue"
                (S)999916, // "Teal"
                (S)999917, // "Olive"
            };

        // Add the colors 
        public static void APC() => PL.PlayerColors = new[]
            {
                //* DON'T TOUCH THIS *//
                new Color32(198, 17, 17, byte.MaxValue),
                new Color32(19, 46, 210, byte.MaxValue),
                new Color32(17, 128, 45, byte.MaxValue),
                new Color32(238, 84, 187, byte.MaxValue),
                new Color32(240, 125, 13, byte.MaxValue),
                new Color32(246, 246, 87, byte.MaxValue),
                new Color32(63, 71, 78, byte.MaxValue),
                new Color32(215, 225, 241, byte.MaxValue),
                new Color32(107, 47, 188, byte.MaxValue),
                new Color32(113, 73, 30, byte.MaxValue),
                new Color32(56, 255, 221, byte.MaxValue),
                new Color32(80, 240, 57, byte.MaxValue),
                PL.FromHex(6233390),
                PL.FromHex(15515859),
                PL.FromHex(15787944),
                PL.FromHex(7701907),
                PL.FromHex(9537655),
                PL.FromHex(14115940),
                //* DON'T TOUCH THIS *//

                // New colours goes here
                new Color32(124, 155, 10, byte.MaxValue),
                new Color32(2, 90, 143, byte.MaxValue),
                new Color32(151, 0, 0, byte.MaxValue),
                new Color32(89, 52, 0, byte.MaxValue),
                new Color32(236, 109, 0, byte.MaxValue),
                new Color32(167, 0, 24, byte.MaxValue),
                new Color32(218, 165, 32, byte.MaxValue),
                new Color32(168, 255, 195, byte.MaxValue),
                new Color32(201, 146, 224, byte.MaxValue),
                new Color32(55, 24, 182, byte.MaxValue),
                new Color32(43, 78, 39, byte.MaxValue),
                new Color32(236, 178, 170, byte.MaxValue),
                new Color32(255, 255, 255, byte.MaxValue),
                new Color32(198, 193, 5, byte.MaxValue),
                new Color32(88, 99, 240, byte.MaxValue),
                new Color32(29, 0, 112, byte.MaxValue),
                new Color32(0, 128, 128, byte.MaxValue),
                new Color32(99, 114, 24, byte.MaxValue)
            };

        // Add the shadows
        public static void ACS() => PL.ShadowColors = new[]
            {
                // DON'T TOUCH THIS //
                new Color32(122, 8, 56, byte.MaxValue),
                new Color32(9, 21, 142, byte.MaxValue),
                new Color32(10, 77, 46, byte.MaxValue),
                new Color32(172, 43, 174, byte.MaxValue),
                new Color32(180, 62, 21, byte.MaxValue),
                new Color32(195, 136, 34, byte.MaxValue),
                new Color32(30, 31, 38, byte.MaxValue),
                new Color32(132, 149, 192, byte.MaxValue),
                new Color32(59, 23, 124, byte.MaxValue),
                new Color32(94, 38, 21, byte.MaxValue),
                new Color32(36, 169, 191, byte.MaxValue),
                new Color32(21, 168, 66, byte.MaxValue),
                PL.FromHex(4263706),
                PL.FromHex(14586547),
                PL.FromHex(13810825),
                PL.FromHex(4609636),
                PL.FromHex(5325118),
                PL.FromHex(11813730),
                // DON'T TOUCH THIS //

                // New colours goes here
                new Color32(101, 116, 10, byte.MaxValue),
                new Color32(20, 47, 143, byte.MaxValue),
                new Color32(70, 0, 0, byte.MaxValue),
                new Color32(39, 24, 0, byte.MaxValue),
                new Color32(178, 47, 0, byte.MaxValue),
                new Color32(86, 8, 24, byte.MaxValue),
                new Color32(156, 117, 22, byte.MaxValue),
                new Color32(123, 186, 143, byte.MaxValue),
                new Color32(156, 113, 173, byte.MaxValue),
                new Color32(28, 8, 124, byte.MaxValue),
                new Color32(0, 62, 29, byte.MaxValue),
                new Color32(255, 101, 174, byte.MaxValue),
                new Color32(12, 12, 12, byte.MaxValue),
                new Color32(163, 157, 7, byte.MaxValue),
                new Color32(71, 81, 200, byte.MaxValue),
                new Color32(21, 0, 58, byte.MaxValue),
                new Color32(0, 100, 100, byte.MaxValue),
                new Color32(66, 91, 15, byte.MaxValue),
            };

        [HarmonyPatch(typeof(TC), "GetString", new[]
        {
            typeof(S),
            typeof(Il2CppReferenceArray<Il2CppSystem.Object>)
        })]

        public static class CNP
        {
            public static bool Prefix(ref string __result, [HarmonyArgument(0)] S name)
            {
                var colorString = (int)name switch
                {
                    999900  =>  "ACIDGREEN",
                    999901  =>  "AQUABLUE",
                    999902  =>  "BLOODRED",
                    999903  =>  "CHOCOLATE",
                    999904  =>  "FLAME",
                    999905  =>  "CRIMSON",
                    999906  =>  "GOLD",
                    999907  =>  "MINT",
                    999908  =>  "LAVENDER",
                    999909  =>  "NIGHTBLUE",
                    999910  =>  "JUNGLEGREEN",
                    999911  =>  "LIGHTPINK",
                    999912  =>  "PANDA",
                    999913  =>  "MUSTARD",
                    999914  =>  "BLURPLE",
                    999915  =>  "NAVYBLUE",
                    999916  =>  "TEAL",
                    999917  =>  "OLIVE",

                    _ =>
                    default // everything else is null
                };

                if (colorString == null) return true;

                __result = colorString;
                return false;
            }
        }

        public static class SCROLL
        {
            // Soon™
        }

        // Make color selector more compact
        [HarmonyPatch(typeof(P), "OnEnable")]
        public static class CMPCT
        {
            public static void Postfix(P __instance)
            {
                var p = __instance;

                foreach (var colorChip in p.ColorChips)
                    GameObject.Destroy(colorChip.gameObject);

                p.ColorChips.Clear();

                for (var i = 0; i < PL.PlayerColors.Length; i++)
                {
                    var _ = -0.935f + (i % 5 * 0.47f);
                    var __ = 1.65f - (i / 5 * 0.47f);

                    var cc = GameObject.Instantiate(p.ColorTabPrefab, p.ColorTabArea, true);
                    cc.Inner.transform.localScale *= 0.76f;
                    cc.Inner.transform.localPosition = new Vector3(_, __, -1f);

                    var j = i;
                    cc.Button.OnClick.AddListener((Action)delegate
                    {
                        p.SelectColor(j);
                    });

                    cc.Inner.color = PL.PlayerColors[i];
                    p.ColorChips.Add(cc);
                }
            }
        }

        [HarmonyPatch(typeof(P), "SelectColor")]
        public static class H
        {
            // Change hat's color when changing
            public static void Postfix(P __instance, [HarmonyArgument(0)] int colorId)
            {
                var p = __instance;
                p.HatImage.SetColor(colorId);
            }
        }

        [HarmonyPatch(typeof(PingTracker), "Update")]
        private static class CreditPing
        {
            private static void Postfix(PingTracker __instance)
            {
                __instance.text.alignment = TextAlignmentOptions.Center;
                if (AmongUsClient.Instance.GameState != InnerNet.InnerNetClient.GameStates.Started)
                {
                    var t = __instance.text;
                    t.text += "\nBetterColours by DevsUs\n";
                }
            }
        }
    }
}