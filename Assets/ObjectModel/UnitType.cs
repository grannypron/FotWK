using System;
using System.Collections.Generic;
using System.Text;

namespace FotWK
{
	public class UnitType
	{
        private float mWeight;
        private string mName;

        public UnitType(string name, float weight) {
            mName = name;
            mWeight = weight;
        }

        public string getName() {
            return mName;
        }

        public float getWeight() {
            return mWeight;
        }
	}
}
