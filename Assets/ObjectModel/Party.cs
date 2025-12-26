using System;
using System.Collections.Generic;
using System.Text;

namespace FotWK
{
	public class Party
	{
		public Force force;
		public int gold;
		public int rations;
		public SpellCollection spells;
		public Equipment[] equipment;
		public List<MagicMap> magicMaps;
		public SpecialItemCollection mSpecialItems;
		public SupportUnitCollection supportUnits;

		public Party()
        {
			force = new Force();
			gold = 0;
			rations = 0;
			spells = new SpellCollection();
			equipment = new Equipment[0];
			magicMaps = new List<MagicMap>();
			mSpecialItems = new SpecialItemCollection();
			supportUnits = new SupportUnitCollection();
		}

		public void initializeToStartingValues()
		{
			rations = 25;
			gold = 50;
			spells = new SpellCollection();
			spells[SpellType.SEEING] = 1;
			spells[SpellType.TELEPORT] = 1;
			force = new Force();
			//force.Add(UnitTypeID.Cleric, 2);
			//force.Add(UnitTypeID.Scout, 3);
			//force.Add(UnitTypeID.Raider, 3);
			force.Add(UnitTypeID.Warrior, 50);
			supportUnits = new SupportUnitCollection();
			supportUnits[SupportUnitType.Mule] = 1;
			mSpecialItems = new SpecialItemCollection();
			mSpecialItems.Add(SpecialItemType.DragonSlayer);
		}
		public bool hasSpecialItem(SpecialItemType type)
		{
			return mSpecialItems.Contains(type);
		}
		public void addSpecialItem(SpecialItemType type)
		{
			mSpecialItems.Add(type);
		}
		public void removeSpecialItem(SpecialItemType type)
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
	}
}
