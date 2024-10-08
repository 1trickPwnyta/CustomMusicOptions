using HarmonyLib;
using UnityEngine;
using Verse;

namespace CustomMusicOptions
{
    public class CustomMusicOptionsMod : Mod
    {
        public const string PACKAGE_ID = "custommusicoptions.1trickPwnyta";
        public const string PACKAGE_NAME = "Custom Music Options";

        public static CustomMusicOptionsSettings Settings;

        public CustomMusicOptionsMod(ModContentPack content) : base(content)
        {
            Settings = GetSettings<CustomMusicOptionsSettings>();

            var harmony = new Harmony(PACKAGE_ID);
            harmony.PatchAll();
            harmony.Patch(AccessTools.TypeByName("MusicMod").Method("DoSettingsWindowContents"), null, null, typeof(Patch_MusicMod).Method(nameof(Patch_MusicMod.Transpiler)));

            Log.Message($"[{PACKAGE_NAME}] Loaded.");
        }

        public override string SettingsCategory() => PACKAGE_NAME;

        public override void DoSettingsWindowContents(Rect inRect)
        {
            base.DoSettingsWindowContents(inRect);
            CustomMusicOptionsSettings.DoSettingsWindowContents(inRect);
        }
    }
}
