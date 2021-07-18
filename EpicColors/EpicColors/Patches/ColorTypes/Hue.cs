using UnityEngine;

namespace EpicColors.Patches.ColorTypes
{
	class Hue : BaseColor
	{
		public (float saturation, float value) BodyColor;
		public (float saturation, float value) ShadowColor;

		public override void Initialize(string data)
		{
			string[] colorData = data.Split(' ');
			IsSpecial = true;
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
					case "duration":
					{
						if (!float.TryParse(colorField[1], out float duration)) continue;
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
						Debug.logger.Log(ShadowColor.ToString());
						break;
					}
				}
			}
			if (!setMain) BodyColor = (1f, 1f);
			if (!setShadow) ShadowColor = (BodyColor.saturation, BodyColor.value * 0.6f);
			Debug.logger.Log(ShadowColor.ToString());
		}

		public override Color GetBodyColor()
		{
			return Color.HSVToRGB(Timer, BodyColor.saturation, BodyColor.value);
		}

		public override Color GetShadowColor()
		{
			return Color.HSVToRGB(Timer, ShadowColor.saturation, ShadowColor.value);
		}

		private static (float saturation, float value) ParseColor(string field, (float saturation, float value) defaultValue)
		{
			string[] colorHSV = field.Split(',');
			Debug.logger.Log(colorHSV.Length.ToString());
			Debug.logger.Log(tryParseHundred(colorHSV[0], out byte saturation2).ToString());
			Debug.logger.Log(tryParseHundred(colorHSV[1], out byte value2).ToString());

			if (colorHSV.Length != 2 || tryParseHundred(colorHSV[0], out byte saturation) || tryParseHundred(colorHSV[1], out byte value)) return defaultValue;
			return ((float)saturation / 100, (float)value / 100);
		}

		private static bool tryParseHundred(string field, out byte value)
		{
			bool canChange = !byte.TryParse(field, out value);
			if (value > 100f) return true;
			return canChange;
		}
	}
}
