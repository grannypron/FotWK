using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class BattleSceneEvents : MonoBehaviour
{
    private bool mListenForRetreat = false;
    private bool mRetreatPressed = false;
    private string mRunAwayKey = null;
    private bool mBattleOver = false;

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

            float playerAdvantageFactor = 0;  // D in the AppleSoft Basic source code - zeroed out when the battle begins at 2220
            Text txtScreenText = GameObject.Find("txtScreenText").GetComponent<Text>();
            txtScreenText.text = Utility.centerString("PRESS 'R' TO RETREAT\n");

            // TODO: Improve - should only have one
            FotWK.UnitTypeID unitTypeID = FotWK.UnitTypeID.Warrior;
            foreach (FotWK.UnitTypeID id in enemyForce.Keys)
            {
                unitTypeID = id;
            }

            // 2300  PRINT "THE BATTLE RAGES ON"
            txtScreenText.text += "\nTHE BATTLE RAGES ON\n";
            mListenForRetreat = true;
            mRetreatPressed = false;
            yield return new WaitForSeconds(2);

            float armorOfDefenseMultiplier = 1;  // This is X3 in the game code - It can be 1.85 in the final fortress when player has the armor.  See line 2650

            FotWK.Force playerForce = player.getParty().force;
            //Y1 = (I % (P, 5) + (I % (P, 11) * 20) + (RND(1) * 15) + (I % (P, 17) * 15) * X3) + I % (P, 22) * 2
            float forceWeight = playerForce[FotWK.UnitTypeID.Warrior] +
                (playerForce[FotWK.UnitTypeID.Wizard] * 20) + (FotWK.RNG.rollPercentage0To1() * 15) +
                (player.getParty().hasSpecialItem(FotWK.SpecialItemType.HammerOfThor) ? 1 * 15 : 0 * armorOfDefenseMultiplier) +   // TODO: Can multiple hammers of thor be had?
                (playerForce[FotWK.UnitTypeID.Dwarf] * 2);

            if (FotWK.RNG.rollPercentage0To1() < forceWeight * Globals.ATTACK_PRESS_CHANCE_MULTIPLIER)
            { /*IF RND(1) < Y1 * .012 THEN*** Note here that ALL statements after on the line after the "THEN" keyword will be executed(when there are multiple separated by colons), not just the first */
                FotWK.UnityGameEngine.getEngine().getSoundEngine().playSound("Attention", GetBattleSceneEvents());     //GOSUB 1400: 
                // PRINT N$(P)"'S SIDE PRESSES THE ATTACK ":
                txtScreenText.text += "\n";
                txtScreenText.text += Utility.centerString(GameStateManager.getGameState().getCurrentPlayerState().getName().ToUpper() + "'S SIDE PRESSES THE ATTACK");
                txtScreenText.text += "\n";
                /* 
                 *D = D + RND(1) * Y1 + 1:  //I feel like D is some kind of player "advantage".  Like is keeping track of whether they are doing well for a sort of rubber banding effect?
                 */
                playerAdvantageFactor += FotWK.RNG.rollPercentage0To1() * forceWeight + 1;
                /*
                * GOSUB 3840*/  //TODO: This handles the "Your blah blah's are Crushing the enemy" and also handles spell casting by wizards/elves - so I guess this is like AttackActions
                PlayerAttackActions(); // TODO

                if (enemyForce.IsEmpty()) {
                    txtScreenText.text += "VICTORY IS YOURS!!\n";  // 2540  IF X < 1 THEN  GOSUB 1380:
                    FotWK.UnityGameEngine.getEngine().getSoundEngine().playSound("AttentionAndCharge", GetBattleSceneEvents());
                    yield return new WaitForSeconds(3);
                    // 2530...IF I%(P,5) < 1 THEN  PRINT "JUST BARELY!!"
                    if (playerForce[FotWK.UnitTypeID.Warrior] < 1)
                    {
                        txtScreenText.text += "JUST BARELY!!\n";
                    }
                    GiveLoot(originalEnemyForce);
                    mBattleOver = true;
                }

            }

            txtScreenText.text += "";
            if (mRetreatPressed)
            {
                if (FotWK.RNG.rollAgainstPercentage(Globals.RUN_AWAY_CHANCE))   // 2270  ...: IF Y AND  RND (1) < .8 THEN  GOSUB 1390: ...
                {
                    // TODO: WILL YOU RUN AWAY
                    yield return StartCoroutine(RunAwayOption());
                }
            }
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
        if (monsterId == FotWK.UnitTypeID.Dragon && originalEnemyForce[FotWK.UnitTypeID.Dragon] > 0)
        {
            if (player.getParty().hasSpecialItem(FotWK.SpecialItemType.DragonSlayer))
            {
                player.getParty().removeSpecialItem(FotWK.SpecialItemType.DragonSlayer);
            }
        }

        //3530  IF  RND (1) < .25 THEN I%(P,1) = I%(P,1) + 1: PRINT I$(1)  
        //7010 IF RND(1)< .25 THEN I% (P, 1) = I % (P, 1) + 1:PRINT I$(1)
        if (FotWK.RNG.rollAgainstPercentage(Globals.LOOT_TELEPORT_SPELL_PERCENTAGE))
        {
            player.getParty().spells[FotWK.SpellType.TELEPORT] = player.getParty().spells[FotWK.SpellType.TELEPORT] + 1;
        }

        // 3540  IF RND(1) < .2 AND I% (P, 6) < 1 THEN I% (P, 6) = 1: PRINT I$(6) 
        // 7020 IF RND(1)< .2 AND I% (P, 6) < 1 THEN I% (P, 6) = 1:PRINT I$(6)
        if (FotWK.RNG.rollAgainstPercentage(Globals.LOOT_DRAGON_SLAYER_PERCENTAGE))
        {
            if (!player.getParty().hasSpecialItem(FotWK.SpecialItemType.DragonSlayer))
            {
                player.getParty().addSpecialItem(FotWK.SpecialItemType.DragonSlayer);
            }
        }

        // 3550  IF RND(1) < .15 AND I% (P, 8) < 1 THEN I% (P, 8) = 1: PRINT I$(8)  
        // 7025 IF RND(1)< .15 AND I% (P, 8) < 1 THEN I% (P, 8) = 1:PRINT I$(8)
        if (FotWK.RNG.rollAgainstPercentage(Globals.LOOT_SPELL_OF_SEEKING_PERCENTAGE))
        {
            player.getParty().spells[FotWK.SpellType.SEEKING] = player.getParty().spells[FotWK.SpellType.SEEKING] + 1;
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
        
        txtScreenText.text += Utility.centerString(lootValue + Utility.addS(lootValue, "GOLD PIECE") + "\n"); // 3580  PRINT Z" "I$(4);: IF Z > 1 THEN  PRINT "S"  // 7033 PRINT Z" "I$(4);:IF Z> 1 THEN PRINT"S";

        if ((player.getParty().gold + lootValue) > Globals.PLAYER_GOLD_CAP)
        {  //3590  IF I%(P,4) + Z > 30000 THEN I%(P,4) = 30000:Z = 0 // 7034 IF I% (P, 4) + Z > 30000 THEN I% (P, 4) = 30000:Z = 0
            player.getParty().gold = Globals.PLAYER_GOLD_CAP;
        }


        txtScreenText.text += "\n";  // 3600  PRINT :  // 7035 PRINT:
        //3600 ...I%(P,4) = I%(P,4) + Z:...
        // 7035 ...I % (P, 4) = I % (P, 4) + Z
        player.getParty().gold += lootValue;

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

            // 3640  GOSUB 3700: GOSUB 3740: IF S<  > 10 THEN 30
            //3650 Z = 9: FOR X = 0 TO 3: IF S% (X, 0) = P % (P, 0) AND S% (X, 1) = P % (P, 1) THEN Z = X
            //3660  NEXT: IF Z > 3 GOTO 30
            //3670 Z = Z + 12: IF I% (P, Z) > 1 GOTO 30
            //3680  PRINT I$(Z): IF I% (P, Z) > 0 THEN I% (P, Z) = 11: GOTO 30
            //3690 I % (P, Z) = 10: GOTO 30
            //3700  IF L< 10 OR RND(1) > .3 THEN RETURN
            //3710 Z = INT(RND(1) * 3) + 17: IF I% (P, Z) > 0 THEN RETURN
            //3720  IF Z = 18 THEN MV = MV + 1: IF MV< 1 THEN MV = 1
            //3730 I % (P, Z) = 1: PRINT I$(Z): RETURN
            //3740  IF S = 11 THEN PRINT "THE CROWN OF THE WITCH KING": PRINT "THE SCEPTER OF THE WITCH KING": PRINT "THE ORB OF THE WITCH KING": PRINT "HUNDREDS OF GEMS AND PIECES OF JEWELERY"
            //3750  RETURN 
            // It seems like there is somehow a bug.  The code below seems to indicate that 
            // I%(P, 13) through I%(P, 15) are used as the count of 4 different magic maps.  Also see line 1280:
            //          1280 Z = 0: FOR X = 12 TO 15: IF I% (P, X) = 1 OR I% (P, X) = 11 THEN Z = Z + 1
            //          1290  NEXT: TEXT: HOME: IF Z = 0 THEN PRINT "YOU HAVE NO MAGIC MAPS": PRINT: GOTO 30
            //          1300  PRINT "YOU HAVE "Z" MAP";: IF Z > 1 THEN PRINT "S";
            //          1310  PRINT: PRINT: FOR X = 1 TO Z: PRINT X" = MAP#"X: NEXT: PRINT: PRINT "WHICH MAP DO YOU WISH TO LOOK AT";
            // However, it seems I%(P, 12) through 15 are also used to store special artifacts like WIZARD, HORN OF OPENING, BOOTS OF STEALTH, 15: ARMOUR OF DEFENSE
            // See line 2610:
            //  2610  GOSUB 1380:Y3 = 1:X = I % (P, 3): IF I% (P, 12) > 1 THEN X = X + 75
            //  2620  IF RND(I) > X * .01 THEN PRINT "YOU CAN NOT FIND THE ENTRANCE": GOSUB 1390: GOTO 30

            // Maybe the apple II port differs from the c64 version?
            // c64 version:
            // 7045 Z = INT(RND(1) * 4) + 12:IF I% (P, Z) = 1 OR I% (P, Z) = 11 THEN 7090
            //7050 PRINT"MAGIC MAP"
            //7060 IF I% (P, Z) > 0 THEN I% (P, Z) = 11:GOTO 7090
            //7070 I % (P, Z) = 1
            //7090 GOSUB 7200:GOSUB 7300:IF S<>10 THEN 85
            //7100 Z = 9:FOR X = 0 TO 3:IF S% (X, 0) = P % (P, 0)AND S% (X, 1) = P % (P, 1)THEN Z = X
            //7110 NEXT: IF Z> 3 THEN 85
            //7120 Z = Z + 12:IF I% (P, Z) > 1 THEN 85
            //7130 PRINT I$(Z):IF I% (P, Z) > 0 THEN I% (P, Z) = 11:GOTO 85
            //7140 I % (P, Z) = 10:GOTO 85
            //7200 IF L<10 OR RND(1)> .3 THEN RETURN
            //7210 Z = INT(RND(1) * 3) + 17:IF I% (P, Z) > 0 THEN RETURN
            //7215 IF Z = 18 THEN MV = MV + 1:IF MV<1 THEN MV = 1
            //7220 I % (P, Z) = 1:PRINT I$(Z):RETURN
            //7300 IF S<>11 THEN RETURN



        }

        /*
         
         3640  GOSUB 3700: GOSUB 3740: IF S <  > 10 THEN 30
         3650 Z = 9: FOR X = 0 TO 3: IF S%(X,0) = P%(P,0) AND S%(X,1) = P%(P,1) THEN Z = X
         3660  NEXT : IF Z > 3 GOTO 30
         3670 Z = Z + 12: IF I%(P,Z) > 1 GOTO 30
         3680  PRINT I$(Z): IF I%(P,Z) > 0 THEN I%(P,Z) = 11: GOTO 30
         3690 I%(P,Z) = 10: GOTO 30
         3700  IF L < 10 OR  RND (1) > .3 THEN  RETURN 
         3710 Z =  INT ( RND (1) * 3) + 17: IF I%(P,Z) > 0 THEN  RETURN 
         3720  IF Z = 18 THEN MV = MV + 1: IF MV < 1 THEN MV = 1
         3730 I%(P,Z) = 1: PRINT I$(Z): RETURN 
         3740  IF S = 11 THEN  PRINT "THE CROWN OF THE WITCH KING": PRINT "THE SCEPTER OF THE WITCH KING": PRINT "THE ORB OF THE WITCH KING": PRINT "HUNDREDS OF GEMS AND PIECES OF JEWELERY"
         3750  RETURN 
        */
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

    void PlayerAttackActions()
    {
        // TODO: Implement
        Text txtScreenText = GameObject.Find("txtScreenText").GetComponent<Text>();
        txtScreenText.text += "YOUR WARRIORS ARE CRUSHING THE ENEMY\n";
        GameStateManager.getGameState().getCurrentEnemyForce().Clear();
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("r") && mListenForRetreat)
        {
            mRetreatPressed = true;
        }
    }

    public BattleSceneEvents GetBattleSceneEvents()
    {
        return GameObject.Find("EventSystem").GetComponent<BattleSceneEvents>();
    }

    void harness_ForTesting()
    {
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
            f[FotWK.UnitTypeID.Goblin] = 1;
            GameStateManager.getGameState().setCurrentEnemyForce(f);
            Text txtScreenText = GameObject.Find("txtScreenText").GetComponent<Text>();
            txtScreenText.text = "Sample force: " + f;
        }
    }
}
