using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuEvents : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GameStateManager.getGameState().initForDemo();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("m"))
        {
            Move();
        } else if (Input.GetKeyDown("i"))
        {
            Inventory();
        }


    }

    void Move()
    {
        StartCoroutine(LoadScene("MoveScene"));
    }

    void Inventory()
    {
        StartCoroutine(LoadScene("InventoryScene"));
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
