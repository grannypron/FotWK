using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace FotWK
{
	public class Sanctuary : BaseLocation
	{
		public override void onVisit()
		{
			UnityGameEngine.getEngine().getSoundEngine().playSound("Sanctuary", VisitSceneEvents.GetVisitSceneEvents());
			VisitSceneEvents visitSceneEvents = VisitSceneEvents.GetVisitSceneEvents();
			PlayerState player = GameStateManager.getGameState().getCurrentPlayerState();
			if (player.getParty().force[UnitTypeID.Warrior] < Globals.MIN_WARRIOR_SANCTUARY_HELP)
			{
				visitSceneEvents.AddTextLine("SOME WARRIORS HERE JOIN THE RANKS");   // Line 980
				int addition = 15 + RNG.rollInRange(1, 10);
				player.getParty().force[UnitTypeID.Warrior] += addition;
			}
			if (player.getParty().rations < Globals.MIN_RATION_SANCTUARY_HELP)
            {
				visitSceneEvents.AddTextLine("THE GOOD MONKS GIVE YOU RATIONS");   // Line 990
				int addition = 15 + RNG.rollInRange(1, 10);
				player.getParty().rations += addition;
			}
			visitSceneEvents.AddTextLine("'GO IN PEACE'");
			visitSceneEvents.AddTextLine("");
			visitSceneEvents.AddTextLine("PRESS RETURN TO CONTINUE");
			InputReceiverEvents.GetInputReceiverEvents().ActivateInputKeypress(handleInput);
		}

		private void handleInput(string key)
		{
			NextScene("MoveScene");
		}
	}
}