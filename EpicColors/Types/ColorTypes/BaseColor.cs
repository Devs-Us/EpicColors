using System;
using UnityEngine;

namespace EpicColors.Types.ColorTypes
{
	public abstract class BaseColor
	{
		public Int32 Id;
		public String Name;
		public Boolean IsSpecial;
		public Single Timer = 0f;
		public Single Duration = 5f;

		public virtual void Initialize(String data)
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
