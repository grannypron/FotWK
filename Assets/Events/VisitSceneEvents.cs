using UnityEngine;
using UnityEngine.UI;
using FotWK;

public class VisitSceneEvents : MonoBehaviour
{
    public const int SCREEN_NUM_CHARS_PER_LINE = 51;
    public const int SCREEN_LINE_CENTERING_OFFSET = 5;

    // Start is called before the first frame update
    void Start()
    {
        string tile = GameStateManager.getGameState().getCurrentTileName();
        string formattedTileString = Utility.centerString(tile);

        Text txtScreenText = GameObject.Find("txtScreenText").GetComponent<Text>();
        txtScreenText.text = formattedTileString.ToUpper();

        BaseLocation location = LocationFactory.getVisitByTileName(tile);
        location.onVisit();
    }

    // Update is called once per frame
    void Update()
    {
    }

    public static VisitSceneEvents GetVisitSceneEvents()
    {
        return GameObject.Find("EventSystem").GetComponent<VisitSceneEvents>();
    }

    public void SetText(string txt, bool center = false)
    {
        if (center)
        {
            txt = Utility.centerString(txt);
        }
        GameObject.Find("txtScreenText").GetComponent<Text>().text = txt;
    }

    public void AddTextLine(string txt)
    {
        GameObject.Find("txtScreenText").GetComponent<Text>().text += "\n" + txt;
    }


}
