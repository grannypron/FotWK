using System;
using System.Collections.Generic;
using UnityEngine;

namespace FotWK
{
	[System.Serializable]
	public class Party
	{
		[SerializeField]
		public Force force;
		[SerializeField]
		public int gold;
		[SerializeField]
		public int rations;
		[SerializeField]
		public SpellCollection spells;
		[SerializeField]
		public EquipmentCollection equipment;
		[SerializeField]
		public List<MagicMap> magicMaps;
		[SerializeField]
		public SpecialItemCollection mSpecialItems;
		[SerializeField]
		public SupportUnitCollection supportUnits;

		public Party()
        {
			force = new Force();
			gold = 0;
			rations = 0;
			spells = new SpellCollection();
			equipment = new EquipmentCollection();
			magicMaps = new List<MagicMap>();
			mSpecialItems = new SpecialItemCollection();
			supportUnits = new SupportUnitCollection();
		}

		public void initializeToStartingValues()
		{
			rations = 25;
			gold = 5000;
			spells = new SpellCollection();
			spells.Set(SpellType.SEEING, 1);
			spells.Set(SpellType.TELEPORT, 1);
			equipment.Set(EquipmentID.Raft, 2);
			force = new Force();
			//force.Add(UnitTypeID.Cleric, 2);
			//force.Add(UnitTypeID.Scout, 3);
			//force.Add(UnitTypeID.Raider, 3);
			force.Add(UnitTypeID.Warrior, 50);
			supportUnits = new SupportUnitCollection();
			supportUnits.Set(FotWK.SupportUnitTypeID.Mule, 1);
			mSpecialItems = new SpecialItemCollection();
			mSpecialItems.Add(SpecialItemTypeID.DragonSlayer);
		}
		public bool hasSpecialItem(SpecialItemTypeID type)
		{
			return mSpecialItems.Contains(type);
		}
		public void addSpecialItem(SpecialItemTypeID type)
		{
			mSpecialItems.Add(type);
		}
		public void removeSpecialItem(SpecialItemTypeID type)
		{
			Utility.assert(mSpecialItems.Contains(type));
			mSpecialItems.Remove(type);
		}
		public void addGold(int _gold) {
			gold += _gold; 
		}
		public void setGold(int _gold)
		{
			gold = _gold;
		}

		public float calculateAvailableEncumbrance()
        {
			//3760  TEXT: HOME: Z = I % (P, 3) * 5 + I % (P, 5) * 10 + I % (P, 10) * 50 + 5:Y = 0
			return supportUnits.Get(FotWK.SupportUnitTypeID.Scout) * Globals.AVAILABLE_ENCUMBRANCE_PER_SCOUT
				+ force.Get(FotWK.UnitTypeID.Warrior) * Globals.AVAILABLE_ENCUMBRANCE_PER_WARRIOR
				+ supportUnits.Get(FotWK.SupportUnitTypeID.Mule) * Globals.AVAILABLE_ENCUMBRANCE_PER_MULE
				+ Globals.AVAILABLE_ENCUMBRANCE_BASE;  // We'll just use this weird formula
		}
	}
}
