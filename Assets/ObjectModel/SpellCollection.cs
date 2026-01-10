using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FotWK
{
    [System.Serializable]
    public class SpellCollection : SerializableDictionary.Scripts.SerializableDictionary<SpellType, int>
    {
        public SpellCollection()
        {
            // Initialize all to 0 - be careful with this if new spells are added later and old save states are restored
            foreach (SpellType spellType in Enum.GetValues(typeof(SpellType))) {
                this.Add(spellType, 0);
            }
        }
    }
}
