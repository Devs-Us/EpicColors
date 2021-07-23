using System;
using System.Reflection;
using UnhollowerBaseLib;
using UnityEngine;

using static EpicColors.Logger;

namespace EpicColors
{
    public static class SpriteMaskHandler
    {
        public static Sprite SpriteMask(Assembly assembly = null)
        {
            try
            {
                var imageBytes = Convert.FromBase64String(mask);
                Texture2D tex = new Texture2D(2, 2);
                tex.LoadImage(imageBytes, true);
                return Sprite.Create(tex, new Rect(0.0f, 0.0f, tex.width, tex.height), new Vector2(0.5f, 0.5f), 100.0f);
            }
            catch (Exception e)
            {
                ErrorLogger("loading spritemask", e);
            }
            return null;
        }

        public static bool LoadImage(this Texture2D tex, byte[] data, bool markNonReadable)
        {
            if (ICall_LoadImage == null)
                ICall_LoadImage = IL2CPP.ResolveICall<d_LoadImage>("UnityEngine.ImageConversion::LoadImage");

            var il2cppArray = (Il2CppStructArray<byte>)data;

            return ICall_LoadImage.Invoke(tex.Pointer, il2cppArray.Pointer, markNonReadable);
        }

        internal delegate bool d_LoadImage(IntPtr tex, IntPtr data, bool markNonReadable);
        internal static d_LoadImage ICall_LoadImage;

        internal static string mask =
        @"iVBORw0KGgoAAAANSUhEUgAAAAEAAAABCAIAAACQd1PeAAAAAXNSR0IArs4c6QAAAARnQU1BAACxjwv8YQUAAAAJcEhZcwAADsMAAA7DAcdvqGQAAAAMSURBVBhXY/j//z8ABf4C/qc1gYQAAAAASUVORK5CYII=";
    }
}