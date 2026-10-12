using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class ZoomOutMapSceneEvents : MonoBehaviour
{

    enum ScreenMode
    {
        ZoomOutMap,
        SpellOfSeeing,
        Teleport
    }

    private ScreenMode mScreenMode = ScreenMode.ZoomOutMap;
    private Tile mCursorTile;
    private Vector2Int cursorPosition;

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
            // Reset
            GameStateManager.getGameState().getSceneTransitionData().viewOnlyMap = false;

            if (GameStateManager.getGameState().getSceneTransitionData().spellActive) { 
                // Reset
                GameStateManager.getGameState().getSceneTransitionData().spellActive = false;
                mScreenMode = ScreenMode.SpellOfSeeing;
            } 
            else 
            { 
                mScreenMode = ScreenMode.ZoomOutMap;
            }
        }
        mScreenMode = ScreenMode.Teleport; // TODO: Testing - Remove
        
        switch (mScreenMode) {
            case ScreenMode.ZoomOutMap:
                GameObject.Find("txtMoveGuideText").GetComponent<Text>().text = "PRESS RETURN TO CONTINUE";
            break;
            case ScreenMode.SpellOfSeeing:
                GameObject.Find("txtMoveGuideText").GetComponent<Text>().text = "PRESS ANY KEY TO CANCEL!";
            break;
            case ScreenMode.Teleport:
                GameObject.Find("txtMoveGuideText").GetComponent<Text>().text = "ENTER(I)TO GO UP..(K)TO GO RIGHT..\n(J)TO GO LEFT..(M)TO GO DOWN..\n(B)TO GO BACK..(T)TO TELEPORT..";
                InvokeRepeating(nameof(TeleportCursorFlashAction), 0, Globals.CURSOR_FLASH_INTERVAL);
            break;
        }
        Tilemap zoomedOutMap = GameObject.Find("CursorTilemap").GetComponent<Tilemap>();
        cursorPosition = GameStateManager.getGameState().getCurrentPlayerState().getMapPosition();
        placeCursor(cursorPosition, zoomedOutMap);
        
    }


    // Update is called once per frame
    void Update()
    {
        if (Input.anyKey)
        {
            switch (mScreenMode)
            {
                case ScreenMode.ZoomOutMap:
                    if (Input.GetKeyDown(KeyCode.Return)) { 
                        // 30  INPUT "PRESS RETURN TO CONTINUE"; X$: RETURN
                        StartCoroutine(LoadScene("MainMenu"));
                    }
                    return;
                case ScreenMode.SpellOfSeeing:
                    // 700  ... "PRESS ANY KEY TO CANCEL!": POKE KC,0: GOSUB 110...
                    StartCoroutine(LoadScene("MainMenu"));
                    break;
                case ScreenMode.Teleport:
                    //TODO - code for handling navigation keypresses and simply update cursorPosition
                    Tilemap zoomedOutMap = GameObject.Find("CursorTilemap").GetComponent<Tilemap>();
                    placeCursor(cursorPosition, zoomedOutMap);
                    return;
            }
        }

    }

    void TeleportCursorFlashAction()
    {
        // Flash cursor
        TilemapRenderer cursorTR = GameObject.Find("CursorTilemap").GetComponent<TilemapRenderer>();
        cursorTR.enabled = !cursorTR.enabled;
    }

    // Places the cursor on the map
    private void placeCursor(Vector2Int position, Tilemap cursorTilemap)
    {
        cursorTilemap.ClearAllTiles();
        cursorTilemap.SetTile(new Vector3Int(position.x, position.y, 0), mCursorTile);
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

