using CustomMusic;
using HarmonyLib;
using RimWorld;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using Verse;

namespace CustomMusicOptions
{
    [HarmonyPatch(typeof(MusicSequenceWorker))]
    [HarmonyPatch(nameof(MusicSequenceWorker.InitializeWorker))]
    public static class Patch_MusicSequenceWorker_InitializeWorker
    {
        public static void Postfix(MusicSequenceWorker __instance)
        {
            List<SongDef> songs = (List<SongDef>)typeof(MusicSequenceWorker).Field("songs").GetValue(null);
            if (CustomMusicOptionsSettings.DisableVanillaMusic)
            {
                songs.Clear();
            }
            if (__instance.def == DefDatabase<MusicSequenceDef>.GetNamed("HorrorCombat"))
            {
                songs.AddRange(DefDatabase<SongDef>.AllDefsListForReading.Where(d => d is CustomSongDef && d.GetSongType() == SongType.HorrorCombat));
            }
            if (__instance.def == DefDatabase<MusicSequenceDef>.GetNamed("HorrorTension") || __instance.def == DefDatabase<MusicSequenceDef>.GetNamed("HorrorMonolithAdvanced"))
            {
                songs.AddRange(DefDatabase<SongDef>.AllDefsListForReading.Where(d => d is CustomSongDef && d.GetSongType() == SongType.HorrorTension));
            }
            if (__instance.def == DefDatabase<MusicSequenceDef>.GetNamed("HorrorRelax"))
            {
                songs.AddRange(DefDatabase<SongDef>.AllDefsListForReading.Where(d => d is CustomSongDef && d.GetSongType() == SongType.HorrorRelax));
            }
            __instance.GetType().Method("Shuffle").Invoke(__instance, new object[] { });
        }
    }

    [HarmonyPatch(typeof(MusicSequenceWorker))]
    [HarmonyPatch(nameof(MusicSequenceWorker.SelectSong))]
    public static class Patch_MusicSequenceWorker_SelectSong
    {
        public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            foreach (CodeInstruction instruction in instructions)
            {
                if (instruction.opcode == OpCodes.Ldfld && (FieldInfo)instruction.operand == typeof(MusicSequenceDef).Field(nameof(MusicSequenceDef.songs)))
                {
                    yield return new CodeInstruction(OpCodes.Pop);
                    yield return new CodeInstruction(OpCodes.Ldsfld, typeof(MusicSequenceWorker).Field("songs"));
                    continue;
                }

                yield return instruction;
            }
        }

        public static void Postfix(SongDef __result)
        {
            if (__result is CustomSongDef)
            {
                AccessTools.TypeByName("CustomMusic.HarmonyPatches").Method("GetAudioClip").Invoke(null, new object[] { "file://" + __result.clipPath, __result });
            }
            CustomMusicCore.musicInfoCache.updateNeeded = true;
        }
    }
}
