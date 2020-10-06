using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState
{
    private Vector2 mapPosition;
    public PlayerState()
    {
        mapPosition = new Vector2(0, 0);
    }
    public Vector2 getMapPosition() { return mapPosition; }
    public void setMapPosition(Vector2 pos) { mapPosition = pos; }

}
