using System;
using System.Collections.Generic;
using System.Text;

namespace FotWK
{
	public enum SpellType
    {
		SEEING = 1,
		TELEPORT = 2,
		SEEKING = 3   // Or is this just a typo?
	}
	public class Spell
	{
		private SpellType mType;
		private Spell() { }
		public Spell(SpellType type)
        {
			mType = type;
        }
		public static String SpellTypeToString(SpellType spellType)
        {
			switch (spellType)
            {
				case SpellType.SEEING:
					return "SPELL OF SEEING";
				case SpellType.TELEPORT:
					return "TELEPORT SPELL";
				case SpellType.SEEKING:
					return "SPELL OF SEEKING";
				default:
					return "UNKNOWN SPELL";
			}
        }
	}
}
