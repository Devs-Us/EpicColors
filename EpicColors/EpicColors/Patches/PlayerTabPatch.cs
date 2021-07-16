using System;
using HarmonyLib;
using UnityEngine;

using P=PlayerTab;
using PL=Palette;

namespace EpicColors {

    [HarmonyPatch(typeof(P), nameof(P.OnEnable))]
    internal static class ColorChipPatch {
        public static void Postfix(P __instance) {
            var p = __instance;
            try {
                foreach (var colorChip in p.ColorChips)
                    GameObject.Destroy(colorChip.gameObject);
                p.ColorChips.Clear();

                if (Inner == null || !Inner.scene.IsValid())
                    p.Scroll();

                for (var i = 0; i < PL.PlayerColors.Length; i++) {
                    var xOffset = -0.935f + (i % 5 * 0.47f);
                    var yOffset = 1.55f - (i / 5 * 0.47f);

                    var cc = GameObject.Instantiate(p.ColorTabPrefab, Inner.transform, true);
                    cc.Update();
                    cc.Inner.transform.localScale *= 0.76f;
                    cc.Inner.transform.localPosition = new Vector3(xOffset, yOffset, -1f);

                    var j = i;
                    cc.Button.OnClick.AddListener((System.Action)delegate
                    {
                        p.SelectColor(j);
                    });

                    cc.Inner.color = PL.PlayerColors[i];
                    p.ColorChips.Add(cc);
                }

                var row = Mathf.Max((__instance.ColorChips.Count / 5) - 7.4f, 0);
                var y = (row * 0.55f) + 0.25f;
                scroll.YBounds = new FloatRange(0f, y);
            }   catch (Exception e) {ColorsPlugin.Logger.LogError(e);}
        }

        private static void Scroll(this PlayerTab __instance) {
            Inner = new GameObject { layer = 5, name = "Inner" };
            var scroller = new GameObject { layer = 5, name = "Scroller" };
            scroll = scroller.AddComponent<Scroller>();
            var mask = new GameObject();

            scroller.transform.SetParent(__instance.transform);
            scroll.allowX = false;
            scroll.allowY = true;
            scroll.velocity = new Vector2(0.008f, 0.005f);
            scroll.ScrollerYRange = new FloatRange(0f, 0f);
            scroll.YBounds = new FloatRange(0f, 3f);
            scroll.Inner = Inner.transform;

            Inner.transform.SetParent(__instance.ColorTabArea);
            Inner.transform.localPosition = Vector3.zero;

            mask.name = "SpriteMask";
            mask.layer = 5;
            mask.transform.SetParent(__instance.ColorTabArea);
            mask.transform.localPosition = new Vector3(0f, 0f, 0f);
            mask.transform.localScale = new Vector3(250f, 380f, 1f);

            var spriteMask = mask.AddComponent<SpriteMask>();
            spriteMask.sprite = SpriteMaskHandler.SpriteMask();

            var collider = mask.AddComponent<BoxCollider2D>();
            collider.size = Vector2.one;
            collider.enabled = true;

            mask.SetActive(true);
        }

        private static void Update(this ColorChip chip) {
            var renderer = chip.GetComponent<SpriteRenderer>();
            renderer.maskInteraction = SpriteMaskInteraction.VisibleInsideMask;

            var fg = chip.transform.GetChild(0).gameObject;
            var controllerHighlight = chip.transform.GetChild(1).gameObject;

            var oldShade = fg.transform.GetChild(0).gameObject;
            var newShade = fg.transform.GetChild(1).gameObject;

            var highlightRenderer = controllerHighlight.GetComponent<SpriteRenderer>();
            highlightRenderer.maskInteraction = SpriteMaskInteraction.VisibleInsideMask;

            var shadeRenderer = newShade.GetComponent<SpriteRenderer>();
            shadeRenderer.maskInteraction = SpriteMaskInteraction.VisibleInsideMask;
            shadeRenderer.color = new Color(0.5f, 0.5f, 0.5f, 0.5f);
            GameObject.Destroy(fg.GetComponent<SpriteMask>());
            GameObject.Destroy(oldShade);
        }

        private static GameObject Inner = null;
        private static Scroller scroll;
    }

    [HarmonyPatch(typeof(P), nameof(P.Update))]
	public static class PUpdatePatch
	{
		public static void Postfix(PlayerTab __instance)
		{
			int id = PlayerControl.LocalPlayer.Data.ColorId;
			__instance.HatImage.SetColor(id);

            if (!ConverterHelper.IncludeBuiltinColor()) return;
                for (int i = 0; i < AnimatedColors.ColorsList.Count; i++) 
                    __instance.ColorChips[AnimatedColors.ColorsList[i].id].gameObject.GetComponent<SpriteRenderer>().color 
                        = Palette.PlayerColors[AnimatedColors.ColorsList[i].id];
		}
	}
}