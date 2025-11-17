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
            //TODO: check that there is not already a GameEngine object here - maybe throw a lock around this?
            DontDestroyOnLoad(this.gameObject);
            mGameEngine = this.gameObject.GetComponent<UnityGameEngine>();
            mGameEngine.mSoundEngine = this.gameObject.GetComponent<UnitySoundEngine>();
        }

        public static void setGameEngine_ForTesting(UnityGameEngine e)
        {
            mGameEngine = e;
        }

        public void setSoundEngine_ForTesting(UnitySoundEngine e)
        {
            mSoundEngine = e;
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
