using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Reflection;
using UnhollowerRuntimeLib;
using UnityEngine;
using static EpicColors.CustomColorHandler;

namespace EpicColors.Patches.Animated
{
    class Animated
    {
        [HarmonyPatch(typeof(PlayerControl), nameof(PlayerControl.SetPlayerMaterialColors), typeof(int), typeof(Renderer))]
        public class PlayerControlPatch
        {
            public static bool Prefix([HarmonyArgument(0)] int colorId, [HarmonyArgument(1)] Renderer rend)
            {
                colorId -= RemoveVanillaColors(out var oldColor) ? 0 : oldColor;
                AnimatedColors colorComponent = rend.gameObject.GetComponent<AnimatedColors>();
                if (colorComponent != null && colorComponent.ColorId != colorId) 
                    UnityEngine.Object.Destroy(colorComponent);
                else if (colorComponent != null) return true;
                for (int i = 0; i < CustomColorHandler.AllColors.Count; i++)
                {
					ColorTypes.BaseColor color = CustomColorHandler.AllColors[i];
                    if (!color.IsSpecial || color.Id != colorId) continue;
                    colorComponent = rend.gameObject.AddComponent<AnimatedColors>();
                    colorComponent.Initialize(colorId, rend);
                    return false;
                }
                return true;
            }
        }
    }
}
