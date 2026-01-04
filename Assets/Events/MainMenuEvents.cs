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
            Utility.inittedAlreadyTODORemove = true;
        }
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
            StartCoroutine(LoadScene("MoveScene"));
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
