﻿using BepInEx;
using BepInEx.IL2CPP;
using HarmonyLib;
using System;
using UnityEngine.SceneManagement;
using System.Linq;
using BepInEx.Logging;
using System.Collections.Generic;

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
        public static string[] BuiltInColor = {
            // Static color
            "name;Acid_Green main;124,155,10                                ",
            "name;Aqua_Blue  main;2,90,143                                  ",
            "name;Blood_Red main;151,0,0                                    ",
            "name;Chocolate main;89,52,0                                    ",
            "name;Flame main;236,109,0                                      ",
            "name;Crimson main;167,0,24                                     ",
            "name;Gold main;218,156,32                                      ",
            "name;Mint main;168,255,195                                     ",
            "name;Lavender main;201,146,224                                 ",
            "name;Midnight_Blue main;55,24,182                              ",
            "name;Jungle_Green main;43,78,39                                ",
            "name;Light_Pink main;236,178,170                               ",
            "name;Panda main;255,255,255 shadow;12,12,12                    ",
            "name;Mustard main;198,193,5                                    ",
            "name;Blurple main;88,99,240                                    ",
            "name;NavyBlue main;29,0,112                                    ",
            "name;Teal main;0,128,128                                       ",
            "name;Olive main;99,114,24                                      ",
            "name;Peach main;255,229,180                                    ",
            "name;Lapis_Lazuli main;38,97,156                               ",
            "name;Silver main;192,192,192                                   ",
            "name;Cadmium_Yellow main;255,255,0                             ",
            "name;Brazilwood main;189,166,133                               ",
            "name;Mummybrown main;143,75,40                                 ",
            "name;Quercitron main;229,176,61                                ",
            "name;Cochineal main;159,35,45                                  ",

            // Special colors
            "name;Rainbow main;1,1,1 special;rainbow                        ",
            "name;Seasonal main;1,1,1 special;seasonal                      "
            };

        public static void LoadColors() {
            if (WasRun) return;

            ModManager.Instance.ShowModStamp();
            CustomColorHandler.CustomColor();

            foreach (string colorfin in BuiltInColor)
            {
                var (main, shadow, isNotNull) = colorfin.ToColorMainShadow();
                var (name, isTrueName) = colorfin.ToColorName();
                
                if (isNotNull && isTrueName && ConverterHelper.IncludeBuiltinColor())
                    ConverterHelper.AddCustomColor(main, shadow, name.NewStringNames());      
            }

            foreach (string data in CustomColorHandler.CustomColorList) {
                var (main, shadow, isNotNull) = data.ToColorMainShadow();
                var (name, isTrueName) = data.ToColorName();

                if (isNotNull && isTrueName)
                    ConverterHelper.AddCustomColor(main, shadow, name.NewStringNames());
            }

            WasRun = true;
        }
    }
}