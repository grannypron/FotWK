using System;
using System.Collections.Generic;
using System.Text;

namespace FotWK
{
	public class UnitType
	{
        private float mWeight;
        private string mName;
        private bool mIsCaster;

        public UnitType(string name, float weight, bool caster = false) {
            mName = name;
            mWeight = weight;
            mIsCaster = caster;
        }

        public string getName() {
            return mName;
        }

        public float getWeight() {
            return mWeight;
        }

        public bool isCaster()
        {
            return mIsCaster;
        }
	}
}
