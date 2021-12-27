using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;

public class MoveSceneEvents : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Tilemap closeUpMapTilemap = GameObject.Find("CloseUpMapTilemap").GetComponent<Tilemap>();
        renderMap(GameStateManager.getGameState().getCurrentPlayerState().getMapPosition(), closeUpMapTilemap);
    }

    // Update is called once per frame
    void Update()
    {
        int xMove = 0;
        int yMove = 0;
        if (Input.GetKeyDown(KeyCode.Alpha1) || Input.GetKeyDown(KeyCode.Keypad1)) {
            yMove--;           // N
        } else if (Input.GetKeyDown(KeyCode.Alpha2) || Input.GetKeyDown(KeyCode.Keypad2)) {
            yMove--; xMove++;  // NE
        } else if (Input.GetKeyDown(KeyCode.Alpha3) || Input.GetKeyDown(KeyCode.Keypad3)) {
            xMove++;           // E
        } else if (Input.GetKeyDown(KeyCode.Alpha4) || Input.GetKeyDown(KeyCode.Keypad4)) {
            yMove++; xMove++;  // SE
        } else if (Input.GetKeyDown(KeyCode.Alpha5) || Input.GetKeyDown(KeyCode.Keypad5)) {
            yMove++;           // S
        } else if (Input.GetKeyDown(KeyCode.Alpha6) || Input.GetKeyDown(KeyCode.Keypad6)) {
            yMove++; xMove--;  // SW
        } else if (Input.GetKeyDown(KeyCode.Alpha7) || Input.GetKeyDown(KeyCode.Keypad7)) {
            xMove--;           // W
        } else if (Input.GetKeyDown(KeyCode.Alpha8) || Input.GetKeyDown(KeyCode.Keypad8)) {
            yMove--; xMove--;  // NW
        } else if (Input.GetKeyDown("s")) {
            // Stay
            NextScreen();
        }

        if (xMove != 0 || yMove != 0) { 
            // Change position
            Vector2 position = GameStateManager.getGameState().getCurrentPlayerState().getMapPosition();
            // Bounds checking
            if (position.x + xMove <= 0 || position.x + xMove > Globals.MAX_MAP_X) {
            } else if (position.y + yMove <= 0 || position.y + yMove > Globals.MAX_MAP_Y) {
            } else {
                int newX = (int)position.x + xMove;
                int newY = (int)position.y + yMove;
                GameStateManager.getGameState().getCurrentPlayerState().setMapPosition(new Vector2(newX, newY));

                Tilemap closeUpMapTilemap = GameObject.Find("CloseUpMapTilemap").GetComponent<Tilemap>();
                TileBase tile = closeUpMapTilemap.GetTile(new Vector3Int(newX, newY, 0));
                renderMap(GameStateManager.getGameState().getCurrentPlayerState().getMapPosition(), closeUpMapTilemap);

                // Get the tile that is now active (under the position indicator)
                string tileName = getTileNameUnderCursor(new Vector2Int(newX, newY), closeUpMapTilemap);
                GameStateManager.getGameState().setCurrentTileName(tileName);
            }
            NextScreen();
        }
    }

    // Renders the map and return the new transformed (world?) position of the position in the tileset
    private Vector2 renderMap(Vector2 position, Tilemap closeUpMapTilemap)
    {
        // x pos +/- .4 for y and .8 for x
        // Get start position of tileset
        float xTransformed = -1*(position.x - 1) * closeUpMapTilemap.transform.localScale.x;
        float yTransformed = (position.y - 1) * closeUpMapTilemap.transform.localScale.y;

        Vector3 transformedPosition = new Vector3(xTransformed, yTransformed, 0);
        closeUpMapTilemap.transform.localPosition = transformedPosition;
        closeUpMapTilemap.CompressBounds();
        return transformedPosition;

    }

    private string getTileNameUnderCursor(Vector2Int position, Tilemap closeUpMapTilemap)
    {
        // Get cursor local position
        Tilemap positionTilemap = GameObject.Find("PositionTilemap").GetComponent<Tilemap>();

        Tile currentTile = (Tile)closeUpMapTilemap.GetTile(new Vector3Int(position.x, position.y, (int)closeUpMapTilemap.origin.z));

        // The array is laid out from the bottom row to the top row and left to right is how the index runs.  No idea why, but calculate the index as such:
        TileBase[] arrBlocks = closeUpMapTilemap.GetTilesBlock(closeUpMapTilemap.cellBounds);
        int tileIdx = (Globals.MAX_MAP_Y - position.y) * 40 + (position.x - 1);

        return arrBlocks[tileIdx].name;
    }


    void NextScreen()
    {
        StartCoroutine(LoadVisitScreen());
    }


    IEnumerator LoadVisitScreen()
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("VisitScene");

        // Wait until the asynchronous scene fully loads
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }
}

