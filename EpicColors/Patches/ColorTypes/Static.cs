using UnityEngine;

namespace EpicColors.Patches.ColorTypes
{
	class Static : BaseColor
	{
		public Color BodyColor;
		public Color ShadowColor;

		public override void Initialize(string data)
		{
			string[] colorData = data.Split(' ');
			IsSpecial = false;
			bool setMain = false;
			bool setShadow = false;
			foreach (string colorFieldString in colorData)
			{
				string[] colorField = colorFieldString.Split(';');
				if (colorField.Length != 2) continue;
				switch (colorField[0])
				{
					case "name":
					{
						Name = ConverterHelper.RealColorName(colorField[1]);
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

		private static Color StringToColor(string joinedValues, Color defaultValue)
		{
			string[] valueRGB = joinedValues.Split(',');
			if (valueRGB.Length != 3) return defaultValue;
			bool canR = byte.TryParse(valueRGB[0], out byte valueR);
			bool canG = byte.TryParse(valueRGB[1], out byte valueG);
			bool canB = byte.TryParse(valueRGB[2], out byte valueB);
			if (!canR || !canG || !canB) return defaultValue;
			return new Color(valueR / 255f, valueG / 255f, valueB / 255f);
		}
	}
}
