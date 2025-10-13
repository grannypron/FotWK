using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class BattleSceneEvents : MonoBehaviour
{
    private bool mListenForRetreat = false;
    private bool mRetreatPressed = false;

    // Start is called before the first frame update
    void Start()
    {
        testHarness(); // TODO: Remove

        StartCoroutine(battle());

        initForDemo();
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

        float playerAdvantageFactor = 0;  // D in the AppleSoft Basic source code - zeroed out when the battle begins at 2220
        Text txtScreenText = GameObject.Find("txtScreenText").GetComponent<Text>();
        txtScreenText.text = Utility.centerString("PRESS 'R' TO RETREAT\n");

        // TODO: Improve - should only have one
        FotWK.UnitTypeID unitTypeID = FotWK.UnitTypeID.Warrior;
        foreach (FotWK.UnitTypeID id in enemyForce.Keys) {
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
            (player.getParty().magicMaps.Length * 20) + (FotWK.RNG.rollPercentage0To1() * 15) + 
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
        }

        txtScreenText.text += "";
        if (mRetreatPressed)
        {
            if (FotWK.RNG.rollAgainstPercentage(Globals.RUN_AWAY_CHANCE))   // 2270  ...: IF Y AND  RND (1) < .8 THEN  GOSUB 1390: ...
            {
                // Successful run away 
                txtScreenText.text += "     COWARD\n";   // 2270  ...PRINT "     COWARD":CO = 1: GOTO 1390
                                                         // I believe the CO = 1 is irrelevant here, because that is only considered on 2760, which I believe is during a Witch-King attack?
                FotWK.UnityGameEngine.getEngine().getSoundEngine().playSound("Disappointment", GetBattleSceneEvents());
                // TODO: GO BACK TO MAIN MENU
            }
        }
    }
    void PlayerAttackActions()
    {

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

    void testHarness()
    {
        GameStateManager.getGameState().initForDemo();
        FotWK.Force f = new FotWK.Force();
        f[FotWK.UnitTypeID.Hydra] = 1;
        GameStateManager.getGameState().setCurrentEnemyForce(f);
    }
}
