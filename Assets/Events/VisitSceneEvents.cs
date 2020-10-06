using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;
using FotWK;

public class VisitSceneEvents : MonoBehaviour
{
    private const int NUM_CHARS_PER_LINE = 51;
    private const int LINE_CENTERING_OFFSET = 5;
    // Start is called before the first frame update
    void Start()
    {
        string tile = GameStateManager.getGameState().getCurrentTileName();
        string formattedTileString = centerString(tile);

        Text txtScreenText = GameObject.Find("txtScreenText").GetComponent<Text>();
        txtScreenText.text = formattedTileString.ToUpper();

        ILocation location = LocationFactory.getVisitByTileName(tile);
        location.onVisit();
        NextScreen();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public static void SetText(string txt)
    {
        GameObject.Find("txtScreenText").GetComponent<Text>().text = txt;
    }

    public static void AddTextLine(string txt)
    {
        GameObject.Find("txtScreenText").GetComponent<Text>().text += "\n" + txt;
    }

    void NextScreen()
    {
        StartCoroutine(LoadMainMenu());
    }

    private string centerString(string str)
    {
        int paddingLeft = (int) Math.Floor((NUM_CHARS_PER_LINE - str.Length) / 2f);
        return str.PadLeft(paddingLeft + LINE_CENTERING_OFFSET);
    }

    IEnumerator LoadMainMenu()
    {
        yield return new WaitForSeconds(Globals.VISIT_SCREEN_NO_EVENT_PAUSE_TIME);
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("MainMenu");

        // Wait until the asynchronous scene fully loads
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }
}
