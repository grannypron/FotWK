using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace FotWK
{
    public abstract class EncounterableLocation : BaseLocation
    {

        IEnumerator WaitAction()
        {
            yield return new UnityEngine.WaitForSeconds(3);
        }

        public IEnumerator encounter()
        {
            VisitSceneEvents visitSceneEvents = VisitSceneEvents.GetVisitSceneEvents();
            visitSceneEvents.SetText("AN ENCOUNTER!!", true);
            UnityGameEngine.getEngine().getSoundEngine().playSound("AttentionAndCharge", visitSceneEvents);   // I think this corresponds to "GOSUB 1380" in the Apple II basic

            // Wait about 3 seconds while the sound plays, but you don't have to wait until it stops
            yield return new WaitForSeconds(3);

            PlayerState player = GameStateManager.getGameState().getCurrentPlayerState();
            UnitsData units = UnitsDataFactory.getUnitsData();
            player.setSurprised(false);

            Force force = generateRandomOpposingForce(player.getParty().force);
            foreach (KeyValuePair<UnitTypeID, int> entry in force)
            {
                // 2210  PRINT "YOU HAVE COME UPON "X" "O$(S);: 
                //          IF X > 1 THEN  PRINT "S";                   - plurals
                visitSceneEvents.AddTextLine(String.Format("YOU HAVE COME UPON {0} {1}{2}",
                    entry.Value,
                    units.getUnitTypeByID(entry.Key).getName().ToUpper(),
                    entry.Value > 1 ? "S" : ""));

                visitSceneEvents.AddTextLine("");
            }

            // Roll for surprise & play sound if surprised - 20% chance of being surprised
            // 2220  PRINT :D = 0:D1 = 0:X3 = 1: IF  RND (1) < .2 THEN  PRINT : PRINT "YOU HAVE BEEN SURPRISED!": GOSUB 1390: GOTO 2290
            if (RNG.rollAgainstPercentage(Globals.SURPRISE_CHANCE)) {
                visitSceneEvents.AddTextLine("YOU HAVE BEEN SURPRISED!");
                player.setSurprised(true);
                UnityGameEngine.getEngine().getSoundEngine().playSound("Disappointment", visitSceneEvents);
                // Battle automatically happens when you are surprised
            } else {
                player.setSurprised(false);
                //2230  PRINT : IF S = 0 OR S > 14 THEN  PRINT "DO YOU WISH TO PARLEY";: GOSUB 40: IF  NOT Y THEN 2270
                UnitTypeID unitTypeID = force.Keys.First<UnitTypeID>(); // Assume here that the force is only made up of one unit type
                if (UnitsDataFactory.getUnitsData().isParlayable(unitTypeID))
                {
                    // Collect input.  If y, do the parlay routine
                    visitSceneEvents.AddTextLine("DO YOU WISH TO PARLEY");        // GOSUB 40 prints the "(Y/N)?"
                    GameStateManager.getGameState().setCurrentEnemyForce(force);
                    InputReceiverEvents.GetInputReceiverEvents().ActivateInputKeypress(handleParlayInput);

                    //TODO: Here we should go to the battle screen - or maybe just move the else clause below out of the else and it's parent else
                }
            }

            GameStateManager.getGameState().setCurrentEnemyForce(force);
            yield return LoadNextScene("BattleScene");

        }

        private void handleParlayInput(string key)
        {
            PlayerState player = GameStateManager.getGameState().getCurrentPlayerState();
            VisitSceneEvents visitSceneEvents = VisitSceneEvents.GetVisitSceneEvents();

            //2240  IF S > 0 AND S < 15 THEN 2270       - This probably is needed in case it is GOSUB/GOTO'd from somewhere other than 2230

            //2250  IF  RND (1) < (I%(P,0) + I%(P,4)) * .01 THEN  PRINT "THEY WILL JOIN THE RANKS!!": GOSUB 1400: GOTO 2580
            // I%(P,0) is rations - see line 380 and I%(P,4) is gold = see line 1710
            if (RNG.rollAgainstPercentage(player.getParty().rations + player.getParty().gold))   // TODO: Double check this
            {
                visitSceneEvents.AddTextLine("THEY WILL JOIN THE RANKS!!");

                // Add these folks to the party count
                Force force = GameStateManager.getGameState().getCurrentEnemyForce();
                UnitTypeID unitTypeID = force.Keys.First<UnitTypeID>();
                player.getParty().force[unitTypeID] += force.Values.First<int>();
                
                // Clean up and change the scene
                GameStateManager.getGameState().setCurrentEnemyForce(null);
                visitSceneEvents.StartCoroutine(LoadNextScene("MoveScene"));
            }

        }

        public override void onVisit()
        {
            if (RNG.rollAgainstPercentage(Globals.ENCOUNTER_CHANCE_MULTIPLIER * getEncounterChance()))
            {
                VisitSceneEvents visitSceneEvents = VisitSceneEvents.GetVisitSceneEvents();
                visitSceneEvents.StartCoroutine(encounter());
            }
            else
            {
                NextScene("MoveScene");
            }
        }

        
        // This comes from the Apple II source:  
        // line 2170  TEXT : HOME : PRINT  TAB( 15)"AN ENCOUNTER!!": GOSUB 1380
        // I think the game gets to 2170 from two places:
        // 1780  IF  RND (1) < .8 THEN 2170
        // ...
        // 1800  IF  RND (1) < .28 THEN 2170
        // I think the game gets to these from:
        // 1220  IF DF = 8 THEN  PRINT  TAB( 16)"MOUNTAINS": GOSUB 1760: GOTO 1250
        // 1230  IF DF = 12 THEN  PRINT  TAB( 17)"FOREST": GOSUB 1800: GOTO 1250
        // So, it looks to me like there are two different percentages for mountains and forest
        // child classes will implement as appropriate
        public abstract int getEncounterChance();


        public static Force generateRandomOpposingForce(Force playerForce) {
            // 2180 S =  INT ( RND (1) * 17): IF S = 11 OR S = 10 THEN 2180
            // Calculate a random number from 1-17, except for 11 or 10
            // Probably the 11/10 exception was to skip "WITCH KING" and "DRAGON"
            // Here, getEncounterableForceUnits() filters those two out for us
            UnitsData units = UnitsDataFactory.getUnitsData();
            List<UnitTypeID> encounterableTypes = units.getEncounterableForceUnitTypeID();
            int unitTypeIdx = RNG.rollInRange(3, 3);//encounterableTypes.Count)  TODO: Remove;
            UnitTypeID monsterId = encounterableTypes[unitTypeIdx];
            UnitType monster = units.getUnitTypeByID(monsterId);
            
            Force opposingForce = new Force();

            // 2190 L = O(S):                                               - I THINK this is pulling the number that is stored with each of the names somehow
            //          X =  INT (1 / L * (I%(P,5) + I%(P,22))):            - Note this from Wade Clarke's FAQ:  "Monsters usually appear in groups whose size is calculated relative to the size of your own forces"   I%(P,5) is the number of warriors in your group - see line 140.  I%(P,22) is the number of dwarves - see line 2410
            int qty = (int)((1/monster.getWeight()) * (playerForce[UnitTypeID.Warrior] + playerForce[UnitTypeID.Dwarf]));

            //          X =  INT (X + (X * .15 * ( RND (1) * 3 - 1))): 
            qty = (int)(qty + (qty * Globals.ENCOUNTER_ENEMY_SIZE_MULTIPLIER * (UnityEngine.Random.Range(0, Globals.ENCOUNTER_ENEMY_SIZE_RANDOM_FACTOR) - 1) ));

            //          IF X < 1 THEN X = 1                                 - At least one
            if (qty < 1) {
                qty = 1;
            }

            // 2200 X7 = X * L:                                            - Not sure what X7 is used for right now, might be loot generation - see 3560

            //          IF S > 14 AND X < 2 THEN X = 2                      -  So for the last 3 types in the list, Elves, Dwarves, and Hobgolins(interestingly?), there must always be at least 2
            if (monsterId == UnitTypeID.Elf || monsterId == UnitTypeID.Dwarf || monsterId == UnitTypeID.Hobgoblin) {
                if (qty < 2) {
                    qty = 2;
                }
            }

            opposingForce[monsterId] = qty;

            return opposingForce;
        }

    }

}
