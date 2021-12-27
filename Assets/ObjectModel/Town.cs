using System;
using System.Collections.Generic;
using System.Text;

namespace FotWK
{
	public class Town : BaseLocation
	{
		Force force;
		Array equipment;
		Array units;
		public override void onVisit()
		{
			NextScene("MoveScene");
		}
	}
}
