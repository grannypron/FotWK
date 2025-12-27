using System;
using System.Collections.Generic;
using System.Text;

namespace FotWK
{
	public enum SpecialItemTypeID
    {
		DragonSlayer,
		HornOfOpening,
		BootsOfStealth,
		ArmourOfDefense,
		SwordOfStrength,
		HammerOfThor,
		TalismanOfSpeed
	}

	public static class SpecialItemTypeData
    {
		static Dictionary<SpecialItemTypeID, String> mItemNames;
		static SpecialItemTypeData()
		{
			mItemNames = new Dictionary<SpecialItemTypeID, string>();
			mItemNames.Add(SpecialItemTypeID.DragonSlayer, "DRAGON SLAYER");
			mItemNames.Add(SpecialItemTypeID.HornOfOpening, "HORN OF OPENING");
			mItemNames.Add(SpecialItemTypeID.BootsOfStealth, "BOOTS OF STEALTH");
			mItemNames.Add(SpecialItemTypeID.ArmourOfDefense, "ARMOUR OF DEFENSE");
			mItemNames.Add(SpecialItemTypeID.SwordOfStrength, "SWORD OF STRENGTH");
			mItemNames.Add(SpecialItemTypeID.HammerOfThor, "HAMMER OF THOR");
			mItemNames.Add(SpecialItemTypeID.TalismanOfSpeed, "TALISMAN OF SPEED");
		}
		public static String getName(SpecialItemTypeID id)
        {
			return mItemNames[id];
        }
	}
	public class SpecialItemCollection 
	{
		private List<SpecialItemTypeID> mItems;
		public SpecialItemCollection()
        {
			mItems = new List<SpecialItemTypeID>();
        }

		public void Add(SpecialItemTypeID type)
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

		public void Remove(SpecialItemTypeID type)
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

		public bool Contains(SpecialItemTypeID type)
        {
			return mItems.Contains(type);
        }

	}
}
