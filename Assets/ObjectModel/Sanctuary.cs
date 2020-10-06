using System;
using System.Collections.Generic;
using System.Text;

namespace FotWK
{
	public class Sanctuary : ILocation
	{
		public void onVisit()
		{
			UnityGameEngine.getEngine().getSoundEngine().playSound("Sanctuary");
		}
	}
}
