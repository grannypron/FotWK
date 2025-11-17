using System;
using System.Collections.Generic;
using System.Text;

namespace FotWK
{
	public class Force : Dictionary<UnitTypeID, int>
	{
		public Force() : base() { }
		public Force(IDictionary<UnitTypeID, int> force) : base(force) { }

		public override string ToString()
		{
			string str = "";
			foreach (UnitTypeID key in this.Keys) {
				str += Enum.GetName(typeof(FotWK.UnitTypeID), key) + ": " + this[key] + "\n";
			}

			return str;
		}

		public bool IsEmpty()
		{
			if (this.Keys.Count == 0)
			{
				return true;
			}
			else
			{
				foreach (UnitTypeID key in this.Keys)
				{
					if (this[key] > 0)
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
			var e = this.Keys.GetEnumerator();
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
			int qty = this[monsterId];
			UnitType monster = units.getUnitTypeByID(monsterId);
			
			return qty * monster.getWeight(); // 2200 X7 = X * L:
		}


	}

}
