using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UseSpellSceneEvents : MonoBehaviour
{

    private FotWK.SpellType mActiveSpellChoice;
    private bool mIsSpellChoiceActive;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(spellChoice());
    }

    IEnumerator spellChoice() { 
        PlayerState playerState = GameStateManager.getGameState().getCurrentPlayerState();

        Text txtScreenText = GameObject.Find("txtScreenText").GetComponent<Text>();

        FotWK.Party party = playerState.getParty();

        bool noSpells = true;
        mIsSpellChoiceActive = false;

        if (party.spells.Get(FotWK.SpellType.SEEKING) > 0)
        {
            //690  IF I% (P, 19) THEN PRINT "CAST "I$(19);: GOSUB 40: IF Y THEN GOSUB 1010: GOTO 440
            txtScreenText.text += "CAST SPELL OF SEEKING (Y/N)?\n";
            noSpells = false;
            mActiveSpellChoice = FotWK.SpellType.SEEKING;
            mIsSpellChoiceActive = true;
            yield return new WaitUntil(() => !mIsSpellChoiceActive);
        }

        if (party.spells.Get(FotWK.SpellType.SEEING) > 0)
        {
            // 700  IF I% (P, 8) THEN PRINT "CAST "I$(8);: GOSUB 40: IF Y THEN GOSUB 170:C1 = SCRN(P % (P, 0), P % (P, 1)):C2 = 0: HOME: PRINT "PRESS ANY KEY TO CANCEL!": POKE KC,0: GOSUB 110:I % (P, 8) = 0: GOTO 440
            txtScreenText.text += "CAST SPELL OF SEEING (Y/N)?\n";
            noSpells = false;
            mActiveSpellChoice = FotWK.SpellType.SEEING;
            mIsSpellChoiceActive = true;
            yield return new WaitUntil(() => !mIsSpellChoiceActive);
        }

        if (party.spells.Get(FotWK.SpellType.TELEPORT) > 0)
        {
            // 710  IF I% (P, 1) < 1 THEN 440
            // 720  PRINT "CAST "I$(1);: GOSUB 40: IF NOT Y THEN 440
            txtScreenText.text += "CAST TELEPORT SPELL (Y/N)?\n";
            noSpells = false;
            mActiveSpellChoice = FotWK.SpellType.TELEPORT;
            mIsSpellChoiceActive = true;
            yield return new WaitUntil(() => !mIsSpellChoiceActive);
        }

        if (noSpells)
        {
            // 680  PRINT: PRINT "NO SPELLS AVAILABLE": PRINT: GOSUB 30: GOTO 440
            txtScreenText.text += "NO SPELLS AVAILABLE\n";
            txtScreenText.text += "\nPRESS RETURN TO CONTINUE";
        } else
        {
            StartCoroutine(LoadMainMenu());
        }

    }

    // Update is called once per frame
    void Update()
    {
        if (mIsSpellChoiceActive)
        {
            if (Input.GetKeyDown(KeyCode.Y))
            {
                if (mActiveSpellChoice == FotWK.SpellType.SEEING)
                {
                    StartCoroutine(LoadZoomOutMapScene());
                }
                if (mActiveSpellChoice == FotWK.SpellType.SEEKING)
                {
                    // TODO
                }
                if (mActiveSpellChoice == FotWK.SpellType.TELEPORT)
                {
                    // TODO
                }
            }
            else if (Input.anyKeyDown) {
                // Do something here to move on to the next spell or back to main menu
                mIsSpellChoiceActive = false;
            }

        }
        else {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                StartCoroutine(LoadMainMenu());
            }
        }
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
    IEnumerator LoadZoomOutMapScene()
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("ZoomOutMapScene");

        // Wait until the asynchronous scene fully loads
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }
}
