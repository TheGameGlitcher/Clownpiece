using HarmonyLib;

namespace Clownpiece
{
    public static class PInfo
    {
        // each loaded plugin needs to have a unique GUID. usually author+generalCategory+Name is good enough
        public const string GUID = "gameglitcher.character.clownpiece";
        public const string Name = "Clownpiece";
        public const string version = "0.1.1";
        public static readonly Harmony harmony = new Harmony(GUID);

    }
}
