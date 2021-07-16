using System;
using System.IO;
using System.Reflection;
using UnhollowerBaseLib;
using UnityEngine;

using static EpicColors.Logger;

namespace EpicColors {
    public static class SpriteMaskHandler {
        public static Sprite SpriteMask(Assembly assembly = null) {
            try {
                Assembly myAssembly = null;
                myAssembly = assembly == null ? Assembly.GetCallingAssembly() : assembly;
                Stream myStream = Assembly.Load(myAssembly.GetName()).GetManifestResourceStream("EpicColors.Patches.mask");

                byte[] image = new byte[myStream.Length];
                myStream.Read(image, 0, (int) myStream.Length);
                Texture2D myTexture = new Texture2D(2, 2, TextureFormat.ARGB32, true);
                LoadImage(myTexture, image, true);
                return Sprite.Create(myTexture, new Rect(0, 0, myTexture.width, myTexture.height), new Vector2(0.5f, 0.5f), 100f);
            } catch (Exception e) {
                ErrorLogger("loading spritemask", e);
             }
            return null;
        }

        private static bool LoadImage(Texture2D tex, byte[] data, bool markNonReadable) {
            if (iCall_LoadImage == null)
                iCall_LoadImage = IL2CPP.ResolveICall<d_LoadImage>("UnityEngine.ImageConversion::LoadImage");

            var il2cppArray = (Il2CppStructArray<byte>) data;

            return iCall_LoadImage.Invoke(tex.Pointer, il2cppArray.Pointer, markNonReadable);
        }

        internal delegate bool d_LoadImage(IntPtr tex, IntPtr data, bool markNonReadable);
        internal static d_LoadImage iCall_LoadImage;
    }
}