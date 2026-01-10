using System;
using System.Collections.Generic;
using System.Text;

namespace FotWK
{
    [System.Serializable]
    public class SupportUnitCollection : SerializableDictionary.Scripts.SerializableDictionary<SupportUnitTypeID, int>
	{
        public SupportUnitCollection()
        {
            // Initialize all to 0 - be careful with this if new spells are added later and old save states are restored
            foreach (SupportUnitTypeID supportUnitTypeID in Enum.GetValues(typeof(SupportUnitTypeID))) {
                this.Add(supportUnitTypeID, 0);
            }
        }
    }
}
