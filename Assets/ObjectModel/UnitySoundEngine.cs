using UnityEngine;
using System.Collections;

namespace FotWK
{
    class UnitySoundEngine : MonoBehaviour, ISoundEngine
    {
        void Start() {}

        void Awake()
        {
            DontDestroyOnLoad(this.gameObject);
        }

        public void playSound(string soundId, MonoBehaviour caller)
        {
            AudioSource audio = GameObject.Find(soundId).GetComponent<AudioSource>();
            audio.Play();
            //caller.StartCoroutine(WaitCoroutine());
        }
        
        IEnumerator WaitCoroutine()
        {
            yield return new WaitForSeconds(10);
        }
    }
}
