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

        Text txtScreenText = GameObject.Find("txtScreenText").GetComponent<Text>();
        txtScreenText.text = Utility.centerString("PRESS 'R' TO RETREAT\n");
        txtScreenText.text += "\nTHE BATTLE RAGES ON\n";
        mListenForRetreat = true;
        mRetreatPressed = false;
        yield return new WaitForSeconds(2);
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


    void testHarness()
    {
        GameStateManager.getGameState().initForDemo();
        FotWK.Force f = new FotWK.Force();
        f[FotWK.UnitTypeID.Hydra] = 1;
        GameStateManager.getGameState().setCurrentEnemyForce(f);
    }
}
