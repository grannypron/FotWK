using System;
using System.Collections.Generic;
using System.Text;

namespace FotWK
{
	/*
	 * Even though there were only 4 magic maps in the original game, I decided to set up a more OOP way of representing
	 * magic maps for mods or future development
	 */
	[System.Serializable]
	public class MagicMap
	{
		[UnityEngine.SerializeField]
		private int mId;

		public MagicMap(int id)
        {
			mId = id;
        }
		public override bool Equals(Object obj)
		{
			if (obj == null || !(obj is MagicMap))
				return false;
			else
				return this.mId == ((MagicMap)obj).mId;
		}

		public override int GetHashCode()
		{
			return this.mId.GetHashCode();
		}
	}
}
