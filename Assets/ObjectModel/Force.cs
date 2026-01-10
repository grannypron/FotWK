using System;
using System.Collections.Generic;
using System.Text;

namespace FotWK
{
	[System.Serializable]
	public class Force : SerializableDictionary.Scripts.SerializableDictionary<UnitTypeID, int>
	{
		public Force() : base() { }
		public Force(SerializableDictionary.Scripts.SerializableDictionary<UnitTypeID, int> force) 
		{
			foreach (UnitTypeID key in this.Keys())
			{
				this.Set(key, force.Get(key));
			}
		}

		public override string ToString()
		{
			string str = "";
			foreach (UnitTypeID key in this.Keys()) {
				str += Enum.GetName(typeof(FotWK.UnitTypeID), key) + ": " + this.Get(key) + "\n";
			}

			return str;
		}

		public bool IsEmpty()
		{
			if (this.Keys().Count == 0)
			{
				return true;
			}
			else
			{
				foreach (UnitTypeID key in this.Keys())
				{
					if (this.Get(key) > 0)
					{
						return false;
					}
				}
				return true;
			}
		}

		// Since the orignal game only has forces of one type, this is useful
		public UnitTypeID GetFirstUnitTypeID()
        {
			var e = this.Keys().GetEnumerator();
			e.MoveNext();
			return e.Current;

        }

		// This is called X7 in the Apple II code
		public float CalculateLootFactor()
		{

			if (this.IsEmpty())
            {
				return 0;
            }
			UnitsData units = UnitsDataFactory.getUnitsData();
			UnitTypeID monsterId = this.GetFirstUnitTypeID();
			int qty = this.Get(monsterId);
			UnitType monster = units.getUnitTypeByID(monsterId);
			
			return qty * monster.getWeight(); // 2200 X7 = X * L:
		}

		public Dictionary<UnitTypeID, int>.Enumerator GetEnumerator()
		{
			return base.GetEnumerator();
		}

	}

}
