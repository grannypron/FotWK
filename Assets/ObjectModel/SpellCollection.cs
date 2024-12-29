using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FotWK
{
    public class SpellCollection : Dictionary<SpellType, int>
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
