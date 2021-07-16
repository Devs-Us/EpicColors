using System;
using UnityEngine;
using System.Text.RegularExpressions;
using System.Linq;
using System.IO;
using static EpicColors.CustomColorHandler;

using PL = Palette;

namespace EpicColors
{
    public static class Logger {
        public static void ErrorLogger(object value, Exception ex) {
            ColorsPlugin.Logger.LogError($"It seems like there was an error when {value}. Here's some log that might be helpful.");
            ColorsPlugin.Logger.LogError($"======================");
            ColorsPlugin.Logger.LogError(ex);
            ColorsPlugin.Logger.LogError($"======================");
            
            return;
        }
    }
}