using FotWK;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState
{
    private string mName;
    private Vector2 mMapPosition;
    private Party mParty;
    public PlayerState()
    {
        mMapPosition = new Vector2(0, 0);
        mParty = new Party();
        mName = "Player";
    }
    public Vector2 getMapPosition() { return mMapPosition; }
    public void setMapPosition(Vector2 pos) { mMapPosition = pos; }
    public Party getParty() { return mParty; }
    public string getName() { return mName; }
    public void setName(string name)
    {
        mName = name;
    }

}
