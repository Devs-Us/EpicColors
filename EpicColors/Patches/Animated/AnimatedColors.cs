using System;
using UnityEngine;
using static EpicColors.CustomColorHandler;

namespace EpicColors.Patches.Animated
{
    public class AnimatedColors : MonoBehaviour
    {
        public int ColorId;
        public Renderer PlayerRender;

        public AnimatedColors(IntPtr ptr) : base(ptr)
        {
        }

        public void Initialize(int colorId, Renderer playerRender)
        {
            PlayerRender = playerRender;
            ColorId = colorId;
        }

        public void Update()
        {
            try
            {
                if (!PlayerRender) return;
                int paletteCount = (RemoveVanillaColors(out var oldColor) ? 0 : oldColor);
                PlayerRender.material.SetColor("_BodyColor", Palette.PlayerColors[ColorId + paletteCount]);
                PlayerRender.material.SetColor("_BackColor", Palette.ShadowColors[ColorId + paletteCount]);
            }
            catch { }
        }
    }
}
