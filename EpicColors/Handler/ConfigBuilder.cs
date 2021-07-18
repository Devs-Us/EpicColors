﻿using System.IO;
using System.Text;
using UnityEngine;

namespace EpicColors
{
	class ConfigBuilder
	{
		public static string BuildDefaultConfig()
		{
			StringBuilder stringSettings = new();
			_ = stringSettings.AppendLine("author;");
			_ = stringSettings.AppendLine("");
			
			foreach (string colorData in EpicColors.defaultConfig)
				_ = stringSettings.AppendLine(colorData.Trim());

			string ccPath = Path.Combine(Directory.GetCurrentDirectory(), "CustomColors.txt");
			File.WriteAllText(ccPath, stringSettings.ToString());
			return stringSettings.ToString();
		}
	}
}
