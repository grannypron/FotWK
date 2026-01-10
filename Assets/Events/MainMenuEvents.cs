using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuEvents : MonoBehaviour
{
    // Start is called before the first frame update
    private String mEncumbranceMessage = "";
    private String mDefaultText = "";
    void Start()
    {
        if (!Utility.inittedAlreadyTODORemove) {
            GameStateManager.getGameState().initForDemo();
            String s = JsonUtility.ToJson(GameStateManager.getGameState());
            // Do save state here - ask for file name?
            UnityEngine.Debug.Log(s);

            Utility.inittedAlreadyTODORemove = true;
        }

        mDefaultText = GameObject.Find("txtMenuText").GetComponent<Text>().text;
        StartCoroutine(handleEncumbrance());

        StartCoroutine(showMenu());
    }

    IEnumerator showMenu()
    {
        yield return new WaitUntil(() => mEncumbranceMessage == "");

        int movementFactorsRemaining = GameStateManager.getGameState().getMovementFactorsRemaining();
        string playerName = GameStateManager.getGameState().getCurrentPlayerState().getName();
        int turn = GameStateManager.getGameState().getTurn();

        GameObject.Find("txtMenuText").GetComponent<Text>().text = String.Format(mDefaultText, turn, playerName, movementFactorsRemaining);

    }

    IEnumerator handleEncumbrance()
    {
        mEncumbranceMessage = "";

        FotWK.Party party = GameStateManager.getGameState().getCurrentPlayerState().getParty();

        // Handle pre-menu items like overencumbrance - This is done before the main menu is displayed - see line 450  GOSUB 3760:...
        //3760  TEXT: HOME: Z = I % (P, 3) * 5 + I % (P, 5) * 10 + I % (P, 10) * 50 + 5:Y = 0
        float availableEncumbrance = party.calculateAvailableEncumbrance();


        Text txtScreen = GameObject.Find("txtMenuText").GetComponent<Text>();

        //3770 DF = I % (P, 9) * 4: IF DF = 0 THEN 3800
        //3780  IF I% (P, 10) < DF THEN PRINT "YOU JUST DROPPED A RAFT!":Y = 1:I % (P, 9) = I % (P, 9) - 1: GOTO 3770
        while (party.equipment.Get(FotWK.EquipmentID.Raft) * Globals.NUM_MULES_REQUIRED_PER_RAFT > party.supportUnits.Get(FotWK.SupportUnitTypeID.Mule))
        {
            //Apparently, you must have 4 mules for every raft 
            
            mEncumbranceMessage += "YOU JUST DROPPED A RAFT!\n";
            party.equipment.Set(FotWK.EquipmentID.Raft, party.equipment.Get(FotWK.EquipmentID.Raft) - 1);

            //3790 Z = Z - DF * 50 // Adjust new weight after dropping any rafts - we are just going to recalculate after this loop which drops them
        }

        availableEncumbrance = party.calculateAvailableEncumbrance();

        //3800  IF I% (P, 4) > Z THEN PRINT "YOU HAD TO DROP "I % (P, 4) - Z" "I$(4);: IF I% (P, 4) - Z > 1 THEN PRINT "S";
        if (party.gold > availableEncumbrance)
        {
            int droppedGold = (int)Math.Ceiling(party.gold - availableEncumbrance);
            mEncumbranceMessage += "YOU HAD TO DROP " + droppedGold + " " + Utility.addS(droppedGold, "GOLD PIECE") + "\n";

            //3810  PRINT: IF I% (P, 4) > Z THEN I% (P, 4) = Z:Y = 1
            party.gold = party.gold - droppedGold;
        }

        if (mEncumbranceMessage != "")
        {
            txtScreen.text = mEncumbranceMessage;
            txtScreen.text += "PRESS RETURN TO CONTINUE\n";
        }

        // NOTE: We are assuming that all encumbrance situations have been resolved after this method finishes.  Another approach would be to 
        // reload this scene and if some encumbrance situations still remain for some reason, they would be resolved there (like recursion).
        yield return null;
    }


    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("m"))
        {
            if (GameStateManager.getGameState().getMovementFactorsRemaining() > 0) { 
                StartCoroutine(LoadScene("MoveScene"));
            }
        } else if (Input.GetKeyDown("i"))
        {
            StartCoroutine(LoadScene("InventoryScene"));
        } else if (Input.GetKeyDown("e"))
        {
            StartCoroutine(LoadScene("EndTurnScene"));
        }
        else if (Input.GetKeyDown(KeyCode.Return))
        {
            if (mEncumbranceMessage != "")
            {
                // We are showing an encumbrance message.  Clear it and return to the main menu.
                mEncumbranceMessage = "";
            }
        }


    }
    IEnumerator LoadScene(string sceneName)
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);

        // Wait until the asynchronous scene fully loads
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }
}
