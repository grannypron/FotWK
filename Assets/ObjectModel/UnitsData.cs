using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FotWK
{
    [System.Serializable]
    public enum UnitTypeID {
        Warrior,
        Bandit,
        Werebear,
        Goblin,
        Orc,
        Troll,
        Ogre,
        EarthGiant,
        Wizard,
        Hacker,
        Dragon,
        WitchKing,
        Hydra,
        Gorgon,
        Hobgoblin,
        Elf,
        Dwarf
    }
    [System.Serializable]
    public enum SupportUnitTypeID
    {
        Mule = 1,
        Cleric = 2,
        Scout = 3
    }

    public class UnitsData
    {

        private Dictionary<UnitTypeID, UnitType> mUnitTypes;
        private Dictionary<SupportUnitTypeID, UnitType> mSupportUnitTypes;

        private UnitsData() {}
        public UnitsData(UnitsDataFactory uf) {
            this.init();
        }

        public List<UnitTypeID> getEncounterableForceUnitTypes() {
            return new List<UnitTypeID>(new UnitTypeID[] {
                UnitTypeID.Warrior,
                UnitTypeID.Bandit,
                UnitTypeID.Werebear,
                UnitTypeID.Goblin,
                UnitTypeID.Orc,
                UnitTypeID.Troll,
                UnitTypeID.Ogre,
                UnitTypeID.EarthGiant,
                UnitTypeID.Wizard,
                UnitTypeID.Hacker,
                UnitTypeID.Hydra,
                UnitTypeID.Gorgon,
                UnitTypeID.Hobgoblin,
                UnitTypeID.Elf,
                UnitTypeID.Dwarf
            });
        }

        public bool isParlayable(UnitTypeID unitTypeID)
        {
            return unitTypeID == UnitTypeID.Warrior || unitTypeID == UnitTypeID.Elf || unitTypeID == UnitTypeID.Dwarf;
        }

        public UnitType getUnitTypeByID(UnitTypeID id) {
            return mUnitTypes[id];
        }
        public UnitType getSupportUnitTypeByID(SupportUnitTypeID id)
        {
            return mSupportUnitTypes[id];
        }

        private void init() {
            mUnitTypes = new Dictionary<UnitTypeID, UnitType>();
            // These come from WKDTINIT#fc0801  - in the code, they are referenced as O(*) - they are used to determine the number of units that are present in an encounter
            //WARRIOR, 1,BANDIT,  .7,WEREBEAR,5,GOBLIN,.3,ORC,.5,TROLL,10,OGRE,16,EARTH GIANT,20,WIZARD,25,HACKER,50,DRAGON,100 
            //WITCH KING,150,HYDRA,40,GORGON,30,HOBGOBLIN,.6,ELVE,2,DWARVE,3
            mUnitTypes.Add(UnitTypeID.Warrior, new UnitType("Warrior", 1f));
            mUnitTypes.Add(UnitTypeID.Bandit, new UnitType("Bandit", .7f));
            mUnitTypes.Add(UnitTypeID.Werebear, new UnitType("Werebear", 5f));
            mUnitTypes.Add(UnitTypeID.Goblin, new UnitType("Goblin", .3f));
            mUnitTypes.Add(UnitTypeID.Orc, new UnitType("Orc", .5f));
            mUnitTypes.Add(UnitTypeID.Troll, new UnitType("Troll", 10f));
            // As I trace this, it is possible that an OGRE, DRAGON or WITCH KING can cast a spell (as well as wizards and elves)
            // - if S=6 or S=10, it will fall through line 4000,
            // either fall through line 4010 or not and still get to line 4050, which will send it to line 4090 - the spell casting line
            // 4000  PRINT: IF S = 8 OR S = 9 OR S = 11 THEN 4050
            // 4010  IF RND(1) < .80 THEN 4050
            // 4020  PRINT "THE "O$(S);: IF X > 1 THEN PRINT "S ARE CHARGING";: GOTO 4040
            // 4030  PRINT " IS CHARGING";
            // 4040  PRINT: GOSUB 1390:D1 = D1 + RND(1) * Y * .3
            // 4050  PRINT: IF S = 6 OR S = 9 OR S = 11 OR S = 8 OR S = 10 THEN 4090
            mUnitTypes.Add(UnitTypeID.Ogre, new UnitType("Ogre", 16f, true));
            mUnitTypes.Add(UnitTypeID.EarthGiant, new UnitType("EarthGiant", 20f));
            mUnitTypes.Add(UnitTypeID.Wizard, new UnitType("Wizard", 25f, true));
            mUnitTypes.Add(UnitTypeID.Hacker, new UnitType("Hacker", 50f, true));
            mUnitTypes.Add(UnitTypeID.Dragon, new UnitType("Dragon", 100f, true));
            mUnitTypes.Add(UnitTypeID.WitchKing, new UnitType("WitchKing", 150f, true));
            mUnitTypes.Add(UnitTypeID.Hydra, new UnitType("Hydra", 40f));
            mUnitTypes.Add(UnitTypeID.Gorgon, new UnitType("Gorgon", 30f));
            mUnitTypes.Add(UnitTypeID.Hobgoblin, new UnitType("Hobgoblin", .6f));
            mUnitTypes.Add(UnitTypeID.Elf, new UnitType("Elf", 2f, true));
            mUnitTypes.Add(UnitTypeID.Dwarf, new UnitType("Dwarf", 3f));

            mSupportUnitTypes = new Dictionary<SupportUnitTypeID, UnitType>();
            mSupportUnitTypes.Add(SupportUnitTypeID.Mule, new UnitType("Mule", 0f));
            mSupportUnitTypes.Add(SupportUnitTypeID.Cleric, new UnitType("Cleric", 0f));
        }
    }

    public class UnitsDataFactory
    {

        private static UnitsData mSingleton;

        public static UnitsData getUnitsData() {
            if (mSingleton == null) {
                mSingleton = new UnitsData(new UnitsDataFactory());
            }
            return mSingleton;
        }
    }
}
