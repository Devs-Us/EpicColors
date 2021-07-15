using System.Collections.Generic;
using System.Linq;
using HarmonyLib;
using UnhollowerBaseLib;

using TC = TranslationController;
using S = StringNames;

namespace EpicColors
{
    public static class StringNamesHandler {
        private static Dictionary<int, string> strings = new Dictionary<int, string>();
        private static int id = 9999;
        public static StringNames NewStringNames(this string str) {
            strings[id] = str;
            return (S)id++;
        }

        [HarmonyPatch(typeof(TC), nameof(TC.GetString), new[] 
        {typeof(S),typeof(Il2CppReferenceArray<Il2CppSystem.Object>)
        })]
        class StringNamesDeclarer {
            public static bool Prefix(ref string __result, [HarmonyArgument(0)] S name) {
                if ((int)name >= 9999) {

                    string text = strings[(int)name];
                    if (text is null) 
                        return true;

                    __result = text;
                    return false;
                } return true;

            }
        }
    }
}