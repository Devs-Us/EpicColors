using EpicColors.Handler;
using EpicColors.Types.ColorTypes;
using HarmonyLib;
using UnityEngine;

namespace EpicColors.Types.Animated
{
	class Animated
	{
		[HarmonyPatch(typeof(PlayerControl), nameof(PlayerControl.SetPlayerMaterialColors), typeof(System.Int32), typeof(Renderer))]
		public class PlayerControlPatch
		{
			public static System.Boolean Prefix([HarmonyArgument(0)] System.Int32 colorId, [HarmonyArgument(1)] Renderer rend)
			{
				colorId -= ColorData.OldMainCount;
				AnimatedColors colorComponent = rend.gameObject.GetComponent<AnimatedColors>();
				if (colorComponent != null && colorComponent.ColorId != colorId)
					Object.Destroy(colorComponent);
				else if (colorComponent != null) return true;
				for (System.Int32 i = 0; i < ColorData.AllColors.Count; i++)
				{
					BaseColor color = ColorData.AllColors[i];
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
