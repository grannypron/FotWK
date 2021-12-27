using System;
using System.Collections.Generic;
using System.Text;

namespace FotWK
{
	public class Mountains : EncounterableLocation
	{
		const int mEncounterChance = 80;

		public override int getEncounterChance() {
			return mEncounterChance;
		}

	}
}
