using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace EpicColors.Patches.ColorTypes
{
	public abstract class BaseColor
	{
		public int Id;
		public string Name;
		public bool IsSpecial;
		public float Timer = 0f;
		public float Duration = 5f;

		public virtual void Initialize(string data)
		{
		}

		public virtual Color GetBodyColor()
		{
			return new Color(0.15f, 0.65f, 0.4f);
		}

		public virtual Color GetShadowColor()
		{
			return new Color(0.05f, 0.25f, 0.1f);
		}
	}
}
