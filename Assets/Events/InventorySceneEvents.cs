using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class InventorySceneEvents : MonoBehaviour
{

    private int COLUMN_1_LENGTH = 7;        // 7 characters for the number - left justified
    private int COLUMN_2_LENGTH = 4;
    private int INVENTORY_LINE_WHITESPACE_LENGTH = 8;
    // Start is called before the first frame update
    void Start()
    {
        PlayerState playerState = GameStateManager.getGameState().getCurrentPlayerState();

        Text txtScreenText = GameObject.Find("txtScreenText").GetComponent<Text>();

        FotWK.Party party = playerState.getParty();

        txtScreenText.text += AddInventoryLine(party.rations, "RATIONS");
        txtScreenText.text += AddInventoryLine(party.gold, "GOLD");
        foreach (KeyValuePair <FotWK.SpellType, int> spell in party.spells)
        {
            if (spell.Value > 0) {
                txtScreenText.text += AddInventoryLine(spell.Value, FotWK.Spell.SpellTypeToString(spell.Key));
            }
        }
        foreach (KeyValuePair<FotWK.UnitTypeID, int> unit in party.force)
        {
            if (unit.Value > 0) {
                txtScreenText.text += AddInventoryLine(unit.Value, "UNIT _ TODO");
            }
        }
        foreach (KeyValuePair<FotWK.SupportUnitType, int> unit in party.supportUnits)
        {
            if (unit.Value > 0)
            {
                txtScreenText.text += AddInventoryLine(unit.Value, "UNIT _ TODO");
            }
        }
        foreach (FotWK.SpecialItemType itemType in Enum.GetValues(typeof(FotWK.SpecialItemType)))
        {
            if (party.hasSpecialItem(itemType))
            { 
                txtScreenText.text += AddInventoryLine(1, "SPECIAL ITEM _ TODO" + itemType);
            }
        }


        txtScreenText.text += "\n\n     PRESS RETURN TO CONTINUE";

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            StartCoroutine(LoadMainMenu());
        }
    }

    string AddInventoryLine(int count, string description)
    {
        return "\n" 
                + "".ToString().PadRight(INVENTORY_LINE_WHITESPACE_LENGTH, ' ') 
                + count.ToString().PadRight(COLUMN_1_LENGTH, ' ') 
                + ":".PadRight(COLUMN_2_LENGTH, ' ') 
                + description;
    }

    IEnumerator LoadMainMenu()
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("MainMenu");

        // Wait until the asynchronous scene fully loads
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }
}
