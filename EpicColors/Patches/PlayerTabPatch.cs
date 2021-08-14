using System;
using EpicColors.Handler;
using HarmonyLib;
using UnityEngine;
using static Palette;
using PL = PlayerTab;

namespace EpicColors.Patches
{
	[HarmonyPatch]
	internal static class PlayerTabPatch
	{
		private static GameObject Inner = null;
		private static Scroller scroll = null;

		[HarmonyPatch(typeof(PL), nameof(PL.OnEnable))]
		[HarmonyPostfix]
		public static void OnEnablePatch(PL __instance)
		{
			PL p = __instance;

			foreach (ColorChip colorChip in p.ColorChips)
			{
				UnityEngine.Object.Destroy(colorChip.gameObject);
			}
			p.ColorChips.Clear();

			if (Inner == null || !Inner.scene.IsValid())
			{
				Inner = new GameObject { layer = 5, name = "Inner" };
				GameObject scroller = new() { layer = 5, name = "Scroller" };
				scroll = scroller.AddComponent<Scroller>();
				GameObject mask = new();

				scroller.transform.SetParent(p.transform);
				scroll.allowX = false;
				scroll.allowY = true;
				scroll.velocity = new Vector2(0.008f, 0.005f);
				scroll.ScrollerYRange = new FloatRange(0f, 0f);
				scroll.YBounds = new FloatRange(0f, 3f);
				scroll.Inner = Inner.transform;

				Inner.transform.SetParent(p.ColorTabArea);
				Inner.transform.localPosition = Vector3.zero;

				mask.name = "SpriteMask";
				mask.layer = 5;
				mask.transform.SetParent(p.ColorTabArea);
				mask.transform.localPosition = new Vector3(0f, 0f, 0f);
				mask.transform.localScale = new Vector3(250f, 380f, 1f);

				SpriteMask spriteMask = mask.AddComponent<SpriteMask>();
				spriteMask.sprite = SpriteHelpers.GetSpriteMask();

				BoxCollider2D collider = mask.AddComponent<BoxCollider2D>();
				collider.size = Vector2.one;
				collider.enabled = true;

				mask.SetActive(true);
			}

			for (Int32 i = 0; i < PlayerColors.Length; i++)
			{
				Single xOffset = -0.935f + (i % 5 * 0.47f);
				Single yOffset = 1.55f - (i / 5 * 0.47f);

				ColorChip cc = UnityEngine.Object.Instantiate(p.ColorTabPrefab, Inner.transform, true);
				cc.Inner.transform.localScale *= 0.76f;
				cc.Inner.transform.localPosition = new Vector3(xOffset, yOffset, -1f);

				SpriteRenderer renderer = cc.GetComponent<SpriteRenderer>();
				renderer.maskInteraction = SpriteMaskInteraction.VisibleInsideMask;

				GameObject fg = cc.transform.GetChild(0).gameObject;
				GameObject controllerHighlight = cc.transform.GetChild(1).gameObject;

				GameObject oldShade = fg.transform.GetChild(0).gameObject;
				GameObject newShade = fg.transform.GetChild(1).gameObject;

				SpriteRenderer highlightRenderer = controllerHighlight.GetComponent<SpriteRenderer>();
				highlightRenderer.maskInteraction = SpriteMaskInteraction.VisibleInsideMask;

				SpriteRenderer shadeRenderer = newShade.GetComponent<SpriteRenderer>();
				shadeRenderer.maskInteraction = SpriteMaskInteraction.VisibleInsideMask;
				shadeRenderer.color = new Color(0.5f, 0.5f, 0.5f, 0.5f);
				UnityEngine.Object.Destroy(fg.GetComponent<SpriteMask>());
				UnityEngine.Object.Destroy(oldShade);

				Int32 j = i;
				cc.Button.OnClick.AddListener((Action)delegate
				{
					p.SelectColor(j);
				});

				cc.Inner.color = PlayerColors[i];
				p.ColorChips.Add(cc);
			}

			Single row = Mathf.Max((p.ColorChips.Count / 5) - 7.4f, 0);
			Single y = (row * 0.55f) + 0.25f;
			scroll.YBounds = new FloatRange(0f, y);
		}

		[HarmonyPatch(typeof(PL), nameof(PL.Update))]
		[HarmonyPostfix]
		public static void UpdatePatch(PL __instance)
		{
			Int32 id = PlayerControl.LocalPlayer?.Data.ColorId ?? 0;
			__instance.HatImage.SetColor(id);

			for (Int32 i = 0; i < PlayerColors.Length; i++)
			{
				__instance.ColorChips[i].gameObject.GetComponent<SpriteRenderer>().color = PlayerColors[i];
			}
		}
	}
}