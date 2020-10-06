using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StartSceneEvents : MonoBehaviour
{
    Text txtTitleAndLoadGame;
    GameObject objNumPlayers;
    bool bNumUsersShown = false;

    void Start()
    {
        txtTitleAndLoadGame = GameObject.Find("txtTitleAndLoadGame").GetComponent<Text>();
        objNumPlayers = GameObject.Find("objNumPlayers");
        Utility.HideObject(objNumPlayers);
    }

    // Update is called once per frame
    void Update()
    {
        if (!bNumUsersShown)
        {
            if (Input.GetKeyDown("n"))
            {
                bNumUsersShown = true;
                txtTitleAndLoadGame.text += " N";
                Utility.ShowObject(objNumPlayers);
                InputField inputNumPlayers = GameObject.Find("inputNumPlayers").GetComponent<InputField>();
                inputNumPlayers.Select();
                inputNumPlayers.ActivateInputField();
            }
        }
        else
        {
            if (Input.GetKeyDown("return"))
            {
                NextScreen();
            }
        }

    }

    void NextScreen()
    {
        StartCoroutine(LoadMainMenu());
    }


    IEnumerator LoadMainMenu()
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("MainMenu");

        // Wait until the asynchronous scene fully loads
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }
}
