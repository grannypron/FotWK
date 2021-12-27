using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace FotWK
{
	public abstract class BaseLocation
	{
		public abstract void onVisit();

		public void NextScene(string sceneName)
        {
            VisitSceneEvents.GetVisitSceneEvents().StartCoroutine(LoadNextScene(sceneName));
        }


        public IEnumerator LoadNextScene(string sceneName)
        {
            yield return new WaitForSeconds(Globals.VISIT_SCREEN_NO_EVENT_PAUSE_TIME);
            AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);

            // Wait until the asynchronous scene fully loads
            while (!asyncLoad.isDone)
            {
                yield return null;
            }
        }
    }
}
