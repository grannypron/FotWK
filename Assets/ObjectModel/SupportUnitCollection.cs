using System;
using System.Collections.Generic;
using System.Text;

namespace FotWK
{
	public enum SupportUnitType
    {
		Mule = 1,
        Cleric = 2
    }

	public class SupportUnitCollection : Dictionary<SupportUnitType, int>
	{
        public SupportUnitCollection()
        {
            // Initialize all to 0 - be careful with this if new spells are added later and old save states are restored
            foreach (SupportUnitType supportUnitType in Enum.GetValues(typeof(SupportUnitType))) {
                this.Add(supportUnitType, 0);
            }
        }
    }
}
