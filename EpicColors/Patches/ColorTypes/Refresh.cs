using UnityEngine;

namespace EpicColors.Patches.ColorTypes
{
    class Refresh : BaseColor
    {
        public Color[] BodyColors;
        public Color[] ShadowColors;

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
                for (int i = 0; i < BodyColors.Length; i++)
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
            for (int i = 1; i < colors.Length; i++)
            {
                if (Timer > (float)i / colors.Length) outputColor = colors[i];
            }
            return outputColor;
        }

        private static Color[] StringToRefresh(string joinedValues, Color[] defaultValue)
        {
            string[] splitValues = joinedValues.Split('>');
            Color[] colors = new Color[splitValues.Length];
            for (int i = 0; i < splitValues.Length; i++)
            {
                string[] valueRGB = splitValues[i].Split(',');
                bool canR = byte.TryParse(valueRGB[0], out byte valueR);
                bool canG = byte.TryParse(valueRGB[1], out byte valueG);
                bool canB = byte.TryParse(valueRGB[2], out byte valueB);
                if (valueRGB.Length != 3 || !canR || !canG || !canB) return defaultValue;
                colors[i] = new Color((float)valueR / 255, (float)valueG / 255, (float)valueB / 255);
            }
            return colors;
        }
    }
}
