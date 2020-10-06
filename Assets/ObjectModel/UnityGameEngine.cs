namespace FotWK
{
    class UnityGameEngine : IGameEngine
    {
        public static UnityGameEngine mSingleton = new UnityGameEngine();

        UnitySoundEngine mSoundEngine = new UnitySoundEngine();
        UnityGameEngine()
        {
        }

        public ISoundEngine getSoundEngine()
        {
            return mSoundEngine;
        }

        public static IGameEngine getEngine()
        {
            return mSingleton;
        }
    }
}
