using System;
using EpicColors.Handler;
using UnityEngine;

namespace EpicColors.Types.Animated
{
	public class AnimatedColors : MonoBehaviour
	{
		public Int32 ColorId;
		public Renderer PlayerRender;

		public AnimatedColors(IntPtr ptr) : base(ptr)
		{
		}

		public void Initialize(Int32 colorId, Renderer playerRender)
		{
			PlayerRender = playerRender;
			ColorId = colorId;
		}

		public void Update()
		{
			if (!PlayerRender) return;
			PlayerRender.material.SetColor("_BodyColor", Palette.PlayerColors[ColorId + ColorData.OldMainCount]);
			PlayerRender.material.SetColor("_BackColor", Palette.ShadowColors[ColorId + ColorData.OldMainCount]);
		}
	}
}
