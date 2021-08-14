using EpicColors.Extensions;
using UnityEngine;

namespace EpicColors.Types.ColorTypes
{
	class Static : BaseColor
	{
		public Color BodyColor;
		public Color ShadowColor;

		public override void Initialize(System.String data)
		{
			System.String[] colorData = data.Split(' ');
			IsSpecial = false;
			System.Boolean setMain = false;
			System.Boolean setShadow = false;
			foreach (System.String colorFieldString in colorData)
			{
				System.String[] colorField = colorFieldString.Split(';');
				if (colorField.Length != 2) continue;
				switch (colorField[0])
				{
					case "name":
					{
						Name = Helpers.RealColorName(colorField[1]);
						break;
					}
					case "main":
					{
						setMain = true;
						BodyColor = StringToColor(colorField[1], BodyColor);
						break;
					}
					case "shadow":
					{
						setShadow = true;
						ShadowColor = StringToColor(colorField[1], ShadowColor);
						break;
					}
				}
			}
			if (!setMain) BodyColor = new Color(0.15f, 0.65f, 0.4f);
			if (!setShadow) ShadowColor = Color.Lerp(BodyColor, Color.black, 0.4f);
		}

		public override Color GetBodyColor()
		{
			return BodyColor;
		}

		public override Color GetShadowColor()
		{
			return ShadowColor;
		}

		private static Color StringToColor(System.String joinedValues, Color defaultValue)
		{
			System.String[] valueRGB = joinedValues.Split(',');
			if (valueRGB.Length != 3) return defaultValue;
			System.Boolean canR = System.Byte.TryParse(valueRGB[0], out System.Byte valueR);
			System.Boolean canG = System.Byte.TryParse(valueRGB[1], out System.Byte valueG);
			System.Boolean canB = System.Byte.TryParse(valueRGB[2], out System.Byte valueB);
			if (!canR || !canG || !canB) return defaultValue;
			return new Color(valueR / 255f, valueG / 255f, valueB / 255f);
		}
	}
}
