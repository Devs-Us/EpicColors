using System;
using EpicColors.Extensions;
using UnityEngine;

namespace EpicColors.Types.ColorTypes
{
	class Hue : BaseColor
	{
		public (Single saturation, Single value) BodyColor;
		public (Single saturation, Single value) ShadowColor;

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
						BodyColor = ParseColor(colorField[1], BodyColor);
						break;
					}
					case "shadow":
					{
						setShadow = true;
						ShadowColor = ParseColor(colorField[1], ShadowColor);
						break;
					}
				}
			}
			if (!setMain) BodyColor = (1f, 1f);
			if (!setShadow) ShadowColor = (BodyColor.saturation, BodyColor.value * 0.6f);
		}

		public override Color GetBodyColor()
		{
			return Color.HSVToRGB(Timer, BodyColor.saturation, BodyColor.value);
		}

		public override Color GetShadowColor()
		{
			return Color.HSVToRGB(Timer, ShadowColor.saturation, ShadowColor.value);
		}

		private static (Single saturation, Single value) ParseColor(String field, (Single saturation, Single value) defaultValue)
		{
			String[] colorHSV = field.Split(',');

			if (colorHSV.Length != 2 || TryParseHundred(colorHSV[0], out Byte saturation) || TryParseHundred(colorHSV[1], out Byte value)) return defaultValue;
			return ((Single)saturation / 100, (Single)value / 100);
		}

		private static Boolean TryParseHundred(String field, out Byte value)
		{
			Boolean canChange = !Byte.TryParse(field, out value);
			if (value > 100f) return true;
			return canChange;
		}
	}
}
