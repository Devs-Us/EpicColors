using HarmonyLib;
using System.Collections.Generic;
using S = StringNames;

namespace EpicColors
{
    [HarmonyPatch(typeof(LanguageUnit), nameof(LanguageUnit.GetString))]
    public static class StringNamesHandler
    {
        private static Dictionary<int, string> Strings = new Dictionary<int, string>();
        private static int Id = -9999;
        public static StringNames NewStringNames(this string str)
        {
            Strings[Id] = str;
            return (S)Id--;
        }

        [HarmonyPrefix]
        public static bool PatchAll(ref string __result, [HarmonyArgument(0)] S name)
        {
            if ((int)name <= -9999)
            {
                string text = Strings[(int)name];
                __result = text ?? default;
                return false;
            }
            return true;
        }
    }
}