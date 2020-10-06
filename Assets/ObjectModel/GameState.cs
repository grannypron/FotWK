using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameState
{
    private PlayerState[] playerStates;
    private string currentTileName;

    public GameState() {
        playerStates = new PlayerState[1];
        playerStates[0] = new PlayerState();
        initForDemo();
    }
    public PlayerState getPlayerState(int idx) { return playerStates[idx - 1]; }

    public string getCurrentTileName()
    {
        return currentTileName;
    }

    public void setCurrentTileName(string tileName)
    {
        currentTileName = tileName;
    }

    private void initForDemo()
    {
        playerStates[0].setMapPosition(new Vector2(18, 20));
    }

}
