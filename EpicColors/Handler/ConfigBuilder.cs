using System;
using System.IO;
using System.Linq;
using System.Text;
using static EpicColors.CustomColorHandler;

namespace EpicColors
{
    class ConfigBuilder
    {
        public static string BuildDefaultConfig(bool IncludeColor)
        {
            StringBuilder stringSettings = new();
            _ = stringSettings.AppendLine("version;2");
            _ = stringSettings.AppendLine("");

            foreach (string colorData in EpicColors.defaultConfig)
                if (IncludeColor)
                    _ = stringSettings.AppendLine(colorData.Trim());

            string ccPath = Path.Combine(Directory.GetCurrentDirectory(), "CustomColors.txt");
            File.WriteAllText(ccPath, stringSettings.ToString());
            return stringSettings.ToString();
        }

        // Append list from outdated CustomColors.txt
        public static void GetOldList(string[] list)
        {
            if (list.Contains("version;2")) return;
            string ccPath = Path.Combine(Directory.GetCurrentDirectory(), "CustomColors.txt");

            if (File.Exists(ccPath))
                foreach (string content in File.ReadLines(ccPath))
                    OldColors.Add(content);

            File.Delete(ccPath);
            BuildDefaultConfig(!list.Contains("removeBuiltIn;"));
            File.AppendAllLines(ccPath, OldColors);
        }
    }
}
