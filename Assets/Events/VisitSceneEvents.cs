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

    public delegate void VisitSceneInputCallback(string key);

    private const int NUM_CHARS_PER_LINE = 51;
    private const int LINE_CENTERING_OFFSET = 5;

    private VisitSceneInputCallback mInputCallback = null;

    // Start is called before the first frame update
    void Start()
    {
        string tile = GameStateManager.getGameState().getCurrentTileName();
        string formattedTileString = centerString(tile);

        Text txtScreenText = GameObject.Find("txtScreenText").GetComponent<Text>();
        txtScreenText.text = formattedTileString.ToUpper();

        BaseLocation location = LocationFactory.getVisitByTileName(tile);
        location.onVisit();
    }

    // Update is called once per frame
    void Update()
    {
        if (mInputCallback != null && Input.anyKeyDown)
        {
            mInputCallback = null;
            mInputCallback(Input.inputString);
        }

    }

    public static VisitSceneEvents GetVisitSceneEvents()
    {
        return GameObject.Find("EventSystem").GetComponent<VisitSceneEvents>();
    }

    public void SetText(string txt, bool center = false)
    {
        if (center)
        {
            txt = centerString(txt);
        }
        GameObject.Find("txtScreenText").GetComponent<Text>().text = txt;
    }

    public void AddTextLine(string txt)
    {
        GameObject.Find("txtScreenText").GetComponent<Text>().text += "\n" + txt;
    }

    public void ActivateInputKeypress(VisitSceneInputCallback inputCallback)
    {
        this.mInputCallback = inputCallback;
        InputField inputCursor = GameObject.Find("inputCursor").GetComponent<InputField>();
        inputCursor.Select();
        inputCursor.ActivateInputField();
    }

    public static string centerString(string str)
    {
        int paddingLeft = (int) Math.Floor((NUM_CHARS_PER_LINE - str.Length) / 2f);
        return str.PadLeft(paddingLeft + LINE_CENTERING_OFFSET);
    }

}
