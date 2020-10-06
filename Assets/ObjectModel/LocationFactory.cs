using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FotWK
{
    public static class LocationFactory
    {
        public static ILocation getVisitByTileName(String tileName) 
        {
            tileName = tileName.ToLower();
            if (tileName == "forest")
            {
                return new Forest();
            }
            if (tileName == "mountains")
            {
                return new Mountains();
            }
            if (tileName == "town")
            {
                return new Town();
            }
            if (tileName == "sanctuary")
            {
                return new Sanctuary();
            }
            if (tileName == "lake")
            {
                return new Lake();
            }
            if (tileName == "fortress")
            {
                return new Fortress();
            }
            if (tileName == "dragonden")
            {
                return new DragonDen();
            }
            throw new Exception("Unknown tile: " + tileName);
        }
    }
}
