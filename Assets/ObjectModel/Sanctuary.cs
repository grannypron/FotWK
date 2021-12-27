using System;
using System.Collections.Generic;
using System.Text;

namespace FotWK
{
	public class Sanctuary : BaseLocation
	{
		public override void onVisit()
		{
			UnityGameEngine.getEngine().getSoundEngine().playSound("Sanctuary", VisitSceneEvents.GetVisitSceneEvents());
			NextScene("MoveScene");
		}
	}
}
