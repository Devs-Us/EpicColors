using System;
using System.Linq;
using EpicColors.Handler;
using EpicColors.Types.ColorTypes;
using UnityEngine;
using static Palette;

namespace EpicColors.Extensions
{
	public static class Helpers
	{
		/// <summary>
		/// Emptys <see cref="PlayerColors"/>, <see cref="ShadowColors"/> and <see cref="ColorNames"/> array.
		/// </summary>
		public static void ClearPalette()
		{
			PlayerColors = ShadowColors = Array.Empty<Color32>();
			ColorNames = Array.Empty<StringNames>();
		}

		/// <summary>
		/// Add colors to <see cref="PlayerColors"/>, <see cref="ShadowColors"/> and <see cref="ColorNames"/> array from <see cref="BaseColor"/>.
		/// </summary>
		/// <param name="baseColor">targeted <see cref="BaseColor"/> class</param>
		public static void AddCustomColor(this BaseColor baseColor)
		{
			PlayerColors = PlayerColors.Concat(new Color32[] { baseColor.GetBodyColor() }).ToArray();
			ShadowColors = ShadowColors.Concat(new Color32[] { baseColor.GetShadowColor() }).ToArray();
			ColorNames = ColorNames.Concat(new StringNames[] { StringHelpers.NewStringNames(baseColor.Name) }).ToArray();
		}

		/// <summary>
		/// Replaces "_" to a space.
		/// </summary>
		/// <param name="data">targeted string.</param>
		/// <returns></returns>
		public static String RealColorName(this String data)
		{
			return data.Replace("_", " ");
		}

		/// <summary>
		/// Converts <see cref="Color32"/> into <see cref="String"/>.
		/// </summary>
		/// <param name="c">targeted <see cref="Color32"/>.</param>
		/// <returns></returns>
		public static String ToHexString(this Color32 c)
		{
			return String.Format("{0:X2}{1:X2}{2:X2}", c.r, c.g, c.b);
		}
	}
}