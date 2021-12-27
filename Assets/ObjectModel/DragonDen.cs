using System;
using System.Collections.Generic;
using System.Text;

namespace FotWK
{
	public class DragonDen : BaseLocation
	{
		Force force;

		public override void onVisit()
		{
			NextScene("MoveScene");
		}
	}
}
