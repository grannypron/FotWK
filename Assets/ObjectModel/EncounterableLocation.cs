using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FotWK
{
    public abstract class EncounterableLocation : ILocation
    {
        public void encounter()
        {
            VisitSceneEvents.SetText("An Encounter!");
            UnityGameEngine.getEngine().getSoundEngine().playSound("AttentionAndCharge");
        }
        public virtual void onVisit()
        {
            if (RNG.rollPercentage(Globals.ENCOUNTER_CHANCE))
            {
                encounter();
            }
        }
    }
}
