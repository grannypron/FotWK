using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuEvents : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("m"))
        {
            Move();
        }

    }

    void Move()
    {
        StartCoroutine(LoadMoveScreen());
    }


    IEnumerator LoadMoveScreen()
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("MoveScreen");

        // Wait until the asynchronous scene fully loads
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }
}
