using System.Collections;
using System.Collections.Generic;
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

        if (FotWK.RNG.rollPercentage0To1() < forceWeight * Globals.ATTACK_PRESS_CHANCE_MULTIPLIER) /*IF RND(1) < Y1 * .012 THEN*** Note here that ALL statements after on the line after the "THEN" keyword will be executed(when there are multiple separated by colons), not just the first */
            FotWK.UnityGameEngine.getEngine().getSoundEngine().playSound("Attention", GetBattleSceneEvents());     //GOSUB 1400: 
				/*PRINT N$(P)"'S SIDE PRESSES THE ATTACK ":
				D = D + RND(1) * Y1 + 1:
				GOSUB 3840*/

        txtScreenText.text += "\n";
        txtScreenText.text += Utility.centerString(GameStateManager.getGameState().getCurrentPlayerState().getName().ToUpper() + "'S SIDE PRESSES THE ATTACK");
        txtScreenText.text += "\n";

        txtScreenText.text += "";
        if (mRetreatPressed)
        {
            retreat();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("r") && mListenForRetreat)
        {
            mRetreatPressed = true;
        }
    }

    void retreat() { }

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
