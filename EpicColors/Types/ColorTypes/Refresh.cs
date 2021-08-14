using System;
using EpicColors.Extensions;
using UnityEngine;

namespace EpicColors.Types.ColorTypes
{
	class Refresh : BaseColor
	{
		public Color[] BodyColors;
		public Color[] ShadowColors;

		public override void Initialize(String data)
		{
			String[] colorData = data.Split(' ');
			IsSpecial = true;
			Boolean setMain = false;
			Boolean setShadow = false;
			foreach (String colorFieldString in colorData)
			{
				String[] colorField = colorFieldString.Split(';');
				if (colorField.Length != 2) continue;
				switch (colorField[0])
				{
					case "name":
					{
						Name = Helpers.RealColorName(colorField[1]);
						break;
					}
					case "duration":
					{
						if (!Single.TryParse(colorField[1], out Single duration)) continue;
						Duration = duration;
						break;
					}
					case "main":
					{
						setMain = true;
						BodyColors = StringToRefresh(colorField[1], BodyColors);
						break;
					}
					case "shadow":
					{
						setShadow = true;
						ShadowColors = StringToRefresh(colorField[1], ShadowColors);
						break;
					}
				}
			}
			if (!setMain) BodyColors = new Color[] { new Color(1f, 0f, 0f), new Color(1f, 1f, 0f), new Color(0f, 1f, 0f), new Color(0f, 1f, 1f), new Color(0f, 0f, 1f), new Color(1f, 0f, 1f) };
			if (!setShadow)
			{
				for (Int32 i = 0; i < BodyColors.Length; i++)
				{
					ShadowColors[i] = Color32.Lerp(BodyColors[i], Color.black, 0.4f);
				}
			}
		}

		public override Color GetBodyColor()
		{
			return GetColor(BodyColors);
		}

		public override Color GetShadowColor()
		{
			return GetColor(ShadowColors);
		}

		private Color GetColor(Color[] colors)
		{
			Color outputColor = colors[0];
			for (Int32 i = 1; i < colors.Length; i++)
			{
				if (Timer > (Single)i / colors.Length) outputColor = colors[i];
			}
			return outputColor;
		}

		private static Color[] StringToRefresh(String joinedValues, Color[] defaultValue)
		{
			String[] splitValues = joinedValues.Split('>');
			Color[] colors = new Color[splitValues.Length];
			for (Int32 i = 0; i < splitValues.Length; i++)
			{
				String[] valueRGB = splitValues[i].Split(',');
				Boolean canR = Byte.TryParse(valueRGB[0], out Byte valueR);
				Boolean canG = Byte.TryParse(valueRGB[1], out Byte valueG);
				Boolean canB = Byte.TryParse(valueRGB[2], out Byte valueB);
				if (valueRGB.Length != 3 || !canR || !canG || !canB) return defaultValue;
				colors[i] = new Color((Single)valueR / 255, (Single)valueG / 255, (Single)valueB / 255);
			}
			return colors;
		}
	}
}
