using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuEvents : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        if (!Utility.inittedAlreadyTODORemove) {
            GameStateManager.getGameState().initForDemo();
            String s = JsonUtility.ToJson(GameStateManager.getGameState());
            // Do save state here - ask for file name?
            UnityEngine.Debug.Log(s);

            Utility.inittedAlreadyTODORemove = true;
        }

        // Handle pre-menu items like overencumbrance - This is done before the main menu is displayed - see line 450  GOSUB 3760:...
        //3760  TEXT: HOME: Z = I % (P, 3) * 5 + I % (P, 5) * 10 + I % (P, 10) * 50 + 5:Y = 0
        //3770 DF = I % (P, 9) * 4: IF DF = 0 THEN 3800
        //3780  IF I% (P, 10) < DF THEN PRINT "YOU JUST DROPPED A RAFT!":Y = 1:I % (P, 9) = I % (P, 9) - 1: GOTO 3770
        //3790 Z = Z - DF * 50
        //3800  IF I% (P, 4) > Z THEN PRINT "YOU HAD TO DROP "I % (P, 4) - Z" "I$(4);: IF I% (P, 4) - Z > 1 THEN PRINT "S";
        //3810  PRINT: IF I% (P, 4) > Z THEN I% (P, 4) = Z:Y = 1

        int movementFactorsRemaining = GameStateManager.getGameState().getMovementFactorsRemaining();
        string playerName = GameStateManager.getGameState().getCurrentPlayerState().getName();
        int turn = GameStateManager.getGameState().getTurn();

        GameObject.Find("txtMenuText").GetComponent<Text>().text = String.Format(GameObject.Find("txtMenuText").GetComponent<Text>().text, turn, playerName, movementFactorsRemaining);
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
