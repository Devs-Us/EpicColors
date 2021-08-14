using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using EpicColors.Types.ColorTypes;
using UnityEngine;
using static Palette;

namespace EpicColors.Handler
{
	public static class ColorData
	{
		public static Int32 OldMainCount => RemoveVanillaColors ? 0 : OldMain.Count;
		public static Boolean RemoveVanillaColors => txtContentList.Contains("RemoveVanillaColors;");
		public static List<Color32> OldMain = PlayerColors.ToList();
		public static List<BaseColor> AllColors = new();
		public static List<String> OldColors = new();
		private static List<String> txtContentList = new();
		public static String Author = String.Empty;

		public static void Load()
		{
			String ccPath = Path.Combine(Directory.GetCurrentDirectory(), "CustomColors.txt");

			// Read CustomColors.txt contents and add to list
			if (File.Exists(ccPath))
			{
				ConfigBuilder.GetOldList(File.ReadLines(ccPath).ToArray());
				foreach (String datalist in File.ReadLines(ccPath))
					txtContentList.Add(datalist);
			}
			else
			{
				String defaultLines = ConfigBuilder.BuildDefaultConfig(true);
				txtContentList = defaultLines.Split('\n').ToList();
			}

			foreach (String content in txtContentList)
			{
				Int32 index = content.IndexOf("author;");
				if (index == -1) continue;
				Author += "\n" + content[(index + 7)..];
			}

			Int32 idTracker = 0;
			foreach (String colorLine in txtContentList)
			{
				if (colorLine.Contains("name;"))
					AllColors.Add(StringToObject(colorLine, idTracker++));
			}
		}

		private static BaseColor StringToObject(String colorLine, Int32 id)
		{
			String[] colorData = colorLine.Split(' ');
			foreach (String colorFieldString in colorData)
			{
				String[] colorField = colorFieldString.Split(';');
				if (colorField.Length != 2 || colorField[0] != "special") continue;
				switch (colorField[1])
				{
					case "hue":
					{
						Hue colorAnim = new() { Id = id };
						colorAnim.Initialize(colorLine);
						return colorAnim;
					}
					case "refresh":
					{
						Refresh colorAnim = new() { Id = id };
						colorAnim.Initialize(colorLine);
						return colorAnim;
					}
				}
			}
			Static color = new();
			color.Initialize(colorLine);
			return color;
		}
	}
}