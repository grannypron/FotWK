using UnityEngine;
using UnityEngine.UI;
using System;
using FotWK;
using System.Collections;
using UnityEngine.SceneManagement;

public class EndTurnSceneEvents : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        int turnCount = GameStateManager.getGameState().endTurn();
        GameStateManager.getGameState().getCurrentPlayerState().eatRations();
        GameStateManager.getGameState().resetMovementFactorsRemaining();
        GameObject.Find("txtScreenText").GetComponent<Text>().text = String.Format(GameObject.Find("txtScreenText").GetComponent<Text>().text, turnCount);
        InputReceiverEvents.GetInputReceiverEvents().ActivateInputKeypress(handleInput);
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void handleInput(string key)
    {
        if (key != null && key.ToLower() == "y")
        {
            String s = JsonUtility.ToJson(GameStateManager.getGameState());
            // Do save state here - ask for file name?
            UnityEngine.Debug.Log(s);
        }
        EndTurnSceneEvents.GetEndTurnSceneEvents().StartCoroutine(LoadNextScene("MainMenu"));
    }
    public static EndTurnSceneEvents GetEndTurnSceneEvents()
    {
        return GameObject.Find("EventSystem").GetComponent<EndTurnSceneEvents>();
    }

    public IEnumerator LoadNextScene(string sceneName)
    {
        yield return new WaitForSeconds(Globals.VISIT_SCREEN_NO_EVENT_PAUSE_TIME);
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);

        // Wait until the asynchronous scene fully loads
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }

}
