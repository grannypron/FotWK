using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class BattleSceneEvents : MonoBehaviour
{
    private bool mListenForRetreat = false;
    private bool mRetreatPressed = false;
    private bool mEnterPressed = false;
    private string mRunAwayKey = null;
    private bool mBattleOver = false;

    private float mDamageAgainstPlayer;  // TODO: come back and improve this - I should probably be using async/await instead of this member variable

    // Start is called before the first frame update
    void Start()
    {
        harness_ForTesting(); // TODO: Remove
        mBattleOver = false;
        
        StartCoroutine(battle());

        //initForDemo(); // TODO: Remove
    }

    public void initForDemo()
    {
        GameStateManager.getGameState().initForDemo();
        PlayerState player = GameStateManager.getGameState().getCurrentPlayerState();
        FotWK.Force force = FotWK.EncounterableLocation.generateRandomOpposingForce(player.getParty().force);
        GameStateManager.getGameState().setCurrentEnemyForce(force);
    }


    public IEnumerator battle() {

        PlayerState player = GameStateManager.getGameState().getCurrentPlayerState();
        FotWK.Force enemyForce = GameStateManager.getGameState().getCurrentEnemyForce();
        FotWK.Force originalEnemyForce = new FotWK.Force(enemyForce);
        // Initial surprise mechanic that you get if you are not surprised
        if (!player.getSurprised())
        {
            yield return RunAwayOption();
        }

        while (!mBattleOver)
        {
            mEnterPressed = false;
            Text txtScreenText = GameObject.Find("txtScreenText").GetComponent<Text>();
            txtScreenText.text = Utility.centerString("PRESS 'R' TO RETREAT\n");

            // 2220  PRINT: D = 0:D1 = 0:X3 = 1
            float playerDamage = 0;  // D I assumed this to be for "Damage"
            mDamageAgainstPlayer = 0;
            float hammerOfThorMultiplier = 1;  // This is X3 in the game code - It can be 1.85 in the final fortress when player has the armor.  See line 2650

            FotWK.UnitTypeID enemyUnitTypeID = enemyForce.GetFirstUnitTypeID();

            // 2300  PRINT "THE BATTLE RAGES ON"
            txtScreenText.text += "\nTHE BATTLE RAGES ON\n";
            mListenForRetreat = true;
            mRetreatPressed = false;
            yield return new WaitForSeconds(2);


            FotWK.Force playerForce = player.getParty().force;

            //2290... Y = X * L:Y1 = (I % (P, 5) + (I % (P, 11) * 20) + (RND(1) * 15) + (I % (P, 17) * 15) * X3) + I % (P, 22) * 2
            float Y = enemyForce.Get(enemyUnitTypeID) * FotWK.UnitsDataFactory.getUnitsData().getUnitTypeByID(enemyUnitTypeID).getWeight();
            float playerForceWeight = playerForce.Get(FotWK.UnitTypeID.Warrior) +
                (playerForce.Get(FotWK.UnitTypeID.Wizard) * 20) + (FotWK.RNG.rollPercentage0To1() * 15) +
                (player.getParty().hasSpecialItem(FotWK.SpecialItemTypeID.HammerOfThor) ? 1 * 15 : 0 * hammerOfThorMultiplier) +   // TODO: Can multiple hammers of thor be had?
                (playerForce.Get(FotWK.UnitTypeID.Dwarf) * 2);

            if (FotWK.RNG.rollPercentage0To1() < playerForceWeight * Globals.ATTACK_PRESS_CHANCE_MULTIPLIER)
            { /*IF RND(1) < Y1 * .012 THEN*** Note here that ALL statements after on the line after the "THEN" keyword will be executed(when there are multiple separated by colons), not just the first */
                FotWK.UnityGameEngine.getEngine().getSoundEngine().playSound("Attention", GetBattleSceneEvents());     //GOSUB 1400: 
                // PRINT N$(P)"'S SIDE PRESSES THE ATTACK ":
                txtScreenText.text += "\n";
                txtScreenText.text += Utility.centerString(GameStateManager.getGameState().getCurrentPlayerState().getName().ToUpper() + "'S SIDE PRESSES THE ATTACK");
                txtScreenText.text += "\n";
                /* 
                 *D = D + RND(1) * Y1 + 1:
                 */
                playerDamage += FotWK.RNG.rollPercentage0To1() * playerForceWeight + 1;

                yield return BattleActions(playerDamage);
            }

            // -- RETURNING FROM GOSUB 3840:
            // 2310  IF RND(1) < Y * .01 THEN PRINT "THE "O$(S);: IF X > 1 THEN PRINT "S ADVANCE";: GOTO 2340
            if (FotWK.RNG.rollPercentage0To1() < Y * Globals.MONSTERS_ADVANCE_COMPARE)
            {
                // 2340  PRINT: D1 = D1 + RND(1) * Y * .4: GOSUB 1390: GOSUB 4000
                // 2350 Z = INT(RND(1) * 12): IF Z = 5 OR Z< 2 OR Z = 4 OR Z = 6 OR Z = 8 OR Z = 9 THEN 2380
                // 2360  IF I% (P, Z) < 1 THEN 2380
                // 2370 I % (P, Z) = I % (P, Z) - 1: PRINT "SIRE WE HAVE JUST LOST A "I$(Z): GOSUB 1390: IF(Z = 2 OR Z = 3) AND RND(1) < .32 THEN 2360
                // 2380  IF I% (P, 21) < 1 OR RND(1) > .4 THEN 2400
                // 2390 I % (P, 21) = I % (P, 21) - 1: PRINT "WE LOST AN ELF SIRE": IF I% (P, 21) = 1 THEN I% (P, 21) = 0: PRINT "SIRE WE LOST ANOTHER ELF"
                // 2400  IF I% (P, 22) < 1 OR RND(1) > .4 THEN 2420
                // 2410 I % (P, 22) = I % (P, 22) - 1: PRINT "WE LOST A DWARF SIRE": IF I% (P, 22) = 1 THEN I% (P, 22) = 0: PRINT "SIRE WE LOST ANOTHER DWARF"
                // 2420  IF RND(1) < .2 THEN 2380
                // 2430  IF RND(0) < .3 THEN 2350
            }
            else
            {
                // 2320  IF NOT  RND(0) < Y * .01 THEN 2440
                // RND(0) is the last randomly generated number
            }

            // 2330  PRINT " ADVANCES";    // I don't think this line ever gets hit

            // 2440  PRINT: PRINT "ROUND UPDATE:": PRINT: I % (P, 5) = I % (P, 5) - D1: IF I% (P, 5) < 1 THEN I% (P, 5) = 0
            txtScreenText.text += Utility.centerString("ROUND UPDATE:");
            txtScreenText.text += "\n";
            Debug.Log("mDamageAgainstPlayer:" + mDamageAgainstPlayer);
            Debug.Log("mDamageAgainstPlayer:" + playerDamage);
            playerForce.Set(FotWK.UnitTypeID.Warrior, System.Math.Max(0, playerForce.Get(FotWK.UnitTypeID.Warrior) - (int)System.Math.Floor(mDamageAgainstPlayer)));

            // 2450  ON I% (P, 2) < 1 GOTO 2480: ON RND(1) > (.35 + I % (P, 2) * .01) GOTO 2480: ON D1<  = 0 GOTO 2480
            int numClerics = player.getParty().supportUnits.Get(FotWK.SupportUnitTypeID.Cleric);
            if (numClerics > 0 && FotWK.RNG.rollPercentage0To1() < (0.35 + numClerics * .01) && mDamageAgainstPlayer > 0)
            {
                // 2460 Z = INT(D1 * I % (P, 2) * .03 + 1): IF D1<Z THEN Z = D1
                int restoredUnits = (int)(mDamageAgainstPlayer * numClerics * .03f + 1f);
                restoredUnits = (int)System.Math.Min(mDamageAgainstPlayer, restoredUnits);

                // 2470 Z = INT(Z):I % (P, 5) = I % (P, 5) + Z: PRINT "YOUR "I$(2)"S RESTORE "Z" "I$(5);: IF Z > 1 THEN PRINT "S";
                playerForce.Set(FotWK.UnitTypeID.Warrior, playerForce.Get(FotWK.UnitTypeID.Warrior) + restoredUnits);

                txtScreenText.text += Utility.centerString("YOUR CLERICS RESTORE " + restoredUnits + " " + Utility.addS(restoredUnits, "WARRIOR"));
                txtScreenText.text += "\n";


            }

            // 2480 D1 = 0: PRINT: PRINT "YOU HAVE "I % (P, 5)" "I$(5);: IF I% (P, 5) <  > 1 THEN PRINT "S";
            txtScreenText.text += "\n";
            txtScreenText.text += Utility.centerString("YOU HAVE " + playerForce.Get(FotWK.UnitTypeID.Warrior) + " " + Utility.addS(playerForce.Get(FotWK.UnitTypeID.Warrior), "WARRIOR"));
            txtScreenText.text += "\n";

            // 2490  PRINT: Z = INT(D / L): ...
            float enemyWeight = FotWK.UnitsDataFactory.getUnitsData().getUnitTypeByID(enemyUnitTypeID).getWeight();
            int Z = (int) (playerDamage / enemyWeight);
                
            // 2490  ... IF Z< 1 THEN 2510
            if (Z >= 1)
            {
                // 2500 X = X - Z:D = D - Z * L: IF D< 0 THEN D = 0
                enemyForce.Set(enemyUnitTypeID, enemyForce.Get(enemyUnitTypeID) - Z);
                playerDamage -= Z * enemyWeight;
                playerDamage = System.Math.Max(0, playerDamage);

            }

            // 2510  IF X< 1 THEN X = 0
            enemyForce.Set(enemyUnitTypeID, System.Math.Max(enemyForce.Get(enemyUnitTypeID), 0));
            // 2520  PRINT "THEY HAVE "X" "O$(S);: IF X<  > 1 THEN PRINT "S";
            txtScreenText.text += Utility.centerString("THEY HAVE " + enemyForce.Get(enemyUnitTypeID) + " " + Utility.addS(enemyForce.Get(enemyUnitTypeID), FotWK.UnitsDataFactory.getUnitsData().getUnitTypeByID(enemyUnitTypeID).getName().ToUpper()));
            txtScreenText.text += "\n";

            if (enemyForce.IsEmpty()) {
                txtScreenText.text += "VICTORY IS YOURS!!\n";  // 2540  IF X < 1 THEN  GOSUB 1380:
                FotWK.UnityGameEngine.getEngine().getSoundEngine().playSound("AttentionAndCharge", GetBattleSceneEvents());
                yield return new WaitForSeconds(3);
                // 2530...IF I%(P,5) < 1 THEN  PRINT "JUST BARELY!!"
                if (playerForce.Get(FotWK.UnitTypeID.Warrior) < 1)
                {
                    txtScreenText.text += "JUST BARELY!!\n";
                }
                GiveLoot(originalEnemyForce);
                mBattleOver = true;
            }


            txtScreenText.text += "\n\n     PRESS RETURN TO CONTINUE";
            yield return new WaitUntil(() => mEnterPressed);

            if (mRetreatPressed)
            {
                if (FotWK.RNG.rollAgainstPercentage(Globals.RUN_AWAY_CHANCE))   // 2270  ...: IF Y AND  RND (1) < .8 THEN  GOSUB 1390: ...
                {
                    // TODO: WILL YOU RUN AWAY
                    yield return StartCoroutine(RunAwayOption());
                }
            }
        }

        AsyncOperation asyncLoad = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync("MainMenu");
        // Wait until the asynchronous scene fully loads
        while (!asyncLoad.isDone)
        {
            yield return null;
        }

    }

    void GiveLoot(FotWK.Force originalEnemyForce)
    {
        Text txtScreenText = GameObject.Find("txtScreenText").GetComponent<Text>();
        txtScreenText.text = Utility.centerString("YOU HAVE FOUND THE FOLLOWING ITEMS\n\n");  //*  3520  PRINT "YOU HAVE FOUND THE FOLLOWING ITEMS": PRINT : ...  //7000 PRINT"YOU HAVE FOUND THE FOLLOWING ITEMS":PRINT

        PlayerState player = GameStateManager.getGameState().getCurrentPlayerState();
        FotWK.Force enemyForce = GameStateManager.getGameState().getCurrentEnemyForce();
        FotWK.UnitTypeID monsterId = originalEnemyForce.GetFirstUnitTypeID();

        // *3520  ...IF S = 10 THEN I% (P, 6) = 0 // 7005 IF S = 10 THEN I% (P, 6) = 0
        if (monsterId == FotWK.UnitTypeID.Dragon && originalEnemyForce.Get(FotWK.UnitTypeID.Dragon) > 0)
        {
            if (player.getParty().hasSpecialItem(FotWK.SpecialItemTypeID.DragonSlayer))
            {
                player.getParty().removeSpecialItem(FotWK.SpecialItemTypeID.DragonSlayer);
            }
        }

        //3530  IF  RND (1) < .25 THEN I%(P,1) = I%(P,1) + 1: PRINT I$(1)  
        //7010 IF RND(1)< .25 THEN I% (P, 1) = I % (P, 1) + 1:PRINT I$(1)
        if (FotWK.RNG.rollAgainstPercentage(Globals.LOOT_TELEPORT_SPELL_PERCENTAGE))
        {
            player.getParty().spells.Set(FotWK.SpellType.TELEPORT, player.getParty().spells.Get(FotWK.SpellType.TELEPORT) + 1);
        }

        // 3540  IF RND(1) < .2 AND I% (P, 6) < 1 THEN I% (P, 6) = 1: PRINT I$(6) 
        // 7020 IF RND(1)< .2 AND I% (P, 6) < 1 THEN I% (P, 6) = 1:PRINT I$(6)
        if (FotWK.RNG.rollAgainstPercentage(Globals.LOOT_DRAGON_SLAYER_PERCENTAGE))
        {
            if (!player.getParty().hasSpecialItem(FotWK.SpecialItemTypeID.DragonSlayer))
            {
                player.getParty().addSpecialItem(FotWK.SpecialItemTypeID.DragonSlayer);
            }
        }

        // 3550  IF RND(1) < .15 AND I% (P, 8) < 1 THEN I% (P, 8) = 1: PRINT I$(8)  
        // 7025 IF RND(1)< .15 AND I% (P, 8) < 1 THEN I% (P, 8) = 1:PRINT I$(8)
        if (FotWK.RNG.rollAgainstPercentage(Globals.LOOT_SPELL_OF_SEEING_PERCENTAGE))
        {
            player.getParty().spells.Set(FotWK.SpellType.SEEING, 1);
        }

        float lootFactor = originalEnemyForce.CalculateLootFactor();
        // 3560 Z =  INT ( RND (1) * X7 + X7) + 10: 
        // 7030 Z = INT(RND(1) * X7 + X7) + 10:
        int lootValue = ((int)(FotWK.RNG.rollPercentage0To1() * lootFactor + lootFactor)) + 10;
        // 3560  ...IF S = 10 OR S = 12 THEN Z = Z * 2
        // 7030  ...IF S = 10 OR S = 12 THEN Z = Z * 2
        if (monsterId == FotWK.UnitTypeID.Dragon || monsterId == FotWK.UnitTypeID.Hydra)
        {
            lootValue = lootValue * 2;
        }
        // 3570  IF S = 11 THEN Z = 2000
        // 7031 IF S = 11 THEN Z = 2000
        if (monsterId == FotWK.UnitTypeID.WitchKing)
        {
            lootValue = Globals.WITCH_KING_LOOT_VALUE;
        }
        
        txtScreenText.text += Utility.centerString(lootValue + Utility.addS(lootValue, " GOLD PIECE") + "\n"); // 3580  PRINT Z" "I$(4);: IF Z > 1 THEN  PRINT "S"  // 7033 PRINT Z" "I$(4);:IF Z> 1 THEN PRINT"S";

        if ((player.getParty().gold + lootValue) > Globals.PLAYER_GOLD_CAP)
        {  //3590  IF I%(P,4) + Z > 30000 THEN I%(P,4) = 30000:Z = 0 // 7034 IF I% (P, 4) + Z > 30000 THEN I% (P, 4) = 30000:Z = 0
            player.getParty().setGold(Globals.PLAYER_GOLD_CAP);
        } else


        txtScreenText.text += "\n";  // 3600  PRINT :  // 7035 PRINT:
        //3600 ...I%(P,4) = I%(P,4) + Z:...
        // 7035 ...I % (P, 4) = I % (P, 4) + Z
        player.getParty().addGold(lootValue);

        //3600... IF RND(1) < .7 THEN 3640
        //7040 IF RND(1)< .7 THEN 7090
        if (FotWK.RNG.rollAgainstPercentage(Globals.MAGIC_MAP_CHANCE))
        {
            //3610 Z = INT(RND(1) * 4):Z = Z + 12:...  In the original, Z is the index in the inventory slot between 12 and 15.  These indexes represent the magic map "slots" for the 4 magic maps.
            int magicMapIndex = (int)(FotWK.RNG.rollPercentage0To1() * 4);

            // For inventory slots, 12-15 represent both a magic map and a "special item" (e.g. Boots of Stealth, Armour of Defense, etc)
            // The writer used the value "1", "10", or "11" to indicate whether the player has a magic map in that slot (i.e. magic map #1, #2, etc), by the number being 1 in the ones place and 10 or 11 representing the presence of the special item because of the 1 in the tens place
            // Kind of like a faux representation of a bitwise operator, but it didn't really work that way since the integer is still represented by 8 bits either way, but whatevs

            //3610 IF I% (P, Z) = 1 OR I% (P, Z) = 11 THEN 3640
            bool alreadyHaveMap = false;
            FotWK.MagicMap foundMap = new FotWK.MagicMap(magicMapIndex);
            foreach (FotWK.MagicMap m in player.getParty().magicMaps)
            {
                if (m == foundMap)
                {
                    alreadyHaveMap = true;
                }
            }
            if (!alreadyHaveMap)
            {
                //3620  PRINT "MAGIC MAP": IF I% (P, Z) > 0 THEN I% (P, Z) = 11: GOTO 3640
                txtScreenText.text += Utility.centerString("MAGIC MAP\n");
                //3630 I % (P, Z) = 1
                player.getParty().magicMaps.Add(foundMap);
            }

            // Doing my best to cover the logic here - most of the below has to do with magic maps and the 1/11 flags i think
            //3640  GOSUB 3700: GOSUB 3740: IF S<  > 10 THEN 30
            //3650 Z = 9: FOR X = 0 TO 3: IF S% (X, 0) = P % (P, 0) AND S% (X, 1) = P % (P, 1) THEN Z = X
            //3660  NEXT: IF Z > 3 GOTO 30
            //3670 Z = Z + 12: IF I% (P, Z) > 1 GOTO 30
            //3680  PRINT I$(Z): IF I% (P, Z) > 0 THEN I% (P, Z) = 11: GOTO 30
            //3690 I % (P, Z) = 10: GOTO 30

            // Special item - reversed the boolean
            //3700  IF L< 10 OR RND(1) > .3 THEN RETURN
            if (FotWK.UnitsDataFactory.getUnitsData().getUnitTypeByID(monsterId).getWeight() > 10 && 
                FotWK.RNG.rollAgainstPercentage(Globals.LOOT_SPECIAL_ITEM_PERCENTAGE))
            {
                //3710 Z = INT(RND(1) * 3) + 17: 
                int z = FotWK.RNG.rollInRange(0, 2);
                // IF I% (P, Z) > 0 THEN RETURN     // Do not give out more than one of these and if you roll the same one, you end up getting nothing
                
                switch (z)
                {
                    case 0:
                        if (!player.getParty().hasSpecialItem(FotWK.SpecialItemTypeID.HammerOfThor))
                        {
                            //3730 I % (P, Z) = 1: PRINT I$(Z): RETURN
                            player.getParty().addSpecialItem(FotWK.SpecialItemTypeID.HammerOfThor);
                        }
                        break;
                    case 1:
                        if (!player.getParty().hasSpecialItem(FotWK.SpecialItemTypeID.TalismanOfSpeed))
                        {
                            //3730 I % (P, Z) = 1: PRINT I$(Z): RETURN
                            player.getParty().addSpecialItem(FotWK.SpecialItemTypeID.TalismanOfSpeed);
                        }

                        //3720  IF Z = 18 THEN MV = MV + 1: IF MV< 1 THEN MV = 1
                        // TODO: AFFECT MOVE SPEED HERE - MV is MOVES!

                        break;
                    case 2:
                        if (player.getParty().spells.Get(FotWK.SpellType.SEEKING) <= 0)
                        {
                            //3730 I % (P, Z) = 1: PRINT I$(Z): RETURN
                            player.getParty().spells.Set(FotWK.SpellType.SEEKING, 1);
                        }
                        break;
                    default:
                        Utility.assert(false);
                        break;
                }

            }


            if (monsterId == FotWK.UnitTypeID.WitchKing)
            {
                //3740  IF S = 11 THEN PRINT "THE CROWN OF THE WITCH KING": PRINT "THE SCEPTER OF THE WITCH KING": PRINT "THE ORB OF THE WITCH KING": PRINT "HUNDREDS OF GEMS AND PIECES OF JEWELERY"
                //3750  RETURN 
                // TODO: Handle ending here I guess?
            }


        }

    }

    IEnumerator RunAwayOption()
    {
        // 2270  PRINT "WILL YOU RUN AWAY";: GOSUB 40: IF Y AND RND(1) < .8...
        Text txtScreenText = GameObject.Find("txtScreenText").GetComponent<Text>();
        txtScreenText.text += "\nWILL YOU RUN AWAY (Y/N)?\n";
        InputReceiverEvents.GetInputReceiverEvents().ActivateInputKeypress(handleRunAwayInput);
        yield return new WaitUntil(() => mRunAwayKey != null);
        if (mRunAwayKey == "y")
        {
            mBattleOver = true;
            // Successful run away 
            txtScreenText.text += "     COWARD\n";   // 2270  ...PRINT "     COWARD":CO = 1: GOTO 1390
                                                     // I believe the CO = 1 is irrelevant here, because that is only considered on 2760, which I believe is during a Witch-King attack?
            FotWK.UnityGameEngine.getEngine().getSoundEngine().playSound("Disappointment", GetBattleSceneEvents());
            yield return new WaitForSecondsRealtime(2);

            AsyncOperation asyncLoad = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync("MainMenu");
            // Wait until the asynchronous scene fully loads
            while (!asyncLoad.isDone)
            {
                yield return null;
            }

        }
        mRunAwayKey = null;
        yield return null;
    }

    private void handleRunAwayInput(string key)
    {
        mRunAwayKey = key;
    }

    IEnumerator BattleActions(float damageAgainstMonsters)
    {

        PlayerState player = GameStateManager.getGameState().getCurrentPlayerState();
        FotWK.Force enemyForce = GameStateManager.getGameState().getCurrentEnemyForce();
        FotWK.UnitTypeID enemyType = enemyForce.GetFirstUnitTypeID();
        FotWK.Force playerForce = player.getParty().force;
        FotWK.UnitType enemyUnitTypeData = FotWK.UnitsDataFactory.getUnitsData().getUnitTypeByID(enemyType);

        Text txtScreenText = GameObject.Find("txtScreenText").GetComponent<Text>();

        int pp = 0;  // Don't know what this does yet

        // 3840  PRINT: IF I% (P, 5) < 1 OR RND(1) < .75 THEN GOSUB 3890: GOTO 3910  // If you have no warriors, or if you fail against a 25% chance roll, go to 3890 to do the dwarf check 

        // If you don't have dwarves or you do have dwarves but you fail a 50% chance roll, then go back and move on to spellcasting chance
        // 3890  IF I% (P, 22) < 1 OR RND(1) < .5 THEN RETURN

        // Dwarf check - If you do have dwarves or pass a 50% chance roll 
        // 3900 M = 22:N = 2: GOTO 3860
        // Your Dwarve(s) are smashing the enemy
        // 3860  PRINT "YOUR "I$(M);: IF I% (P, M) > 1 THEN PRINT "S ARE SMASHING THE ENEMY";: GOTO 3880

        // 3880  PRINT : GOSUB 1400:D = D +  RND (1) * I%(P,M) * 2: RETURN // So D += RND (1) * Num Warriors * .8
        // 3870  PRINT " IS CRUSHING YOUR FOE";
        // "Your Dwarves are smashing the enemy" or "Your Dwarf is crushing your foe

        // Otherwise 
        // 3850  GOSUB 3890:M = 5:N = .8: GOSUB 3860: GOTO 3910   // go to 3890 to do the dwarf check then print "Your Warriors are smashing the enemy" or "Your Warrior is crushing your foe"
        // 3860  PRINT "YOUR "I$(M);: IF I% (P, M) > 1 THEN PRINT "S ARE SMASHING THE ENEMY";: GOTO 3880
        // 3870  PRINT " IS CRUSHING YOUR FOE";
        // "Your Warriors are smashing the enemy" or "Your Warrior is crushing your foe"
        // 3880  PRINT : GOSUB 1400:D = D +  RND (1) * I%(P,M) * 2: RETURN // So D += RND (1) * Num Dwarves * 2

        // So, this should equate to:
        // First, If you have warriors and you roll greater than 75, "Your warriors are crusing the enemy"
        // Next (and in addition), If you have dwarves and (another) roll greater than 50, "Your dwarves are crushing the enemy"

        if (playerForce.Get(FotWK.UnitTypeID.Warrior) > 0 && FotWK.RNG.rollAgainstPercentage(Globals.WARRIORS_CRUSHING_CHANCE))
        {
            FotWK.UnityGameEngine.getEngine().getSoundEngine().playSound("Attention", GetBattleSceneEvents());
            if (playerForce.Get(FotWK.UnitTypeID.Warrior) == 1)
            {
                txtScreenText.text += "YOUR WARRIOR IS CRUSHING YOUR FOE\n";
            }
            else {
                txtScreenText.text += "YOUR WARRIORS ARE CRUSHING THE ENEMY\n";
            }
            damageAgainstMonsters += FotWK.RNG.rollPercentage0To1() * playerForce.Get(FotWK.UnitTypeID.Warrior) * Globals.WARRIORS_CRUSHING_DAMAGE_MULTIPLER;
        }
        if (playerForce.Get(FotWK.UnitTypeID.Dwarf) > 0 && FotWK.RNG.rollAgainstPercentage(Globals.DWARVES_CRUSHING_CHANCE))
        {
            FotWK.UnityGameEngine.getEngine().getSoundEngine().playSound("Attention", GetBattleSceneEvents());
            if (playerForce.Get(FotWK.UnitTypeID.Dwarf) == 1)
            {
                txtScreenText.text += "YOUR DWARF IS CRUSHING YOUR FOE\n";
            }
            else {
                txtScreenText.text += "YOUR DWARVES ARE CRUSHING THE ENEMY\n";
            }
            damageAgainstMonsters += FotWK.RNG.rollPercentage0To1() * playerForce.Get(FotWK.UnitTypeID.Dwarf) * Globals.DWARVES_CRUSHING_DAMAGE_MULTIPLER;
        }

        // 3910  PRINT: IF I% (P, 11) < 1 OR RND(1) < .5 THEN 3980
        if (playerForce.Get(FotWK.UnitTypeID.Wizard) > 0 && FotWK.RNG.rollAgainstPercentage(Globals.WIZARDS_CAST_CHANCE))
        {

            string spellName = "";
            int df = 0;
            // 3920  PRINT "YOUR "I$(11)" HAS CAST A ";:DF = INT(RND(1) * 4 + 1): ON DF GOSUB 3930,3940,3950,3960: GOSUB 1400:D = D + DF * RND(1) * 1.3 + 1: GOTO 3980
            (df, pp) = CastRandomSpell("WIZARD");
            FotWK.UnityGameEngine.getEngine().getSoundEngine().playSound("Attention", GetBattleSceneEvents());

            // 3920...  D = D + DF * RND(1) * 1.3 + 1
            damageAgainstMonsters += df * FotWK.RNG.rollPercentage0To1() * Globals.WIZARD_SPELL_GENERAL_DAMAGE_MULTIPLIER + 1;

            txtScreenText.text += "YOUR WIZARD HAS CAST A " + spellName + "\n";
        }

        // 3980  PRINT: IF I% (P, 21) < 1 OR RND(1) < .65 THEN 3970  // 3970 is leaving the subroutine so, this really is a 35% chance
        if (playerForce.Get(FotWK.UnitTypeID.Elf) > 0 && FotWK.RNG.rollAgainstPercentage(Globals.ELVES_CAST_CHANCE))
        {
            // 3990  PRINT "YOUR "I$(21)"S HAVE CAST A ";:DF = INT(RND(1) * 4 + 1): ON DF GOSUB 3930,3940,3950,3960: GOSUB 1400:D = D + DF * RND(1) + I % (P, 21) / 2: GOTO 3970
            int df = 0;
            (df, pp) = CastRandomSpell("ELVES");
            // 3920...  D = D + DF * RND(1) + I % (P, 21) / 2
            damageAgainstMonsters += df * FotWK.RNG.rollPercentage0To1() * playerForce.Get(FotWK.UnitTypeID.Elf) / 2;
        }

        // 4000  PRINT: IF S = 8 OR S = 9 OR S = 11 THEN 4050
        // Wizard, Hacker, and Witch King do NOT have a chance to charge
        if (!(enemyType == FotWK.UnitTypeID.Wizard && enemyType == FotWK.UnitTypeID.Hacker && enemyType == FotWK.UnitTypeID.WitchKing))
        {
            // 4010  IF RND(1) < .80 THEN 4050
            if (FotWK.RNG.rollAgainstPercentage(Globals.MONSTERS_CHARGE_CHANCE))
            {
                string plural = " IS";
                if (enemyForce.Get(enemyType) > 1)
                {
                    plural = "S ARE";
                }
                // 4020  PRINT "THE "O$(S);: IF X > 1 THEN PRINT "S ARE CHARGING";: GOTO 4040
                // 4030  PRINT " IS CHARGING";
                string enemyName = enemyUnitTypeData.getName().ToUpper();
                txtScreenText.text += "THE " + enemyName + plural + " CHARGING\n";

                // 2290  ... Y = X * L
                float wieghtedForceUnits = enemyForce.Get(enemyType) * enemyUnitTypeData.getWeight();

                // 4040  PRINT: GOSUB 1390:D1 = D1 + RND(1) * Y * .3  // Note: 2290 ...Y = X * L
                mDamageAgainstPlayer += FotWK.RNG.rollPercentage0To1() * wieghtedForceUnits * 0.3f;  // TODO: Make constant

                FotWK.UnityGameEngine.getEngine().getSoundEngine().playSound("Disappointment", GetBattleSceneEvents());
                yield return new WaitForSeconds(3);

            }


        }


        if (enemyType == FotWK.UnitTypeID.Elf)
        {
            // 4070  IF S = 15 THEN 4140     // I rearranged this, but I believe this is how it would work because it would fall through
                                             // lines 4050 and 4060
            // Enemy elves cast faerie fire, then end
            
            // 4140  IF  RND (1) < .7 THEN 3970
            if (FotWK.RNG.rollAgainstPercentage(Globals.MONSTERS_CAST_CHANCE))
            {
                // 4150  PRINT : PRINT "THE "O$(S)"S CAST A FAERIE FIRE":D1 = D1 +  RND (1) * 10: PRINT :PP = 1: GOSUB 1460: GOTO 3970
                txtScreenText.text += "THE ELVES CAST A FAERIE FIRE\n";
                mDamageAgainstPlayer += FotWK.RNG.rollPercentage0To1() * 10;   // TODO: Make this a constant
                pp = 1;

            }
        }
        // 4050  PRINT: IF S = 6 OR S = 9 OR S = 11 OR S = 8 OR S = 10 THEN 4090
        else if (enemyUnitTypeData.isCaster())
        {
            // Enemy spell casters

            // 4090  IF RND(1) < .6 THEN 3970
            if (FotWK.RNG.rollAgainstPercentage(Globals.MONSTERS_CAST_CHANCE))
            {
                int df = 0;
                // 4100  PRINT "THE "O$(S)" CASTS A ";:DF = INT(RND(1) * 4 + 1): ON DF GOSUB 3930,3940,3950,3960:...
                (df, pp) = CastRandomSpell(enemyUnitTypeData.getName().ToUpper());

                // 4100 ...D1 = D1 + RND(1) * DF * 1.2: GOSUB 1390: IF S<  > 10 THEN 3970
                mDamageAgainstPlayer += mDamageAgainstPlayer + FotWK.RNG.rollPercentage0To1() * df * 1.2f;   // TODO: Make constant
            }
        } 

        // 4060  IF S = 12 THEN 4110
        // Hydra or dragon (see 4100...IF S <  > 10 THEN 3970 for dragon) breathes fire, then end


        // 3970  FOR DF = 1 TO 500: NEXT: RETURN    // The return here I think actully returns back to the caller - line 2300
        // This line is really the "exit" to this method after all the things happen (I think - it's hard to trace)
        yield return new WaitForSeconds(3);



    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("r") && mListenForRetreat)
        {
            mRetreatPressed = true;
        } else if (Input.GetKeyDown(KeyCode.Return))
        {
            mEnterPressed = true;
        }
    }

    public (int, int) CastRandomSpell(string casterName)
    {
        Text txtScreenText = GameObject.Find("txtScreenText").GetComponent<Text>();
        int df = 0, pp = 0;
        string spellName = "";
        switch (FotWK.RNG.rollInRange(1, 4))
        {
            case 1:
                // 3930  PRINT "LIGHTNING BOLT":DF = 10: GOTO 1480
                df = Globals.SPELL_DAMAGE_FACTOR_LIGHTNING_BOLT;
                spellName = "LIGHTNING BOLT";
                break;
            case 2:
                // 3940  PRINT "FIRE BALL":DF = 15:PP = 5: GOTO 1460
                df = Globals.SPELL_DAMAGE_FACTOR_FIRE_BALL;
                pp = 5;
                spellName = "FIRE BALL";
                break;
            case 3:
                // 3950  PRINT "TOWER OF FLAME":DF = 10:PP = 5: GOTO 1460
                df = 10;
                pp = 5;
                spellName = "TOWER OF FLAME";
                break;
            case 4:
                // 3960  PRINT "DEATH RAY":DF = 7:PP = 2: GOTO 1460
                df = 7;
                pp = 2;
                spellName = "DEATH RAY";
                break;
            default:
                Utility.assert(false);
                break;
        }

        // 3990  PRINT "YOUR "I$(21)"S HAVE CAST A ";:DF = INT(RND(1) * 4 + 1): ON DF GOSUB 3930,3940,3950,3960: GOSUB 1400:D = D + DF * RND(1) + I % (P, 21) / 2: GOTO 3970
        txtScreenText.text += "YOUR " + casterName + " CASTS A " + spellName + "\n";

        return (df, pp);

    }
    public BattleSceneEvents GetBattleSceneEvents()
    {
        return GameObject.Find("EventSystem").GetComponent<BattleSceneEvents>();
    }

    void harness_ForTesting()
    {
        if (Utility.inittedAlreadyTODORemove) {
            return;
        }
        Utility.inittedAlreadyTODORemove = true;
        if (FotWK.UnityGameEngine.getEngine() == null)
        {
            GameObject engineObj = new GameObject("engine_ForTesting");
            FotWK.UnityGameEngine engine = engineObj.AddComponent<FotWK.UnityGameEngine>();
            FotWK.UnitySoundEngine soundEngine = engineObj.AddComponent<FotWK.UnitySoundEngine>();
            engine.setSoundEngine_ForTesting(soundEngine);
            FotWK.UnityGameEngine.setGameEngine_ForTesting(engine);
        }
        if (GameStateManager.getGameState().getCurrentEnemyForce() == null) { 
            GameStateManager.getGameState().initForDemo();
            FotWK.Force f = new FotWK.Force();
            f.Add(FotWK.UnitTypeID.Goblin, 1);
            GameStateManager.getGameState().setCurrentEnemyForce(f);
            Text txtScreenText = GameObject.Find("txtScreenText").GetComponent<Text>();
            txtScreenText.text = "Sample force: " + f;
        }
    }
}
