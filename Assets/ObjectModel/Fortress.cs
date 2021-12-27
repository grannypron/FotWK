using System;
using System.Collections.Generic;
using System.Text;

namespace FotWK
{
	public class Fortress : BaseLocation
	{
		public override void onVisit()
		{
            VisitSceneEvents.GetVisitSceneEvents().SetText("INVADING THE FORTRESS OF THE WITCH KING");
			NextScene("MoveScene");   // or EndScene if they win
		}

	}
}
