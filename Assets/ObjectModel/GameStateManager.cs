using System.Collections;
using System.Collections.Generic;

public static class GameStateManager
{
    static GameState gameState = new GameState();
    
    public static GameState getGameState() {
        return gameState;
    }

}
