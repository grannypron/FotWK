using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FotWK
{
    public static class RNG
    {
        public static bool rollAgainstPercentage(int percentage)
        {
            return rollAgainstPercentage((float) percentage);
        }
        public static bool rollAgainstPercentage(float percentage)
        {
            int roll = UnityEngine.Random.Range(1, 100);
            return roll <= percentage;
        }
        public static int rollInRange(int start, int end)
        {
            int roll = UnityEngine.Random.Range(start, end);
            return roll;
        }
        public static float rollPercentage0To1()
        {
            return UnityEngine.Random.Range(0, 100.0f)/100.0f;
        }
    }
}
