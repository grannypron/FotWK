using UnityEngine;

namespace FotWK
{
    class UnityGameEngine : MonoBehaviour, IGameEngine
    {

        private UnitySoundEngine mSoundEngine;
        private static UnityGameEngine mGameEngine;

        void Start() { }

        void Awake()
        {
            DontDestroyOnLoad(this.gameObject);
            mGameEngine = this.gameObject.GetComponent<UnityGameEngine>();
            mGameEngine.mSoundEngine = this.gameObject.GetComponent<UnitySoundEngine>();
        }

        UnityGameEngine()
        {
        }

        public ISoundEngine getSoundEngine()
        {
            return mGameEngine.mSoundEngine;
        }

        public static IGameEngine getEngine()
        {
            return mGameEngine;
        }
    }
}
