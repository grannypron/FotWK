using System;
using System.Collections.Generic;
using System.Text;

namespace FotWK
{
	public enum SpecialItemType
    {
		DragonSlayer,
		HornOfOpening,
		BootsOfStealth,
		ArmourOfDefense,
		SwordOfStrength,
		HammerOfThor,
		TalismanOfSpeed
	}
	public class SpecialItemCollection 
	{
		private List<SpecialItemType> mItems;
		public SpecialItemCollection()
        {
			mItems = new List<SpecialItemType>();
        }

		public void Add(SpecialItemType type)
        {
			if (mItems.Contains(type))
            {
				Utility.assert(false);
            }
			else
            {
				mItems.Add(type);
            }
        }

		public void Remove(SpecialItemType type)
		{
			if (mItems.Contains(type))
			{
				Utility.assert(false);
			}
			else
			{
				mItems.Remove(type);
			}
		}

		public bool Contains(SpecialItemType type)
        {
			return mItems.Contains(type);
        }

	}
}
