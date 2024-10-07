using HarmonyLib;
using Verse;

namespace CustomMusicOptions
{
    public class CustomMusicOptionsMod : Mod
    {
        public const string PACKAGE_ID = "custommusicoptions.1trickPwnyta";
        public const string PACKAGE_NAME = "Custom Music Options";

        public CustomMusicOptionsMod(ModContentPack content) : base(content)
        {
            var harmony = new Harmony(PACKAGE_ID);
            harmony.PatchAll();

            Log.Message($"[{PACKAGE_NAME}] Loaded.");
        }
    }
}
