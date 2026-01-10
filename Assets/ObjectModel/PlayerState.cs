using FotWK;
using UnityEngine;
using Unity.Properties;


[System.Serializable]
public class PlayerState
{
    [SerializeField]
    private string mName;
    [SerializeField] 
    private Vector2 mMapPosition;
    [SerializeField] 
    private Party mParty;
    [SerializeField] 
    private bool mSurprised;   // This is necessary to "pass" between EncounterableLocation and BattleScene :/

    public PlayerState()
    {
        mMapPosition = new Vector2(0, 0);
        mParty = new Party();
        mName = "Player";
        mSurprised = false;
    }
    public Vector2 getMapPosition() { return mMapPosition; }
    public void setMapPosition(Vector2 pos) { mMapPosition = pos; }
    public Party getParty() { return mParty; }
    public string getName() { return mName; }
    public void setName(string name) { mName = name; }
    public bool getSurprised() { return mSurprised; }
    public void setSurprised(bool surprised) { mSurprised = surprised; }

    public void eatRations()
    {
        // 380 ... I%(P,0) = I%(P,0) - (I%(P,5) / 10)
        mParty.rations -= (int)(mParty.force.Get(FotWK.UnitTypeID.Warrior) / 10);
    }

}
