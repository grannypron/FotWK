using System;
using System.Collections.Generic;
using System.Text;

namespace FotWK
{
	public class Forest : EncounterableLocation
	{
		const int mEncounterChance = 28;

		public override int getEncounterChance() {
			return mEncounterChance;
		}
	}
}
