using System;
using System.Collections.Generic;
using HarmonyLib;
using SN = StringNames;

namespace EpicColors.Handler
{
	[HarmonyPatch(typeof(LanguageUnit), nameof(LanguageUnit.GetString))]
	public static class StringHelpers
	{
		private static readonly Dictionary<Int32, String> Strings = new();
		private static Int32 Id = -9999;
		public static SN NewStringNames(String str)
		{
			Strings[Id] = str;
			return (SN)Id--;
		}

		[HarmonyPrefix]
		public static Boolean PatchAll(ref String __result, [HarmonyArgument(0)] SN name)
		{
			if ((Int32)name <= -9999)
			{
				String text = Strings[(Int32)name];
				__result = text ?? default;
				return false;
			}
			return true;
		}
	}
}