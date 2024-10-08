using CustomMusic;
using HarmonyLib;
using RimWorld;
using System.Reflection.Emit;
using Verse;

namespace CustomMusicOptions
{
    [HarmonyPatch(typeof(CustomSongDef))]
    [HarmonyPatch(nameof(CustomSongDef.SetAllowedSeasons))]
    public static class Patch_CustomSongDef_SetAllowedSeasons
    {
        public static void Postfix(CustomSongDef __instance)
        {
            __instance.allowedSeasons.RemoveAll(s => s == Season.Undefined);
        }
    }

    [HarmonyPatch(typeof(CustomSongDef))]
    [HarmonyPatch(nameof(CustomSongDef.ExposeData))]
    public static class Patch_CustomSongDef_ExposeData
    {
        public static void Postfix(CustomSongDef __instance)
        {
            SongType songType = __instance.GetSongType();
            Scribe_Values.Look(ref songType, "songType", __instance.tense ? SongType.Combat : SongType.Normal);
            __instance.SetSongType(songType);
        }
    }
}
