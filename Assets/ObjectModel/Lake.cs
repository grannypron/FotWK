using System;
using System.Collections.Generic;
using System.Text;

namespace FotWK
{
	public class Lake : BaseLocation
	{
		public override void onVisit()
		{
			NextScene("MoveScene");
		}

	}
}
