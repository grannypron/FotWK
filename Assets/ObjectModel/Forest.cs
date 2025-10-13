using System;
using System.Collections.Generic;
using System.Text;

namespace FotWK
{
	public class Forest : EncounterableLocation
	{
		const int mEncounterChance = 100;//28;   1800  IF  RND (1) < .28 THEN 2170

		public override int getEncounterChance() {
			return mEncounterChance;
		}
	}
}
