using UnityEngine;

namespace FotWK
{
    class UnitySoundEngine : ISoundEngine
    {
        public void playSound(string soundId)
        {
            GameObject.Find(soundId).GetComponent<AudioSource>().Play();
        }
    }
}
