using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FotWK
{
    public static class RNG
    {
        public static bool rollPercentage(int percentage)
        {
            int roll = UnityEngine.Random.Range(1, 100);
            return roll <= percentage;
        }
    }
}
