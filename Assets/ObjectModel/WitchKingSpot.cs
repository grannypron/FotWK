using System;
using System.Collections.Generic;
using System.Text;

namespace FotWK
{
	public class WitchKingSpot : BaseLocation
	{
		public override void onVisit()
		{
			NextScene("MoveScene");  // or EndScene???
		}
	}
}
