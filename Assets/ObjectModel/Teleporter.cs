using System;
using System.Collections.Generic;
using System.Text;

namespace FotWK
{
	public class Teleporter : BaseLocation
	{
		public override void onVisit()
		{
			NextScene("MoveScene");
		}
	}
}
