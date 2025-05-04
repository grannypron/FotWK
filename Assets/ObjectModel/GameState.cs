using FotWK;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameState
{
    private PlayerState[] mPlayerStates;
    private string mCurrentTileName;
    private Force mCurrentEnemyForce;

    public GameState() {
        mPlayerStates = new PlayerState[1];
        mPlayerStates[0] = new PlayerState();
        initForDemo();
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

    public void initForDemo()
    {
        mPlayerStates[0].setMapPosition(new Vector2(18, 20));
        mPlayerStates[0].getParty().force = new Force();
        mPlayerStates[0].getParty().initializeToStartingValues();
        foreach (UnitTypeID unitTypeId in Enum.GetValues(typeof(UnitTypeID)))  
        {  
            mPlayerStates[0].getParty().force[unitTypeId] = 0;
        }  
        mPlayerStates[0].getParty().force[UnitTypeID.Warrior] = 50;
    }
    

}
