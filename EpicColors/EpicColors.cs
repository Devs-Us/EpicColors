using BepInEx;
using BepInEx.IL2CPP;
using HarmonyLib;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using BepInEx.Logging;
using UnhollowerRuntimeLib;

using static EpicColors.Logger;
using static EpicColors.CustomColorHandler;
using static EpicColors.ConverterHelper;

namespace EpicColors
{
    [BepInPlugin(Id, "EpicColors", Version)]
    [BepInProcess("Among Us.exe")]
    public class ColorsPlugin : BasePlugin
    {
        static internal ManualLogSource Logger;
        public const string Id = "DevsUs.EpicColors";
        public const string Version = "1.0.0";

        public Harmony Harmony { get; } = new Harmony(Id);

        public override void Load()
        {
            Logger = Log;

            // Because some mods overwrite PL.PlayerColors if
            // it is loaded after EpicColors
            SceneManager.add_sceneLoaded((System.Action<Scene, LoadSceneMode>)((_, __) => 
            EpicColors.LoadColors()));

            Harmony.PatchAll();
        }
    }

    public class EpicColors {

        // Somehow it got called triple times O_O
        private static bool WasRun = false;
        public static string[] defaultConfig = {
            // Static colors
            "name;Acid_Green main;124,155,10",
            "name;Aqua_Blue main;2,90,143",
            "name;Blood_Red main;151,0,0",
            "name;Chocolate main;89,52,0",
            "name;Flame main;236,109,0",
            "name;Crimson main;167,0,24",
            "name;Gold main;218,156,32",
            "name;Mint main;168,255,195",
            "name;Lavender main;201,146,224",
            "name;Midnight_Blue main;55,24,182",
            "name;Jungle_Green main;43,78,39",
            "name;Light_Pink main;236,178,170",
            "name;Panda main;255,255,255 shadow;12,12,12",
            "name;Mustard main;198,193,5",
            "name;Blurple main;88,99,240",
            "name;NavyBlue main;29,0,112",
            "name;Teal main;0,128,128",
            "name;Olive main;99,114,24",
            "name;Peach main;255,229,180",
            "name;Lapis_Lazuli main;38,97,156",
            "name;Silver main;192,192,192",
            "name;Cadmium_Yellow main;255,255,0",
            "name;Brazilwood main;189,166,133",
            "name;Mummybrown main;143,75,40",
            "name;Quercitron main;229,176,61",
            "name;Cochineal main;159,35,45",
            "", 
            // Special colors
            "name;Rainbow special;hue duration;5 main;100,100 shadow;100,65",
            "name;Seasonal special;refresh duration;6 main;255,0,0>255,255,0>0,255,0>0,255,255>0,0,255>255,0,255 shadow;166,0,0>166,166,0>0,166,0>0,166,166>0,0,166>166,0,166",
            ""
            };

        public static void LoadColors() {
            if (WasRun) return;
            WasRun = true;

            ModManager.Instance.ShowModStamp();
            CustomColorHandler.CustomColor();

            if (RemoveVanillaColors)
                ConverterHelper.ClearPalette();
            
            foreach (var data in CustomColorHandler.AllColors)
                ConverterHelper.AddCustomColor(data.GetBodyColor(), data.GetShadowColor(), 
                    data.Name.NewStringNames());
        }
    }
}