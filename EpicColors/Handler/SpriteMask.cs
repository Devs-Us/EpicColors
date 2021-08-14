using System;
using UnhollowerBaseLib;
using UnityEngine;

namespace EpicColors.Handler
{
	public static class SpriteHelpers
	{
		public static Sprite GetSpriteMask()
		{
			try
			{
				Byte[] imageBytes = Convert.FromBase64String(mask);
				Texture2D tex = new(2, 2);
				ICall_LoadImage = IL2CPP.ResolveICall<d_LoadImage>("UnityEngine.ImageConversion::LoadImage");
				Il2CppStructArray<Byte> il2CPPArray = imageBytes;
				_ = ICall_LoadImage.Invoke(tex.Pointer, il2CPPArray.Pointer, false);
				return Sprite.Create(tex, new Rect(0.0f, 0.0f, tex.width, tex.height), new Vector2(0.5f, 0.5f), 100.0f);
			}
			catch (Exception e)
			{
				EpicColors.Logger.LogError(e);
			}
			return null;
		}

		internal delegate Boolean d_LoadImage(IntPtr tex, IntPtr data, Boolean markNonReadable);
		internal static d_LoadImage ICall_LoadImage;

		internal static String mask =
			"iVBORw0KGgoAAAANSUhEUgAAAAEAAAABCAI" +
			"AAACQd1PeAAAAAXNSR0IArs4c6QAAAARnQ" +
			"U1BAACxjwv8YQUAAAAJcEhZcwAADsMAAA7DAcd" +
			"vqGQAAAAMSURBVBhXY/j//z8ABf4C/qc1gYQAAAA" +
			"ASUVORK5CYII=";
	}
}