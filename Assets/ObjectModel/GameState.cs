using FotWK;
using System;
using UnityEngine;


[System.Serializable]
public class GameState
{
    [SerializeField]
    private PlayerState[] mPlayerStates;
    [SerializeField]
    private string mCurrentTileName;
    [SerializeField] 
    private Force mCurrentEnemyForce;
    [SerializeField] 
    private int mMovementFactorsRemaining;
    [SerializeField] 
    private int mTurnCount = 1;

    public GameState() {
        mPlayerStates = new PlayerState[1];
        mPlayerStates[0] = new PlayerState();
        if (!Utility.inittedAlreadyTODORemove)
        {
            initForDemo();
            Utility.inittedAlreadyTODORemove = true;
        }
    }
    public PlayerState getPlayerState(int idx) { 
        if (idx <= 0) {
            throw new Exception("Player state index must be greater than 0");
        }
        if (idx > mPlayerStates.Length) {
            throw new Exception("Player state index is higher than number of players");
        }
        return mPlayerStates[idx - 1]; 
    }

    public string getCurrentTileName()
    {
        return mCurrentTileName;
    }

    public void setCurrentTileName(string tileName)
    {
        mCurrentTileName = tileName;
    }

    public PlayerState getCurrentPlayerState()
    {
        return getPlayerState(1);   //TODO: Multiplayer fix
    }

    public Force getCurrentEnemyForce() {
        return mCurrentEnemyForce;
    }

    public void setCurrentEnemyForce(Force force) {
        mCurrentEnemyForce = force;
    }

    public int getMovementFactorsRemaining()
    {
        return mMovementFactorsRemaining;
    }

    public void decMovementFactorsRemaining(int moves)
    {
        mMovementFactorsRemaining -= moves;
    }

    public void resetMovementFactorsRemaining()
    {
        mMovementFactorsRemaining = Globals.MOVES_PER_TURN;
    }

    public void initForDemo()
    {
        mPlayerStates[0].setMapPosition(new Vector2(18, 20));
        mPlayerStates[0].getParty().force = new Force();
        mPlayerStates[0].getParty().initializeToStartingValues();
        foreach (UnitTypeID unitTypeId in Enum.GetValues(typeof(UnitTypeID)))  
        {  
            mPlayerStates[0].getParty().force.Set(unitTypeId, 0);
        }  
        mPlayerStates[0].getParty().force.Set(UnitTypeID.Warrior, 50);
        mPlayerStates[0].setName("Hotmustard");
        mMovementFactorsRemaining = 0;
    }

    public int getTurn()
    {
        return mTurnCount;
    }


    public int endTurn()
    {
        return ++mTurnCount;
    }
}
