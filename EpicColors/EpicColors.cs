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

    // WARNING: THIS BAD CODE MAY HURT YOUR EYES
    // TODO: Separate special colors from built in array
    public class EpicColors {

        // Somehow it got called triple times O_O
        private static bool WasRun = false;
        public static string[] defaultConfig = {
            // Document details
            "author;",
            "", // Normal colors
            "name;Red main;198,17,17 shadow;122,56,8",
            "name;Blue main;19,210,46 shadow;9,142,21",
            "name;Green main;17,45,128 shadow;10,46,77",
            "name;Pink main;238,187,84 shadow;172,174,43",
            "name;Orange main;240,13,125 shadow;180,21,62",
            "name;Yellow main;246,87,246 shadow;195,34,136",
            "name;Black main;63,78,71 shadow;30,38,31",
            "name;White main;215,241,225 shadow;132,192,149",
            "name;Purple main;107,188,47 shadow;59,124,23",
            "name;Brown main;113,30,73 shadow;94,21,38",
            "name;Cyan main;56,221,255 shadow;36,191,169",
            "name;Lime main;80,57,240 shadow;21,66,168",
            "name;Maroon main;95,46,29 shadow;65,26,15",
            "name;Rose main;236,211,192 shadow;222,179,146",
            "name;Banana main;240,168,231 shadow;210,137,188",
            "name;Gray main;117,147,133 shadow;70,100,86",
            "name;Tan main;145,119,136 shadow;81,62,65",
            "name;Coral main;215,100,100 shadow;180,98,67",
            "", // Static colors
            "name;Acid_Green main;124,155,10",
            "name;Aqua_Blue  main;2,90,143",
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
            "", // Special colors
            "name;Rainbow special;hue duration;5 main;100,100 shadow;100,65",
            "name;Seasonal special;refresh duration;6 main;255,0,0>255,255,0>0,255,0>0,255,255>0,0,255>255,0,255 shadow;166,0,0>166,166,0>0,166,0>0,166,166>0,0,166>166,0,166",
            };

        public static void LoadColors() {
            if (WasRun) return;
            WasRun = true;

            ModManager.Instance.ShowModStamp();
            CustomColorHandler.CustomColor();

            ConverterHelper.ClearPalette();
            foreach (Patches.ColorTypes.BaseColor data in CustomColorHandler.AllColors) ConverterHelper.AddCustomColor(data.GetBodyColor(), data.GetShadowColor(), data.Name.NewStringNames());

            WasRun = true;
        }
    }
}