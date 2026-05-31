using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class ZoomOutMapSceneEvents : MonoBehaviour
{
    private bool mViewOnly = false;
    private Tile mCursorTile;

    // Start is called before the first frame update
    void Start()
    {
        mCursorTile = Resources.Load<Tile>("CloseUpMapPalette/ZoomOutCursor");
        if (mCursorTile == null)
        {
            UnityEngine.Debug.LogWarning("Cursor Tile not found.");
        }

        if (GameStateManager.getGameState().getSceneTransitionData().viewOnlyMap)
        {
            mViewOnly = true;
            GameStateManager.getGameState().getSceneTransitionData().viewOnlyMap = false;
            GameObject.Find("txtMoveGuideText").GetComponent<Text>().text = "PRESS RETURN TO CONTINUE";
        }
        Tilemap zoomedOutMap = GameObject.Find("CursorTilemap").GetComponent<Tilemap>();
        renderCursor(GameStateManager.getGameState().getCurrentPlayerState().getMapPosition(), zoomedOutMap);

    }

    // Update is called once per frame
    void Update()
    {
        if (mViewOnly)
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                StartCoroutine(LoadScene("MainMenu"));
            }
            else
            {
                return;
            }
        }
    }

    // Renders the map and return the new transformed (world?) position of the position in the tileset
    private void renderCursor(Vector2 position, Tilemap cursorTilemap)
    {

        cursorTilemap.ClearAllTiles();
        cursorTilemap.SetTile(new Vector3Int(0, 0, 0), mCursorTile);


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

